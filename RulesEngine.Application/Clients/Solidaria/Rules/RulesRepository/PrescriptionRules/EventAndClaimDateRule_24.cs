using NRules.Fluent.Dsl;
using RulesEngine.Domain.Common;
using RulesEngine.Domain.RulesEntities.Mundial.Entities;
using RulesEngine.Domain.RulesEntities.Solidaria.Entities;
using RulesEngine.Domain.ValueObjects;

namespace RulesEngine.Application.Clients.Solidaria.Rules.RulesRepository.PrescriptionRules
{
    public class EventAndClaimDateRule_24 : Rule
    {
        public override void Define()
        {
            InvoiceToCheckSolidaria? invoiceToCheck = default;

            When()
                .Match(() => invoiceToCheck!, x => Date.IsNotNullable(x.EventDate, x.ClaimDate) &&
                                                   Convert.ToDateTime(x.EventDate.Value).AddYears(5) < Convert.ToDateTime(x.ClaimDate.Value));

            Then()
                .Do(w => invoiceToCheck!.AlertSolidaria.Add(CreateAlert()));
        }

        private static AlertSolidaria CreateAlert()
        {
            return new AlertSolidaria
            {
                NameAction = "Aplicar objeción parcial",
                Type = "Reglas de prescripción",
                Module = "Reclamaciones",
                Description = "Si entre la fecha de ocurrencia del evento y la fecha de aviso supera los cinco años",
                Message = "Se debe aplicar la objeción total teniendo en cuenta que la reclamación se encuentra prescrita. Carta de objeción # xxx",
                Typification = string.Empty,
                HasPriority = false
            };
        }
    }
}