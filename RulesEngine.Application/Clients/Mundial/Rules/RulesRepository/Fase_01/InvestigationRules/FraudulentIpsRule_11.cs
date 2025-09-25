using NRules.Fluent.Dsl;
using RulesEngine.Application.Actions;
using RulesEngine.Domain.Common;
using RulesEngine.Domain.RulesEntities.Mundial.Entities;

namespace RulesEngine.Application.Clients.Mundial.Rules.RulesRepository.Fase_01.InvestigationRules
{
    public class FraudulentIpsRule_11 : Rule, ITrackableRule
    {
        public Action OnMatch { get; set; } = () => { };
        public override void Define()
        {
            InvoiceToCheck? invoiceToCheck = default;

            When()
                .Match(() => invoiceToCheck!, x => x.FraudulentIps != null
                                                    && x.IpsNit == x.FraudulentIps.IpsNit
                                                    && x.FraudulentIps.Result == "Envio a investigar");

            Then()
                .Do(w => invoiceToCheck!.Alerts.Add(CreateAlert()))
                .Do(ctx => OnMatch());
        }

        private static Alert CreateAlert()
        {
            var alert = new Alert
            {
                AlertAction = "Alert",
                AlertNameAction = "Alerta",
                AlertType = "Reglas de investigación",
                AlertDescription = "El NIT de la IPS en la tabla de origen es igual al NIT de la IPS en la tabla de consulta y registra como acción \"Enviar a Investigar\"",
                AlertMessage = "IPS se encuentra en la matriz de IPS con alto indice de fraude con instrucción de Enviar a investigar"
            };

            return alert;
        }
    }
}
