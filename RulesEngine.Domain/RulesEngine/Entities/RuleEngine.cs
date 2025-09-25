using RulesEngine.Domain.Primitives;

namespace RulesEngine.Domain.RulesEngine.Entities
{
    public class RuleEngine : Entity<string>
    {
        
        public string Tenant { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public List<RuleEntity>? Rules { get; set; }
    }

    public class RuleEntity
    {
        public string Name { get; set; } = string.Empty;
        public string ClassName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool Active { get; set; }
        public string Typification { get; set; }
        public bool HasPriority { get; set; }
    }
}
