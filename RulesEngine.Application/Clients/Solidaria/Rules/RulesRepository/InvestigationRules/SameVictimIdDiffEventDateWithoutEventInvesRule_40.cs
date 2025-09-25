using Autofac;
using MongoDB.Driver;
using NRules.Fluent.Dsl;
using RulesEngine.Domain.Common;
using RulesEngine.Domain.RulesEntities.Solidaria.Entities;
using RulesEngine.Domain.ValueObjects;

namespace RulesEngine.Application.Clients.Solidaria.Rules.RulesRepository.InvestigationRules
{
    public class SameVictimIdDifferentEventDateWithoutEventInvestigationRule_40 : Rule
    {
        public override void Define()
        {
            InvoiceToCheckSolidaria? invoiceToCheck = default;

            When()
                .Match(() => invoiceToCheck!, x => x.DuplicatedInvoices.Where(d => d.VictimId == x.VictimId && Date.IsNotNullable(x.EventDate, d.EventDate) && Date.Different(x.EventDate, d.EventDate))
                                                                        .Select(y => new { y.VictimId, y.EventDate }).Distinct().Count()
                                                                        > x.SameVictimIdDifferentEventNumber
                                                                        && Date.IsNullable(x.InvestigationResponseDate));

            Then()
                .Do(w => invoiceToCheck!.Alerts.Add(CreateAlert(invoiceToCheck)));
        }

        private Alert CreateAlert(InvoiceToCheckSolidaria invoiceToCheck)
        {
            var alert = new Alert
            {
                AlertAction = "SendToInvestigation",
                AlertNameAction = "Enviar a investigación",
                AlertType = "Reglas de investigación",
                AlertDescription = "El número de documento de identidad de la víctima en la tabla de origen es igual al número de documento de identidad de la víctima en la tabla de consulta, debe construir la llave \"siniestro\" en la tabla de origen y contar la cantidad de siniestros diferentes en la tabla consulta , si la cantidad de siniestros es mayor a x y no tiene resultado de investigación asociado al siniestro",
                AlertMessage = "Se debe enviar a investigar, evento múltiple",
            };

            return alert;
        }
    }
}
