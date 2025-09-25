using NRules.Fluent.Dsl;
using RulesEngine.Application.Actions;
using RulesEngine.Domain.Common;
using RulesEngine.Domain.RulesEntities.Mundial.Entities;

namespace RulesEngine.Application.Clients.Mundial.Rules.RulesRepository.Fase_02.PreviousObjections
{
    public class NitIpsAndInvoiceNumberTotalObjectionsStageIIRule9 : Rule, ITrackableRule
    {
        public Action OnMatch { get; set; } = () => { };
        public override void Define()
        {
            InvoiceToCheck invoiceToCheck = default!;

            When()
                .Match(() => invoiceToCheck, x => x.PreviousObjections != null && x.PreviousObjections.RadNumbers!.Length > 0);

            Then()
                .Do(w => invoiceToCheck.Alerts.Add(CreateAlert()))
                .Do(ctx => OnMatch());
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
