using Autofac;
using NRules.Extensibility;
using NRules.Fluent;
using NRules.Fluent.Dsl;
using RulesEngine.Domain.Invoices.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RulesEngine.Application.Clients.Solidaria.Rules.Dependencies
{
    public class AutofacRuleActivator : IRuleActivator
    {
        private readonly ILifetimeScope _scope;

        public AutofacRuleActivator(ILifetimeScope scope)
        {
            _scope = scope;
        }

        public IEnumerable<Rule> Activate(Type type)
        {
            yield return (Rule)_scope.Resolve(type);
        }

        public object Resolve(IResolutionContext context, Type serviceType)
        {
            return _scope.Resolve(serviceType);
        }
    }
}
