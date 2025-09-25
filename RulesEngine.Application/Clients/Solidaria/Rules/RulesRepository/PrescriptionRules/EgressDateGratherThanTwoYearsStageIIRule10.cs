using NRules.Fluent.Dsl;
using RulesEngine.Domain.Common;
using RulesEngine.Domain.RulesEntities.Solidaria.Entities;
using RulesEngine.Domain.ValueObjects;

namespace RulesEngine.Application.Clients.Solidaria.Rules.RulesRepository.PrescriptionRules
{
    public class EgressDateGratherThanTwoYearsStageIIRule10 : Rule
    {
        public override void Define()
        {
            InvoiceToCheckSolidaria invoiceToCheck = default!;

            When()
                .Match(() => invoiceToCheck, x => !Date.IsNullable(x.EventDate) && !Date.IsNullable(x.EgressDate) 
                                                    && x.EventDate.Value!.Value.AddYears(2) > x.EgressDate.Value!.Value);

            Then()
                .Do(w => invoiceToCheck.Alerts.Add(CreateAlert()));
        }

        private Alert CreateAlert()
        {
            Alert alert = new Alert
            {
                AlertAction = "SendToQuality",
                AlertNameAction = "Enviar a Calidad",
                AlertType = "Reclamaciones Prescritas",
                AlertDescription = "Valida si la fecha de egreso es mayor a dos años",
                AlertMessage = "Asignación a calidad por Cuenta Prescrita."
            };

            return alert;
        }

    }
}
