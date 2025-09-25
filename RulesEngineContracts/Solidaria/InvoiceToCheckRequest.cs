namespace RulesEngineAPI.Contracts.Solidaria
{
    public sealed record InvoiceToCheckRequest(string RadNumber, string Stage, string ModuleName);
}
