using NRules.Fluent.Dsl;
using RulesEngine.Application.Actions;
using RulesEngine.Domain.Common;
using RulesEngine.Domain.RulesEntities.Solidaria.Entities;
using RulesEngine.Domain.ValueObjects;

namespace RulesEngine.Application.Clients.Solidaria.Rules.RulesRepository.DateRules
{
    public class IncomeDateGreaterEgressDate_5 : Rule, ITrackableRule
    {
        public Action OnMatch { get; set; } = () => { };

        public override void Define()
        {
            InvoiceToCheckSolidaria? invoiceToCheck = default;

            When()
                .Match(() => invoiceToCheck!, x => Date.IsNotNullable(x.IncomeDate, x.EgressDate) && 
                                                   Date.GreaterThan(x.IncomeDate, x.EgressDate));

            Then()
                .Do(w => invoiceToCheck!.AlertSolidaria.Add(CreateAlert()))
                .Do(ctx => OnMatch());
        }

        private static AlertSolidaria CreateAlert()
        {
            return new AlertSolidaria
            {
                NameAction = "Devolver Reclamación",
                Type = "Regla lógica de fechas",
                Module = "Reclamaciones",
                Description = "Permite validar si la fecha de ingreso es posterior a la fecha de egreso de la reclamación, lo que conlleva a la devolución de la reclamación",
                Message = "Se debe aplicar devolución  teniendo en cuenta que el ingreso no puede ser posterior al egreso de la víctima a la institución. Carta de devolución # xxx",
                Typification = string.Empty,
                HasPriority = false
            };
        }
    }
}
