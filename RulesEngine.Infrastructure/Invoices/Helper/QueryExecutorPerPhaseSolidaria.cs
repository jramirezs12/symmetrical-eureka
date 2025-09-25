using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using RulesEngine.Application.Abstractions.Services;
using RulesEngine.Application.DataSources;
using RulesEngine.Domain.Agregations.Entities;
using RulesEngine.Domain.ClaimsQueue.Entities;
using RulesEngine.Domain.ClaimsQueue.Repository;
using RulesEngine.Domain.Constants;
using RulesEngine.Domain.Constants.Entities;
using RulesEngine.Domain.DisputeProcess.Entities;
using RulesEngine.Domain.DisputeProcess.Repository;
using RulesEngine.Domain.ElectronicBillingRuleEngine.Repository;
using RulesEngine.Domain.Invoices.Entities;
using RulesEngine.Domain.Invoices.Repositories;
using RulesEngine.Domain.LegalProceedings.Entities;
using RulesEngine.Domain.LegalProceedings.Repository;
using RulesEngine.Domain.Parameters.Entities;
using RulesEngine.Domain.Parameters.Repositories;
using RulesEngine.Domain.Provider.Repository;
using RulesEngine.Domain.Research.Entities;
using RulesEngine.Domain.Research.Repository;
using RulesEngine.Domain.TransactionContracts.Entities;

namespace RulesEngine.Application.Mundial.Invoices.Helper;

public class QueryExecutorPerPhaseSolidaria : IQueryExecutorPerPhase
{
     public string Tenant => "Mundial";
    private readonly ILegalProcessesAndTransactionContractsRepository _processesContractsRepository;
    private readonly IElectronicBillingRepository _electronicBillingRepository;
    private readonly IConstantsRepository _constantsRepository;
    private readonly IParameterRepository _parameterRepository;
    private readonly IResearchRepository _researchRepository;
    private readonly IProviderRepository _providerRepository;
    private readonly IInvoiceRepository _IInvoiceRepository;
    private readonly IExternalDataLoader _loader;
    private readonly IUtilityService _utilityService;
    private readonly IClaimsQueueRepository _claimsQueueRepository;
    private readonly ILegalProceedingsRepository _legalProceedingsRepository;
    private readonly ITransactionContractsRepository _transactionContracts;
    private readonly ILegalProcessesAndTransactionContractsRepository _disputeProcessRepository;

    public QueryExecutorPerPhaseSolidaria(IConstantsRepository constantsRepository,
                                IElectronicBillingRepository electronicBillingRepository,
                                IProviderRepository providerRepository,
                                ILegalProcessesAndTransactionContractsRepository processesContractsRepository,
                                IParameterRepository parameterRepository,
                                IInvoiceRepository iInvoiceRepository,
                                IResearchRepository researchRepository,
                                IUtilityService utilityService,
                                IExternalDataLoader loader,
                                IClaimsQueueRepository claimsQueueRepository,
                                ILegalProceedingsRepository legalProceedingsRepository,
                                ITransactionContractsRepository transactionContracts,
                                ILegalProcessesAndTransactionContractsRepository disputeProcessRepository)
    {
        _constantsRepository = constantsRepository;
        _electronicBillingRepository = electronicBillingRepository;
        _providerRepository = providerRepository;
        _processesContractsRepository = processesContractsRepository;
        _parameterRepository = parameterRepository;
        _IInvoiceRepository = iInvoiceRepository;
        _researchRepository = researchRepository;
        _utilityService = utilityService;
        _loader = loader;
        _claimsQueueRepository = claimsQueueRepository;
        _legalProceedingsRepository = legalProceedingsRepository;
        _transactionContracts = transactionContracts;
        _disputeProcessRepository = disputeProcessRepository;
    }


    public async Task QueryPerStage01(InvoiceData data, string radNumber, string tenant)
    {
        string? test;
        BsonDocument[] doc;


        string[] listBusinessCase = ["MUND-002", "MUND-004", "MUND-006", "MUND-008-Copy", "MUND-011"];
        IEnumerable<Parameter> groupAgregations = await _loader.GetGroupAgregationData(listBusinessCase, tenant);

        // Get Data Electronic Billing
        data.ElectronicBilling = await _loader.GetElectronicBilling(data.InvoiceNumber, data.NitIps);
                
        // Get validation double charge rule
        Parameter doubleChargeParameter = groupAgregations.FirstOrDefault(s => s.BusinessCode == "MUND-004")!;
        test = doubleChargeParameter.Value.Replace("##NitIps##", data.NitIps).Replace("##InvoiceNumber##", data.InvoiceNumber).Replace("##RadNumber##", radNumber);
        doc = BsonSerializer.Deserialize<BsonDocument[]>(test);
        BsonDocument? differentRadicates = await _IInvoiceRepository.GetInvoiceByAggregation(doc);

        if (differentRadicates != null)
            data.InvoiceDifferentRadicates = BsonSerializer.Deserialize<InvoiceDifferentRadicates>(differentRadicates.ToJson());

        // Get sinister validation
        Parameter constitutionParameter = groupAgregations.FirstOrDefault(s => s.BusinessCode == "MUND-002")!;
        string accidentNumber = data.Sections!.ClaimDataFurips2!.AccidentNumber;
        test = constitutionParameter.Value.Replace("##AccidentNumber##", accidentNumber);
        doc = BsonSerializer.Deserialize<BsonDocument[]>(test);
        BsonDocument? constitutionDocument = await _IInvoiceRepository.GetInvoiceByAggregation(doc);

        if (constitutionDocument != null)
            data.ValidationSinister = BsonSerializer.Deserialize<SinisterAggregation>(constitutionDocument.ToJson());

        //License plate ambulance
        string? lPAmbulance = data.Sections!.MobilizationAndTransportationVictim!.PrimaryTransferAmbulancePlate;
        Parameter mTransportParameter = groupAgregations.FirstOrDefault(s => s.BusinessCode == "MUND-006")!;
        test = mTransportParameter.Value.Replace("##LicensePlate##", lPAmbulance).Replace("##RadNumber##", radNumber);
        doc = BsonSerializer.Deserialize<BsonDocument[]>(test);
        BsonDocument? multipleTransports = await _IInvoiceRepository.GetInvoiceByAggregation(doc);

        if (multipleTransports != null)
            data.MultipleTransports = BsonSerializer.Deserialize<InvoiceDifferentRadicates>(multipleTransports.ToJson());

        Parameter phoneVerificationParameter = groupAgregations.FirstOrDefault(s => s.BusinessCode == "MUND-011")!;
        if (phoneVerificationParameter != default)
            data.InvoicePhoneVerificationValue = phoneVerificationParameter.Value;

        ConstantsEntity parametrizedRuleValues = await _loader.GetConstantsByCodeAsync("0186");

        // Get Provider information
        data.ProviderData = await _providerRepository.FindOneAsync(x => x.NitIps == data.NitIps);

        Parameter rulesAggretationParameter = groupAgregations.FirstOrDefault(s => s.BusinessCode == "MUND-008-Copy")!;

        // Get Data for Rule 31-40 - fase 01
        string? lPlate = data.Sections!.InvolvedVehicleInformation!.LicensePlate;
        test = rulesAggretationParameter.Value;
        string? licensePlate = string.IsNullOrWhiteSpace(lPlate) ? null : lPlate;
        string? soatNumber = string.IsNullOrWhiteSpace(data.Sections.InvolvedVehicleInformation!.SoatNumber) ? null : data.Sections.InvolvedVehicleInformation.SoatNumber;

        CatastrophicPlaceEvent catastrophicPlaceEvent = data.Sections.CatastrophicPlaceEvent!;
        string? eventDate = catastrophicPlaceEvent != null && catastrophicPlaceEvent.EventDate != null && catastrophicPlaceEvent.EventDate.HasValue == true ? catastrophicPlaceEvent.EventDate.Value.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ss.fffZ") : null;
        string? eventDateMoreDays = catastrophicPlaceEvent != null && catastrophicPlaceEvent.EventDate.HasValue? catastrophicPlaceEvent.EventDate.Value.AddDays(1).ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ss.fffZ") : null;

        string? identificationNumber = string.IsNullOrWhiteSpace(data.Sections.VictimData!.IdentificationNumber) ? null : data.Sections.VictimData.IdentificationNumber;

        VictimData victimData = data.Sections.VictimData!;
        string? documentType = victimData?.DocumentType != null && !string.IsNullOrWhiteSpace(victimData.DocumentType.Value) ? victimData.DocumentType.Value : null;
        InvolvedVehicleInformation involvedVehicle = data.Sections.InvolvedVehicleInformation!;
        string? vehicleType = involvedVehicle?.VehicleType != null && !string.IsNullOrWhiteSpace(involvedVehicle.VehicleType.Value) ? involvedVehicle.VehicleType.Value : null;

        string? parameterRule35And36 = string.IsNullOrWhiteSpace(parametrizedRuleValues.ListType?.FirstOrDefault(x => x.Code == "Meets_Rule_35And36")?.Description) ? null : parametrizedRuleValues.ListType.FirstOrDefault(x => x.Code == "Meets_Rule_35And36")?.Description;
        string? parameterRule37And38 = string.IsNullOrWhiteSpace(parametrizedRuleValues.ListType?.FirstOrDefault(x => x.Code == "Meets_Rule_37And38")?.Description) ? null : parametrizedRuleValues.ListType.FirstOrDefault(x => x.Code == "Meets_Rule_37And38")?.Description;
        string? parameterRule39And40 = string.IsNullOrWhiteSpace(parametrizedRuleValues.ListType?.FirstOrDefault(x => x.Code == "Meets_Rule_39And40")?.Description) ? null : parametrizedRuleValues.ListType.FirstOrDefault(x => x.Code == "Meets_Rule_39And40")?.Description;

        var replacements = new Dictionary<string, string?>
            {
                { "##LicensePlate##", licensePlate },
                { "##RadNumber##", radNumber },
                { "##SoatNumber##", soatNumber },
                { "##IdentificationNumber##", identificationNumber },
            };

        foreach (var replacement in replacements)
        {
            test = test.Replace(replacement.Key, replacement.Value);
        }

        doc = BsonSerializer.Deserialize<BsonDocument[]>(test);
        BsonDocument? resultAggregation = await _IInvoiceRepository.GetInvoiceByAggregation(doc);

        if (resultAggregation != null)
        {
            data.ResultAggregationRules = BsonSerializer.Deserialize<ValidationAggregationRules_31_40>(resultAggregation.ToJson());
            data.ResultAggregationRules.ParameterRule35And36 = Convert.ToInt32(parameterRule35And36);
            data.ResultAggregationRules.ParameterRule37And38 = Convert.ToInt32(parameterRule37And38);
            data.ResultAggregationRules.ParameterRule39And40 = Convert.ToInt32(parameterRule39And40);
        }
    }

    public async Task QueryPerStage02(InvoiceData data, string radNumber, string tenant)
    {
        string? test;
        BsonDocument[] doc;

        string[] listBusinessCase = ["MUND-004", "MUND-002", "MUND-005", "MUND-006", "MUND-007",];
        IEnumerable<Parameter> groupAgregations = await _loader.GetGroupAgregationData(listBusinessCase, tenant);

        // Get Data Electronic Billing
        data.ElectronicBilling = await _loader.GetElectronicBilling(data.InvoiceNumber, data.NitIps);



        // Get validation double charge rule
        Parameter doubleChargeParameter = groupAgregations.FirstOrDefault(s => s.BusinessCode == "MUND-004")!;
        test = doubleChargeParameter.Value.Replace("##NitIps##", data.NitIps).Replace("##InvoiceNumber##", data.InvoiceNumber).Replace("##RadNumber##", radNumber);
        doc = BsonSerializer.Deserialize<BsonDocument[]>(test);
        BsonDocument? differentRadicates = await _IInvoiceRepository.GetInvoiceByAggregation(doc);

        if (differentRadicates != null)
            data.InvoiceDifferentRadicates = BsonSerializer.Deserialize<InvoiceDifferentRadicates>(differentRadicates.ToJson());

        // Get Provider information
        string providerKey = $"provider:{data.NitIps}";
        data.ProviderData = await _utilityService.GetOrSetDataCache(providerKey, async () =>
        {
            return await _providerRepository.FindOneAsync(x => x.NitIps == data.NitIps);
        }, 2);

        // Get sinister validation
        Parameter constitutionParameter = groupAgregations.FirstOrDefault(s => s.BusinessCode == "MUND-002" && s.Tenant == tenant)!;
        string accidentNumber = data.Sections!.ClaimDataFurips2!.AccidentNumber;
        test = constitutionParameter.Value.Replace("##AccidentNumber##", accidentNumber);
        doc = BsonSerializer.Deserialize<BsonDocument[]>(test);
        BsonDocument? constitutionDocument = await _IInvoiceRepository.GetInvoiceByAggregation(doc);

        if (constitutionDocument != null)
            data.ValidationSinister = BsonSerializer.Deserialize<SinisterAggregation>(constitutionDocument.ToJson());

        // Get validation double charge rule
        Parameter pObjectionParameter = groupAgregations.FirstOrDefault(s => s.BusinessCode == "MUND-005" && s.Tenant == tenant)!;
        test = pObjectionParameter.Value.Replace("##NitIps##", data.NitIps).Replace("##InvoiceNumber##", data.InvoiceNumber).Replace("##RadNumber##", radNumber);
        doc = BsonSerializer.Deserialize<BsonDocument[]>(test);
        BsonDocument? previousObjections = await _IInvoiceRepository.GetInvoiceByAggregation(doc);

        if (previousObjections != null)
            data.PreviousObjections = BsonSerializer.Deserialize<InvoiceDifferentRadicates>(previousObjections.ToJson());

        //License plate ambulance
        string? lpAmbulance = data.Sections!.MobilizationAndTransportationVictim!.PrimaryTransferAmbulancePlate;
        Parameter mTransportParameter = groupAgregations.FirstOrDefault(s => s.BusinessCode == "MUND-006" && s.Tenant == tenant)!;
        test = mTransportParameter.Value.Replace("##LicensePlate##", lpAmbulance).Replace("##RadNumber##", radNumber);
        doc = BsonSerializer.Deserialize<BsonDocument[]>(test);
        BsonDocument? multipleTransports = await _IInvoiceRepository.GetInvoiceByAggregation(doc);

        if (multipleTransports != null)
            data.MultipleTransports = BsonSerializer.Deserialize<InvoiceDifferentRadicates>(multipleTransports.ToJson());

        // Get Research Request validation
        Parameter researchParameter = groupAgregations.FirstOrDefault(s => s.BusinessCode == "MUND-007" && s.Tenant == tenant)!;
        test = researchParameter.Value.Replace("##AccidentNumber##", accidentNumber).Replace("##RadNumber##", radNumber);
        doc = BsonSerializer.Deserialize<BsonDocument[]>(test);
        BsonDocument? researchResults = await _IInvoiceRepository.GetInvoiceByAggregation(doc);

        if (researchResults != null)
            data.ResearchRequest = BsonSerializer.Deserialize<ResearchRequest>(researchResults.ToJson());

        //ConstantsEntity parametrizedHelpType = await _constantsRepository.FindOneAsync(s => s.BusinessCode == "0207");
        ConstantsEntity parametrizedHelpType = await _loader.GetConstantsByCodeAsync("0207");
        data.ParametrizedHelpType = parametrizedHelpType != null ? parametrizedHelpType.ListType?.Where(x => x.State == true).Select(c => c.Description).FirstOrDefault()! : null!;

        string? eventDate = data.Sections.CatastrophicPlaceEvent != null && data.Sections.CatastrophicPlaceEvent.EventDate != null && data.Sections.CatastrophicPlaceEvent.EventDate.HasValue == true ? data.Sections.CatastrophicPlaceEvent.EventDate.Value.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ss.fffZ") : null;
        string? eventDate2 = data.Sections.CatastrophicPlaceEvent != null && data.Sections.CatastrophicPlaceEvent.EventDate != null && data.Sections.CatastrophicPlaceEvent.EventDate.HasValue == true ? data.Sections.CatastrophicPlaceEvent.EventDate.Value.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssZ") : null;
        string[] eventDates = { eventDate, eventDate2 };

        var dateFilters = eventDates.Select(date => Builders<ResearchEntity>.Filter.Eq("Request.eventdate", date)).ToList();

        var researchFilter = Builders<ResearchEntity>.Filter.Eq(x => x.Request!.LicensePlate, data.Sections.InvolvedVehicleInformation!.LicensePlate) &
                             Builders<ResearchEntity>.Filter.Or(dateFilters);

        var researchs = await _researchRepository.FilterBy(researchFilter);
        data.ResearchData = researchs.ToArray();

        FilterDefinition<ClaimsQueueEntity> filterQueue = Builders<ClaimsQueueEntity>.Filter.Eq(x => x.RadNumber, radNumber);

        data.ClaimsQueue = await _claimsQueueRepository.FindOneAsync(filterQueue);

        FilterDefinition<LegalProceedingsEntity> legalFilter = Builders<LegalProceedingsEntity>.Filter.Eq(x => x.ClaimantId, data.NitIps) &
                                                               Builders<LegalProceedingsEntity>.Filter.Eq(x => x.InvoiceProcess, data.InvoiceNumber);

        data.LegalProceedings = await _legalProceedingsRepository.FilterBy(legalFilter);


        FilterDefinition<ConsolidatedTransactionContracts> filterContracts = Builders<ConsolidatedTransactionContracts>.Filter.Eq(x => x.ClaimantId, data.NitIps) &
                                                                             Builders<ConsolidatedTransactionContracts>.Filter.Eq(x => x.ClaimNumber, data.InvoiceNumber);

        data.ConsolidatedTransactionContracts = await _transactionContracts.FilterBy(filterContracts);

        FilterDefinition<DisputeProcessEntity> processFilter = Builders<DisputeProcessEntity>.Filter.Eq(x => x.ClaimantId, data.NitIps) &
                                                         Builders<DisputeProcessEntity>.Filter.Eq(x => x.InvoiceNumber, data.InvoiceNumber);

        // procesos legales y contratos de transacción
        data.LegalProcessesAndTransactionContractsParameters = await _processesContractsRepository.FilterBy(processFilter);
    }
}