using NRules.Fluent.Dsl;
using RulesEngine.Application.Actions;
using RulesEngine.Domain.Common;
using RulesEngine.Domain.RulesEntities.Mundial.Entities;

namespace RulesEngine.Application.Clients.Mundial.Rules.RulesRepository.Fase_03.InvestigationRules
{
    public class VehiculeTypeWithMultipleAccidentsRule38 : Rule, ITrackableRule
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
                            c.LicensePlate != null
                            && c.VehicleType != null
                            && c.LicensePlate == x.LicensePlate
                            && c.VehicleType == x.VehicleType
                        ) >= x.ValidationAggregationRules_31_40.ParameterRule37And38
                );
            Then()
                .Do(ctx => invoiceToCheck!.Alerts.Add(CreateAlert(invoiceToCheck)))
                .Do(ctx => OnMatch());
        }

        private Alert CreateAlert(InvoiceToCheck invoiceToCheck)
        {
            var typification = invoiceToCheck.TypificationMap.GetValueOrDefault(GetType().Name, "Sin typification");
            bool haspriority = invoiceToCheck.HasPriorityMap.GetValueOrDefault(GetType().Name, false);

            var alert = new Alert
            {
                AlertAction = "Alert",
                AlertNameAction = "Enviar a Investigar",
                AlertType = "Regla de investigación",
                AlertDescription = "El número de placa del vehículo se encuentra en la tabla de consulta y cuenta con mas de un siniestro",
                AlertMessage = $"Se debe enviar a investigar,evento multiple",
                Typification = typification,
                HasPriority = haspriority
            };

            return alert;
        }
    }
}
