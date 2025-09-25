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
using System.Collections.Concurrent;
using static RulesEngine.Domain.RulesEntities.Solidaria.Entities.InvoiceToCheckSolidaria;
using Date = RulesEngine.Domain.ValueObjects.Date;

namespace RulesEngine.Application.InputSources;

public class LoadInputSourcesSolidaria
{
    private readonly IInvoiceRepository _InvoiceRepository;
    private readonly InputSourcesModel _sourcesModel;
    private readonly InvoiceData _Data;
    private InvoiceToCheckSolidaria _Entity;
    private string _Stage;
    private readonly IUtilityService _utilityService;

    public LoadInputSourcesSolidaria(InvoiceToCheckSolidaria invoice, InvoiceData data, string stage,
        InputSourcesModel inputSources, IInvoiceRepository invoiceRepository, IUtilityService utilityService)
    {
        _InvoiceRepository = invoiceRepository;
        _Entity = invoice;
        _Stage = stage;
        _Data = data;
        _sourcesModel = inputSources;
        _utilityService = utilityService;
    }

    public async Task<InvoiceToCheckSolidaria> Create()
    {
        _Entity.IpsNit = _Data.NitIps;
        _Entity.LicensePlate = _Data.Sections?.InvolvedVehicleInformation?.LicensePlate!;
        _Entity.SoatNumber = _Data.Sections?.InvolvedVehicleInformation?.SoatNumber!;
        _Entity.VictimId = _Data.Sections?.VictimData?.IdentificationNumber!;
        _Entity.InvoiceNumber = _Data.InvoiceNumber;
        _Entity.LicensePlateAmbulance = _Data.Sections?.MobilizationAndTransportationVictim?.PrimaryTransferAmbulancePlate!;


        var loadedData = new ConcurrentDictionary<string, object>();
        var tasks = new List<Task>();

        //var findDuplicatedTask = FindDuplicated();
        //tasks.Add(findDuplicatedTask);


        if (_Stage == "fase_02")
        {
            tasks.Add(Task.Run(async () => loadedData["ipsNitList"] = await _utilityService.GetOrSetDataCache("ipsNitList", _sourcesModel.IpsNitList.Create, 72)));
            tasks.Add(Task.Run(async () => loadedData["invoiceNumber"] = await _utilityService.GetOrSetDataCache("invoiceNumber", _sourcesModel.InvoiceNumber.Create, 72)));
            tasks.Add(Task.Run(async () => loadedData["usersAllowedToClaim"] = await _utilityService.GetOrSetDataCache("usersAllowedToClaim", _sourcesModel.AllowedUsers.Create, 72)));
            tasks.Add(Task.Run(async () => loadedData["serviceCodes"] = await _utilityService.GetOrSetDataCache("serviceCodes", _sourcesModel.ServiceCodes.Create, 72)));
            tasks.Add(Task.Run(async () => loadedData["atypicalEvent"] = await _utilityService.GetOrSetDataCache("atypicalEvent", _sourcesModel.AtypicalEvent.Create, 72)));
            tasks.Add(Task.Run(async () => loadedData["ambulanceControl"] = await _utilityService.GetOrSetDataCache("ambulanceControl", _sourcesModel.AmbulanceControl.Create, 72)));

            await Task.WhenAll(tasks);
            _Entity.IpsNitList = ((IEnumerable<IpsNitFile>)loadedData["ipsNitList"]).Where(x => x.NitIps == _Entity.IpsNit).ToList();
            _Entity.InvoiceNumberFile = ((IEnumerable<InvoiceNumberFile>)loadedData["invoiceNumber"]).Where(x => x.InvoiceNumber == _Entity.InvoiceNumber).ToList();
            _Entity.AllowedUsers = (List<AllowedUser>)loadedData["usersAllowedToClaim"];
            _Entity.ServiceCodeFiles = (List<ServiceCodeFile>)loadedData["serviceCodes"];
            _Entity.AtypicalEvent = ((IEnumerable<AtypicalEvent>)loadedData["atypicalEvent"]).Where(x => x.VictimId == _Entity.VictimId ||
                            x.SoatNumber == _Entity.SoatNumber ||
                            x.LicensePlate == _Entity.LicensePlate).ToList();
            _Entity.AmbulanceControl = ((IEnumerable<AmbulanceControl>)loadedData["ambulanceControl"])
                .Where(x => x.LicensePlate == _Entity.LicensePlateAmbulance).ToList();
        }
        else // fase_01 y fase_03
        {
            tasks.Add(Task.Run(async () => loadedData["ipsPhoneVerification"] = await _utilityService.GetOrSetDataCache("ipsPhoneVerification", _sourcesModel.IpsPhoneVerification.Create, 72)));
            tasks.Add(Task.Run(async () => loadedData["catastrophicEvent"] = await _utilityService.GetOrSetDataCache("catastrophicEvent", _sourcesModel.CatastrophicEvent.Create, 72)));
            tasks.Add(Task.Run(async () => loadedData["ambulanceControl"] = await _utilityService.GetOrSetDataCache("ambulanceControl", _sourcesModel.AmbulanceControl.Create, 72)));
            tasks.Add(Task.Run(async () => loadedData["ipsInvestigation"] = await _utilityService.GetOrSetDataCache("ipsInvestigation", _sourcesModel.IpsInvestigation.Create, 72)));
            tasks.Add(Task.Run(async () => loadedData["atypicalEvent"] = await _utilityService.GetOrSetDataCache("atypicalEvent", _sourcesModel.AtypicalEvent.Create, 72)));
            tasks.Add(Task.Run(async () => loadedData["fraudulentIps"] = await _utilityService.GetOrSetDataCache("fraudulentIps", _sourcesModel.FraudulentIps.Create, 72)));

            await Task.WhenAll(tasks);
            _Entity.FraudulentIps = ((IEnumerable<FraudulentIps>)loadedData["fraudulentIps"]).FirstOrDefault(x => x.IpsNit == _Entity.IpsNit);
            _Entity.IpsInvestigation = ((IEnumerable<IpsInvestigation>)loadedData["ipsInvestigation"]).FirstOrDefault(x => x.IpsNit == _Entity.IpsNit);
            _Entity.CatastrophicEvent = ((IEnumerable<CatastrophicEvent>)loadedData["catastrophicEvent"]).Where(x => x.SoatNumber == _Entity.SoatNumber).ToList();
            _Entity.IpsPhoneVerification = ((IEnumerable<IpsPhoneVerification>)loadedData["ipsPhoneVerification"]).Where(x => x.NitIps == _Entity.IpsNit).ToList();
            _Entity.AtypicalEvent = ((IEnumerable<AtypicalEvent>)loadedData["atypicalEvent"]).Where(x => x.VictimId == _Entity.VictimId ||
                            x.SoatNumber == _Entity.SoatNumber || x.LicensePlate == _Entity.LicensePlate).ToList();
            _Entity.AmbulanceControl = ((IEnumerable<AmbulanceControl>)loadedData["ambulanceControl"])
                .Where(x => x.LicensePlate == _Entity.LicensePlateAmbulance).ToList();
        }

        _Entity = FillInformation(_Data, _Entity);
        await FindDuplicated();
        return _Entity;
    }

    private async Task FindDuplicated()
    {
        bool searchInvoices = false;

        FilterDefinition<Invoice> filter = Builders<Invoice>.Filter.Ne(x => x.RadNumber, _Entity.RadNumber) &
                                           Builders<Invoice>.Filter.Eq("Detail.moduleName", _Entity.ModuleName) &
                                           Builders<Invoice>.Filter.Exists(x => x.WorkflowData);

        if (!string.IsNullOrWhiteSpace(_Entity.SoatNumber) &&
           !string.IsNullOrWhiteSpace(_Entity.VictimId))
        {
            searchInvoices = true;
            filter &= Builders<Invoice>.Filter.Eq("Detail.forms.sections.data.soatnumber", _Entity.SoatNumber) |
                       Builders<Invoice>.Filter.Eq("Detail.forms.sections.data.identificationnumber", _Entity.VictimId);
        }
        else if (!string.IsNullOrWhiteSpace(_Entity.SoatNumber) &&
                  string.IsNullOrWhiteSpace(_Entity.VictimId))
        {
            searchInvoices = true;
            filter &= Builders<Invoice>.Filter.Eq("Detail.forms.sections.data.soatnumber", _Entity.SoatNumber);
        }
        else if (string.IsNullOrWhiteSpace(_Entity.SoatNumber) &&
                !string.IsNullOrWhiteSpace(_Entity.VictimId))
        {
            searchInvoices = true;
            filter &= Builders<Invoice>.Filter.Eq("Detail.forms.sections.data.identificationnumber", _Entity.VictimId);
        }

        if (searchInvoices)
        {
            var duplicated = await _InvoiceRepository.FilterByAsync(filter, new FindOptions<Invoice>());

            var detail = duplicated.Select(x => x.Detail.ToBsonDocument());

            var data = detail.SelectMany(x => x["forms"].AsBsonArray)
                                .Where(y => y["formName"].AsString == "furips1")
                                .Select(z => z["sections"].AsBsonArray
                                    .Where(s => s["sectionName"].AsString == "victimData"
                                        || s["sectionName"].AsString == "involvedVehicleInformation"
                                        || s["sectionName"].AsString == "catastrophicPlaceEvent"));

            _Entity.DuplicatedInvoices.AddRange
            (
                data.Select(x =>
                {
                    var victimData = x.FirstOrDefault(v => v["sectionName"] == "victimData")?["data"] as BsonDocument;
                    var catastrophicPlaceEvent = x.FirstOrDefault(v => v["sectionName"] == "catastrophicPlaceEvent")?["data"] as BsonDocument;
                    var involvedVehicleInformation = x.FirstOrDefault(v => v["sectionName"] == "involvedVehicleInformation")?["data"] as BsonDocument;
                    var ambulanceInformation = x.FirstOrDefault(v => v["sectionName"] == "mobilizationAndTransportationVictim")?["data"] as BsonDocument;
                    var informationProvider = x.FirstOrDefault(v => v["sectionName"] == "healthCareServiceProvider")?["data"] as BsonDocument;

                    return new DuplicatedInvoice()
                    {
                        VictimId = victimData != null ? Check(victimData, "identificationnumber") : null,
                        DocumentType = victimData != null && !string.IsNullOrEmpty(Check(victimData, "documenttype", "value")) ? Enum.Parse<DocumentTypeEnum>(Check(victimData, "documenttype", "value").ToUpper()).ToString() : string.Empty,
                        EventDate = catastrophicPlaceEvent != null && !string.IsNullOrEmpty(Check(catastrophicPlaceEvent, "eventdate")) ? Convert.ToDateTime(Check(catastrophicPlaceEvent, "eventdate")) : null,
                        LicensePlate = involvedVehicleInformation != null ? Check(involvedVehicleInformation, "licenseplate") : null,
                        SoatNumber = involvedVehicleInformation != null ? Check(involvedVehicleInformation, "soatnumber") : null,
                        VehicleType = involvedVehicleInformation != null ? Check(involvedVehicleInformation, "vehicletype", "value") : null,
                        HabilitationCodeProvider = informationProvider != null ? Check(informationProvider, "healthcareserviceprovidercode") : null,
                        LicencePlateAmbulance = ambulanceInformation != null ? Check(ambulanceInformation, "primarytransferambulanceplate") : null
                    };
                })
            );
            var res = _Entity.DuplicatedInvoices;
        }
    }

    private static string Check(BsonDocument document, string field)
    {
        if (document.Contains(field) &&
           document[field] is not BsonNull)
        {
            return document[field].BsonType switch
            {
                BsonType.String => document[field].AsString,
                BsonType.Int32 => document[field].AsInt32.ToString(),
                BsonType.Int64 => document[field].AsInt64.ToString(),
                BsonType.DateTime => document[field].ToUniversalTime().ToString(),
                _ => string.Empty
            };
        }
        return string.Empty;
    }

    private static string Check(BsonDocument document, string field, string field2)
    {
        if (document.Contains(field))
            if (document[field] is not BsonNull)
                if (!document[field].IsString)
                    if (document[field][field2] is not BsonNull)
                        return document[field][field2].AsString;
        return string.Empty;
    }

    private static InvoiceToCheckSolidaria FillInformation(InvoiceData data, InvoiceToCheckSolidaria dataTofill)
    {
        dataTofill.IpsNit = !string.IsNullOrEmpty(data.NitIps) ? data.NitIps! : string.Empty;
        dataTofill.ModuleName = !string.IsNullOrEmpty(data.ModuleName) ? data.ModuleName : string.Empty;
        dataTofill.IpsNitRips = !string.IsNullOrEmpty(data.NitIps!) ? data.NitIps! : string.Empty;
        dataTofill.SoatNumber = !string.IsNullOrEmpty(data.Sections?.InvolvedVehicleInformation?.SoatNumber) ? data.Sections.InvolvedVehicleInformation.SoatNumber! : string.Empty;
        dataTofill.LicensePlate = !string.IsNullOrEmpty(data.Sections!.InvolvedVehicleInformation!.LicensePlate) ? data.Sections!.InvolvedVehicleInformation!.LicensePlate : string.Empty;
        dataTofill.VictimId = !string.IsNullOrEmpty(data.Sections!.VictimData!.IdentificationNumber) ? data.Sections!.VictimData!.IdentificationNumber : string.Empty;
        dataTofill.DocumentType = data.Sections!.VictimData!.DocumentType! != default ? data.Sections!.VictimData!.DocumentType!.Value : string.Empty;
        dataTofill.InvoiceNumberF1 = data.Sections.ClaimDataFurips1 != null && !string.IsNullOrEmpty(data.Sections!.ClaimDataFurips1!.InvoiceNumber) ? data.Sections!.ClaimDataFurips1!.InvoiceNumber : string.Empty;
        dataTofill.InvoiceNumberF2 = data.Sections.ClaimDataFurips2 != null && !string.IsNullOrEmpty(data.Sections.ClaimDataFurips2!.ReclaimInvoiceNumber) ? data.Sections.ClaimDataFurips2!.ReclaimInvoiceNumber : string.Empty;

        dataTofill.EventDate = Date.ConvertToUtcFormattedDate(data.Sections?.CatastrophicPlaceEvent?.EventDate.ToString() ?? null!);
        dataTofill.DeathDate = Date.ConvertToUtcFormattedDate(data.Sections?.VictimData?.DeadDate.ToString() ?? null!);
        dataTofill.ClaimDate = Date.ConvertToUtcFormattedDate(data.Sections?.ClaimDataFurips1?.Notificationdate.ToString() ?? null!);
        dataTofill.InvoiceDate = Date.ConvertToUtcFormattedDate(data.DateCreation?.ToString() ?? null!);
        dataTofill.IncomeDate = Date.ConvertToUtcFormattedDate(data.Sections?.MedicalCertification?.MedicalCertificationIncomeDate.ToString() ?? null!);
        dataTofill.EgressDate = Date.ConvertToUtcFormattedDate(data.Sections?.MedicalCertification?.MedicalCertificationEgressDate.ToString() ?? null!);
        dataTofill.InvoiceMAOSDate = Date.ConvertToUtcFormattedDate(data.Sections!.Invoice!.ServiceList.Where(c => c.MosData != null).Select(x => x?.MosData?.ProviderInvoiceDate).FirstOrDefault());
        dataTofill.IpsNitF2 = data.Sections.ClaimDataFurips2 != null && !string.IsNullOrEmpty(data.Sections.ClaimDataFurips2.ClaimProviderNit!) ? data.Sections.ClaimDataFurips2.ClaimProviderNit! : string.Empty;
        dataTofill.InvoiceValue = Currency.Create(data.Sections.ClaimDataFurips2!.ClaimInvoiceValue.ToString()!);
        dataTofill.BilledMedicalExpenses = Currency.Create(data.Sections.CoveragesClaimed!.TotalBilledMedicalExpenses.ToString()!);
        dataTofill.BilledTransportation = Currency.Create(data.Sections.CoveragesClaimed!.TotalBilledTransportation.ToString()!);
        dataTofill.InvoiceNumberFurips = !string.IsNullOrEmpty(data.InvoiceNumber) ? data.InvoiceNumber : string.Empty;
        dataTofill.IpsNitFE = data.ElectronicBilling != null && !string.IsNullOrEmpty(data.ElectronicBilling.NitIps) ? data.ElectronicBilling.NitIps : string.Empty;
        dataTofill.InvoiceNumberFE = data.ElectronicBilling != null && !string.IsNullOrEmpty(data.ElectronicBilling.InvoiceNumber) ? data.ElectronicBilling.InvoiceNumber : string.Empty;
        dataTofill.InvoiceDifferentRadicates = data.InvoiceDifferentRadicates != null ? data.InvoiceDifferentRadicates : null!;
        dataTofill.LicensePlateAmbulance = data.Sections.MobilizationAndTransportationVictim != null ? data.Sections.MobilizationAndTransportationVictim.PrimaryTransferAmbulancePlate : string.Empty;
        dataTofill.ProviderData = data.ProviderData!;
        dataTofill.AlertsEncountered = data.Alerts! != default ? data.Alerts : null!;
        dataTofill.Research = data.ResearchData != default ? data.ResearchData : null!;
        dataTofill.InvestigationResponseDate = data.ResearchData != default ? Date.ConvertToUtcFormattedDate(data.ResearchData.Where(x => x.OriginModule == data.ModuleName).Max(x => x.ResponseDate).ToString() ?? null!) : Date.Create(null!);
        long? approvedValue = data.Sections.Invoice.GlossTotals.Select(x => x.ApprovedValue).FirstOrDefault();
        long? glossValue = data.Sections.Invoice.GlossTotals.Select(x => x.GlossValue).FirstOrDefault();
        dataTofill.TotalAuthorizedValue = Currency.Create((approvedValue ?? 0).ToString());
        dataTofill.TotalGlossedValue = Currency.Create((glossValue ?? 0).ToString());
        dataTofill.ProcessAndContracts = data.LegalProcessesAndTransactionContractsParameters != null ? data.LegalProcessesAndTransactionContractsParameters!.Where(x => x.InvoiceNumber == data.InvoiceNumber && x.ClaimantId == data.NitIps).ToList() : null!;
        dataTofill.SinisterAggretation = data.ValidationSinister! != null ? data.ValidationSinister! : null!;
        dataTofill.PreviousObjections = data.PreviousObjections != null ? data.PreviousObjections! : null!;
        dataTofill.MultipleTransposrts = data.MultipleTransports != null ? data.MultipleTransports! : null!;
        dataTofill.ResearchRequest = data.ResearchRequest != null ? data.ResearchRequest! : null!;
        dataTofill.ListServiceCodes = data.Sections.Invoice.ServiceList.Where(x => x.ServiceDescription != null).Select(x => x.ServiceDescription!.Value).ToList();
        dataTofill.NotNullErrorsInModel = data.NotNullErrorsInModel != null ? data.NotNullErrorsInModel! : null!;
        dataTofill.TypeErrorsInModel = data.TypeErrorsInModel != null ? data.TypeErrorsInModel! : null!;
        dataTofill.ValidationAggregationRules_31_40 = data.ResultAggregationRules != null ? data.ResultAggregationRules! : null!;
        dataTofill.InvoicePhoneVerificationValue = Currency.Create(data.InvoicePhoneVerificationValue);
        dataTofill.UserClaim = data.ClaimsQueue != null ? data.ClaimsQueue.UserAccount : null;
        dataTofill.HelpType = data.HelpType;
        dataTofill.HelpTypeToValidate = data.ParametrizedHelpType;
        dataTofill.PrimaryTransportationDate = Date.Create(null);
        dataTofill.VehicleType = data.Sections.InvolvedVehicleInformation.VehicleType != null ? data.Sections.InvolvedVehicleInformation.VehicleType.Value : null;

        return dataTofill;
    }
}