namespace RulesEngine.Application.Actions
{
    public interface ITrackableRule
    {
        Action OnMatch { get; set; }
    }
}
