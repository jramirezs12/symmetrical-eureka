using MongoDB.Driver;
using NRules.Fluent.Dsl;
using RulesEngine.Domain.Common;
using RulesEngine.Domain.RulesEntities.Solidaria.Entities;

namespace RulesEngine.Application.Clients.Solidaria.Rules.RulesRepository.InvestigationRules
{
    public class SoatRule_20 : Rule
    {
        public override void Define()
        {
            InvoiceToCheckSolidaria? invoiceToCheck = default;

            When()
                .Match(() => invoiceToCheck!, x => x.AtypicalEvent != null && x.AtypicalEvent.Any(y => y.SoatNumber == x.SoatNumber));

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
                AlertDescription = "El número de póliza SOAT en la tabla de origen es igual a la placa en la tabla de consulta",
                AlertMessage = $"El número de póliza SOAT presenta la siguiente alerta: {invoiceToCheck.AtypicalEvent!.First(x => x.SoatNumber == invoiceToCheck.SoatNumber).AlertDescription}"
            };

            return alert;
        }
    }
}
