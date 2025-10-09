using NRules.Fluent.Dsl;
using RulesEngine.Domain.RulesEntities.Mundial.Entities;
using RulesEngine.Domain.ValueObjects;
using RulesEngine.Domain.Common;
using RulesEngine.Domain.RulesEntities.Solidaria.Entities;
using RulesEngine.Application.Actions;

namespace RulesEngine.Application.Clients.Solidaria.Rules.RulesRepository.CoincidenceRules
{
    public class InvoiceValueRule_26 : Rule, ITrackableRule
    {
        public Action OnMatch { get; set; } = () => { };
        public override void Define()
        {
            InvoiceToCheckSolidaria? invoiceToCheck = default;

            When()
                .Match(() => invoiceToCheck!, x => !Currency.IsNullable(x.InvoiceValue, x.BilledMedicalExpenses, x.BilledTransportation)
                                                    && x.InvoiceValue.Value != x.BilledMedicalExpenses.Value + x.BilledTransportation.Value);

            Then()
                .Do(w => invoiceToCheck!.Alerts.Add(CreateAlert()))
                .Do(ctx => OnMatch());
        }

        private static Alert CreateAlert()
        {
            var alert = new Alert
            {
                AlertAction = "Alert",
                AlertNameAction = "Alerta",
                AlertType = "Regla de coincidencia",
                AlertDescription = "El total facturado de la tabla origen es diferente al valor factura de la tabla consulta",
                AlertMessage = "El valor total facturado descrito en el FURIPS I difiere del valor total facturado descrito en la factura de venta."
            };

            return alert;
        }
    }
}
