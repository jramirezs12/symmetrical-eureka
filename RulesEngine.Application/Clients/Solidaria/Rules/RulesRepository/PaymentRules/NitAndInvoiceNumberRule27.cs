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
                .Do(w => invoiceToCheck!.Alerts.Add(CreateAlert(invoiceToCheck)));
        }

        private static Alert CreateAlert(InvoiceToCheckSolidaria invoice)
        {
            var alert = new Alert
            {
                AlertAction = "Alert",
                AlertNameAction = "Alerta",
                AlertType = "Regla de doble cobro",
                AlertDescription = "El NIT y número de factura registrado de la tabla origen se encuentra en la tabla consulta bajo otro número de radicado",
                AlertMessage = $"La reclamación ya se encuentra bajo el radicado {invoice.RadNumber}"
            };

            return alert;
        }
    }
}
