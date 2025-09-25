using NRules.Fluent.Dsl;
using RulesEngine.Application.Actions;
using RulesEngine.Domain.Common;
using RulesEngine.Domain.RulesEntities.Mundial.Entities;

namespace RulesEngine.Application.Clients.Mundial.Rules.RulesRepository.Fase_02.MedicalProtection
{
    public class MedicalExpensesAndTransportationExpensesStageIIRule3 : Rule, ITrackableRule
    {
        public Action OnMatch { get; set; } = () => { };
        public override void Define()
        {
            InvoiceToCheck? invoiceToCheck = default;

            When()
                .Match(() => invoiceToCheck, x => !string.IsNullOrEmpty(x.HelpType) && !string.IsNullOrEmpty(x.HelpTypeToValidate) && x.HelpType == x.HelpTypeToValidate);
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
                AlertType = "Regla por Amparo",
                AlertDescription = "Valida si el amparo de la reclamación es al menos uno de los parametros",
                AlertMessage = "Asignación a calidad por Amparo"
            };

            return alert;
        }
    }
}
