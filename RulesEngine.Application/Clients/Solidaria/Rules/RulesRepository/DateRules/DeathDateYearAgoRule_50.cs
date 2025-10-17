using MongoDB.Driver;
using NRules.Fluent.Dsl;
using RulesEngine.Application.Actions;
using RulesEngine.Domain.Common;
using RulesEngine.Domain.RulesEntities.Solidaria.Entities;
using RulesEngine.Domain.ValueObjects;

namespace RulesEngine.Application.Clients.Solidaria.Rules.RulesRepository.DateRules
{
    public class DeathDateYearAgoRule_50 : Rule, ITrackableRule
    {
        public Action OnMatch { get; set; } = () => { };

        public override void Define()
        {
            InvoiceToCheckSolidaria? invoiceToCheck = default;

            When()
                .Match(() => invoiceToCheck!, x => !Date.IsNullable(x.DeathDate, x.EventDate) && x.DeathDate.Value > x.EventDate.Value!.Value.AddYears(1));

            Then()
                .Do(w => invoiceToCheck!.AlertSolidaria.Add(CreateAlert()));
        }

        private static AlertSolidaria CreateAlert()
        {
            return new AlertSolidaria
            {
                NameAction = "Devolver Reclamación",
                Type = "Regla lógica de fechas",
                Module = "Reclamaciones",
                Description = "Valida si la fecha de muerte ocurrió un año después de la fecha del evento/accidente, lo que conlleva a la objeción",
                Message = "Se debe aplicar devolución, teniendo en cuenta que la reclamación no puede ser presentada con anterioridad a la fecha de ocurrencia del evento. Carta de devolución # xxx",
                Typification = string.Empty,
                HasPriority = false
            };
        }
    }
}
