using NRules.Fluent.Dsl;
using RulesEngine.Domain.Common;
using RulesEngine.Domain.RulesEntities.Solidaria.Entities;

namespace RulesEngine.Application.Clients.Solidaria.Rules.RulesRepository.InvestigationRules
{
    public class ValidationInvoiceValueRule46: Rule
    {
        public override void Define()
        {
            InvoiceToCheckSolidaria? invoiceToCheck = default;
            When()
                    .Match(() => invoiceToCheck!, x => x.IpsPhoneVerification != null
                                && x.IpsPhoneVerification.Any(c => x.IpsNit == c.NitIps
                                && x.InvoiceValue.Value > x.InvoicePhoneVerificationValue.Value));
            Then()
                .Do(ctx => invoiceToCheck!.Alerts.Add(CreateAlert()));
        }


        private static Alert CreateAlert()
        {
            var alert = new Alert
            {
                AlertAction = "SendToInvestigation",
                AlertNameAction = "Enviar a Investigar",
                AlertType = "Regla de investigación",
                AlertDescription = "EL nit que se está validando es igual al nit de la tabla de consulta y el valor de la factura es mayor al valor permitido",
                AlertMessage = "Se debe enviar a verificación telefonica"
            };

            return alert;
        }
    }
}
