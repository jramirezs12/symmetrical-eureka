using MongoDB.Bson.Serialization.Attributes;

namespace RulesEngine.Domain.Invoices.Entities;

public class Atypicality
{
    public string ProcessName { get; set; } = string.Empty;
    public string ProcessCode { get; set; } = string.Empty;
    public decimal ObjetionProbability { get; set; } = 0.0m;
    public string DescriptionProbability { get; set; } = string.Empty;
    public string Observation { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public List<AtypicalityTraceability> Traceability { get; set; } = [];
}

[BsonNoId]
public class AtypicalityTraceability
{
    public int ID { get; set; } = 0;
    public bool WasValidated { get; set; } = false;
    public string UserAccount { get; set; } = string.Empty;
    public string ModuleName { get; set; } = string.Empty;
    public DateTime ValidationDate { get; set; }

    public AtypicalityTraceability(int id,
                                   bool wasValidated,
                                   string userAccount,
                                   string moduleName,
                                   DateTime validationDate)
    {
        ID = id;
        WasValidated = wasValidated;
        UserAccount = userAccount;
        ModuleName = moduleName;
        ValidationDate = validationDate;
    }

    public static AtypicalityTraceability Create(int id,
                                                 bool wasValidated,
                                                 string userAccount,
                                                 string moduleName,
                                                 DateTime validationDate)
    {
        return new AtypicalityTraceability(id, wasValidated, userAccount, moduleName, validationDate);
    }
}
