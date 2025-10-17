using NRules.Fluent.Dsl;
using RulesEngine.Domain.Common;
using RulesEngine.Domain.RulesEntities.Solidaria.Entities;

namespace RulesEngine.Application.Clients.Solidaria.Rules.RulesRepository.PreviousObjections
{
    /// <summary>
    /// Assigns to Quality if previous objections exist by NIT + Invoice
    /// </summary>
    public class QualityPreviousObjectionsRule_9 : Rule
    {
        public override void Define()
        {
            InvoiceToCheckSolidaria? invoice = default;

            When()
                  .Match(() => invoice, x => x.PreviousObjections != null && x.PreviousObjections.RadNumbers!.Length > 0);

            Then()
                .Do(_ => invoice!.AlertSolidaria.Add(CreateAlert()));
        }

        private static AlertSolidaria CreateAlert()
        {
            return new AlertSolidaria
            {
                NameAction = "Enviar a Calidad",
                Type = "Objeciones Previas",
                Module = "Reclamaciones",
                Description = "Valida si la factura tiene obejeciones previas totales (Validacion entre invoiceTotal y GlossValue)",
                Message = "Asignación a calidad por Objeciones previas.",
                Typification = string.Empty,
                HasPriority = false
            };
        }
    }
}