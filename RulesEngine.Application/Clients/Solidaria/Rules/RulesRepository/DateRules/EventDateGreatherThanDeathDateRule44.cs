using NRules.Fluent.Dsl;
using RulesEngine.Domain.Common;
using RulesEngine.Domain.RulesEntities.Solidaria.Entities;
using RulesEngine.Domain.ValueObjects;

namespace RulesEngine.Application.Clients.Solidaria.Rules.RulesRepository.DateRules
{
    public class EventDateGreatherThanDeathDateRule44 : Rule
    {
        public override void Define()
        {
            InvoiceToCheckSolidaria? invoiceToCheck = default;

            When()
                .Match(() => invoiceToCheck!, x =>  !string.IsNullOrEmpty(x.DocumentType) && !string.IsNullOrEmpty(x.VictimId ) &&
                             Date.IsNotNullable(x.DeathDate, x.EventDate) && Date.GreaterThan(x.EventDate, x.DeathDate));

            Then()
                .Do(w => invoiceToCheck!.Alerts.Add(CreateAlert()));
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
