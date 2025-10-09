using NRules.Fluent.Dsl;
using RulesEngine.Application.Actions;
using RulesEngine.Domain.Common;
using RulesEngine.Domain.RulesEntities.Solidaria.Entities;

namespace RulesEngine.Application.Clients.Solidaria.Rules.RulesRepository.DuplicatedEventRules
{
    public class DuplicateSinisterSoatnumberRule31 : Rule, ITrackableRule
    {
        public Action OnMatch { get; set; } = () => { };

        public override void Define()
        {
            InvoiceToCheckSolidaria? invoiceToCheck = default;

            When()
                .Match(() => invoiceToCheck!, x => x.ValidationAggregationRules_31_40 != null && x.ValidationAggregationRules_31_40.SoatNumberCase
                                    .Any(c => c.EventDate != x.EventDate.Value.Value.ToString("yyyy-MM-dd") &&
                                         c.SoatNumber == x.SoatNumber &&
                                         c.IdentificationNumber != x.VictimId));

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
                AlertType = "Regla duplicidad de siniestro",
                AlertDescription = "El número de póliza SOAT en la tabla origen es igual al número de de póliza SOAT en la tabla consulta y el siniestro en la tabla de origen es diferente en la tabla de consulta",
                AlertMessage = "La póliza ya tiene siniestros creados, verifique si corresponde a otro siniestro"
            };

            return alert;
        }
    }
}
