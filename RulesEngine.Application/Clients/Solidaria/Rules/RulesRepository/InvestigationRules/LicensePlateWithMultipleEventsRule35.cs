using MongoDB.Driver;
using NRules.Fluent.Dsl;
using RulesEngine.Domain.Common;
using RulesEngine.Domain.RulesEntities.Solidaria.Entities;

namespace RulesEngine.Application.Clients.Solidaria.Rules.RulesRepository.InvestigationRules
{
    public class LicensePlateWithMultipleEventsRule35 : Rule
    {

        public override void Define()
        {
            InvoiceToCheckSolidaria? invoiceToCheck = default;

            When()
               .Match(() => invoiceToCheck!, x => x.ValidationAggregationRules_31_40 != null && x.ValidationAggregationRules_31_40.LicensePlateCase
                     .Count(c => c.LicensePlate == x.LicensePlate &&
                          c.EventDate != x.EventDate.Value.Value.ToString("yyyy-MM-dd")) >= x.ValidationAggregationRules_31_40.ParameterRule35And36);
            Then()
                .Do(ctx => invoiceToCheck!.Alerts.Add(CreateAlert(invoiceToCheck)));
        }

        private static Alert CreateAlert(InvoiceToCheckSolidaria invoice)
        {
            var validations = invoice.ValidationAggregationRules_31_40;
            int eventCount = validations.LicensePlateCase.Count(x => invoice.LicensePlate == x.LicensePlate && invoice.EventDate.Value.Value.ToString("yyyy-MM-dd") != x.EventDate);
            var alert = new Alert
            {
                AlertAction = "Alert",
                AlertNameAction = "Alerta",
                AlertType = "Regla de investigación",
                AlertDescription = "El número de placa del vehículo se encuentra en la tabla de consulta con más de un evento",
                AlertMessage = $"Para esta placa existen {eventCount} Eventos"
            };

            return alert;
        }
    }
}
