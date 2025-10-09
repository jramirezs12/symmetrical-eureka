using NRules.Fluent.Dsl;
using RulesEngine.Application.Actions;
using RulesEngine.Domain.Common;
using RulesEngine.Domain.RulesEntities.Solidaria.Entities;

namespace RulesEngine.Application.Clients.Solidaria.Rules.RulesRepository.DuplicatedEventRules
{
    public class SameRadicateDifferentPolicyRule34 : Rule, ITrackableRule

    {
        public Action OnMatch { get; set; } = () => { };

        public override void Define()
        {
            InvoiceToCheckSolidaria? invoiceToCheck = default;

            When()
                .Match(() => invoiceToCheck!, x => x.ValidationAggregationRules_31_40 != null && x.ValidationAggregationRules_31_40.IdentificationNumberCase
                             .Any(c => c.IdentificationNumber == x.VictimId &&
                                  c.EventDate == x.EventDate.Value.Value.ToString("yyyy-MM-dd") && c.SoatNumber != x.SoatNumber));

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
                AlertDescription = "El número de identificación de la victima y la fecha de ocurrencia del evento es igual a la tabla consulta pero el número de póliza SOAT es diferente documento en la tabla de origen es diferente en la tabla de consulta",
                AlertMessage = "El siniestro ya cuenta con un siniestro afectando otra póliza"
            };

            return alert;
        }
    }
}
