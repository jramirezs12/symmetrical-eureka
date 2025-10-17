using NRules.Fluent.Dsl;
using RulesEngine.Application.Actions;
using RulesEngine.Domain.Common;
using RulesEngine.Domain.RulesEntities.Mundial.Entities;
using RulesEngine.Domain.RulesEntities.Solidaria.Entities;
using RulesEngine.Domain.ValueObjects;

namespace RulesEngine.Application.Clients.Solidaria.Rules.RulesRepository.Atypicality
{
    public class ClaimPendingAtypicalityStageIIRule8 : Rule, ITrackableRule
    {
        public Action OnMatch { get; set; } = () => { };
        public override void Define()
        {
            InvoiceToCheckSolidaria invoiceToCheck = default!;

            When()
                .Match(() => invoiceToCheck, x => x.AtypicalEvent != null && x.AtypicalEvent.Any(c=>c.LicensePlate == x.LicensePlate && c.VictimId == x.VictimId) 
                                                    && Date.IsNullable(x.InvestigationResponseDate));

            Then()
                .Do(w => invoiceToCheck.AlertSolidaria.Add(CreateAlert()))
                .Do(ctx => OnMatch());
        }

        private AlertSolidaria CreateAlert()
        {
            return new AlertSolidaria
            {
                NameAction = "Enviar a Calidad",
                Type = "Regla de Atipicidad",
                Module = "Reclamaciones",
                Description = "Valida si la reclamación tiene alertas de tipo Atipicidad y sin resultados de investigación",
                Message = "Asignación a calidad por Atipicidades",
                Typification = string.Empty,
                HasPriority = false
            };
        }
    }
}
