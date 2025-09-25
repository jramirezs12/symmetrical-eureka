using Autofac;
using MongoDB.Driver;
using NRules.Fluent.Dsl;
using RulesEngine.Application.Actions;
using RulesEngine.Domain.Common;
using RulesEngine.Domain.RulesEntities.Mundial.Entities;
using RulesEngine.Domain.ValueObjects;

namespace RulesEngine.Application.Clients.Mundial.Rules.RulesRepository.Fase_01.InvestigationRules
{
    public class SameVictimIdDifferentEventDateRule_39 : Rule, ITrackableRule
    {
        public Action OnMatch { get; set; } = () => { };
        public override void Define()
        {
            InvoiceToCheck? invoiceToCheck = default;

            When()
                .Match(() => invoiceToCheck!, x => x.DuplicatedInvoices.Where(d => d.VictimId == x.VictimId && Date.IsNotNullable(x.EventDate, d.EventDate) && Date.Different(x.EventDate, d.EventDate))
                                                                        .Select(y => new { y.VictimId, y.EventDate }).Distinct().Count()
                                                                        > Convert.ToInt32(x.SameVictimIdDifferentEventNumber));

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
                AlertDescription = "El número de documento de identidad de la víctima en la tabla de origen es igual al número de documento de identidad de la víctima en la tabla de consulta, debe construir la llave \"siniestro\" en la tabla de origen y contar la cantidad de siniestros diferentes en la tabla consulta, si la cantidad de siniestros es mayor a x",
                AlertMessage = "Evento múltiple"
            };

            return alert;
        }
    }
}
