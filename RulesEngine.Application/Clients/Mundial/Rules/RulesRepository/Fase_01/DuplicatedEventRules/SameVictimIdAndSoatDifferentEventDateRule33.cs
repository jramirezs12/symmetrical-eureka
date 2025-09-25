using NRules.Fluent.Dsl;
using RulesEngine.Application.Actions;
using RulesEngine.Domain.Common;
using RulesEngine.Domain.RulesEntities.Mundial.Entities;

namespace RulesEngine.Application.Clients.Mundial.Rules.RulesRepository.Fase_01.DuplicatedEventRules
{
    public class SameVictimIdAndSoatDifferentEventDateRule33 : Rule, ITrackableRule
    {
        public Action OnMatch { get; set; } = () => { };
        public override void Define()
        {
            InvoiceToCheck? invoiceToCheck = default;

            When()
                .Match(() => invoiceToCheck!, x => x.ValidationAggregationRules_31_40 != null
                    && x.VictimId != null
                    && x.SoatNumber != null
                    && x.ValidationAggregationRules_31_40.IdentificationNumberCase
                        .Any(c => c.IdentificationNumber == x.VictimId && c.SoatNumber == x.SoatNumber)
                );

            Then()
                .Do(w => invoiceToCheck!.Alerts.Add(CreateAlert()))
                .Do(ctx => OnMatch());
        }

        private static Alert CreateAlert()
        {
            var alert = new Alert
            {
                AlertAction = "Alert",
                AlertNameAction = "Alerta",
                AlertType = "Regla de duplicidad de siniestro",
                AlertDescription = "El numero de documento de la victima y el numero de poliza es igua a la tabla consultada pero la fecha de ocurrencia del evento que se está validando es diferente es diferente a la Fecha de ocurrencia del evento en la tabla de consulta",
                AlertMessage = "La victima ya cuenta con un siniestro con otra fecha de accidente"
            };

            return alert;
        }
    }
}
