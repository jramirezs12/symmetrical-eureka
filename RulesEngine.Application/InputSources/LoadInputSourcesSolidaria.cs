using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using RulesEngine.Application.Abstractions.Services;
using RulesEngine.Domain.InputSourcesEntities;
using RulesEngine.Domain.Invoices.Entities;
using RulesEngine.Domain.Invoices.Repositories;
using RulesEngine.Domain.Provider.Entities;
using RulesEngine.Domain.RulesEntities.Solidaria.Entities;
using RulesEngine.Domain.ValueObjects;
using System.Collections.Concurrent;
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
            //_entity.IpsNit = _data.Provider?.IdNumber ?? string.Empty;
            
            //_entity.LicensePlate = _data.Claim?.Vehicle?.PlateNumber ?? string.Empty;
            //_entity.SoatNumber = _data.Claim?.Vehicle?.Soat?.Policy?.Number
            //                     ?? _data.Claim?.Vehicle?.Soat?.SIRASFilingNumber
            //                     ?? string.Empty;
            //_entity.VictimId = _data.Claim?.Victims?.FirstOrDefault()?.IdNumber ?? string.Empty;
            //_entity.InvoiceNumber = _data.InvoiceNumber;
            //_entity.LicensePlateAmbulance = _data.Claim?.Victims?
            //                                      .FirstOrDefault()?
            //                                      .RemissionInfo?
            //                                      .Transport?
            //                                      .PrimaryTransferAmbulancePlate ?? string.Empty;

            _entity.IpsNit = _data.IpsNit ?? string.Empty;
            _entity.LicensePlate = _data.Sections.InvolvedVehicleInformation.LicensePlate ?? string.Empty;
            _entity.SoatNumber = _data.Sections.InvolvedVehicleInformation.SoatNumber ?? string.Empty;
            _entity.VictimId = _data.Sections.VictimData.IdentificationNumber ?? string.Empty;
            _entity.InvoiceNumber = _data.Sections.ClaimData.RadNumber ?? string.Empty;
            //_entity.InvoiceNumber = _data.InvoiceNumber ?? string.Empty;
            _entity.LicensePlateAmbulance = _data.Sections.RemissionInfo.PrimaryTransferAmbulancePlate ?? string.Empty;


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
                    HabilitationCodeProvider = _data.HabilitationCode ?? string.Empty,
                    LicencePlateAmbulance = transport?.GetValue("PrimaryTransferAmbulancePlate", "").AsString
                });
            }
        }

        private static InvoiceToCheckSolidaria FillInformation(SolidariaInvoiceData data, InvoiceToCheckSolidaria target)
        {
            // Campos equivalentes adaptados
            target.IpsNit = data.IpsNit;
            target.ModuleName = data.BusinessInvoiceStatus ?? string.Empty;

            target.IpsNitRips = data.IpsNit ?? string.Empty;

            // Vehículo / SOAT
            target.SoatNumber = data.Sections.InvolvedVehicleInformation.SoatNumber ?? string.Empty;
            target.LicensePlate = data.Sections.InvolvedVehicleInformation.LicensePlate ?? string.Empty;

            // Víctima principal
            var victim = data.Sections;
            target.VictimId = victim.VictimData.IdentificationNumber ?? string.Empty;
            target.DocumentType = victim.VictimData.DocumentType ?? string.Empty;

            // Números de factura
            target.InvoiceNumberF1 = data.Sections.ClaimData.InvoiceNumber;
            target.InvoiceNumberF2 = data.Sections.ClaimData.InvoiceNumber;
            target.InvoiceNumberFurips = data.Sections.ClaimData.InvoiceNumber;

            // Fechas
            target.EventDate = Date.ConvertToUtcFormattedDate(data.Sections.EventInformation.EventDate);
            target.DeathDate = Date.ConvertToUtcFormattedDate(data.Sections.VictimData.DeathDate);
            target.ClaimDate = Date.ConvertToUtcFormattedDate(data.Sections.ClaimData.FillingDate);
            target.InvoiceDate = Date.ConvertToUtcFormattedDate(data.InvoiceDate);
            target.IncomeDate = Date.ConvertToUtcFormattedDate(victim.MedicalCertification.IncomeDate);
            target.EgressDate = Date.ConvertToUtcFormattedDate(victim.MedicalCertification.EgressDate);
            target.InvoiceMAOSDate = Date.Create(null);

            // Valores económicos
            target.InvoiceValue = Currency.Create(data.Sections.ClaimData.InvoiceValue.ToString() ?? "0");
            target.BilledMedicalExpenses = Currency.Create(victim.VictimData.MedicalSurgicalExpenses.ToString() ?? "0");
            target.BilledTransportation = Currency.Create(victim.VictimData.VictimTransportAndMobilizationExpenses.ToString() ?? "0");

            // Aggregations
            target.LicensePlateAmbulance = victim.RemissionInfo.PrimaryTransferAmbulancePlate ?? string.Empty;

            target.ProviderData = new ProviderData()
            {
                HabilitationCode = data.HabilitationCode,
                NameIps = data.NameIps,
                NitIps = data.IpsNit ?? string.Empty
            };

            // FIX: mapear a la lista List<AlertSolidaria> AlertSolidaria (no AlertsEncountered[])
            target.AlertSolidaria = data.AlertsNode != null
                ? data.AlertsNode.Select(a => new RulesEngine.Domain.Common.AlertSolidaria
                {
                    NameAction = a.NameAction,
                    Type = a.Type,
                    Module = a.Module,
                    Message = a.Message,
                    Description = a.Description,
                    Typification = string.Empty,
                    HasPriority = false
                }).ToList()
                : new List<RulesEngine.Domain.Common.AlertSolidaria>();

            // Totales de glosas
            var totalApproved = data.Sections.TotalGlossValues.TotalInvoiceApprovedValue ?? 0;
            var totalGlossed = data.Sections.TotalGlossValues.TotalInvoiceObjectedValue ?? 0;
            target.TotalAuthorizedValue = Currency.Create(totalApproved.ToString());
            target.TotalGlossedValue = Currency.Create(totalGlossed.ToString());

            target.NotNullErrorsInModel = data.NotNullErrorsInModel ?? new List<string>();
            target.TypeErrorsInModel = data.TypeErrorsInModel ?? new List<string>();
            target.ValidationAggregationRules_31_40 = data.ResultAggregationRules;

            target.HelpType = data.ProtectionType ?? string.Empty;
            target.HelpTypeToValidate = data.ParametrizedHelpType;
            target.PrimaryTransportationDate = Date.Create(null);

            target.VehicleType = data.Sections.InvolvedVehicleInformation.TypeValue;

            return target;
        }
    }
}