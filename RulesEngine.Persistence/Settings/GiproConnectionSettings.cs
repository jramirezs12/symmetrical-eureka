namespace RulesEngine.Persistence.Settings
{
    public class GiproConnectionSettings
    {
        public const string SectionName = "GiproDatabases";
        public string? ConnectionString { get; set; } = string.Empty;
        public DatabaseNameCollection? DatabaseNameCollection { get; set; }
    }

    public class DatabaseNameCollection : Dictionary<string, string> { }
}
