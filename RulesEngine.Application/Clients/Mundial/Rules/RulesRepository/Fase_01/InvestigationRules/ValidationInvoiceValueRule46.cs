using NRules.Fluent.Dsl;
using RulesEngine.Application.Actions;
using RulesEngine.Domain.Common;
using RulesEngine.Domain.RulesEntities.Mundial.Entities;

namespace RulesEngine.Application.Clients.Mundial.Rules.RulesRepository.Fase_01.InvestigationRules
{
    public class ValidationInvoiceValueRule46: Rule, ITrackableRule
    {
        public Action OnMatch { get; set; } = () => { };
        public override void Define()
        {
            InvoiceToCheck? invoiceToCheck = default;
            When()
                    .Match(() => invoiceToCheck!, x => x.IpsPhoneVerification != null
                                && x.IpsPhoneVerification.Any(c => x.IpsNit == c.NitIps
                                && x.InvoiceValue.Value > x.InvoicePhoneVerificationValue.Value));
            Then()
                .Do(ctx => invoiceToCheck!.Alerts.Add(CreateAlert()))
                .Do(ctx => OnMatch());
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
