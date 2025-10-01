using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using RulesEngine.Application.Abstractions.Services;
using RulesEngine.Application.Builders;
using RulesEngine.Application.Clients.Solidaria.Validator;
using RulesEngine.Application.Common.Enumerations;
using RulesEngine.Application.DataSources;
using RulesEngine.Application.InputSources;
using RulesEngine.Domain.BlobsStorage.Repositories;
using RulesEngine.Domain.Constants;
using RulesEngine.Domain.Invoices.Entities;
using RulesEngine.Domain.Invoices.Repositories;
using RulesEngine.Domain.Parameters.Repositories;
using RulesEngine.Domain.Primitives;
using RulesEngine.Domain.RulesEntities.Solidaria.Entities;
using RulesEngine.Infrastructure.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RulesEngine.Infrastructure.Builders
{
    public class InvoiceToCheckBuilderSolidaria : IInvoiceToCheckBuilder
    {
        public string Tenant => "Solidaria";

        private readonly IEnumerable<IExternalDataLoader> _loaders;
        private readonly IUtilityService _utilityService;
        private readonly IParameterRepository _parameterRepository;
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly IBlobStorageRepository _blobStorageRepository;
        private readonly IConstantsRepository _constantsRepository;
        private readonly IQueryExecutorPerPhaseSolidaria _executor;

        // Constructor simplificado: YA NO usamos la fábrica genérica
        public InvoiceToCheckBuilderSolidaria(
            IEnumerable<IExternalDataLoader> loaders,
            IUtilityService utilityService,
            IParameterRepository parameterRepository,
            IInvoiceRepository invoiceRepository,
            IBlobStorageRepository blobStorageRepository,
            IConstantsRepository constantsRepository,
            IQueryExecutorPerPhaseSolidaria executor)
        {
            _loaders = loaders;
            _utilityService = utilityService;
            _parameterRepository = parameterRepository;
            _invoiceRepository = invoiceRepository;
            _blobStorageRepository = blobStorageRepository;
            _constantsRepository = constantsRepository;
            _executor = executor;
        }

        public async Task<IInvoiceToCheckContext> BuildAsync(string radNumber, string moduleName, string stage, string tenant)
        {
            if (!string.Equals(tenant, Tenant, StringComparison.OrdinalIgnoreCase))
                throw new InvalidOperationException($"Builder incorrecto para tenant '{tenant}'. Se esperaba '{Tenant}'.");

            // Loader específico Solidaria
            var loader = _loaders.FirstOrDefault(l =>
                string.Equals(l.Tenant, tenant, StringComparison.OrdinalIgnoreCase))
                ?? throw new InvalidOperationException($"No hay loader para el tenant '{tenant}'.");

            // Blob storage (puede provenir de caché interna de loader)
            var blobStorage = await loader.GetBlobStorageAsync(tenant);

            // Parámetro de agregación que obtiene la factura
            var parameter = await loader.GetInvoiceAgregationParameterAsync(tenant);

            // Construir pipeline: se asume que el parámetro Value es un JSON de agregación con placeholders
            string jsonPipeline = parameter.Value
                .Replace("##RadNumber##", radNumber)
                .Replace("##ModuleName##", moduleName);

            var pipelineDocs = BsonSerializer.Deserialize<BsonDocument[]>(jsonPipeline);
            var invoiceDataDoc = await _invoiceRepository.GetInvoiceByAggregation(pipelineDocs);
            if (invoiceDataDoc == null)
                throw new Exception("No se encontró información de la factura");

            // Deserializar al modelo propio Solidaria (sin herencia)
            var invoiceData = BsonSerializer.Deserialize<SolidariaInvoiceData>(invoiceDataDoc);

            // Opcional: forzar ModuleName si deseas asegurarlo al valor entrante
            if (string.IsNullOrWhiteSpace(invoiceData.ModuleName))
                invoiceData.ModuleName = moduleName;

            // Ejecutar lógica por fase usando el executor específico Solidaria
            switch (stage)
            {
                case "Fase_01":
                case "Fase_03":
                    await _executor.QueryPerStage01(invoiceData, radNumber, tenant);
                    break;
                case "Fase_02":
                    await _executor.QueryPerStage02(invoiceData, radNumber, tenant);
                    break;
                default:
                    // Puedes lanzar excepción si stage desconocida
                    // throw new ArgumentException($"Etapa '{stage}' no soportada para Solidaria");
                    break;
            }

            // Validaciones de negocio / formato para Solidaria
            var validator = new InvoiceValidatorSolidaria(_constantsRepository);
            var validationResult = await validator.ValidateAsync(invoiceData);

            if (!validationResult.IsValid)
            {
                invoiceData.NotNullErrorsInModel ??= new List<string>();
                invoiceData.TypeErrorsInModel ??= new List<string>();

                string[] requiredTokens = ["requerida", "requerido", "obligatorio", "obligatoria"];

                invoiceData.NotNullErrorsInModel = validationResult.Errors
                    .Where(err =>
                        err.ErrorCode == "NotNullValidator" ||
                        err.ErrorCode == "NotEmptyValidator" ||
                        (err.ErrorCode == "CustomMustValidator" &&
                         requiredTokens.Any(t => err.ErrorMessage.Contains(t, StringComparison.OrdinalIgnoreCase))))
                    .Select(err => err.FormattedMessagePlaceholderValues.TryGetValue("PropertyName", out var nameObj) ? nameObj?.ToString() : null)
                    .Where(n => !string.IsNullOrWhiteSpace(n))
                    .Distinct()
                    .ToList()!;

                invoiceData.TypeErrorsInModel = validationResult.Errors
                    .Where(err =>
                        err.ErrorCode == "CustomMustValidator" &&
                        !requiredTokens.Any(t => err.ErrorMessage.Contains(t, StringComparison.OrdinalIgnoreCase)))
                    .Select(err => err.FormattedMessagePlaceholderValues.TryGetValue("PropertyName", out var nameObj) ? nameObj?.ToString() : null)
                    .Where(n => !string.IsNullOrWhiteSpace(n))
                    .Distinct()
                    .ToList()!;
            }

            // Matrices e insumos externos (Blob)
            var inputSources = new InputSourcesModel
            {
                FraudulentIps = new(blobStorage.ConnectionString, BlobStorageExtensions.FindResource(blobStorage, InputSources.MatrizIpsFraudulentas)),
                IpsInvestigation = new(blobStorage.ConnectionString, BlobStorageExtensions.FindResource(blobStorage, InputSources.MatrizIpsEsquemaInvestigación)),
                AtypicalEvent = new(blobStorage.ConnectionString, BlobStorageExtensions.FindResource(blobStorage, InputSources.MatrizAtipicidades)),
                CatastrophicEvent = new(blobStorage.ConnectionString, BlobStorageExtensions.FindResource(blobStorage, InputSources.TablaSiniestrosCatastroficos)),
                IpsPhoneVerification = new(blobStorage.ConnectionString, BlobStorageExtensions.FindResource(blobStorage, InputSources.MatrizVerificacionTelefonicaIps)),
                AmbulanceControl = new(blobStorage.ConnectionString, BlobStorageExtensions.FindResource(blobStorage, InputSources.MatrizControlAmbulancias)),
                IpsNitList = new(blobStorage.ConnectionString, BlobStorageExtensions.FindResource(blobStorage, InputSources.IpsNits)),
                AllowedUsers = new(blobStorage.ConnectionString, BlobStorageExtensions.FindResource(blobStorage, InputSources.UsuariosQueProcesaReclamacion)),
                InvoiceNumber = new(blobStorage.ConnectionString, BlobStorageExtensions.FindResource(blobStorage, InputSources.ListadoDeFacturas)),
                ServiceCodes = new(blobStorage.ConnectionString, BlobStorageExtensions.FindResource(blobStorage, InputSources.ListadoDeCodigosDeServicios))
            };

            // Construye el contexto usable por motor de reglas
            var context = new InvoiceToCheckSolidaria(radNumber, loader);
            context = await new LoadInputSourcesSolidaria(
                context,
                invoiceData,
                stage,
                inputSources,
                _invoiceRepository,
                _utilityService)
                .Create();

            return context;
        }
    }
}