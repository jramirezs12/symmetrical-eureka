namespace RulesEngine.Domain.Common
{
    public class Alert
    {
        public required string AlertAction { get; set; }
        public required string AlertNameAction { get; set; }
        public required string AlertType { get; set; }
        public required string AlertDescription { get; set; }
        public required string AlertMessage { get; set; }
        public string Typification { get; set; }
        public bool HasPriority { get; set; }
    }
}
