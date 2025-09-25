using NRules.Fluent.Dsl;
using RulesEngine.Domain.Common;
using RulesEngine.Domain.RulesEntities.Solidaria.Entities;

namespace RulesEngine.Application.Clients.Solidaria.Rules.RulesRepository.InvestigationRules
{
    public class VehiculeTypeWithMultipleAccidentsRule38 : Rule
    {
        public override void Define()
        {
            InvoiceToCheckSolidaria? invoiceToCheck = default;

            When()
               .Match(() => invoiceToCheck!, x => x.ValidationAggregationRules_31_40 != null && x.ValidationAggregationRules_31_40.LicensePlateCase
                        .Count(c => c.LicensePlate == x.LicensePlate &&
                            c.VehicleType == x.VehicleType) >= x.ValidationAggregationRules_31_40.ParameterRule37And38);
            Then()
                .Do(ctx => invoiceToCheck!.Alerts.Add(CreateAlert(invoiceToCheck)));
        }

        private Alert CreateAlert(InvoiceToCheckSolidaria invoiceToCheck)
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
