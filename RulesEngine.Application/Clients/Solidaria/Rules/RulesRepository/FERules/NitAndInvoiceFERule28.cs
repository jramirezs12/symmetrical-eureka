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
                .Do(w => invoiceToCheck!.Alerts.Add(CreateAlert()));
        }

        private static Alert CreateAlert()
        {
            var alert = new Alert
            {
                AlertAction = "DenyClaim",
                AlertNameAction = "Devolver Reclamación",
                AlertType = "Regla Facturación Electronica",
                AlertDescription = "El NIT más número de factura de la tabla origen no coincide con el registrado en la tabla de consulta",
                AlertMessage = "Aplicar devolución, la reclamación debe contar con reporte de Facturación Electronica"
            };

            return alert;
        }
    }
}
