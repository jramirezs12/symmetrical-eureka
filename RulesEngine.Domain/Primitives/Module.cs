namespace RulesEngine.Domain.Primitives
{
    public class Module
    {
        public int _Id { get; private set; } = 0;
        public string Code { get; private set; } = string.Empty;
        public string Description { get; private set; } = string.Empty;
    }
}
