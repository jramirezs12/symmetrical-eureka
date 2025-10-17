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
                .Do(ctx => invoiceToCheck!.AlertSolidaria.Add(CreateAlert(invoiceToCheck)));
        }

        private static AlertSolidaria CreateAlert(InvoiceToCheckSolidaria invoice)
        {
            int amountOftAccidents = invoice.ValidationAggregationRules_31_40.LicensePlateCase.Count(x => x.LicensePlate == invoice.LicensePlate && x.VehicleType == invoice.VehicleType);

            return new AlertSolidaria
            {
                NameAction = "Alerta",
                Type = "Regla de investigación",
                Module = "Reclamaciones",
                Description = "La placa del vehículo presenta más de un siniestro",
                Message = $"Para esta placa existen {amountOftAccidents} siniestros",
                Typification = string.Empty,
                HasPriority = false
            };
        }
    }
}