using NRules.Fluent.Dsl;
using RulesEngine.Domain.Common;
using RulesEngine.Domain.RulesEntities.Solidaria.Entities;

namespace RulesEngine.Application.Clients.Solidaria.Rules.RulesRepository.InvestigationRules
{
    public class SameNumberAccidentDifferentClaimsStageIIRule13 : Rule
    {
        public override void Define()
        {
            InvoiceToCheckSolidaria? invoiceToCheck = default;

            When()
                .Match(() => invoiceToCheck, x => x.Research != null && x.Research.Length > 0 && x.Research.Any(x => x.ResponseDate == null && string.IsNullOrEmpty(x.UserResponse)));
            Then()
                .Do(w => invoiceToCheck!.Alerts.Add(CreateAlert()));
        }
        private static Alert CreateAlert()
        {
            var alert = new Alert
            {
                AlertAction = "SendToQuality",
                AlertNameAction = "Enviar a Calidad",
                AlertType = "Regla por solicitud de investigación en curso",
                AlertDescription = "Valida si el número de siniestro tiene otra reclamación en proceso de investigación",
                AlertMessage = "La reclamación tiene una solicitud de investigación en curso"
            };

            return alert;
        }
    }
}
