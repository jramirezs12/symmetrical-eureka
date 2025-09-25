using RulesEngine.Application.Builders;

namespace RulesEngine.Infrastructure.Factories
{
    public class InvoiceToCheckBuilderFactory : IInvoiceToCheckBuilderFactory
    {
        private readonly IEnumerable<IInvoiceToCheckBuilder> _builders;

        public InvoiceToCheckBuilderFactory(IEnumerable<IInvoiceToCheckBuilder> builders)
        {
            _builders = builders;
        }

        public IInvoiceToCheckBuilder ForTenant(string tenant)
        {
            var builder = _builders.FirstOrDefault(b => b.Tenant == tenant);
            if (builder == null)
                throw new InvalidOperationException($"No existe builder para tenant {tenant}");
            return builder;
        }
    }
}
