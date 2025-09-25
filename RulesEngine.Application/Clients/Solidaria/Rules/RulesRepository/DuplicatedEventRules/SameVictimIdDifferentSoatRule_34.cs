using MongoDB.Driver;
using NRules.Fluent.Dsl;
using RulesEngine.Domain.Common;
using RulesEngine.Domain.RulesEntities.Solidaria.Entities;
using RulesEngine.Domain.ValueObjects;

namespace RulesEngine.Application.Clients.Solidaria.Rules.RulesRepository.DuplicatedEventRules
{
    public class SameVictimIdDifferentSoatRule_34 : Rule
    {
        public override void Define()
        {
            InvoiceToCheckSolidaria? invoiceToCheck = default;

            When()
                .Match(() => invoiceToCheck!, x => x.DuplicatedInvoices.Any(d => d.VictimId == x.VictimId && Date.Equal(x.EventDate, d.EventDate)
                                                                            && d.SoatNumber != x.SoatNumber));

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
                AlertDescription = "El siniestro en la tabla origen es igual al siniestro en la tabla de consulta y el número de póliza SOAT de la tabla origen es diferente al número de póliza SOAT de la tabla de consulta",
                AlertMessage = "El siniestro ya cuenta con un siniestro afectando otra póliza"
            };

            return alert;
        }
    }
}
