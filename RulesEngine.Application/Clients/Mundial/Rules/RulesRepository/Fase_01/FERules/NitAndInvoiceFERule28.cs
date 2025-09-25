using NRules.Fluent.Dsl;
using RulesEngine.Application.Actions;
using RulesEngine.Domain.Common;
using RulesEngine.Domain.RulesEntities.Mundial.Entities;

namespace RulesEngine.Application.Clients.Mundial.Rules.RulesRepository.Fase_01.FERules
{
    public class NitAndInvoiceFERule28 : Rule, ITrackableRule
    {
        public Action OnMatch { get; set; } = () => { };
        public override void Define()
        {
            InvoiceToCheck? invoiceToCheck = default;

            When()
                .Match(() => invoiceToCheck!, x => string.IsNullOrEmpty(x.IpsNitFE) || string.IsNullOrEmpty(x.InvoiceNumberFE) ||
                                            x.IpsNit != x.IpsNitFE && x.InvoiceNumberF1 != x.InvoiceNumberFE);

            Then()
                .Do(w => invoiceToCheck!.Alerts.Add(CreateAlert()))
                .Do(ctx => OnMatch());
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
