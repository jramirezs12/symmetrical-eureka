using NRules.Fluent.Dsl;
using RulesEngine.Domain.Common;
using RulesEngine.Domain.RulesEntities.Mundial.Entities;
using RulesEngine.Domain.ValueObjects;

namespace RulesEngine.Application.Clients.Solidaria.Rules.RulesRepository.Atypicality
{
    public class ClaimPendingAtypicalityStageIIRule8 : Rule
    {
        public override void Define()
        {
            InvoiceToCheck invoiceToCheck = default!;

            When()
                .Match(() => invoiceToCheck, x => x.AtypicalEvent != null && x.AtypicalEvent.Any(c=>c.LicensePlate == x.LicensePlate && c.VictimId == x.VictimId) 
                                                    && Date.IsNullable(x.InvestigationResponseDate));

            Then()
                .Do(w => invoiceToCheck.Alerts.Add(CreateAlert()));
        }

        private Alert CreateAlert()
        {
            Alert alert = new Alert
            {
                AlertAction = "SendToQuality",
                AlertNameAction = "Enviar a Calidad",
                AlertType = "Regla de Atipicidad",
                AlertDescription = "Valia si la reclamación tiene alertas de tipo Atipicidad  y sin resultados de investigación",
                AlertMessage = "Asignación a calidad por Atipicidades"
            };

            return alert;
        }
    }
}
