using NRules.Fluent.Dsl;
using RulesEngine.Domain.Common;
using RulesEngine.Domain.RulesEntities.Solidaria.Entities;

namespace RulesEngine.Application.Clients.Solidaria.Rules.RulesRepository.InvestigationRules
{
    public class FraudulentIpsRule_11 : Rule
    {
        public override void Define()
        {
            InvoiceToCheckSolidaria? invoiceToCheck = default;

            When()
                .Match(() => invoiceToCheck!, x => x.FraudulentIps != null
                                                    && x.IpsNit == x.FraudulentIps.IpsNit
                                                    && x.FraudulentIps.Result == "Envio a investigar");

            Then()
                .Do(w => invoiceToCheck!.AlertSolidaria.Add(CreateAlert()));
        }

        private static AlertSolidaria CreateAlert()
        {
            return new AlertSolidaria
            {
                NameAction = "Alerta",
                Type = "Reglas de investigación",
                Module = "Reclamaciones",
                Description = "El NIT de la IPS en la tabla de origen es igual al NIT de la IPS en la tabla de consulta y registra como acción \"Enviar a Investigar\"",
                Message = "IPS se encuentra en la matriz de IPS con alto indice de fraude con instrucción de Enviar a investigar",
                Typification = string.Empty,
                HasPriority = false
            };
        }
    }
}