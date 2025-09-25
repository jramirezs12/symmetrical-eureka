using NRules.Fluent.Dsl;
using RulesEngine.Application.Actions;
using RulesEngine.Domain.Common;
using RulesEngine.Domain.RulesEntities.Mundial.Entities;
using RulesEngine.Domain.ValueObjects;

namespace RulesEngine.Application.Clients.Mundial.Rules.RulesRepository.Fase_03.InvestigationRules
{
    public class MultipleAccidentsSameVictimRule40 : Rule, ITrackableRule
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
                    && x.ValidationAggregationRules_31_40.IdentificationNumberCase
                        .Count(c =>
                            c.IdentificationNumber != null
                            && c.EventDate != null
                            && c.IdentificationNumber == x.VictimId
                            && c.EventDate != x.EventDate.Value.Value.ToString("yyyy-MM-dd")
                        ) >= x.ValidationAggregationRules_31_40.ParameterRule39And40
                );

            Then()
                .Do(w => invoiceToCheck!.Alerts.Add(CreateAlert(invoiceToCheck)))
                .Do(ctx => OnMatch());
        }

        private Alert CreateAlert(InvoiceToCheck invoiceToCheck)
        {
            string typification = invoiceToCheck.TypificationMap.GetValueOrDefault(GetType().Name, "Sin typification");
            bool haspriority = invoiceToCheck.HasPriorityMap.GetValueOrDefault(GetType().Name, false);
            
            int amountEvents = invoiceToCheck.ValidationAggregationRules_31_40.IdentificationNumberCase.Count(x => x.IdentificationNumber == invoiceToCheck.VictimId &&
                                                 x.EventDate != invoiceToCheck.EventDate.Value.Value.ToString("yyyy-MM-dd"));
            var alert = new Alert
            {
                AlertAction = "Alert",
                AlertNameAction = "Enviar a Investigar",
                AlertType = "Reglas de investigación",
                AlertDescription = "número de documento de identidad de la víctima en la víctima en la tabla de consulta, debe construir la llave \"siniestro\"  y contar la cantidad de siniestros diferentes en la tabla consulta (bd) , si la cantidad de siniestros es mayor a x ",
                AlertMessage = $"Para esta víctima existen {amountEvents} eventos",
                Typification = typification,
                HasPriority = haspriority
            };

            return alert;
        }
    }
}
