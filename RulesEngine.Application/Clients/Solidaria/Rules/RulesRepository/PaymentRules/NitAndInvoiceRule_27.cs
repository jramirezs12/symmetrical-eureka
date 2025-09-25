using NRules.Fluent.Dsl;
using RulesEngine.Domain.Common;
using RulesEngine.Domain.RulesEntities.Solidaria.Entities;

namespace RulesEngine.Application.Clients.Solidaria.Rules.RulesRepository.PaymentRules
{
    public class NitAndInvoiceRule_27 : Rule
    {
        public override void Define()
        {
            InvoiceToCheckSolidaria? invoiceToCheck = default;

            When()
                .Match(() => invoiceToCheck!, x => x.IpsNit != x.IpsNitF2 || x.InvoiceNumberF1 != x.InvoiceNumberF2);

            Then()
                .Do(w => invoiceToCheck!.Alerts.Add(CreateAlert()));
        }

        private static Alert CreateAlert()
        {
            var alert = new Alert
            {
                AlertAction = "Alert",
                AlertNameAction = "Alerta",
                AlertType = "Regla de coincidencia",
                AlertDescription = "El nIT más número de factura registrado de la tabla origen se encuentra en la tabla consulta bajo otro número de radicado",
                AlertMessage = "La reclamación ya se encuentra en otro radicado"
            };

            return alert;
        }
    }
}
