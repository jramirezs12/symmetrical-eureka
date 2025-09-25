using MongoDB.Bson;
using MongoDB.Driver;
using NRules.Fluent.Dsl;
using RulesEngine.Domain.Invoices.Repositories;
using RulesEngine.Domain.RulesEntities.Mundial.Entities;
using RulesEngine.Domain.Invoices.Entities;
using Autofac;
using NRules.Fluent;
using Autofac.Core;
using RulesEngine.Application.Actions;
using RulesEngine.Domain.Common;
using RulesEngine.Domain.ValueObjects;

namespace RulesEngine.Application.Clients.Mundial.Rules.RulesRepository.Fase_01.CoincidenceRules
{
    public class InvoiceValueRule_26 : Rule, ITrackableRule
    {
        public Action OnMatch { get; set; } = () => { };
        public override void Define()
        {
            InvoiceToCheck? invoiceToCheck = default;

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
