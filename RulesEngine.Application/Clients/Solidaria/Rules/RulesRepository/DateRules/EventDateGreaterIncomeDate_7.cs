using NRules.Fluent.Dsl;
using RulesEngine.Domain.Common;
using RulesEngine.Domain.RulesEntities.Solidaria.Entities;
using RulesEngine.Domain.ValueObjects;

namespace RulesEngine.Application.Clients.Solidaria.Rules.RulesRepository.DateRules
{
    public class EventDateGreaterIncomeDate_7 : Rule
    {
        public override void Define()
        {
            InvoiceToCheckSolidaria? invoiceToCheck = default;

            When()
                .Match(() => invoiceToCheck!, x => Date.IsNotNullable(x.EventDate, x.IncomeDate) &&
                                                   Date.GreaterThan(x.EventDate, x.IncomeDate));

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
                AlertDescription = "Permite validar si la fecha de ocurrencia del evento es posterior a la fecha de ingreso, lo que conlleva a la devolución de la reclamación",
                AlertMessage = "Se debe aplicar devolución teniendo en cuenta que la fecha de ocurrencia del evento no puede ser posterior a la fecha de ingreso de la víctima. Carta de devolución # xxx"
            };

            return alert;
        }
    }
}
