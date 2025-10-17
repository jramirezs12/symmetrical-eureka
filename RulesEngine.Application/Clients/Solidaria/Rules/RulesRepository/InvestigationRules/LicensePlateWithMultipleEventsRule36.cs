using NRules.Fluent.Dsl;
using RulesEngine.Domain.Common;
using RulesEngine.Domain.RulesEntities.Solidaria.Entities;

namespace RulesEngine.Application.Clients.Solidaria.Rules.RulesRepository.InvestigationRules
{
    public class LicensePlateWithMultipleEventsRule36 : Rule
    {
        public override void Define()
        {
            InvoiceToCheckSolidaria? invoiceToCheck = default;

            When()
               .Match(() => invoiceToCheck!, x => x.ValidationAggregationRules_31_40 != null && x.ValidationAggregationRules_31_40.LicensePlateCase
                     .Count(c => c.LicensePlate == x.LicensePlate &&
                          c.EventDate != x.EventDate.Value.Value.ToString("yyyy-MM-dd")) >= x.ValidationAggregationRules_31_40.ParameterRule35And36);
            Then()
                .Do(ctx => invoiceToCheck!.AlertSolidaria.Add(CreateAlert(invoiceToCheck)));
        }

        private AlertSolidaria CreateAlert(InvoiceToCheckSolidaria invoiceToCheck)
        {
            return new AlertSolidaria
            {
                NameAction = "Enviar a Investigar",
                Type = "Regla de investigación",
                Module = "Reclamaciones",
                Description = "El número de placa del vehículo se encuentra en la tabla de consulta con más de un evento",
                Message = $"Se debe enviar a investigar,evento multiple",
                Typification = string.Empty,
                HasPriority = false
            };
        }
    }
}