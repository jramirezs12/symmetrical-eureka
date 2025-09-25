using ErrorOr;
using MediatR;
using RulesEngineContracts.Mundial;

namespace RulesEngine.Application.Invoices.UpdateInvoice
{
    public sealed record UpdateInvoiceCommand : IRequest<ErrorOr<InvoiceToCheckResponse>>
    {
        public string TenantName { get; set; } = string.Empty;
        public string TenantId { get; set; } = string.Empty;
        public string RadNumber { get; set; } = string.Empty;
        public string ModuleName { get; set; } = string.Empty;
        public string Stage{ get; set; } = string.Empty;

    }
}
