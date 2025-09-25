using MongoDB.Driver;
using NRules.Fluent.Dsl;
using RulesEngine.Domain.Common;
using RulesEngine.Domain.RulesEntities.Solidaria.Entities;
using RulesEngine.Domain.ValueObjects;

namespace RulesEngine.Application.Clients.Solidaria.Rules.RulesRepository.DuplicatedEventRules
{
    public class SameSoatDifferentVictimIdRule_31 : Rule
    {
        public override void Define()
        {
            InvoiceToCheckSolidaria? invoiceToCheck = default;

            When()
                .Match(() => invoiceToCheck!, x => x.DuplicatedInvoices.Any(d => d.SoatNumber == x.SoatNumber
                                                                            &&
                                                                            (
                                                                                d.VictimId != x.VictimId
                                                                                || d.VictimId == x.VictimId && !Date.IsNullable(x.EventDate) && Date.Different(x.EventDate, d.EventDate)
                                                                            )));

            Then()
                .Do(w => invoiceToCheck!.Alerts.Add(CreateAlert()));
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
