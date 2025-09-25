using RulesEngine.Application.Abstractions.Services;
using RulesEngine.Domain.Primitives;

namespace RulesEngine.Infrastructure.Services;

public class RadDomainModelProvider(IDomainModelFactory _factory) : IDomainModelProvider
{
    public Task<IBaseContext> GetModelAsync(string tenant, string radNumber)
    {
        var context = _factory.Create(tenant, radNumber) as IBaseContext;
        return Task.FromResult(context);
    }
}
