namespace RulesEngine.Application.Builders
{
    public interface IInvoiceToCheckBuilderFactory
    {
        IInvoiceToCheckBuilder ForTenant(string tenant);
    }
}
