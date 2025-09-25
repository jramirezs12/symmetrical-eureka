using MongoDB.Bson;
using RulesEngine.Domain.BlobsStorage.Entities;
using RulesEngine.Domain.Constants.Entities;
using RulesEngine.Domain.ElectronicBillingRuleEngine.Entities;
using RulesEngine.Domain.Parameters.Entities;

namespace RulesEngine.Application.DataSources
{
    public  interface IExternalDataLoader
    {
        string Tenant { get; }
        Task<BlobStorage> GetBlobStorageAsync(string tenant);
        Task<Parameter> GetInvoiceAgregationParameterAsync(string tenant);
        Task<BsonDocument?> GetInvoiceDataAsync(string query);
        Task<IEnumerable<Parameter>?> GetGroupAgregationData(string[] listBusinessCase, string tenant);
        Task<ElectronicBillingEntity> GetElectronicBilling(string invoiceNUmber, string nitIps);
        Task<ConstantsEntity> GetConstantsByCodeAsync(string businessCode);
    }
}
