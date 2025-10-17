using NRules.Fluent.Dsl;
using RulesEngine.Application.Actions;
using RulesEngine.Domain.Common;
using RulesEngine.Domain.RulesEntities.Mundial.Entities;
using RulesEngine.Domain.RulesEntities.Solidaria.Entities;

namespace RulesEngine.Application.Clients.Solidaria.Rules.RulesRepository.ClaimantIdRule
{
    public class ClamaintIdIpsNitStageIIRule1 : Rule, ITrackableRule
    {
        public Action OnMatch { get; set; } = () => { };
        public override void Define()
        {
            InvoiceToCheckSolidaria? invoiceToCheck = default;

            When()
                .Match(() => invoiceToCheck, x => x.IpsNitList!.Any(c=>c.NitIps == x.IpsNit));
            Then()
                .Do(w => invoiceToCheck!.AlertSolidaria.Add(CreateAlert()))
                .Do(ctx => OnMatch());
        }
        private static AlertSolidaria CreateAlert()
        {
            return new AlertSolidaria
            {
                NameAction = "Enviar a Calidad",
                Type = "Regla por Id Reclamante",
                Module = "Reclamaciones",
                Description = "Valida si el NIT de la reclamación se encuentra en el archivo matriz",
                Message = "Asignación a calidad por NIT",
                Typification = string.Empty,
                HasPriority = false
            };
        }
    }
}
