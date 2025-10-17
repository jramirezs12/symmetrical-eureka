using NRules.Fluent.Dsl;
using RulesEngine.Domain.Common;
using RulesEngine.Domain.RulesEntities.Solidaria.Entities;

namespace RulesEngine.Application.Clients.Solidaria.Rules.RulesRepository.InvoiceNumber
{
    public class InvoiceNumberOnParametersStageIIRule2 : Rule
    {
        public override void Define()
        {
            InvoiceToCheckSolidaria? invoiceToCheck = default;

            When()
                .Match(() => invoiceToCheck!, x => !string.IsNullOrEmpty(x.InvoiceNumber) && x.InvoiceNumberFile.Any(n => x.InvoiceNumber == n.InvoiceNumber));

            Then()
                .Do(w => invoiceToCheck!.AlertSolidaria.Add(CreateAlert()));
        }

        private static AlertSolidaria CreateAlert()
        {
            return new AlertSolidaria
            {
                NameAction = "Enviar a Calidad",
                Type = "Regla por número de factura",
                Module = "Reclamaciones",
                Description = "Si el número de factura registrado en la reclamación coincide con uno activo en parámetros",
                Message = "Asignación a calidad por Factura.",
                Typification = string.Empty,
                HasPriority = false
            };
        }
    }
}