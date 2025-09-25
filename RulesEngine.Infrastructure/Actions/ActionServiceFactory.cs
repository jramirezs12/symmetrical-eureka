using Autofac.Core;
using RulesEngine.Application.Actions;
using RulesEngine.Application.Actions.Common;

namespace RulesEngine.Infrastructure.Actions
{
    public class ActionServiceFactory : IActionServiceFactory
    {

        private readonly Dictionary<string, IActionService> _map;

        public ActionServiceFactory(IEnumerable<IActionService> services)
        {
            _map = services.ToDictionary(
            s => s.GetType().Name.Replace("ActionService", "", StringComparison.OrdinalIgnoreCase).ToLower(),
            s => s);
        }

        public IActionService CreateForTenant(string tenant)
        {
            if (!_map.TryGetValue(tenant.ToLower(), out var service))
                return new NullAccionService(); // No lanza error, solo no hace nada

            return service;
        }
    }
}
