using NRules;
using RulesEngine.Application.Actions.Common;

namespace RulesEngine.Application.Abstractions.Services
{
    public interface IRuleSessionBuilder
    {
        ISession BuildSession(IEnumerable<Type> ruleTypes, out List<string> matched, IActionService accionService);
    }
}
