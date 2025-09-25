using NRules;
using NRules.Fluent;
using NRules.Fluent.Dsl;
using RulesEngine.Application.Abstractions.Services;
using RulesEngine.Application.Actions;
using RulesEngine.Application.Actions.Common;

namespace RulesEngine.Infrastructure.Rules
{
    public class RuleSessionBuilder : IRuleSessionBuilder
    {
        public ISession BuildSession(IEnumerable<Type> ruleTypes, out List<string> matched, IActionService accionService)
        {
            var matchedNames = new List<string>();

            var repository = new RuleRepository();

            repository.Activator = new DelegateRuleActivator(type =>
            {
                var rule = (Rule)Activator.CreateInstance(type)!;

                if (rule is ITrackableRule trackable)
                    trackable.OnMatch = () => matchedNames.Add(rule.GetType().Name);

                if (rule is IRequiresAccionService requires)
                    requires.SetAccionService(accionService);

                return rule;
            });

            repository.Load(x => x.From(ruleTypes));
            matched = matchedNames;

            return repository.Compile().CreateSession();
        }
    }
    public class DelegateRuleActivator : IRuleActivator
    {
        private readonly Func<Type, object> _factory;

        public DelegateRuleActivator(Func<Type, object> factory)
        {
            _factory = factory;
        }

        public IEnumerable<Rule> Activate(Type type)
        {
            var instance = _factory(type) as Rule;
            if (instance == null)
                throw new InvalidOperationException($"No se pudo activar la regla de tipo {type.FullName}");

            return new[] { instance };
        }

        public object Create(Type type) => _factory(type);
    }
}
