using NRules.Fluent.Dsl;
using RulesEngine.Domain.Common;
using RulesEngine.Domain.RulesEntities.Solidaria.Entities;
using RulesEngine.Domain.ValueObjects;

namespace RulesEngine.Application.Clients.Solidaria.Rules.RulesRepository.Prescription
{
    /// <summary>
    /// Assigns to Quality if claim is prescribed
    /// </summary>
    public class QualityPrescriptionRule_10 : Rule
    {
        public Action OnMatch { get; set; } = () => { };
        public override void Define()
        {
            InvoiceToCheckSolidaria invoiceToCheck = default!;

            When()
                .Match(() => invoiceToCheck, x => !Date.IsNullable(x.EventDate) && !Date.IsNullable(x.EgressDate)
                                                    && x.EventDate.Value!.Value.AddYears(2) > x.EgressDate.Value!.Value);

            Then()
                .Do(w => invoiceToCheck.AlertSolidaria.Add(CreateAlert()))
                .Do(ctx => OnMatch());
        }

        private AlertSolidaria CreateAlert()
        {
            return new AlertSolidaria
            {
                NameAction = "Enviar a Calidad",
                Type = "Reclamaciones Prescritas",
                Module = "Reclamaciones",
                Description = "Valida si la fecha de egreso es mayor a dos años",
                Message = "Asignación a calidad por Cuenta Prescrita.",
                Typification = string.Empty,
                HasPriority = false
            };
        }

    }
}