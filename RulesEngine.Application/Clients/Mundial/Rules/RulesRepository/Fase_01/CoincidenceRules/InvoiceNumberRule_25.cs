using Autofac;
using Autofac.Core;
using MongoDB.Bson;
using MongoDB.Driver;
using NRules.Fluent;
using NRules.Fluent.Dsl;
using RulesEngine.Application.Actions;
using RulesEngine.Domain.Common;
using RulesEngine.Domain.Invoices.Entities;
using RulesEngine.Domain.Invoices.Repositories;
using RulesEngine.Domain.RulesEntities.Mundial.Entities;

namespace RulesEngine.Application.Clients.Mundial.Rules.RulesRepository.Fase_01.CoincidenceRules
{
    public class InvoiceNumberRule_25 : Rule, ITrackableRule
    {
        public Action OnMatch { get; set; } = () => { };
        public override void Define()
        {
            InvoiceToCheck? invoiceToCheck = default;

            When()
                .Match(() => invoiceToCheck!, x => x.InvoiceNumberF1 != x.InvoiceNumberF2);

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
                AlertDescription = "El número de factura de la tabla origen es difernte al número de factura de la tabla consulta",
                AlertMessage = "El número de factura registrado en el FURIPS I es diferente al número de factura registrado en el FURIPS II"
            };

            return alert;
        }
    }
}
