using NRules.Fluent.Dsl;
using RulesEngine.Application.Actions;
using RulesEngine.Domain.Common;
using RulesEngine.Domain.RulesEntities.Solidaria.Entities;

namespace RulesEngine.Application.Clients.Solidaria.Rules.RulesRepository.PreviousObjections
{
    public class NitIpsAndInvoiceNumberTotalObjectionsStageIIRule9 : Rule, ITrackableRule
    {
        public Action OnMatch { get; set; } = () => { };
        public override void Define()
        {
            InvoiceToCheckSolidaria invoiceToCheck = default!;

            When()
                .Match(() => invoiceToCheck, x => x.PreviousObjections != null && x.PreviousObjections.RadNumbers!.Length > 0);

            Then()
                .Do(w => invoiceToCheck.AlertSolidaria.Add(CreateAlert()))
                .Do(ctx => OnMatch());
        }

        private AlertSolidaria CreateAlert()
        {
            return new AlertSolidaria
            {
                NameAction = "Enviar a Calidad",
                Type = "Objeciones Previas",
                Module = "Reclamaciones",
                Description = "Valida si la factura tiene objeciones previas totales (Validación entre invoiceTotal y GlossValue)",
                Message = "Asignación a calidad por Objeciones previas.",
                Typification = string.Empty,
                HasPriority = false
            };
        }
    }
}