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
                .Do(ctx => invoiceToCheck!.AlertSolidaria.Add(CreateAlert(invoiceToCheck)));
        }

        private AlertSolidaria CreateAlert(InvoiceToCheckSolidaria invoiceToCheck)
        {
            var typification = invoiceToCheck.TypificationMap.GetValueOrDefault(GetType().Name, "Sin typification");
            bool haspriority = invoiceToCheck.HasPriorityMap.GetValueOrDefault(GetType().Name, false);

            return new AlertSolidaria
            {
                NameAction = "Enviar a Investigar",
                Type = "Regla de investigación",
                Module = "Reclamaciones",
                Description = "La placa del vehículo presenta más de un siniestro",
                Message = $"Se debe enviar a investigar, evento múltiple",
                Typification = typification,
                HasPriority = haspriority
            };
        }
    }
}