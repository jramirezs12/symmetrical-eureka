using NRules.Fluent.Dsl;
using RulesEngine.Domain.Common;
using RulesEngine.Domain.RulesEntities.Solidaria.Entities;

namespace RulesEngine.Application.Clients.Solidaria.Rules.RulesRepository.MedicalProtection
{
    public class MedicalExpensesAndTransportationExpensesStageIIRule3 : Rule
    {
        public override void Define()
        {
            InvoiceToCheckSolidaria? invoiceToCheck = default;

            When()
                .Match(() => invoiceToCheck, x => !string.IsNullOrEmpty(x.HelpType) && !string.IsNullOrEmpty(x.HelpTypeToValidate) && x.HelpType == x.HelpTypeToValidate);
            Then()
                .Do(w => invoiceToCheck!.Alerts.Add(CreateAlert()));
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
