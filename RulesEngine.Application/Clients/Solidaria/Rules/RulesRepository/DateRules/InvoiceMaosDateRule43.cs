using NRules.Fluent.Dsl;
using RulesEngine.Domain.Common;
using RulesEngine.Domain.RulesEntities.Solidaria.Entities;
using RulesEngine.Domain.ValueObjects;

namespace RulesEngine.Application.Clients.Solidaria.Rules.RulesRepository.DateRules
{
    public class InvoiceMaosDateRule43 : Rule
    {
        public override void Define()
        {
            InvoiceToCheckSolidaria? invoiceToCheck = default;

            When()
                .Match(() => invoiceToCheck!, x => Date.IsNotNullable(x.InvoiceMAOSDate, x.ClaimDate) && x.InvoiceMAOSDate.Value > x.ClaimDate.Value);

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
                AlertDescription = "Se valida si la fecha de factura proveedor MAOS es una fecha posterior a la fecha de aviso de la reclamación",
                AlertMessage = "La fecha de factura del proveedor MAOS no puede ser mayor a la fecha de aviso de la reclamación."
            };

            return alert;
        }
    }
}
