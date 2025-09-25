using RulesEngine.Domain.DisputeProcess.Entities;
using RulesEngine.Domain.Invoices.Repositories;


namespace RulesEngine.Domain.DisputeProcess.Repository
{
    public interface ILegalProcessesAndTransactionContractsRepository : IMongoRepository<DisputeProcessEntity, string>
    { }
}
