using RulesEngine.Domain.Invoices.Entities;

namespace RulesEngine.Application.Abstractions.Services
{
    public interface IQueryExecutorPerPhase
    {
        string Tenant { get; }
        Task QueryPerStage01(InvoiceData data, string radNumber, string tenant);
        Task QueryPerStage02(InvoiceData data, string radNumber, string tenant);
    }
}
