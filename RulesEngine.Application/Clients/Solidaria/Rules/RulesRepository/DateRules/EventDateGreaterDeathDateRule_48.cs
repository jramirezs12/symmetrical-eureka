using NRules.Fluent.Dsl;
using RulesEngine.Domain.Common;
using RulesEngine.Domain.RulesEntities.Solidaria.Entities;
using RulesEngine.Domain.ValueObjects;

namespace RulesEngine.Application.Clients.Solidaria.Rules.RulesRepository.DateRules
{
    public class EventDateGreaterDeathDateRule_48 : Rule
    {
        public override void Define()
        {
            InvoiceToCheckSolidaria? invoiceToCheck = default;

            When()
                .Match(() => invoiceToCheck!, x => Date.IsNotNullable(x.EventDate, x.DeathDate) && 
                                                   Date.GreaterThan(x.EventDate, x.DeathDate));

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
                AlertDescription = "Permite validar si la fecha evento/accidente es posterior a la fecha en caso de muerte, lo que conlleva a la devolución de la reclamación",
                AlertMessage = "Se debe aplicar devolución teniendo en cuenta que la fecha de la muerte no puede ser mayor a la de fecha aviso de la reclamación. Carta de devolución # xxx"
            };

            return alert;
        }
    }
}
