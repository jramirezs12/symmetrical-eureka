using NRules.Fluent.Dsl;
using RulesEngine.Domain.Common;
using RulesEngine.Domain.RulesEntities.Solidaria.Entities;
using RulesEngine.Domain.ValueObjects;

namespace RulesEngine.Application.Clients.Solidaria.Rules.RulesRepository.DateRules
{
    public class EgressDateGreaterInvoiceDate_8 : Rule
    {
        public override void Define()
        {
            InvoiceToCheckSolidaria? invoiceToCheck = default;

            When()
                .Match(() => invoiceToCheck!, x => Date.IsNotNullable(x.EgressDate, x.InvoiceDate) && 
                                                   Date.GreaterThan(x.EgressDate, x.InvoiceDate));

            Then()
                .Do(w => invoiceToCheck!.Alerts.Add(CreateAlert()));
        }

        private static Alert CreateAlert()
        {
            var alert = new Alert
            {
                AlertAction = "DenyClaim",
                AlertNameAction = "Devolver Reclamación",
                AlertType = "Regla lógica de fechas",
                AlertDescription = "Permite validar si la fecha de egreso es posterior a la fecha de factura, lo que conlleva a la devolución de la reclamación",
                AlertMessage = "Se debe aplicar devolución teniendo en cuenta que el egreso no puede ser posterior a la facturación de la atención. Carta de devolución # xxx"
            };

            return alert;
        }
    }
}
