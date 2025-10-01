using RulesEngine.Domain.Primitives;

namespace RulesEngine.Domain.Parameters.Entities
{
    public class Parameter : Entity<string>
    {

        public string Tenant { get; set; } = string.Empty;
        public string BusinessCode { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
    }
}
