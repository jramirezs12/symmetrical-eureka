using NRules.Fluent.Dsl;
using RulesEngine.Domain.Common;
using RulesEngine.Domain.RulesEntities.Solidaria.Entities;

namespace RulesEngine.Application.Clients.Solidaria.Rules.RulesRepository.InvestigationRules
{
    public class IpsInvestigationRule_14 : Rule
    {
        public override void Define()
        {
            InvoiceToCheckSolidaria? invoiceToCheck = default;

            When()
                .Match(() => invoiceToCheck!, x => x.IpsInvestigation != null && x.IpsInvestigation.IpsNit == x.IpsNit);

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
                Description = "El NIT de la IPS en la tabla de origen es igual al NIT de la IPS en la tabla de consulta",
                Message = "IPS se encuentra en la matriz de IPS en esquema de investigación",
                Typification = string.Empty,
                HasPriority = false
            };
        }
    }
}