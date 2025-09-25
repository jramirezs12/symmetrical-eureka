using NRules.Fluent.Dsl;
using RulesEngine.Domain.Common;
using RulesEngine.Domain.RulesEntities.Mundial.Entities;
using RulesEngine.Domain.RulesEntities.Solidaria.Entities;
using RulesEngine.Domain.ValueObjects;

namespace RulesEngine.Application.Clients.Solidaria.Rules.RulesRepository.FraudAlert
{
    /// <summary>
    /// Assigns to Quality if fraud alerts are active
    /// </summary>
    public class QualityFraudAlertRule_13 : Rule
    {
        public Action OnMatch { get; set; } = () => { };
        public override void Define()
        {
            InvoiceToCheckSolidaria invoiceToCheck = default!;

            When()
                .Match(() => invoiceToCheck, x => x.AtypicalEvent != null && x.AtypicalEvent.Any(c => c.LicensePlate == x.LicensePlate && c.VictimId == x.VictimId)
                                                    && Date.IsNullable(x.InvestigationResponseDate));

            Then()
                .Do(w => invoiceToCheck.Alerts.Add(CreateAlert()))
                .Do(ctx => OnMatch());
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
