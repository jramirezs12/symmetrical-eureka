using RulesEngine.Domain.Invoices.Entities;

namespace RulesEngine.Application.Abstractions.Services
{
    public interface IQueryExecutorPerPhaseSolidaria
    {
        string Tenant { get; }  // = "Solidaria"
        Task QueryPerStage01(SolidariaInvoiceData data, string radNumber, string tenant);
        Task QueryPerStage02(SolidariaInvoiceData data, string radNumber, string tenant);
    }
}