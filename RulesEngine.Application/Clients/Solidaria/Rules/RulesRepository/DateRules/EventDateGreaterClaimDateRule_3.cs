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
                AlertDescription = "Permite validar si la fecha de ocurrencia del evento es posterior a la fecha de aviso de la reclamación, lo que conlleva a la devolución de la reclamación",
                AlertMessage = "Se debe aplicar devolución teniendo en cuenta que la reclamación no puede ser presentada con anterioridad a la fecha de ocurrencia del evento. Carta de devolución # xxx"
            };

            return alert;
        }
    }
}
