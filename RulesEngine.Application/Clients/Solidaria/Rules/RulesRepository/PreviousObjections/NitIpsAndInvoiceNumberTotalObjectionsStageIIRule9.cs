using NRules.Fluent.Dsl;
using RulesEngine.Domain.Common;
using RulesEngine.Domain.RulesEntities.Solidaria.Entities;

namespace RulesEngine.Application.Clients.Solidaria.Rules.RulesRepository.PreviousObjections
{
    public class NitIpsAndInvoiceNumberTotalObjectionsStageIIRule9 : Rule
    {
        public override void Define()
        {
            InvoiceToCheckSolidaria invoiceToCheck = default!;

            When()
                .Match(() => invoiceToCheck, x => x.PreviousObjections != null && x.PreviousObjections.RadNumbers!.Length > 0);

            Then()
                .Do(w => invoiceToCheck.Alerts.Add(CreateAlert()));
        }

        private Alert CreateAlert()
        {
            Alert alert = new Alert
            {
                AlertAction = "SendToQuality",
                AlertNameAction = "Enviar a Calidad",
                AlertType = "Objeciones Previas",
                AlertDescription = "Valida si la factura tiene obejeciones previas totales (Validacion entre invoiceTotal y GlossValue)",
                AlertMessage = "Asignación a calidad por Objeciones previas."
            };

            return alert;
        }
    }
}
