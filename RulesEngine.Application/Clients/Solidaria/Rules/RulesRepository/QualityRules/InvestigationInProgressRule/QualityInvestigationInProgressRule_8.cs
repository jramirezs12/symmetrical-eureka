using NRules.Fluent.Dsl;
using RulesEngine.Domain.Common;
using RulesEngine.Domain.RulesEntities.Mundial.Entities;
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
                .Match(() => invoiceToCheck, x => x.Research != null && x.Research.Length > 0 && x.Research.Any(x => x.ResponseDate == null && string.IsNullOrEmpty(x.UserResponse)));
            Then()
                .Do(w => invoiceToCheck!.Alerts.Add(CreateAlert()))
                .Do(ctx => OnMatch());
        }
        private static Alert CreateAlert()
        {
            var alert = new Alert
            {
                AlertAction = "SendToQuality",
                AlertNameAction = "Enviar a Calidad",
                AlertType = "Regla por solicitud de investigaci�n en curso",
                AlertDescription = "Valida si el n�mero de siniestro tiene otra reclamaci�n en proceso de investigaci�n",
                AlertMessage = "La reclamaci�n tiene una solicitud de investigaci�n en curso"
            };

            return alert;
        }
    }
}
