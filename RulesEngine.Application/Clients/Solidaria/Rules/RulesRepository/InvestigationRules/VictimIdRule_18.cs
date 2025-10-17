using MongoDB.Driver;
using Nrules = NRules.Fluent.Dsl;
using RulesEngine.Domain.Common;
using RulesEngine.Domain.RulesEntities.Solidaria.Entities;

namespace RulesEngine.Application.Clients.Solidaria.Rules.RulesRepository.InvestigationRules
{
    public class VictimIdRule_18 : Nrules.Rule
    {
        public override void Define()
        {
            InvoiceToCheckSolidaria? invoiceToCheck = default;

            When()
                .Match(() => invoiceToCheck!, x => x.AtypicalEvent != null && x.AtypicalEvent.Any(y => y.VictimId == x.VictimId));

            Then()
                .Do(w => invoiceToCheck!.AlertSolidaria.Add(CreateAlert(invoiceToCheck)));
        }

        private static AlertSolidaria CreateAlert(InvoiceToCheckSolidaria invoiceToCheck)
        {
            return new AlertSolidaria
            {
                NameAction = "Alerta",
                Type = "Reglas de investigación",
                Module = "Reclamaciones",
                Description = "El número de documento de la víctima coincide",
                Message = $"El número de documento de identidad de la víctima presenta la siguiente alerta: {invoiceToCheck.AtypicalEvent!.First(x => x.VictimId == invoiceToCheck.VictimId).AlertDescription}",
                Typification = string.Empty,
                HasPriority = false
            };
        }
    }
}