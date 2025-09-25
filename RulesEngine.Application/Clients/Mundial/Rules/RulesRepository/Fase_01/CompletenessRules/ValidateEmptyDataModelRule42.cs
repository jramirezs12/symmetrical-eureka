using NRules.Fluent.Dsl;
using RulesEngine.Application.Actions;
using RulesEngine.Domain.Common;
using RulesEngine.Domain.RulesEntities.Mundial.Entities;

namespace RulesEngine.Application.Clients.Mundial.Rules.RulesRepository.Fase_01.CompletenessRules
{
    public class ValidateEmptyDataModelRule42 : Rule, ITrackableRule
    {

        public Action OnMatch { get; set; } = () => { };
        public override void Define()
        {
            InvoiceToCheck? invoiceToCheck = default;

            When()
                .Match(() => invoiceToCheck!, x => x.TypeErrorsInModel != null && x.TypeErrorsInModel.Count() > 0);

            Then()
                .Do(w => invoiceToCheck!.Alerts.Add(CreateAlert(invoiceToCheck)))
                .Do(ctx => OnMatch());
        }

        private Alert CreateAlert(InvoiceToCheck invoiceToCheck)
        {
            Alert alert = new Alert
            {
                AlertAction = "Alert",
                AlertNameAction = "Alerta",
                AlertType = "Regla de completitud",
                AlertDescription = "valida existencia de  de campos vacios para FURIPS 1",
                AlertMessage = invoiceToCheck.TypeErrorsInModel!.Count > 1 ? $" Los campos {string.Join(", ", invoiceToCheck.TypeErrorsInModel!)} no se encuentra dentro de los valores permitidos en el anexo tenico FURIPS1"
                : $"El campo {invoiceToCheck.TypeErrorsInModel[0]} no se encuentra dentro de los valores permitidos en el anexo tenico FURIPS1"
            };
            return alert;
        }
    }
}
