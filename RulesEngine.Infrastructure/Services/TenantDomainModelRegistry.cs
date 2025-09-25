using RulesEngine.Application.Abstractions.Services;

namespace RulesEngine.Infrastructure.Services;

public class TenantDomainModelRegistry : IDomainModelFactory
{
    private readonly Dictionary<string, ITenantDomainModelBuilder> _builders;

    public TenantDomainModelRegistry(IEnumerable<ITenantDomainModelBuilder> builders)
    {
        _builders = builders.ToDictionary(b => b.Tenant);
    }

    public object Create(string tenant, string radNumber)
    {
        if (!_builders.TryGetValue(tenant, out var builder))
            throw new NotImplementedException($"No se encontró un builder para el tenant '{tenant}'");

        return builder.Build(radNumber);
    }
}
