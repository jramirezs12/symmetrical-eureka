namespace RulesEngine.Domain.Primitives
{
    public interface IRuleMetadataAware
    {
        Dictionary<string, string> TypificationMap { get; set; }
        Dictionary<string, bool> HasPriorityMap { get; set; }
    }
}
