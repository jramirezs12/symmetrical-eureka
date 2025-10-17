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
                .Do(w => invoiceToCheck!.AlertSolidaria.Add(CreateAlert()))
                .Do(ctx => OnMatch());
        }

        private static AlertSolidaria CreateAlert()
        {
            return new AlertSolidaria
            {
                NameAction = "Alerta",
                Type = "Regla de coincidencia",
                Module = "Reclamaciones",
                Description = "El total facturado de la tabla origen es diferente al valor factura de la tabla consulta",
                Message = "El valor total facturado descrito en el FURIPS I difiere del valor total facturado descrito en la factura de venta.",
                Typification = string.Empty,
                HasPriority = false
            };
        }
    }
}
