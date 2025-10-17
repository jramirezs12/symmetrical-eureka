using NRules.Fluent.Dsl;
using RulesEngine.Domain.Common;
using RulesEngine.Domain.RulesEntities.Solidaria.Entities;

namespace RulesEngine.Application.Clients.Solidaria.Rules.RulesRepository.InvestigationRules
{
    public class FraudulentIpsToObjectionRule_13 : Rule
    {
        public override void Define()
        {
            InvoiceToCheckSolidaria? invoiceToCheck = default;

            When()
                .Match(() => invoiceToCheck!, x => x.FraudulentIps != null
                                                    && x.IpsNit == x.FraudulentIps.IpsNit
                                                    && x.FraudulentIps.Result == "Objetar todo");

            Then()
                .Do(w => invoiceToCheck!.AlertSolidaria.Add(CreateAlert()));
        }

        private static AlertSolidaria CreateAlert()
        {
            return new AlertSolidaria
            {
                NameAction = "Objetar la reclamación",
                Type = "Reglas de investigación",
                Module = "Reclamaciones",
                Description = "El NIT de la IPS en la tabla de origen es igua al NIT de la IPS en la tabla de consulta y registra como acción \"Objetar todo\"",
                Message = "Se debe aplicar la objeción total teniendo en cuenta que la IPS no se encuentra prestando los servicios en la sede habilitada. Carta de objeció # xxx",
                Typification = string.Empty,
                HasPriority = false
            };
        }
    }
}