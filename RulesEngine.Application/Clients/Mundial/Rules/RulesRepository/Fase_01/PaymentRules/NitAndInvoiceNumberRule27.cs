using NRules.Fluent.Dsl;
using RulesEngine.Application.Actions;
using RulesEngine.Domain.Common;
using RulesEngine.Domain.RulesEntities.Mundial.Entities;

namespace RulesEngine.Application.Clients.Mundial.Rules.RulesRepository.Fase_01.PaymentRules
{
    public class NitAndInvoiceNumberRule27 : Rule, ITrackableRule
    {
        public Action OnMatch { get; set; } = () => { };
        public override void Define()
        {
            InvoiceToCheck? invoiceToCheck = default;

            When()
                 .Match(() => invoiceToCheck!, x => x.InvoiceDifferentRadicates != null && (x.InvoiceDifferentRadicates.TotalRadNumbers > 1 || x.InvoiceDifferentRadicates!.RadNumbers!.Any()));

            Then()
                .Do(w => invoiceToCheck!.Alerts.Add(CreateAlert(invoiceToCheck)))
                .Do(ctx => OnMatch());
        }

        private static Alert CreateAlert(InvoiceToCheck invoice)
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
