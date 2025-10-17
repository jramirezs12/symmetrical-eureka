using Autofac;
using Autofac.Core;
using MongoDB.Bson;
using MongoDB.Driver;
using NRules.Fluent;
using NRules.Fluent.Dsl;
using RulesEngine.Domain.Common;
using RulesEngine.Domain.Invoices.Entities;
using RulesEngine.Domain.Invoices.Repositories;
using RulesEngine.Domain.RulesEntities.Solidaria.Entities;

namespace RulesEngine.Application.Clients.Solidaria.Rules.RulesRepository.InvestigationRules
{
    public class FraudulentIpsWithoutEventInvestigationRule_12 : Rule
    {
        public override void Define()
        {
            InvoiceToCheckSolidaria? invoiceToCheck = default;

            When()
                .Match(() => invoiceToCheck!, x => x.FraudulentIps != null
                                                    && x.IpsNit == x.FraudulentIps.IpsNit
                                                    && x.FraudulentIps.Result == "Envio a investigar"
                                                    && x.InvestigationResponseDate == null
                                                    );

            Then()
                .Do(w => invoiceToCheck!.AlertSolidaria.Add(CreateAlert(invoiceToCheck)));
        }

        private AlertSolidaria CreateAlert(InvoiceToCheckSolidaria invoiceToCheck)
        {
            string typification = invoiceToCheck.TypificationMap.GetValueOrDefault(GetType().Name, "Sin typification");
            bool haspriority = invoiceToCheck.HasPriorityMap.GetValueOrDefault(GetType().Name, false);
            return new AlertSolidaria
            {
                NameAction = "Enviar a Investigar",
                Type = "Reglas de investigación",
                Module = "Reclamaciones",
                Description = "El NIT de la IPS en la tabla de origen es igua al NIT de la IPS en la tabla de consulta y registra como acción \"Enviar a Investigar\" y el siniestro no tiene resultado de investigación",
                Message = "Se debe enviar a investigar, IPS se encuentra en la matriz de IPS con alto indice de fraude y no tiene investigación asociada",
                Typification = typification,
                HasPriority = haspriority
            };
        }
    }
}