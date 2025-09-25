using MongoDB.Bson;
using MongoDB.Driver;
using NRules.Fluent.Dsl;
using RulesEngine.Domain.Invoices.Repositories;
using RulesEngine.Domain.RulesEntities.Solidaria.Entities;
using RulesEngine.Domain.Invoices.Entities;
using Autofac;
using NRules.Fluent;
using Autofac.Core;
using RulesEngine.Domain.Common;
using RulesEngine.Domain.RulesEntities.Mundial.Entities;

namespace RulesEngine.Application.Clients.Solidaria.Rules.RulesRepository.CoincidenceRules
{
    public class InvoiceNumberRule_25 : Rule
    {
        public override void Define()
        {
            InvoiceToCheck? invoiceToCheck = default;

            When()
                .Match(() => invoiceToCheck!, x => x.InvoiceNumberF1 != x.InvoiceNumberF2);

            Then()
                .Do(w => invoiceToCheck!.Alerts.Add(CreateAlert()));
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
