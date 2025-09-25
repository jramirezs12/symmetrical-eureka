using MongoDB.Driver;
using NRules.Fluent.Dsl;
using RulesEngine.Domain.RulesEntities.Mundial.Entities;
using RulesEngine.Application.Actions;
using RulesEngine.Domain.Common;

namespace RulesEngine.Application.Clients.Mundial.Rules.RulesRepository.Fase_01.InvestigationRules
{
    public class VictimIdRule_18 : Rule, ITrackableRule
    {
        public Action OnMatch { get; set; } = () => { };
        public override void Define()
        {
            InvoiceToCheck? invoiceToCheck = default;

            When()
                .Match(() => invoiceToCheck!, x => x.AtypicalEvent != null && x.AtypicalEvent.Any(y => y.VictimId == x.VictimId));

            Then()
                .Do(w => invoiceToCheck!.Alerts.Add(CreateAlert(invoiceToCheck)))
                .Do(ctx => OnMatch());
        }

        private static Alert CreateAlert(InvoiceToCheck invoiceToCheck)
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
