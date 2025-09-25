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

namespace RulesEngine.Application.Clients.Mundial.Rules.RulesRepository.Fase_01.DuplicatedEventRules
{
    public class SameSoatDifferentVictimIdRule_31 : Rule, ITrackableRule
    {
        public Action OnMatch { get; set; } = () => { };
        public override void Define()
        {
            InvoiceToCheck? invoiceToCheck = default;

            When()
                .Match(() => invoiceToCheck!, x => x.DuplicatedInvoices.Any(d => d.SoatNumber == x.SoatNumber
                                                                            &&
                                                                            (
                                                                                d.VictimId != x.VictimId
                                                                                || d.VictimId == x.VictimId && !Date.IsNullable(x.EventDate) && Date.Different(x.EventDate, d.EventDate)
                                                                            )));

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
                AlertType = "Regla duplicidad de siniestro",
                AlertDescription = "El número de póliza SOAT en la tabla origen es igual al número de de póliza SOAT en la tabla consulta y el siniestro en la tabla de origen es diferente en la tabla de consulta",
                AlertMessage = "La póliza ya tiene siniestros creeados, verifique si corresponde a otro siniestro"
            };

            return alert;
        }
    }
}
