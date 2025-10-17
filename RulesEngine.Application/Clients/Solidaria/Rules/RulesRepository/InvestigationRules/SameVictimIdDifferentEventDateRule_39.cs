using Autofac;
using MongoDB.Driver;
using NRules.Fluent.Dsl;
using RulesEngine.Domain.Common;
using RulesEngine.Domain.RulesEntities.Solidaria.Entities;
using RulesEngine.Domain.ValueObjects;

namespace RulesEngine.Application.Clients.Solidaria.Rules.RulesRepository.InvestigationRules
{
    public class SameVictimIdDifferentEventDateRule_39 : Rule
    {
        public override void Define()
        {
            InvoiceToCheckSolidaria? invoiceToCheck = default;

            When()
                .Match(() => invoiceToCheck!, x => x.DuplicatedInvoices.Where(d => d.VictimId == x.VictimId && Date.IsNotNullable(x.EventDate, d.EventDate) && Date.Different(x.EventDate, d.EventDate))
                                                                        .Select(y => new { y.VictimId, y.EventDate }).Distinct().Count()
                                                                        > Convert.ToInt32(x.SameVictimIdDifferentEventNumber));

            Then()
                .Do(w => invoiceToCheck!.AlertSolidaria.Add(CreateAlert()));
        }

        private static AlertSolidaria CreateAlert()
        {
            return new AlertSolidaria
            {
                NameAction = "Alerta",
                Type = "Reglas de investigación",
                Module = "Reclamaciones",
                Description = "El número de documento de identidad de la víctima en la tabla de origen es igual a la de la tabla de consulta; construir llave \"siniestro\" y contar siniestros distintos; si supera el parámetro",
                Message = "Evento múltiple",
                Typification = string.Empty,
                HasPriority = false
            };
        }
    }
}