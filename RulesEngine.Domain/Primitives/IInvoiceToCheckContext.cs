using RulesEngine.Domain.Common;
using RulesEngine.Domain.Primitives;

namespace RulesEngine.Domain.Primitives
{
    public interface IInvoiceToCheckContext : IBaseContext
    {
        string RadNumber { get; }
        string ModuleName { get; }
        string IpsNit { get; }
        List<Alert> Alerts { get; set; }
    }
}
