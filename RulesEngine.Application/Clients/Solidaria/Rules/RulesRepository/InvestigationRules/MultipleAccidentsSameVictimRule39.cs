using NRules.Fluent.Dsl;
using RulesEngine.Domain.Common;
using RulesEngine.Domain.Invoices.Entities;
using RulesEngine.Domain.RulesEntities.Solidaria.Entities;

namespace RulesEngine.Application.Clients.Solidaria.Rules.RulesRepository.InvestigationRules
{
    public class MultipleAccidentsSameVictimRule39 : Rule
    {
        public override void Define()
        {
            InvoiceToCheckSolidaria? invoiceToCheck = default;

            When()
                .Match(() => invoiceToCheck!, x => x.ValidationAggregationRules_31_40 != null && x.ValidationAggregationRules_31_40.IdentificationNumberCase
                             .Count(c => c.IdentificationNumber == x.VictimId &&
                                     c.EventDate != x.EventDate.Value!.Value.ToString("yyyy-MM-dd")) >= x.ValidationAggregationRules_31_40.ParameterRule39And40);

            Then()
                .Do(w => invoiceToCheck!.AlertSolidaria.Add(CreateAlert(invoiceToCheck)));
        }

        private static AlertSolidaria CreateAlert(InvoiceToCheckSolidaria invoice)
        {
            int numberOfAccidents = invoice.ValidationAggregationRules_31_40.IdentificationNumberCase.Count(x => x.IdentificationNumber == invoice.VictimId &&
                        invoice.EventDate.Value!.Value.ToString("yyyy-MM-dd") != x.EventDate);

            return new AlertSolidaria
            {
                NameAction = "Alerta",
                Type = "Reglas de investigación",
                Module = "Reclamaciones",
                Description = "número de documento de identidad de la víctima en la víctima en la tabla de consulta, debe construir la llave \"siniestro\"  y contar la cantidad de siniestros diferentes en la tabla consulta (bd) , si la cantidad de siniestros es mayor a x ",
                Message = $"Para esta víctima existen {numberOfAccidents} eventos",
                Typification = string.Empty,
                HasPriority = false
            };
        }
    }
}