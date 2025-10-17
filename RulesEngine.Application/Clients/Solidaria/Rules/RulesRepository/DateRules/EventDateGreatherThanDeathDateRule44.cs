using NRules.Fluent.Dsl;
using RulesEngine.Application.Actions;
using RulesEngine.Domain.Common;
using RulesEngine.Domain.RulesEntities.Solidaria.Entities;
using RulesEngine.Domain.ValueObjects;

namespace RulesEngine.Application.Clients.Solidaria.Rules.RulesRepository.DateRules
{
    public class EventDateGreatherThanDeathDateRule44 : Rule, ITrackableRule
    {
        public Action OnMatch { get; set; } = () => { };

        public override void Define()
        {
            InvoiceToCheckSolidaria? invoiceToCheck = default;

            When()
                .Match(() => invoiceToCheck!, x =>  !string.IsNullOrEmpty(x.DocumentType) && !string.IsNullOrEmpty(x.VictimId ) &&
                             Date.IsNotNullable(x.DeathDate, x.EventDate) && Date.GreaterThan(x.EventDate, x.DeathDate));

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
                Description = "Se valida si la fecha de ocurrencia el evento es posterior a la feha de fallecimiento, si lo es se rechazará la reclamación",
                Message = "Aplicar la objeción total teniendo en cuenta que la victima se encuentra fallecido en RNEC para la fecha del evento.",
                Typification = string.Empty,
                HasPriority = false
            };
        }
    }
}
