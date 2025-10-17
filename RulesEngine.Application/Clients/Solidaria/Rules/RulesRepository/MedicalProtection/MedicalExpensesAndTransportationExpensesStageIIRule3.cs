using NRules.Fluent.Dsl;
using RulesEngine.Application.Actions;
using RulesEngine.Domain.Common;
using RulesEngine.Domain.RulesEntities.Solidaria.Entities;

namespace RulesEngine.Application.Clients.Solidaria.Rules.RulesRepository.MedicalProtection
{
    public class MedicalExpensesAndTransportationExpensesStageIIRule3 : Rule, ITrackableRule
    {
        public Action OnMatch { get; set; } = () => { };
        public override void Define()
        {
            InvoiceToCheckSolidaria? invoiceToCheck = default;

            When()
                .Match(() => invoiceToCheck, x => !string.IsNullOrEmpty(x.HelpType) && !string.IsNullOrEmpty(x.HelpTypeToValidate) && x.HelpType == x.HelpTypeToValidate);
            Then()
                .Do(w => invoiceToCheck!.AlertSolidaria.Add(CreateAlert()))
                .Do(ctx => OnMatch());
        }
        private static AlertSolidaria CreateAlert()
        {
            return new AlertSolidaria
            {
                NameAction = "Enviar a Calidad",
                Type = "Regla por Amparo",
                Module = "Reclamaciones",
                Description = "Valida si el amparo de la reclamación es al menos uno de los parámetros",
                Message = "Asignación a calidad por Amparo",
                Typification = string.Empty,
                HasPriority = false
            };
        }
    }
}