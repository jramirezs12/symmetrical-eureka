using NRules.Fluent.Dsl;
using RulesEngine.Application.Actions;
using RulesEngine.Domain.Common;
using RulesEngine.Domain.RulesEntities.Solidaria.Entities;
using RulesEngine.Domain.ValueObjects;

namespace RulesEngine.Application.Clients.Solidaria.Rules.RulesRepository.DateRules
{
    public class EventDateGreaterClaimDateRule_3 : Rule, ITrackableRule
    {
        public Action OnMatch { get; set; } = () => { };

        public override void Define()
        {
            InvoiceToCheckSolidaria? invoiceToCheck = default;

            When()
                .Match(() => invoiceToCheck!, x => Date.IsNotNullable(x.EventDate, x.ClaimDate) && 
                                                   Date.GreaterThan(x.EventDate, x.ClaimDate));

            Then()
                .Do(w => invoiceToCheck!.AlertSolidaria.Add(CreateAlert()))
                .Do(ctx => OnMatch());
        }

        private static AlertSolidaria CreateAlert()
        {
            return new AlertSolidaria
            {
                NameAction = "Devolver Reclamación",
                Type = "Regla lógica de fechas",
                Module = "Reclamaciones",
                Description = "Permite validar si la fecha de ocurrencia del evento es posterior a la fecha de aviso de la reclamación, lo que conlleva a la devolución de la reclamación",
                Message = "Se debe aplicar devolución teniendo en cuenta que la reclamación no puede ser presentada con anterioridad a la fecha de ocurrencia del evento. Carta de devolución # xxx",
                Typification = string.Empty,
                HasPriority = false
            };
        }
    }
}
