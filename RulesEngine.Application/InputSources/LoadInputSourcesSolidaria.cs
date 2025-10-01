using Microsoft.Extensions.Caching.Memory;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using RulesEngine.Application.Abstractions.Services;
using RulesEngine.Domain.InputSourcesEntities;
using RulesEngine.Domain.Invoices.Entities;
using RulesEngine.Domain.Invoices.Repositories;
using RulesEngine.Domain.RulesEntities.Solidaria.Entities;
using RulesEngine.Domain.ValueObjects;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static RulesEngine.Domain.RulesEntities.Solidaria.Entities.InvoiceToCheckSolidaria;
using Date = RulesEngine.Domain.ValueObjects.Date;

namespace RulesEngine.Application.InputSources
{
    /// <summary>
    /// Carga fuentes externas (matrices en Blob) y adapta datos del modelo SolidariaInvoiceData
    /// al contexto InvoiceToCheckSolidaria para evaluación de reglas.
    /// Adaptado desde la versión FURIPS/Mundial que usaba Sections.
    /// </summary>
    public class LoadInputSourcesSolidaria
    {
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly InputSourcesModel _sourcesModel;
        private readonly SolidariaInvoiceData _data;
        private InvoiceToCheckSolidaria _entity;
        private readonly string _stage;
        private readonly IUtilityService _utilityService;

        public LoadInputSourcesSolidaria(
            InvoiceToCheckSolidaria invoice,
            SolidariaInvoiceData data,
            string stage,
            InputSourcesModel inputSources,
            IInvoiceRepository invoiceRepository,
            IUtilityService utilityService)
        {
            _invoiceRepository = invoiceRepository;
            _entity = invoice;
            _stage = stage;
            _data = data;
            _sourcesModel = inputSources;
            _utilityService = utilityService;
        }

        public async Task<InvoiceToCheckSolidaria> Create()
        {
            // Asignaciones básicas desde el nuevo modelo
            _entity.IpsNit = _data.Provider?.IdNumber ?? string.Empty;
            _entity.LicensePlate = _data.Claim?.Vehicle?.PlateNumber ?? string.Empty;
            _entity.SoatNumber = _data.Claim?.Vehicle?.Soat?.Policy?.Number
                                 ?? _data.Claim?.Vehicle?.Soat?.SIRASFilingNumber
                                 ?? string.Empty;
            _entity.VictimId = _data.Claim?.Victims?.FirstOrDefault()?.IdNumber ?? string.Empty;
            _entity.InvoiceNumber = _data.InvoiceNumber;
            _entity.LicensePlateAmbulance = _data.Claim?.Victims?
                                                  .FirstOrDefault()?
                                                  .RemissionInfo?
                                                  .Transport?
                                                  .PrimaryTransferAmbulancePlate ?? string.Empty;

            var loadedData = new ConcurrentDictionary<string, object>();
            var tasks = new List<Task>();

            // Normalizamos stage a minúsculas para comparación
            var normalizedStage = _stage?.ToLowerInvariant();

            if (normalizedStage == "fase_02")
            {
                tasks.Add(Task.Run(async () => loadedData["ipsNitList"] =
                    await _utilityService.GetOrSetDataCache("ipsNitList", _sourcesModel.IpsNitList.Create, 72)));

                tasks.Add(Task.Run(async () => loadedData["invoiceNumber"] =
                    await _utilityService.GetOrSetDataCache("invoiceNumber", _sourcesModel.InvoiceNumber.Create, 72)));

                tasks.Add(Task.Run(async () => loadedData["usersAllowedToClaim"] =
                    await _utilityService.GetOrSetDataCache("usersAllowedToClaim", _sourcesModel.AllowedUsers.Create, 72)));

                tasks.Add(Task.Run(async () => loadedData["serviceCodes"] =
                    await _utilityService.GetOrSetDataCache("serviceCodes", _sourcesModel.ServiceCodes.Create, 72)));

                tasks.Add(Task.Run(async () => loadedData["atypicalEvent"] =
                    await _utilityService.GetOrSetDataCache("atypicalEvent", _sourcesModel.AtypicalEvent.Create, 72)));

                tasks.Add(Task.Run(async () => loadedData["ambulanceControl"] =
                    await _utilityService.GetOrSetDataCache("ambulanceControl", _sourcesModel.AmbulanceControl.Create, 72)));

                await Task.WhenAll(tasks);

                _entity.IpsNitList = ((IEnumerable<IpsNitFile>)loadedData["ipsNitList"])
                    .Where(x => x.NitIps == _entity.IpsNit).ToList();

                _entity.InvoiceNumberFile = ((IEnumerable<InvoiceNumberFile>)loadedData["invoiceNumber"])
                    .Where(x => x.InvoiceNumber == _entity.InvoiceNumber).ToList();

                _entity.AllowedUsers = ((IEnumerable<AllowedUser>)loadedData["usersAllowedToClaim"]).ToList();
                _entity.ServiceCodeFiles = ((IEnumerable<ServiceCodeFile>)loadedData["serviceCodes"]).ToList();

                _entity.AtypicalEvent = ((IEnumerable<AtypicalEvent>)loadedData["atypicalEvent"])
                    .Where(x =>
                        x.VictimId == _entity.VictimId ||
                        x.SoatNumber == _entity.SoatNumber ||
                        x.LicensePlate == _entity.LicensePlate)
                    .ToList();

                _entity.AmbulanceControl = ((IEnumerable<AmbulanceControl>)loadedData["ambulanceControl"])
                    .Where(x => x.LicensePlate == _entity.LicensePlateAmbulance)
                    .ToList();
            }
            else // Fase_01 y Fase_03 (o cualquier otra)
            {
                tasks.Add(Task.Run(async () => loadedData["ipsPhoneVerification"] =
                    await _utilityService.GetOrSetDataCache("ipsPhoneVerification", _sourcesModel.IpsPhoneVerification.Create, 72)));

                tasks.Add(Task.Run(async () => loadedData["catastrophicEvent"] =
                    await _utilityService.GetOrSetDataCache("catastrophicEvent", _sourcesModel.CatastrophicEvent.Create, 72)));

                tasks.Add(Task.Run(async () => loadedData["ambulanceControl"] =
                    await _utilityService.GetOrSetDataCache("ambulanceControl", _sourcesModel.AmbulanceControl.Create, 72)));

                tasks.Add(Task.Run(async () => loadedData["ipsInvestigation"] =
                    await _utilityService.GetOrSetDataCache("ipsInvestigation", _sourcesModel.IpsInvestigation.Create, 72)));

                tasks.Add(Task.Run(async () => loadedData["atypicalEvent"] =
                    await _utilityService.GetOrSetDataCache("atypicalEvent", _sourcesModel.AtypicalEvent.Create, 72)));

                tasks.Add(Task.Run(async () => loadedData["fraudulentIps"] =
                    await _utilityService.GetOrSetDataCache("fraudulentIps", _sourcesModel.FraudulentIps.Create, 72)));

                await Task.WhenAll(tasks);

                _entity.FraudulentIps = ((IEnumerable<FraudulentIps>)loadedData["fraudulentIps"])
                    .FirstOrDefault(x => x.IpsNit == _entity.IpsNit);

                _entity.IpsInvestigation = ((IEnumerable<IpsInvestigation>)loadedData["ipsInvestigation"])
                    .FirstOrDefault(x => x.IpsNit == _entity.IpsNit);

                _entity.CatastrophicEvent = ((IEnumerable<CatastrophicEvent>)loadedData["catastrophicEvent"])
                    .Where(x => x.SoatNumber == _entity.SoatNumber)
                    .ToList();

                _entity.IpsPhoneVerification = ((IEnumerable<IpsPhoneVerification>)loadedData["ipsPhoneVerification"])
                    .Where(x => x.NitIps == _entity.IpsNit)
                    .ToList();

                _entity.AtypicalEvent = ((IEnumerable<AtypicalEvent>)loadedData["atypicalEvent"])
                    .Where(x =>
                        x.VictimId == _entity.VictimId ||
                        x.SoatNumber == _entity.SoatNumber ||
                        x.LicensePlate == _entity.LicensePlate)
                    .ToList();

                _entity.AmbulanceControl = ((IEnumerable<AmbulanceControl>)loadedData["ambulanceControl"])
                    .Where(x => x.LicensePlate == _entity.LicensePlateAmbulance)
                    .ToList();
            }

            _entity = FillInformation(_data, _entity);

            await FindDuplicated();

            return _entity;
        }

        /// <summary>
        /// Busca duplicados basados en VictimId / SoatNumber en otros documentos Solidaria.
        /// Se adapta el filtro al nuevo layout (sin 'Detail.forms.sections').
        /// Ajusta los nombres de campos si en la colección real difieren.
        /// </summary>
        private async Task FindDuplicated()
        {
            bool searchInvoices = false;

            // Base filter: no el mismo radNumber y que tenga WorkflowData (si existe esa lógica aún)
            var filter = Builders<Invoice>.Filter.Ne(x => x.RadNumber, _entity.RadNumber) &
                         Builders<Invoice>.Filter.Exists("WorkflowData");

            // Campos claves en nuevo esquema:
            // Claim.Vehicle.Soat.Policy.Number OR Claim.Vehicle.Soat.SIRASFilingNumber
            // Claim.Victims[].IdNumber

            var victimId = _entity.VictimId;
            var soatNumber = _entity.SoatNumber;

            if (!string.IsNullOrWhiteSpace(soatNumber) && !string.IsNullOrWhiteSpace(victimId))
            {
                searchInvoices = true;
                filter &= (Builders<Invoice>.Filter.Eq("Claim.Vehicle.Soat.Policy.Number", soatNumber) |
                           Builders<Invoice>.Filter.Eq("Claim.Vehicle.Soat.SIRASFilingNumber", soatNumber) |
                           Builders<Invoice>.Filter.ElemMatch<BsonValue>("Claim.Victims", new BsonDocument("IdNumber", victimId)));
            }
            else if (!string.IsNullOrWhiteSpace(soatNumber))
            {
                searchInvoices = true;
                filter &= (Builders<Invoice>.Filter.Eq("Claim.Vehicle.Soat.Policy.Number", soatNumber) |
                           Builders<Invoice>.Filter.Eq("Claim.Vehicle.Soat.SIRASFilingNumber", soatNumber));
            }
            else if (!string.IsNullOrWhiteSpace(victimId))
            {
                searchInvoices = true;
                filter &= Builders<Invoice>.Filter.ElemMatch<BsonValue>("Claim.Victims", new BsonDocument("IdNumber", victimId));
            }

            if (!searchInvoices)
                return;

            // Obtiene coincidencias
            var duplicated = await _invoiceRepository.FilterByAsync(filter, new FindOptions<Invoice>());

            foreach (var inv in duplicated)
            {
                // Convertimos cada invoice a BsonDocument para navegar libremente
                var doc = inv.ToBsonDocument();

                var claim = doc.GetValue("Claim", null) as BsonDocument;
                if (claim == null) continue;

                var veh = claim.GetValue("Vehicle", null) as BsonDocument;
                var soat = veh?.GetValue("Soat", null) as BsonDocument;
                var policy = soat?.GetValue("Policy", null) as BsonDocument;
                var victims = claim.GetValue("Victims", new BsonArray()) as BsonArray;
                var eventNode = claim.GetValue("Event", null) as BsonDocument;

                // Tomamos primera víctima (igual que en validaciones) para el duplicado
                var victimDoc = victims?.FirstOrDefault()?.AsBsonDocument;
                var remissionInfo = victimDoc?.GetValue("RemissionInfo", null) as BsonDocument;
                var transport = remissionInfo?.GetValue("Transport", null) as BsonDocument;

                _entity.DuplicatedInvoices.Add(new DuplicatedInvoice
                {
                    VictimId = victimDoc?.GetValue("IdNumber", "").AsString,
                    DocumentType = victimDoc?.GetValue("IdType", null) is BsonDocument idType
                        ? (idType.GetValue("Code", "").IsString ? idType.GetValue("Code", "").AsString : string.Empty)
                        : string.Empty,
                    EventDate = eventNode?.GetValue("Date", BsonNull.Value) is BsonDateTime dt ? dt.ToUniversalTime() : (DateTime?)null,
                    LicensePlate = veh?.GetValue("PlateNumber", "").AsString,
                    SoatNumber = policy?.GetValue("Number", "").AsString ?? soat?.GetValue("SIRASFilingNumber", "").AsString,
                    VehicleType = veh?.GetValue("Type", null) is BsonDocument vehType
                        ? vehType.GetValue("Code", "").AsString
                        : string.Empty,
                    HabilitationCodeProvider = _data.Provider?.HabilitationCode ?? string.Empty,
                    LicencePlateAmbulance = transport?.GetValue("PrimaryTransferAmbulancePlate", "").AsString
                });
            }
        }

        private static InvoiceToCheckSolidaria FillInformation(SolidariaInvoiceData data, InvoiceToCheckSolidaria target)
        {
            // Campos equivalentes adaptados
            target.IpsNit = data.Provider?.IdNumber ?? string.Empty;
            target.ModuleName = data.BusinessInvoiceStatus ?? string.Empty;
            target.IpsNitRips = data.Provider?.IdNumber ?? string.Empty;

            // Vehículo / SOAT
            target.SoatNumber = data.Claim?.Vehicle?.Soat?.Policy?.Number
                                ?? data.Claim?.Vehicle?.Soat?.SIRASFilingNumber
                                ?? string.Empty;
            target.LicensePlate = data.Claim?.Vehicle?.PlateNumber ?? string.Empty;

            // Víctima principal
            var victim = data.Claim?.Victims?.FirstOrDefault();
            target.VictimId = victim?.IdNumber ?? string.Empty;
            target.DocumentType = victim?.IdType?.Code ?? string.Empty;

            // Números de factura (Solidaria no tiene FURIPS1/FURIPS2, se mapean a invoice principal)
            target.InvoiceNumberF1 = data.InvoiceNumber;
            target.InvoiceNumberF2 = data.InvoiceNumber;
            target.InvoiceNumberFurips = data.InvoiceNumber;

            // Fechas (convertimos usando Date helper)
            target.EventDate = Date.ConvertToUtcFormattedDate(data.Claim?.Event?.Date?.ToString());
            target.DeathDate = Date.ConvertToUtcFormattedDate(victim?.DeathInfo?.DeathDate?.ToString());
            // ClaimDate: en FURIPS era Notificationdate; aquí podría mapearse a FilingDate
            target.ClaimDate = Date.ConvertToUtcFormattedDate(data.FillingDate?.ToString());
            target.InvoiceDate = Date.ConvertToUtcFormattedDate(data.InvoiceEmissionDate?.ToString());
            target.IncomeDate = Date.ConvertToUtcFormattedDate(victim?.MedicalAttention?.IncomeDate?.ToString());
            target.EgressDate = Date.ConvertToUtcFormattedDate(victim?.MedicalAttention?.EgressDate?.ToString());
            target.InvoiceMAOSDate = Date.Create(null); // Solidaria: no análogo directo a MosData.ProviderInvoiceDate en ejemplo

            // Valores económicos (si aplican en Solidaria)
            var protections = victim?.ProtectionsClaimed;
            target.InvoiceValue = Currency.Create(data.InvoiceValue?.ToString() ?? "0");
            target.BilledMedicalExpenses = Currency.Create(protections?.MedicalSurgicalExpenses?.TotalBilled?.ToString() ?? "0");
            target.BilledTransportation = Currency.Create(protections?.VictimTransportAndMobilizationExpenses?.TotalBilled?.ToString() ?? "0");

            // Electronic Billing (si se cargó)
            target.IpsNitFE = data.ElectronicBilling?.NitIps ?? string.Empty;
            target.InvoiceNumberFE = data.ElectronicBilling?.InvoiceNumber ?? string.Empty;

            // Aggregations
            target.InvoiceDifferentRadicates = data.InvoiceDifferentRadicates;
            target.LicensePlateAmbulance = victim?.RemissionInfo?.Transport?.PrimaryTransferAmbulancePlate ?? string.Empty;
            target.ProviderData = data.ProviderData;
            target.AlertsEncountered = data.AlertsNode?.Select(a => new RulesEngine.Domain.Common.AlertSolidaria
            {
                NameAction = a.NameAction,
                Type = a.Type,
                Module = a.Module,
                Message = a.Message,
                Description = a.Description
            }).ToArray();

            target.Research = data.ResearchData;
            target.InvestigationResponseDate = data.ResearchData != null && data.ResearchData.Any()
                ? Date.ConvertToUtcFormattedDate(
                    data.ResearchData
                        .Where(r => r.OriginModule == data.BusinessInvoiceStatus)
                        .Max(r => r.ResponseDate)
                        ?.ToString())
                : Date.Create(null);

            // Totales de glosas – en Solidaria están en Claim.TotalGlossValues (adaptar si se requiere)
            var totalApproved = data.Claim?.TotalGlossValues?.TotalInvoiceApprovedValue ?? 0;
            var totalGlossed = data.Claim?.TotalGlossValues?.TotalInvoiceObjectedValue ?? 0;
            target.TotalAuthorizedValue = Currency.Create(totalApproved.ToString());
            target.TotalGlossedValue = Currency.Create(totalGlossed.ToString());

            target.ProcessAndContracts = data.LegalProcessesAndTransactionContractsParameters?
                .Where(x => x.InvoiceNumber == data.InvoiceNumber && x.ClaimantId == data.Provider?.IdNumber)
                .ToList();

            target.SinisterAggretation = data.ValidationSinister;
            target.PreviousObjections = data.PreviousObjections;
            target.MultipleTransposrts = data.MultipleTransports;
            target.ResearchRequest = data.ResearchRequest;
            target.ListServiceCodes = victim?.Services?
                .Where(s => s.ServiceInfo != null)
                .Select(s => s.ServiceInfo!.Code)
                .Distinct()
                .ToList() ?? new List<string>();

            target.NotNullErrorsInModel = data.NotNullErrorsInModel ?? new List<string>();
            target.TypeErrorsInModel = data.TypeErrorsInModel ?? new List<string>();
            target.ValidationAggregationRules_31_40 = data.ResultAggregationRules;

            target.InvoicePhoneVerificationValue = Currency.Create(data.InvoicePhoneVerificationValue ?? "0");
            target.UserClaim = data.ClaimsQueue?.UserAccount;
            target.HelpType = data.ProtectionType?.Value ?? string.Empty;
            target.HelpTypeToValidate = data.ParametrizedHelpType;
            target.PrimaryTransportationDate = Date.Create(null);

            target.VehicleType = data.Claim?.Vehicle?.Type?.Value;

            return target;
        }
    }
}