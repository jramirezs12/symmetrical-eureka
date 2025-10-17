using NRules.Fluent.Dsl;
using RulesEngine.Application.Actions;
using RulesEngine.Domain.Common;
using RulesEngine.Domain.RulesEntities.Solidaria.Entities;

namespace RulesEngine.Application.Clients.Solidaria.Rules.RulesRepository.CompletenessRules
{
    public class RequiredFieldsRule41 : Rule, ITrackableRule
    {
        public Action OnMatch { get; set; } = () => { };
        public override void Define()
        {
            InvoiceToCheckSolidaria? invoiceToCheck = default;

            When()
                .Match(() => invoiceToCheck!, x => x.NotNullErrorsInModel != null && x.NotNullErrorsInModel.Count() > 0);

            Then()
                .Do(w => invoiceToCheck!.AlertSolidaria.Add(CreateAlert(invoiceToCheck)))
                .Do(ctx => OnMatch());

        }

        private AlertSolidaria CreateAlert(InvoiceToCheckSolidaria invoiceToCheck)
        {
            var msg = invoiceToCheck.NotNullErrorsInModel!.Count > 1
                ? $"Los campos {string.Join(", ", invoiceToCheck.NotNullErrorsInModel!)} son obligatorios y no vienen registrados."
                : $"El campo {invoiceToCheck.NotNullErrorsInModel[0]} es obligatorio y no viene registrado.";

            return new AlertSolidaria
            {
                NameAction = "Alerta",
                Type = "Regla de completitud",
                Module = "Reclamaciones",
                Description = "Valida existencia de campos vacíos para FURIPS 1",
                Message = msg,
                Typification = string.Empty,
                HasPriority = false
            };
        }
    }
}
