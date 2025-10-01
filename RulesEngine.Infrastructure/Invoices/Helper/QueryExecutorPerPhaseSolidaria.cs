using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RulesEngine.Application.Mundial.Invoices.Helper
{
    // Este executor se registra para el tenant "Solidaria"
    public class QueryExecutorPerPhaseSolidaria : IQueryExecutorPerPhaseSolidaria
    {
        public string Tenant => "Solidaria";

        private readonly ILegalProcessesAndTransactionContractsRepository _processesContractsRepository;
        private readonly IElectronicBillingRepository _electronicBillingRepository;
        private readonly IConstantsRepository _constantsRepository;
        private readonly IParameterRepository _parameterRepository;
        private readonly IResearchRepository _researchRepository;
        private readonly IProviderRepository _providerRepository;
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly IExternalDataLoader _loader;
        private readonly IUtilityService _utilityService;
        private readonly IClaimsQueueRepository _claimsQueueRepository;
        private readonly ILegalProceedingsRepository _legalProceedingsRepository;
        private readonly ITransactionContractsRepository _transactionContracts;
        private readonly ILegalProcessesAndTransactionContractsRepository _disputeProcessRepository;

        public QueryExecutorPerPhaseSolidaria(
            IConstantsRepository constantsRepository,
            IElectronicBillingRepository electronicBillingRepository,
            IProviderRepository providerRepository,
            ILegalProcessesAndTransactionContractsRepository processesContractsRepository,
            IParameterRepository parameterRepository,
            IInvoiceRepository invoiceRepository,
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
            _invoiceRepository = invoiceRepository;
            _researchRepository = researchRepository;
            _utilityService = utilityService;
            _loader = loader;
            _claimsQueueRepository = claimsQueueRepository;
            _legalProceedingsRepository = legalProceedingsRepository;
            _transactionContracts = transactionContracts;
            _disputeProcessRepository = disputeProcessRepository;
        }

        // Firma de la interfaz (InvoiceData). Internamente casteamos a SolidariaInvoiceData.
        public async Task QueryPerStage01(SolidariaInvoiceData data, string radNumber, string tenant)
        {
            if (data is not SolidariaInvoiceData solidaria)
                throw new InvalidOperationException("Se esperaba SolidariaInvoiceData.");
            await ExecuteSolidariaStage01(solidaria, radNumber, tenant);
        }

        public async Task QueryPerStage02(SolidariaInvoiceData data, string radNumber, string tenant)
        {
            if (data is not SolidariaInvoiceData solidaria)
                throw new InvalidOperationException("Se esperaba SolidariaInvoiceData.");
            await ExecuteSolidariaStage02(solidaria, radNumber, tenant);
        }

        // ================== LÓGICA FASE 01 ==================
        private async Task ExecuteSolidariaStage01(SolidariaInvoiceData data, string radNumber, string tenant)
        {
            string[] listBusinessCase = ["SOL-002", "SOL-004", "SOL-006", "SOL-008-Copy", "SOL-011"];
            var groupAggregations = await _loader.GetGroupAgregationData(listBusinessCase, tenant);

            string providerIdNumber = data.Provider?.IdNumber ?? string.Empty;
            data.ElectronicBilling = await _loader.GetElectronicBilling(data.InvoiceNumber, providerIdNumber);

            // SOL-004 Doble cobro
            var doubleChargeParameter = groupAggregations.FirstOrDefault(x => x.BusinessCode == "SOL-004");
            if (doubleChargeParameter != null)
            {
                string q = ReplaceSafe(doubleChargeParameter.Value, new()
                {
                    { "##NitIps##", providerIdNumber },
                    { "##InvoiceNumber##", data.InvoiceNumber },
                    { "##RadNumber##", radNumber }
                });
                var pipeline = BsonSerializer.Deserialize<BsonDocument[]>(q);
                var result = await _invoiceRepository.GetInvoiceByAggregation(pipeline);
                if (result != null)
                    data.InvoiceDifferentRadicates = BsonSerializer.Deserialize<InvoiceDifferentRadicates>(result);
            }

            // SOL-002 Validación siniestro
            var sinisterParameter = groupAggregations.FirstOrDefault(x => x.BusinessCode == "SOL-002");
            if (sinisterParameter != null)
            {
                string accidentNumber = data.GetAccidentNumber(); // Confirmar si debe usarse Soat.SIRASFilingNumber
                string q = sinisterParameter.Value.Replace("##AccidentNumber##", accidentNumber);
                var pipeline = BsonSerializer.Deserialize<BsonDocument[]>(q);
                var result = await _invoiceRepository.GetInvoiceByAggregation(pipeline);
                if (result != null)
                    data.ValidationSinister = BsonSerializer.Deserialize<SinisterAggregation>(result);
            }

            // SOL-006 Múltiples transportes
            var multiTransportParam = groupAggregations.FirstOrDefault(x => x.BusinessCode == "SOL-006");
            if (multiTransportParam != null)
            {
                string plate = data.GetPrimaryAmbulancePlate();
                string q = ReplaceSafe(multiTransportParam.Value, new()
                {
                    { "##LicensePlate##", plate },
                    { "##RadNumber##", radNumber }
                });
                var pipeline = BsonSerializer.Deserialize<BsonDocument[]>(q);
                var result = await _invoiceRepository.GetInvoiceByAggregation(pipeline);
                if (result != null)
                    data.MultipleTransports = BsonSerializer.Deserialize<InvoiceDifferentRadicates>(result);
            }

            // SOL-011 Verificación telefónica
            var phoneParam = groupAggregations.FirstOrDefault(x => x.BusinessCode == "SOL-011");
            if (phoneParam != null)
                data.InvoicePhoneVerificationValue = phoneParam.Value;

            // Reglas 31-40 (si aplica aún MUND-008-Copy para este tenant)
            var rulesParam = groupAggregations.FirstOrDefault(x => x.BusinessCode == "MUND-008-Copy");
            var constantsRules = await _loader.GetConstantsByCodeAsync("0186");
            if (rulesParam != null)
            {
                string tpl = ReplaceSafe(rulesParam.Value, new()
                {
                    { "##LicensePlate##", data.GetVehiclePlate() },
                    { "##RadNumber##", radNumber },
                    { "##SoatNumber##", data.GetSoatPolicyNumber() },
                    { "##IdentificationNumber##", data.GetFirstVictimId() }
                });

                var pipeline = BsonSerializer.Deserialize<BsonDocument[]>(tpl);
                var result = await _invoiceRepository.GetInvoiceByAggregation(pipeline);
                if (result != null)
                {
                    data.ResultAggregationRules = BsonSerializer.Deserialize<ValidationAggregationRules_31_40>(result);
                    data.ResultAggregationRules.ParameterRule35And36 = constantsRules.TryGetInt("Meets_Rule_35And36");
                    data.ResultAggregationRules.ParameterRule37And38 = constantsRules.TryGetInt("Meets_Rule_37And38");
                    data.ResultAggregationRules.ParameterRule39And40 = constantsRules.TryGetInt("Meets_Rule_39And40");
                }
            }

            // ProviderData (catálogo interno)
            if (data.ProviderData == null && !string.IsNullOrWhiteSpace(providerIdNumber))
                data.ProviderData = await _providerRepository.FindOneAsync(p => p.NitIps == providerIdNumber);
        }

        // ================== LÓGICA FASE 02 ==================
        private async Task ExecuteSolidariaStage02(SolidariaInvoiceData data, string radNumber, string tenant)
        {
            string[] listBusinessCase = ["MUND-004", "MUND-002", "MUND-005", "MUND-006", "MUND-007"];
            var groupAggregations = await _loader.GetGroupAgregationData(listBusinessCase, tenant);

            string providerIdNumber = data.Provider?.IdNumber ?? string.Empty;
            data.ElectronicBilling = await _loader.GetElectronicBilling(data.InvoiceNumber, providerIdNumber);

            // MUND-004 Doble cobro
            var doubleCharge = groupAggregations.FirstOrDefault(x => x.BusinessCode == "MUND-004");
            if (doubleCharge != null)
            {
                string q = ReplaceSafe(doubleCharge.Value, new()
                {
                    { "##NitIps##", providerIdNumber },
                    { "##InvoiceNumber##", data.InvoiceNumber },
                    { "##RadNumber##", radNumber }
                });
                var pipeline = BsonSerializer.Deserialize<BsonDocument[]>(q);
                var result = await _invoiceRepository.GetInvoiceByAggregation(pipeline);
                if (result != null)
                    data.InvoiceDifferentRadicates = BsonSerializer.Deserialize<InvoiceDifferentRadicates>(result);
            }

            // Cache provider
            string providerKey = $"provider:{providerIdNumber}";
            data.ProviderData = await _utilityService.GetOrSetDataCache(providerKey, async () =>
            {
                if (string.IsNullOrWhiteSpace(providerIdNumber)) return null;
                return await _providerRepository.FindOneAsync(x => x.NitIps == providerIdNumber);
            }, 2);

            // MUND-002 Validación siniestro
            var sinisterParam = groupAggregations.FirstOrDefault(x => x.BusinessCode == "MUND-002" && x.Tenant == tenant);
            if (sinisterParam != null)
            {
                string accidentNumber = data.GetAccidentNumber();
                string q = sinisterParam.Value.Replace("##AccidentNumber##", accidentNumber);
                var pipeline = BsonSerializer.Deserialize<BsonDocument[]>(q);
                var result = await _invoiceRepository.GetInvoiceByAggregation(pipeline);
                if (result != null)
                    data.ValidationSinister = BsonSerializer.Deserialize<SinisterAggregation>(result);
            }

            // MUND-005 Objeciones previas
            var prevObjParam = groupAggregations.FirstOrDefault(x => x.BusinessCode == "MUND-005" && x.Tenant == tenant);
            if (prevObjParam != null)
            {
                string q = ReplaceSafe(prevObjParam.Value, new()
                {
                    { "##NitIps##", providerIdNumber },
                    { "##InvoiceNumber##", data.InvoiceNumber },
                    { "##RadNumber##", radNumber }
                });
                var pipeline = BsonSerializer.Deserialize<BsonDocument[]>(q);
                var result = await _invoiceRepository.GetInvoiceByAggregation(pipeline);
                if (result != null)
                    data.PreviousObjections = BsonSerializer.Deserialize<InvoiceDifferentRadicates>(result);
            }

            // MUND-006 Múltiples transportes
            var multiTransport = groupAggregations.FirstOrDefault(x => x.BusinessCode == "MUND-006" && x.Tenant == tenant);
            if (multiTransport != null)
            {
                string plate = data.GetPrimaryAmbulancePlate();
                string q = ReplaceSafe(multiTransport.Value, new()
                {
                    { "##LicensePlate##", plate },
                    { "##RadNumber##", radNumber }
                });
                var pipeline = BsonSerializer.Deserialize<BsonDocument[]>(q);
                var result = await _invoiceRepository.GetInvoiceByAggregation(pipeline);
                if (result != null)
                    data.MultipleTransports = BsonSerializer.Deserialize<InvoiceDifferentRadicates>(result);
            }

            // MUND-007 Solicitud investigación
            var researchParam = groupAggregations.FirstOrDefault(x => x.BusinessCode == "MUND-007" && x.Tenant == tenant);
            if (researchParam != null)
            {
                string accidentNumber = data.GetAccidentNumber();
                string q = ReplaceSafe(researchParam.Value, new()
                {
                    { "##AccidentNumber##", accidentNumber },
                    { "##RadNumber##", radNumber }
                });
                var pipeline = BsonSerializer.Deserialize<BsonDocument[]>(q);
                var result = await _invoiceRepository.GetInvoiceByAggregation(pipeline);
                if (result != null)
                    data.ResearchRequest = BsonSerializer.Deserialize<ResearchRequest>(result);
            }

            // Parametrización tipo de ayuda (0207)
            var helpType = await _loader.GetConstantsByCodeAsync("0207");
            data.ParametrizedHelpType = helpType?.ListType?.FirstOrDefault(x => x.State)?.Description ?? string.Empty;

            // Research (placa + fecha de evento)
            DateTime? eventDate = data.GetEventDate();
            string? iso1 = eventDate.ToUTCString("yyyy-MM-ddTHH:mm:ss.fffZ");
            string? iso2 = eventDate.ToUTCString("yyyy-MM-ddTHH:mm:ssZ");
            var dateValues = new[] { iso1, iso2 }.Where(v => !string.IsNullOrWhiteSpace(v)).ToArray();

            string plateFilter = data.GetVehiclePlate() ?? string.Empty;
            var dateFilters = dateValues.Select(v => Builders<ResearchEntity>.Filter.Eq("Request.eventdate", v)).ToList();

            var researchFilter =
                Builders<ResearchEntity>.Filter.Eq("Request.licenseplate", plateFilter) &
                (dateFilters.Count == 0
                    ? Builders<ResearchEntity>.Filter.Empty
                    : Builders<ResearchEntity>.Filter.Or(dateFilters));

            var researchs = await _researchRepository.FilterBy(researchFilter);
            data.ResearchData = researchs?.ToArray() ?? Array.Empty<ResearchEntity>();

            // ClaimsQueue
            var queueFilter = Builders<ClaimsQueueEntity>.Filter.Eq(x => x.RadNumber, radNumber);
            data.ClaimsQueue = await _claimsQueueRepository.FindOneAsync(queueFilter);

            // Legal Proceedings
            var legalFilter = Builders<LegalProceedingsEntity>.Filter.Eq(x => x.ClaimantId, providerIdNumber) &
                              Builders<LegalProceedingsEntity>.Filter.Eq(x => x.InvoiceProcess, data.InvoiceNumber);
            data.LegalProceedings = await _legalProceedingsRepository.FilterBy(legalFilter);

            // Transaction Contracts
            var contractsFilter = Builders<ConsolidatedTransactionContracts>.Filter.Eq(x => x.ClaimantId, providerIdNumber) &
                                  Builders<ConsolidatedTransactionContracts>.Filter.Eq(x => x.ClaimNumber, data.InvoiceNumber);
            data.ConsolidatedTransactionContracts = await _transactionContracts.FilterBy(contractsFilter);

            // Dispute / Procesos
            var disputeFilter = Builders<DisputeProcessEntity>.Filter.Eq(x => x.ClaimantId, providerIdNumber) &
                                Builders<DisputeProcessEntity>.Filter.Eq(x => x.InvoiceNumber, data.InvoiceNumber);
            data.LegalProcessesAndTransactionContractsParameters = await _processesContractsRepository.FilterBy(disputeFilter);
        }

        // Reemplazos seguros (null-safe)
        private static string ReplaceSafe(string template, Dictionary<string, string?> replacements)
        {
            if (string.IsNullOrEmpty(template)) return string.Empty;
            foreach (var kv in replacements)
                template = template.Replace(kv.Key, kv.Value ?? string.Empty);
            return template;
        }
    }

    // ================== EXTENSIONES ==================
    internal static class SolidariaInvoiceDataLogicExtensions
    {
        public static string GetAccidentNumber(this SolidariaInvoiceData d)
            => d.Claim?.Number
               ?? d.Claim?.Vehicle?.Soat?.SIRASFilingNumber
               ?? string.Empty;

        public static string GetPrimaryAmbulancePlate(this SolidariaInvoiceData d)
            => d.Claim?.Victims?.FirstOrDefault()?.RemissionInfo?.Transport?.PrimaryTransferAmbulancePlate
               ?? string.Empty;

        public static string? GetVehiclePlate(this SolidariaInvoiceData d)
            => d.Claim?.Vehicle?.PlateNumber;

        public static string? GetSoatPolicyNumber(this SolidariaInvoiceData d)
            => d.Claim?.Vehicle?.Soat?.Policy?.Number;

        public static string? GetFirstVictimId(this SolidariaInvoiceData d)
            => d.Claim?.Victims?.FirstOrDefault()?.IdNumber;

        public static DateTime? GetEventDate(this SolidariaInvoiceData d)
            => d.Claim?.Event?.Date;

        public static string? ToUTCString(this DateTime? dt, string format)
            => dt.HasValue ? dt.Value.ToUniversalTime().ToString(format) : null;

        public static int TryGetInt(this ConstantsEntity constants, string code)
        {
            if (constants?.ListType == null) return 0;
            var desc = constants.ListType.FirstOrDefault(x => x.Code == code)?.Description;
            return int.TryParse(desc, out var val) ? val : 0;
        }
    }
}