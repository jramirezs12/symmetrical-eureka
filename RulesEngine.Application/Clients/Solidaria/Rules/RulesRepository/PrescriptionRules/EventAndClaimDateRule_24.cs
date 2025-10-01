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
                .Do(w => invoiceToCheck!.Alerts.Add(CreateAlert()));
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
