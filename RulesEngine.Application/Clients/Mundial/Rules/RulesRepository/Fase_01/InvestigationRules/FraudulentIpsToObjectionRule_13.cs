using NRules.Fluent.Dsl;
using RulesEngine.Application.Actions;
using RulesEngine.Domain.Common;
using RulesEngine.Domain.RulesEntities.Mundial.Entities;

namespace RulesEngine.Application.Clients.Mundial.Rules.RulesRepository.Fase_01.InvestigationRules
{
    public class FraudulentIpsToObjectionRule_13 : Rule, ITrackableRule
    {
        public Action OnMatch { get; set; } = () => { };
        public override void Define()
        {
            InvoiceToCheck? invoiceToCheck = default;

            When()
                .Match(() => invoiceToCheck!, x => x.FraudulentIps != null
                                                    && x.IpsNit == x.FraudulentIps.IpsNit
                                                    && x.FraudulentIps.Result == "Objetar todo");

            Then()
                .Do(w => invoiceToCheck!.Alerts.Add(CreateAlert()))
                .Do(ctx => OnMatch());
        }

        private static Alert CreateAlert()
        {
            var alert = new Alert
            {
                AlertAction = "ClaimObjection",
                AlertNameAction = "Objetar la reclamación",
                AlertType = "Reglas de investigación",
                AlertDescription = "El NIT de la IPS en la tabla de origen es igua al NIT de la IPS en la tabla de consulta y registra como acción \"Objetar todo\"",
                AlertMessage = "Se debe aplicar la objeción total teniendo en cuenta que la IPS no se encuentra prestando los servicios en la sede habilitada. Carta de objeció # xxx"
            };

            return alert;
        }
    }
}
