using NRules.Fluent.Dsl;

namespace RulesEngine.Application.Clients.Mundial.Rules
{
    public interface IRulesCollection
    {
        IEnumerable<Rule> GetRules(string tenant, string fase);
    }
}
