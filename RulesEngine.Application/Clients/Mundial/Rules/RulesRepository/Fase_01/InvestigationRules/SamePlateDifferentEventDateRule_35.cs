using Autofac;
using MongoDB.Driver;
using NRules.Fluent.Dsl;
using RulesEngine.Application.Actions;
using RulesEngine.Domain.Common;
using RulesEngine.Domain.RulesEntities.Mundial.Entities;
using RulesEngine.Domain.ValueObjects;

namespace RulesEngine.Application.Clients.Mundial.Rules.RulesRepository.Fase_01.InvestigationRules
{
    public class SamePlateDifferentEventDateRule_35 : Rule, ITrackableRule
    {
        public Action OnMatch { get; set; } = () => { };
        public override void Define()
        {
            InvoiceToCheck? invoiceToCheck = default;

            When()
                .Match(() => invoiceToCheck!, x => x.DuplicatedInvoices.Where(d => d.LicensePlate == x.LicensePlate && !Date.IsNullable(x.EventDate) && Date.Different(x.EventDate, d.EventDate))
                                                                        .Select(y => new { y.LicensePlate, y.EventDate }).Distinct().Count()
                                                                        > x.SamePlateDifferentEventNumber);

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
                AlertDescription = "La placa en la tabla de origen es igual a la placa en la tabla de consulta, debe construir la llave \"Evento\" en la tabla de origen y contar la cantidad de eventos diferentes en la tabla consulta que estén por placa, si la cantidad de eventos es mayor a x",
                AlertMessage = "Evento múltiple"
            };

            return alert;
        }
    }
}
