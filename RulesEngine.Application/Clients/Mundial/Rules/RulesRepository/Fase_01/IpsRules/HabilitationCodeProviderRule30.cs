using NRules.Fluent.Dsl;
using RulesEngine.Application.Actions;
using RulesEngine.Domain.Common;
using RulesEngine.Domain.RulesEntities.Mundial.Entities;

namespace RulesEngine.Application.Clients.Mundial.Rules.RulesRepository.Fase_01.IpsRules
{
    public class HabilitationCodeProviderRule30: Rule, ITrackableRule
    {
        public Action OnMatch { get; set; } = () => { };
        public override void Define()
        {
            InvoiceToCheck? invoiceToCheck = default;

            When()
               .Match(() => invoiceToCheck!, x => x.ProviderData == null || string.IsNullOrEmpty(x.ProviderData.HabilitationCode));

            Then()
                .Do(ctx => invoiceToCheck!.Alerts.Add(CreateAlert()))
                .Do(ctx => OnMatch());
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
