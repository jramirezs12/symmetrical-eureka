using RulesEngine.Domain.Primitives;
using RulesEngine.Domain.RulesEntities.Mundial.Entities;

namespace RulesEngine.Application.Builders
{
    public interface IInvoiceToCheckBuilder
    {
        string Tenant { get; }
        Task<IInvoiceToCheckContext> BuildAsync(string radNumber, string moduleName, string stage, string tenant);
    }
}
