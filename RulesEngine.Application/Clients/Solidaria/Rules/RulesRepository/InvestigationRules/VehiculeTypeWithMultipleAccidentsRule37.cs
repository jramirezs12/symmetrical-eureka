using NRules.Fluent.Dsl;
using RulesEngine.Domain.Common;
using RulesEngine.Domain.Invoices.Entities;
using RulesEngine.Domain.RulesEntities.Solidaria.Entities;

namespace RulesEngine.Application.Clients.Solidaria.Rules.RulesRepository.InvestigationRules
{
    public class VehiculeTypeWithMultipleAccidentsRule37 : Rule
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

        private static Alert CreateAlert(InvoiceToCheckSolidaria invoice)
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
