using NRules.Fluent.Dsl;
using RulesEngine.Application.Actions;
using RulesEngine.Domain.Common;
using RulesEngine.Domain.RulesEntities.Mundial.Entities;
using RulesEngine.Domain.ValueObjects;

namespace RulesEngine.Application.Clients.Mundial.Rules.RulesRepository.Fase_01.DateRules
{
    public class IncomeDateGreaterEgressDate_5 : Rule, ITrackableRule
    {

        public Action OnMatch { get; set; } = () => { };
        public override void Define()
        {
            InvoiceToCheck? invoiceToCheck = default;

            When()
                .Match(() => invoiceToCheck!, x => Date.IsNotNullable(x.IncomeDate, x.EgressDate) && 
                                                   Date.GreaterThan(x.IncomeDate, x.EgressDate));

            Then()
                .Do(w => invoiceToCheck!.Alerts.Add(CreateAlert()))
                .Do(ctx => OnMatch());
        }

        private static Alert CreateAlert()
        {
            var alert = new Alert
            {
                AlertAction = "DenyClaim",
                AlertNameAction = "Devolver Reclamación",
                AlertType = "Regla lógica de fechas",
                AlertDescription = "Permite validar si la fecha de ingreso es posterior a la fecha de egreso de la reclamación, lo que conlleva a la devolución de la reclamación",
                AlertMessage = "Se debe aplicar devolución  teniendo en cuenta que el ingreso no puede ser posterior al egreso de la víctima a la institución. Carta de devolución # xxx"
            };

            return alert;
        }
    }
}
