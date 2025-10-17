using NRules.Fluent.Dsl;
using RulesEngine.Domain.Common;
using RulesEngine.Domain.RulesEntities.Solidaria.Entities;

namespace RulesEngine.Application.Clients.Solidaria.Rules.RulesRepository.PaymentRules
{
    public class NitAndInvoiceNumberRule27 : Rule
    {
        public override void Define()
        {
            InvoiceToCheckSolidaria? invoiceToCheck = default;

            When()
                 .Match(() => invoiceToCheck!, x => x.InvoiceDifferentRadicates != null && (x.InvoiceDifferentRadicates.TotalRadNumbers > 1 || x.InvoiceDifferentRadicates!.RadNumbers!.Any()));

            Then()
                .Do(w => invoiceToCheck!.AlertSolidaria.Add(CreateAlert(invoiceToCheck)));
        }

        private static AlertSolidaria CreateAlert(InvoiceToCheckSolidaria invoice)
        {
            return new AlertSolidaria
            {
                NameAction = "Alerta",
                Type = "Regla de doble cobro",
                Module = "Reclamaciones",
                Description = "El NIT y número de factura registrados se encuentran en otro radicado",
                Message = $"La reclamación ya se encuentra bajo el radicado {invoice.RadNumber}",
                Typification = string.Empty,
                HasPriority = false
            };
        }
    }
}