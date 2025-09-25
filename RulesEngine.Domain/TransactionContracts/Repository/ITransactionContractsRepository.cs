using RulesEngine.Domain.Invoices.Repositories;
using RulesEngine.Domain.TransactionContracts.Entities;

public interface ITransactionContractsRepository : IMongoRepository<ConsolidatedTransactionContracts, string>
{ }