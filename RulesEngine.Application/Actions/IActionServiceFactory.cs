using RulesEngine.Application.Actions.Common;

namespace RulesEngine.Application.Actions
{
    public interface IActionServiceFactory
    {
        IActionService CreateForTenant(string tenant);
    }
}
