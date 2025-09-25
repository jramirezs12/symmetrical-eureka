using NRules.Fluent.Dsl;
using RulesEngine.Application.Actions;
using RulesEngine.Domain.Common;
using RulesEngine.Domain.RulesEntities.Mundial.Entities;

namespace RulesEngine.Application.Clients.Mundial.Rules.RulesRepository.Fase_01.InvestigationRules
{
    public class VehiculeTypeWithMultipleAccidentsRule37 : Rule, ITrackableRule
    {
        public Action OnMatch { get; set; } = () => { };
        public override void Define()
        {
            InvoiceToCheck? invoiceToCheck = default;

            When()
               .Match(() => invoiceToCheck!, x => x.ValidationAggregationRules_31_40 != null
                    && x.LicensePlate != null
                    && x.VehicleType != null
                    && x.ValidationAggregationRules_31_40.LicensePlateCase
                        .Count(c =>
                            c.LicensePlate == x.LicensePlate
                            && c.VehicleType == x.VehicleType
                        ) >= x.ValidationAggregationRules_31_40.ParameterRule37And38
                );
            Then()
                .Do(ctx => invoiceToCheck!.Alerts.Add(CreateAlert(invoiceToCheck)))
                .Do(ctx => OnMatch());
        }

        private static Alert CreateAlert(InvoiceToCheck invoice)
        {
            int amountOftAccidents = invoice.ValidationAggregationRules_31_40.LicensePlateCase.Count(x => x.LicensePlate == invoice.LicensePlate && x.VehicleType == invoice.VehicleType);

            var alert = new Alert
            {
                AlertAction = "Alert",
                AlertNameAction = "Alerta",
                AlertType = "Regla de investigación",
                AlertDescription = "El número de placa del vehículo se encuentra en la tabla de consulta y cuenta con mas de un siniestro",
                AlertMessage = $"Para esta placa existen {amountOftAccidents} siniestros"
            };

            return alert;
        }
    }
}
