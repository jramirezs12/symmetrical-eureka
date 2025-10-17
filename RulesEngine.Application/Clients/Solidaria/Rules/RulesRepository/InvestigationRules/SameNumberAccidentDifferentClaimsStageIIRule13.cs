using NRules.Fluent.Dsl;
using RulesEngine.Application.Actions;
using RulesEngine.Domain.Common;
using RulesEngine.Domain.RulesEntities.Solidaria.Entities;

namespace RulesEngine.Application.Clients.Solidaria.Rules.RulesRepository.InvestigationRules
{
    public class SameNumberAccidentDifferentClaimsStageIIRule13 : Rule, ITrackableRule
    {
        public Action OnMatch { get; set; } = () => { };
        public override void Define()
        {
            InvoiceToCheckSolidaria? invoiceToCheck = default;

            When()
                .Match(() => invoiceToCheck, x => x.Research != null && x.Research.Length > 0 && x.Research.Any(x => x.ResponseDate == null && string.IsNullOrEmpty(x.UserResponse)));
            Then()
                .Do(w => invoiceToCheck!.AlertSolidaria.Add(CreateAlert()))
                .Do(ctx => OnMatch());
        }
        private static AlertSolidaria CreateAlert()
        {
            return new AlertSolidaria
            {
                NameAction = "Enviar a Calidad",
                Type = "Regla por solicitud de investigación en curso",
                Module = "Reclamaciones",
                Description = "Valida si el número de siniestro tiene otra reclamación en proceso de investigación",
                Message = "La reclamación tiene una solicitud de investigación en curso",
                Typification = string.Empty,
                HasPriority = false
            };
        }
    }
}