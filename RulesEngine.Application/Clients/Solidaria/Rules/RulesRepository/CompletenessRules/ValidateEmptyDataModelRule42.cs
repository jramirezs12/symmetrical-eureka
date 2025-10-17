using NRules.Fluent.Dsl;
using RulesEngine.Application.Actions;
using RulesEngine.Domain.Common;
using RulesEngine.Domain.RulesEntities.Solidaria.Entities;

namespace RulesEngine.Application.Clients.Solidaria.Rules.RulesRepository.CompletenessRules
{
    public class ValidateEmptyDataModelRule42 : Rule, ITrackableRule
    {
        public Action OnMatch { get; set; } = () => { };
        public override void Define()
        {
            InvoiceToCheckSolidaria? invoiceToCheck = default;

            When()
                .Match(() => invoiceToCheck!, x => x.TypeErrorsInModel != null && x.TypeErrorsInModel.Count() > 0);

            Then()
                .Do(w => invoiceToCheck!.AlertSolidaria.Add(CreateAlert(invoiceToCheck)))
                .Do(ctx => OnMatch());
        }

        private AlertSolidaria CreateAlert(InvoiceToCheckSolidaria invoiceToCheck)
        {
            var msg = invoiceToCheck.TypeErrorsInModel!.Count > 1
                ? $"Los campos {string.Join(", ", invoiceToCheck.TypeErrorsInModel!)} no se encuentran dentro de los valores permitidos en el anexo técnico FURIPS1"
                : $"El campo {invoiceToCheck.TypeErrorsInModel[0]} no se encuentra dentro de los valores permitidos en el anexo técnico FURIPS1";

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
