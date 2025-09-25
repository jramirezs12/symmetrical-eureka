using MongoDB.Driver.Core.Operations;
using NRules.Fluent.Dsl;
using RulesEngine.Application.Actions;
using RulesEngine.Domain.Common;
using RulesEngine.Domain.RulesEntities.Mundial.Entities;

namespace RulesEngine.Application.Clients.Mundial.Rules.RulesRepository.Fase_01.CompletenessRules
{
    public class RequiredFieldsRule41 : Rule, ITrackableRule
    {
        public Action OnMatch { get; set; } = () => { };
        public override void Define()
        {
            InvoiceToCheck? invoiceToCheck = default;

            When()
                .Match(() => invoiceToCheck!, x => x.NotNullErrorsInModel != null && x.NotNullErrorsInModel.Count() > 0);

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
                AlertMessage = invoiceToCheck.NotNullErrorsInModel!.Count > 1 ? $" Los campos {string.Join(", ", invoiceToCheck.NotNullErrorsInModel!)} son obligatorios y no vienen registrados."
                : $"El campo {invoiceToCheck.NotNullErrorsInModel[0]} es obligatorio y no viene registrado."
            };
            return alert;
        }
    }
}
