using RulesEngine.Application.Abstractions.Services;

namespace RulesEngine.Application.Builders
{
    public interface IQueryExecutorPerPhaseFactory
    {
        IQueryExecutorPerPhase ForTenant(string tenant);
    }
}
