using NRules.Fluent.Dsl;
using RulesEngine.Application.Actions;
using RulesEngine.Domain.Common;
using RulesEngine.Domain.RulesEntities.Mundial.Entities;
using RulesEngine.Domain.ValueObjects;

namespace RulesEngine.Application.Clients.Mundial.Rules.RulesRepository.Fase_01.PrescriptionRules
{
    public class EventAndClaimDateRule_24 : Rule, ITrackableRule
    {
        public Action OnMatch { get; set; } = () => { };
        public override void Define()
        {
            InvoiceToCheck? invoiceToCheck = default;

            When()
                .Match(() => invoiceToCheck!, x => Date.IsNotNullable(x.EventDate, x.ClaimDate) &&
                                                   Convert.ToDateTime(x.EventDate.Value).AddYears(5) < Convert.ToDateTime(x.ClaimDate.Value));

            Then()
                .Do(w => invoiceToCheck!.Alerts.Add(CreateAlert()))
                .Do(ctx => OnMatch());
        }

        private static Alert CreateAlert()
        {
            var alert = new Alert
            {
                AlertAction = "PartialObjection",
                AlertNameAction = "Aplicar objeción parcial",
                AlertType = "Reglas de prescripción",
                AlertDescription = "Si entre la fecha de ocurrencia del evento de la tabla origen y la fecha de aviso de la tabla consulta supera los cinco años",
                AlertMessage = "Se debe aplicar la objeción total teniendo en cuenta que la reclamación se encuentra prescrita. Carta de objeción # xxx"
            };

            return alert;
        }
    }
}
