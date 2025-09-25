using RulesEngine.Application.Abstractions.Services;
using RulesEngine.Application.Builders;

namespace RulesEngine.Infrastructure.Factories
{
    public class QueryExecutorPerPhaseFactory : IQueryExecutorPerPhaseFactory
    {
        private readonly IEnumerable<IQueryExecutorPerPhase> _executors;
        public QueryExecutorPerPhaseFactory(IEnumerable<IQueryExecutorPerPhase> executors) => _executors = executors;

        public IQueryExecutorPerPhase ForTenant(string tenant) =>
            _executors.FirstOrDefault(e => string.Equals(e.Tenant, tenant, StringComparison.OrdinalIgnoreCase))
            ?? throw new InvalidOperationException($"No hay QueryExecutorPerPhase para tenant '{tenant}'.");
    }
}
