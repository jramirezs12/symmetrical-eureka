namespace RulesEngine.Persistence.Settings
{
    public class BasicMongoConnectionSettings
    {
        public const string SeccionName = "BasicGiproDatabases";
        public string? ConnectionString { get; set; } = string.Empty;
        public DatabaseNameCollection? DatabaseNameCollection { get; set; }
    }
}