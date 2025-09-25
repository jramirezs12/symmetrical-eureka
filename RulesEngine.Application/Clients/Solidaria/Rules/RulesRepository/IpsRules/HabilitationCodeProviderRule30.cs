using NRules.Fluent.Dsl;
using RulesEngine.Domain.Common;
using RulesEngine.Domain.RulesEntities.Solidaria.Entities;

namespace RulesEngine.Application.Clients.Solidaria.Rules.RulesRepository.IpsRules
{
    public class HabilitationCodeProviderRule30: Rule
    {
        public override void Define()
        {
            InvoiceToCheckSolidaria? invoiceToCheck = default;

            When()
               .Match(() => invoiceToCheck!, x => x.ProviderData == null || string.IsNullOrEmpty(x.ProviderData.HabilitationCode));

            Then()
                .Do(ctx => invoiceToCheck!.Alerts.Add(CreateAlert()));
        }

        private static Alert CreateAlert()
        {
            var alert = new Alert
            {
                AlertAction = "Alert",
                AlertNameAction = "Alerta",
                AlertType = "Regla de IPS",
                AlertDescription = "Si el código de habilitación del prestador del radicado que se está validando no existe en la tabla de consulta",
                AlertMessage = $"IPS no se encuentra habilitada en el REPS"
            };
            return alert;
        }
    }
}
