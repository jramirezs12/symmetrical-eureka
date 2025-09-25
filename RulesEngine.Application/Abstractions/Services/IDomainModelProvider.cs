using Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.Adapter;
using RulesEngine.Domain.Primitives;

namespace RulesEngine.Application.Abstractions.Services;

public interface IDomainModelProvider
{
    Task<IBaseContext> GetModelAsync(string tenant, string radNumber);
}
