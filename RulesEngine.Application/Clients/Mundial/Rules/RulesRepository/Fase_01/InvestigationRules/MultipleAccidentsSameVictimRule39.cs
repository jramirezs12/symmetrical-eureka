using NRules.Fluent.Dsl;
using RulesEngine.Application.Actions;
using RulesEngine.Domain.Common;
using RulesEngine.Domain.RulesEntities.Mundial.Entities;
using RulesEngine.Domain.ValueObjects;

namespace RulesEngine.Application.Clients.Mundial.Rules.RulesRepository.Fase_01.InvestigationRules
{
    public class MultipleAccidentsSameVictimRule39 : Rule, ITrackableRule
    {
        public Action OnMatch { get; set; } = () => { };
        public override void Define()
        {
            InvoiceToCheck? invoiceToCheck = default;

            When()
                .Match(() => invoiceToCheck!,
                x =>
                    x.ValidationAggregationRules_31_40 != null
                    && x.VictimId != null
                    && !Date.IsNullable(x.EventDate)
                    && x.EventDate.Value.HasValue
                    && x.ValidationAggregationRules_31_40.IdentificationNumberCase
                        .Count(c => c.IdentificationNumber == x.VictimId
                            && c.EventDate != x.EventDate.Value.Value.ToString("yyyy-MM-dd")
                        ) >= x.ValidationAggregationRules_31_40.ParameterRule39And40
                );

            Then()
                .Do(w => invoiceToCheck!.Alerts.Add(CreateAlert(invoiceToCheck)))
                .Do(ctx => OnMatch());
        }

        private static Alert CreateAlert(InvoiceToCheck invoice)
        {
            int numberOfAccidents = invoice.ValidationAggregationRules_31_40.IdentificationNumberCase.Count(x => x.IdentificationNumber == invoice.VictimId &&
                        invoice.EventDate.Value.Value.ToString("yyyy-MM-dd") != x.EventDate);
            var alert = new Alert
            {
                AlertAction = "Alert",
                AlertNameAction = "Alerta",
                AlertType = "Reglas de investigación",
                AlertDescription = "número de documento de identidad de la víctima en la víctima en la tabla de consulta, debe construir la llave \"siniestro\"  y contar la cantidad de siniestros diferentes en la tabla consulta (bd) , si la cantidad de siniestros es mayor a x ",
                AlertMessage = $"Para esta víctima existen {numberOfAccidents} eventos"
            };

            return alert;
        }
    }
}
