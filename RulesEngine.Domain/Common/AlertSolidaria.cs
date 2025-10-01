namespace RulesEngine.Domain.Common
{
    public class AlertSolidaria
    {
        public required string NameAction { get; set; }
        public required string Type { get; set; }
        public required string Module { get; set; }
        public required string Message { get; set; }
        public required string Description { get; set; }
        public string Typification { get; set; }
        public bool HasPriority { get; set; }
    }
}
