using Autofac;
using MongoDB.Driver;
using NRules.Fluent.Dsl;
using RulesEngine.Domain.Common;
using RulesEngine.Domain.RulesEntities.Solidaria.Entities;
using RulesEngine.Domain.ValueObjects;

namespace RulesEngine.Application.Clients.Solidaria.Rules.RulesRepository.InvestigationRules
{
    public class SamePlateDifferentEventDateWithoutEventInvestigationRule_36 : Rule
    {
        public override void Define()
        {
            InvoiceToCheckSolidaria? invoiceToCheck = default;

            When()
                .Match(() => invoiceToCheck!, x => x.DuplicatedInvoices.Where(d => d.LicensePlate == x.LicensePlate && Date.IsNotNullable(x.EventDate, d.EventDate) && Date.Different(x.EventDate, d.EventDate))
                                                                        .Select(y => new { y.LicensePlate, y.EventDate }).Distinct().Count()
                                                                                    > x.SamePlateDifferentEventNumber
                                                                                    && Date.IsNullable(x.InvestigationResponseDate));

            Then()
                .Do(w => invoiceToCheck!.Alerts.Add(CreateAlert()));
        }

        private Alert CreateAlert()
        {
            var alert = new Alert
            {
                AlertAction = "SendToInvestigation",
                AlertNameAction = "Enviar a investigar",
                AlertType = "Reglas de investigación",
                AlertDescription = "La placa en la tabla de origen es igual a la placa en la tabla de consulta, debe construir la llave \"Evento\" en la tabla de origen y contar la cantidad de eventos diferentes en la tabla consulta que estén por placa, si la cantidad de eventos es mayor a x y no tiene resultado de investigación asociado al siniestro",
                AlertMessage = "Se debe enviar a investigar, evento múltiple",
            };

            return alert;
        }
    }
}
