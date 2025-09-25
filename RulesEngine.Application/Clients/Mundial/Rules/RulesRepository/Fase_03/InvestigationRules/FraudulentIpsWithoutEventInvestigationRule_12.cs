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

namespace RulesEngine.Application.Clients.Mundial.Rules.RulesRepository.Fase_03.InvestigationRules
{
    public class FraudulentIpsWithoutEventInvestigationRule_12 : Rule, ITrackableRule
    {
        public Action OnMatch { get; set; } = () => { };
        public override void Define()
        {

            InvoiceToCheck? invoiceToCheck = default;

            When()
                .Match(() => invoiceToCheck!, x => x.FraudulentIps != null
                                                    && x.IpsNit == x.FraudulentIps.IpsNit
                                                    && x.FraudulentIps.Result == "Envio a investigar"
                                                    && x.InvestigationResponseDate == null
                                                    );

            Then()
                .Do(w => invoiceToCheck!.Alerts.Add(CreateAlert(invoiceToCheck)))
                .Do(ctx => OnMatch());
        }

        private Alert CreateAlert(InvoiceToCheck invoiceToCheck)
        {
            string typification = invoiceToCheck.TypificationMap.GetValueOrDefault(GetType().Name, "Sin typification");
            bool haspriority = invoiceToCheck.HasPriorityMap.GetValueOrDefault(GetType().Name, false);
            var alert = new Alert
            {
                AlertAction = "SendToInvestigation",
                AlertNameAction = "Enviar a Investigar",
                AlertType = "Reglas de investigación",
                AlertDescription = "El NIT de la IPS en la tabla de origen es igua al NIT de la IPS en la tabla de consulta y registra como acción \"Enviar a Investigar\" y el siniestro no tiene resultado de investigación",
                AlertMessage = "Se debe enviar a investigar, IPS se encuentra en la matriz de IPS con alto indice de fraude y no tiene investigación asociada",
                Typification = typification,
                HasPriority = haspriority
            };

            return alert;
        }
    }
}
