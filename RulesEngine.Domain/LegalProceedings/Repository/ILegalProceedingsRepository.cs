using RulesEngine.Domain.Invoices.Repositories;
using RulesEngine.Domain.LegalProceedings.Entities;

namespace RulesEngine.Domain.LegalProceedings.Repository
{
    public interface ILegalProceedingsRepository : IMongoRepository<LegalProceedingsEntity, string>
    { }
}
