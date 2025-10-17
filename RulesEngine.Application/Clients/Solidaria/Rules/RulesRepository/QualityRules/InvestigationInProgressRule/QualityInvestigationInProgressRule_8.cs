using NRules.Fluent.Dsl;
using RulesEngine.Domain.Common;
using RulesEngine.Domain.RulesEntities.Solidaria.Entities;

namespace RulesEngine.Application.Clients.Solidaria.Rules.RulesRepository.InvestigationInProgress
{
    /// <summary>
    /// Assigns to Quality if another claim under same case has ongoing investigation
    /// </summary>
    public class QualityInvestigationInProgressRule_8 : Rule
    {
        public Action OnMatch { get; set; } = () => { };
        public override void Define()
        {
            InvoiceToCheckSolidaria? invoiceToCheck = default;

            When()
                .Match(() => invoiceToCheck, x => x.Research != null && x.Research.Length > 0 && x.Research.Any(r => r.ResponseDate == null && string.IsNullOrEmpty(r.UserResponse)));
            Then()
                .Do(w => invoiceToCheck!.AlertSolidaria.Add(CreateAlert()))
                .Do(ctx => OnMatch());
        }
        private static AlertSolidaria CreateAlert()
        {
            return new AlertSolidaria
            {
                NameAction = "Enviar a Calidad",
                Type = "Regla por solicitud de investigaci�n en curso",
                Module = "Reclamaciones",
                Description = "Valida si el n�mero de siniestro tiene otra reclamaci�n en proceso de investigaci�n",
                Message = "La reclamaci�n tiene una solicitud de investigaci�n en curso",
                Typification = string.Empty,
                HasPriority = false
            };
        }
    }
}