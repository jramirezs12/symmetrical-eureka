using NRules.Fluent.Dsl;
using RulesEngine.Domain.Common;
using RulesEngine.Domain.RulesEntities.Solidaria.Entities;

namespace RulesEngine.Application.Clients.Solidaria.Rules.RulesRepository.FERules
{
    public class NitAndInvoiceFERule28 : Rule
    {
        public override void Define()
        {
            InvoiceToCheckSolidaria? invoiceToCheck = default;

            When()
                .Match(() => invoiceToCheck!, x => string.IsNullOrEmpty(x.IpsNitFE) || string.IsNullOrEmpty(x.InvoiceNumberFE) ||
                                            x.IpsNit != x.IpsNitFE && x.InvoiceNumberF1 != x.InvoiceNumberFE);

            Then()
                .Do(w => invoiceToCheck!.AlertSolidaria.Add(CreateAlert()));
        }

        private static AlertSolidaria CreateAlert()
        {
            return new AlertSolidaria
            {
                NameAction = "Devolver Reclamación",
                Type = "Regla Facturación Electronica",
                Module = "Reclamaciones",
                Description = "El NIT más número de factura de la tabla origen no coincide con el registrado en la tabla de consulta",
                Message = "Aplicar devolución, la reclamación debe contar con reporte de Facturación Electronica",
                Typification = string.Empty,
                HasPriority = false
            };
        }
    }
}