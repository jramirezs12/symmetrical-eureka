using MongoDB.Driver;
using NRules.Fluent.Dsl;
using RulesEngine.Domain.Common;
using RulesEngine.Domain.RulesEntities.Solidaria.Entities;

namespace RulesEngine.Application.Clients.Solidaria.Rules.RulesRepository.InvestigationRules
{
    public class VictimIdRule_18 : Rule
    {
        public override void Define()
        {
            InvoiceToCheckSolidaria? invoiceToCheck = default;

            When()
                .Match(() => invoiceToCheck!, x => x.AtypicalEvent != null && x.AtypicalEvent.Any(y => y.VictimId == x.VictimId));

            Then()
                .Do(w => invoiceToCheck!.Alerts.Add(CreateAlert(invoiceToCheck)));
        }

        private static Alert CreateAlert(InvoiceToCheckSolidaria invoiceToCheck)
        {
            var alert = new Alert
            {
                AlertAction = "Alert",
                AlertNameAction = "Alerta",
                AlertType = "Reglas de investigación",
                AlertDescription = "El número de documento de identidad de la víctima en la tabla de origen es igual a el número de documento de identidad de la víctima en la tabla de consulta",
                AlertMessage = $"El número de documento de identidad de la víctima presenta la siguiente alerta: {invoiceToCheck.AtypicalEvent!.First(x => x.VictimId == invoiceToCheck.VictimId).AlertDescription}"
            };

            return alert;
        }
    }
}
