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
                .Do(w => invoiceToCheck!.AlertSolidaria.Add(CreateAlert(invoiceToCheck)));
        }

        private AlertSolidaria CreateAlert(InvoiceToCheckSolidaria invoiceToCheck)
        {
            string typification = invoiceToCheck.TypificationMap.GetValueOrDefault(GetType().Name, "Sin typification");
            bool haspriority = invoiceToCheck.HasPriorityMap.GetValueOrDefault(GetType().Name, false);
            return new AlertSolidaria
            {
                NameAction = "Enviar a investigar",
                Type = "Reglas de investigación",
                Module = "Reclamaciones",
                Description = "El número de documento de identidad de la víctima coincide y hay múltiples siniestros; no hay resultado de investigación",
                Message = "Se debe enviar a investigar, evento múltiple",
                Typification = typification,
                HasPriority = haspriority
            };
        }
    }
}