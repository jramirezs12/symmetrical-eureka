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

namespace RulesEngine.Application.Clients.Mundial.Rules.RulesRepository.Fase_01.InvestigationRules
{
    public class IpsInvestigationRule_14 : Rule, ITrackableRule
    {
        public Action OnMatch { get; set; } = () => { };
        public override void Define()
        {
            InvoiceToCheck? invoiceToCheck = default;

            When()
                .Match(() => invoiceToCheck!, x => x.IpsInvestigation != null && x.IpsInvestigation.IpsNit == x.IpsNit);

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
                AlertType = "Reglas de investigación",
                AlertDescription = "El NIT de la IPS en la tabla de origen es igual al NIT de la IPS en la tabla de consulta",
                AlertMessage = "IPS se encuentra en la matriz de IPS en esquema de investigación"
            };

            return alert;
        }
    }
}
