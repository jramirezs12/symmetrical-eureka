using NRules.Fluent.Dsl;
using RulesEngine.Domain.Common;
using RulesEngine.Domain.RulesEntities.Solidaria.Entities;
using RulesEngine.Domain.ValueObjects;

namespace RulesEngine.Application.Clients.Solidaria.Rules.RulesRepository.PaymentRules
{
    public class InvoiceValueSameTotalApprovedValueStageIIRule7 : Rule
    {
        public override void Define()
        {
            InvoiceToCheckSolidaria? invoiceToCheck = default;

            When()
                .Match(() => invoiceToCheck, x => x.InvoiceValue.Value > 0 && x.TotalAuthorizedValue.Value > 0  && Equals(x.InvoiceValue,x.TotalAuthorizedValue));

            Then()
                .Do(w => invoiceToCheck!.Alerts.Add(CreateAlert()));
        }

        private Alert CreateAlert()
        {
            Alert alert = new Alert
            {
                AlertAction = "SendToQuality",
                AlertNameAction = "Enviar a Calidad",
                AlertType = "Regla de asignación a calidad - por pago total",
                AlertDescription = "Valida si el total de la factura es igual al total autorizado",
                AlertMessage = "Asignación a calidad por Pago Total"
            };
            return alert;
        }
    }
}
