using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using RulesEngine.Application.Abstractions.Services;
using RulesEngine.Application.Builders;
using RulesEngine.Application.Clients.Mundial.Validatior;
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
        private readonly IQueryExecutorPerPhase _queryExecutor;
        private readonly IQueryExecutorPerPhaseFactory _phaseFactory;


        public InvoiceToCheckBuilderSolidaria(IEnumerable<IExternalDataLoader> loaders,
                                    IUtilityService utilityService,
                                    IParameterRepository parameterRepository,
                                    IInvoiceRepository invoiceRepository,
                                    IBlobStorageRepository blobStorageRepository,
                                    IConstantsRepository constantsRepository,
                                    IQueryExecutorPerPhase queryExecutor,
                                    IQueryExecutorPerPhaseFactory phaseFactory)
        {
            _loaders = loaders;
            _utilityService = utilityService;
            _parameterRepository = parameterRepository;
            _invoiceRepository = invoiceRepository;
            _blobStorageRepository = blobStorageRepository;
            _constantsRepository = constantsRepository;
            _queryExecutor = queryExecutor;
            _phaseFactory = phaseFactory;
        }

        public async Task<IInvoiceToCheckContext> BuildAsync(string radNumber, string moduleName, string stage, string tenant)
        {
            var _loader = _loaders.FirstOrDefault(l =>
            string.Equals(l.Tenant, tenant, StringComparison.OrdinalIgnoreCase))
            ?? throw new InvalidOperationException($"No hay loader para el tenant '{tenant}'.");
            // Se obtiene del cache el blob
            var blobStorage = await _loader.GetBlobStorageAsync(tenant);

            // Se obtiene de la agregación parametrizada del cache
            var parameter = await _loader.GetInvoiceAgregationParameterAsync(tenant);

            // Se reemplazan los valores para obtener la invoice
            string? json = parameter.Value.Replace("##RadNumber##", radNumber).Replace("##ModuleName##", moduleName);

            // Se crea objeto Bson y se consulta la invoice
            var bsonDocs = BsonSerializer.Deserialize<BsonDocument[]>(json);
            var invoiceDataDoc = await _invoiceRepository.GetInvoiceByAggregation(bsonDocs);
            if (invoiceDataDoc == null)
                throw new Exception("No se encontró información de la factura");

            var invoiceData = BsonSerializer.Deserialize<InvoiceData>(invoiceDataDoc.ToJson());

            var executor = _phaseFactory.ForTenant(tenant);
            switch (stage)
            {
                case "Fase_01":
                case "Fase_03":
                    await _queryExecutor.QueryPerStage01(invoiceData, radNumber, tenant);
                    break;
                case "Fase_02":
                    await _queryExecutor.QueryPerStage02(invoiceData, radNumber, tenant);
                    break;
            }

            var validations = new InvoiceValidator(_constantsRepository);
            var result = await validations.ValidateAsync(invoiceData);

            if (!result.IsValid)
            {
                string[] required = ["requerida", "requerido", "obligatorio", "obligatoria"];
                // Filtrar los errores NotNull
                invoiceData.NotNullErrorsInModel = result.Errors
                .Where(error =>
                    (error.ErrorCode == "NotNullValidator" || error.ErrorCode == "NotEmptyValidator") ||
                    (error.ErrorCode == "CustomMustValidator" && required.Any(r => error.ErrorMessage.Contains(r))))
                .Select(error => error.FormattedMessagePlaceholderValues["PropertyName"]?.ToString())
                .Distinct()
                .ToList()!;

                invoiceData.TypeErrorsInModel = result.Errors
                    .Where(error => (error.ErrorCode == "CustomMustValidator") && !required.Any(r => error.ErrorMessage.Contains(r)))
                    .Select(error => error.FormattedMessagePlaceholderValues["PropertyName"]?.ToString())
                    .Distinct()
                    .ToList()!;
            }

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


            // Construir InvoiceToCheck usando LoadInputSources
            var invoice = new InvoiceToCheckSolidaria(radNumber, _loader);
            invoice = await new LoadInputSourcesSolidaria(invoice, invoiceData, stage, inputSources, _invoiceRepository, _utilityService).Create();

            return invoice;

        }
    }
}
