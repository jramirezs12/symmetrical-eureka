using NRules.Fluent.Dsl;
using RulesEngine.Application.Actions;
using RulesEngine.Domain.Common;
using RulesEngine.Domain.RulesEntities.Mundial.Entities;
using RulesEngine.Domain.ValueObjects;

namespace RulesEngine.Application.Clients.Mundial.Rules.RulesRepository.Fase_01.DateRules
{
    public class EventDateGreatherThanDeathDateRule44 : Rule, ITrackableRule
    {

        public Action OnMatch { get; set; } = () => { };

        public override void Define()
        {
            InvoiceToCheck? invoiceToCheck = default;

            When()
                .Match(() => invoiceToCheck!, x =>  !string.IsNullOrEmpty(x.DocumentType) && !string.IsNullOrEmpty(x.VictimId ) &&
                             Date.IsNotNullable(x.DeathDate, x.EventDate) && Date.GreaterThan(x.EventDate, x.DeathDate));

            Then()
                .Do(w => invoiceToCheck!.Alerts.Add(CreateAlert()))
                .Do(ctx => OnMatch());
        }

        private static Alert CreateAlert()
        {
            var alert = new Alert
            {
                AlertAction = "DenyClaim",
                AlertNameAction = "Devolver Reclamación",
                AlertType = "Regla lógica de fechas",
                AlertDescription = "Se valida si la fecha de ocurrencia el evento es posterior a la feha de fallecimiento, si lo es se rechazará la reclamación",
                AlertMessage = "Aplicar la objeción total teniendo en cuenta que la victima se encuentra fallecido en RNEC para la fecha del evento."
            };

            return alert;
        }
    }
}
