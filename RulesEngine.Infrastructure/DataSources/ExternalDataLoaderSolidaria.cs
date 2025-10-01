using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using RulesEngine.Application.Abstractions.Services;
using RulesEngine.Application.DataSources;
using RulesEngine.Domain.BlobsStorage.Entities;
using RulesEngine.Domain.BlobsStorage.Repositories;
using RulesEngine.Domain.Constants;
using RulesEngine.Domain.Constants.Entities;
using RulesEngine.Domain.ElectronicBillingRuleEngine.Entities;
using RulesEngine.Domain.ElectronicBillingRuleEngine.Repository;
using RulesEngine.Domain.Invoices.Repositories;
using RulesEngine.Domain.Parameters.Entities;
using RulesEngine.Domain.Parameters.Repositories;

namespace RulesEngine.Infrastructure.DataSources
{
    public class ExternalDataLoaderSolidaria : IExternalDataLoader
    {
        public string Tenant => "Solidaria";
        private readonly IUtilityService _utilityService;
        private readonly IParameterRepository _parameterRepository;
        private readonly IBlobStorageRepository _blobStorageRepository;
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly IElectronicBillingRepository _electronicBillingRepository;
        private readonly IConstantsRepository _constantsRepository;

        public ExternalDataLoaderSolidaria(IUtilityService utilityService,
            IParameterRepository parameterRepository,
            IBlobStorageRepository blobStorageRepository,
            IInvoiceRepository invoiceRepository,
            IElectronicBillingRepository electronicBillingRepository,
            IConstantsRepository constantsRepository)
        {
            _electronicBillingRepository = electronicBillingRepository;
            _blobStorageRepository = blobStorageRepository;
            _invoiceRepository = invoiceRepository;
            _parameterRepository = parameterRepository;
            _utilityService = utilityService;
            _constantsRepository = constantsRepository;
        }

        public async Task<BlobStorage> GetBlobStorageAsync(string tenant)
        {
            var blobKey = $"blobStorage_{tenant}";
            return await _utilityService.GetOrSetDataCache(blobKey, () =>
                _blobStorageRepository.FindOneAsync(x => x.Tenant == tenant && x.BusinessCode == "BLS_005"), 2);
        }
        public async Task<Parameter> GetInvoiceAgregationParameterAsync(string tenant)
        {
            var paramKey = $"param_SOL-001_{tenant}";
            return await _utilityService.GetOrSetDataCache(paramKey, () =>
                _parameterRepository.FindOneAsync(x => x.Tenant == tenant && x.BusinessCode == "SOL-001"), 2);
        }

        public async Task<BsonDocument?> GetInvoiceDataAsync(string query)
        {
            BsonDocument[] docs = BsonSerializer.Deserialize<BsonDocument[]>(query);
            return await _invoiceRepository.GetInvoiceByAggregation(docs);
        }

        public async Task<IEnumerable<Parameter>?> GetGroupAgregationData(string[] listBusinessCase, string tenant)
        {
            string cacheKey = $"params:{tenant}:{string.Join(",", listBusinessCase)}";

            IEnumerable<Parameter> groupAgregations = await _utilityService.GetOrSetDataCache(cacheKey, async () =>
            {
                var parameterFilter = Builders<Parameter>.Filter.In(x => x.BusinessCode, listBusinessCase) &
                                      Builders<Parameter>.Filter.Eq(x => x.Tenant, tenant);

                return await _parameterRepository.FilterBy(parameterFilter);
            }, 2);
            return groupAgregations;
        }

        public async Task<ElectronicBillingEntity> GetElectronicBilling(string invoiceNUmber, string nitIps)
        {
            // Get Data Electronic Billing
            var filterEB = Builders<ElectronicBillingEntity>.Filter.Eq(x => x.InvoiceNumber, invoiceNUmber) &
                           Builders<ElectronicBillingEntity>.Filter.Eq(x => x.NitIps, nitIps);

            return await _electronicBillingRepository.GetElectronicBilling(filterEB);
        }

        public async Task<ConstantsEntity> GetConstantsByCodeAsync(string businessCode)
        {
            string cacheKey = $"constants_{businessCode}";
            var constansParameters = await _utilityService.GetOrSetDataCache(cacheKey, async () =>
            {
                return await _constantsRepository.FindOneAsync(s => s.BusinessCode == businessCode);
            }, 2);
            return constansParameters;
        }
    }
}
