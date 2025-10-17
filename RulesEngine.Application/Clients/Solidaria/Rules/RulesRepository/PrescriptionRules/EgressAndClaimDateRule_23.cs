using NRules.Fluent.Dsl;
using RulesEngine.Domain.Common;
using RulesEngine.Domain.RulesEntities.Solidaria.Entities;
using RulesEngine.Domain.ValueObjects;


namespace RulesEngine.Application.Clients.Solidaria.Rules.RulesRepository.PrescriptionRules
{
    public class EgressAndClaimDateRule_23 : Rule
    {
        public override void Define()
        {
            InvoiceToCheckSolidaria? invoiceToCheck = default;

            When()
                .Match(() => invoiceToCheck!, x => Date.IsNotNullable(x.EgressDate, x.ClaimDate) &&
                                                   x.EgressDate.Value!.Value.AddYears(2) <= x.ClaimDate.Value);

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
                AlertDescription = "Si entre la fecha de egreso de la tabla origen y la fecha de aviso de la tabla consulta supera los dos años",
                AlertMessage = "Se debe aplicar la objeción total teniendo en cuenta que la reclamación se encuentra prescrita. Carta de objeción # xxx"
            };

            return alert;
        }
    }
}
