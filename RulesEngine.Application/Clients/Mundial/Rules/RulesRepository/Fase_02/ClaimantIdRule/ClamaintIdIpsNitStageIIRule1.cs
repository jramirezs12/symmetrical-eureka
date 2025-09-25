using NRules.Fluent.Dsl;
using RulesEngine.Application.Actions;
using RulesEngine.Domain.Common;
using RulesEngine.Domain.RulesEntities.Mundial.Entities;

namespace RulesEngine.Application.Clients.Mundial.Rules.RulesRepository.Fase_02.ClaimantIdRule
{
    public class ClamaintIdIpsNitStageIIRule1 : Rule, ITrackableRule
    {
        public Action OnMatch { get; set; } = () => { };
        public override void Define()
        {
            InvoiceToCheck? invoiceToCheck = default;

            When()
                .Match(() => invoiceToCheck, x => x.IpsNitList != null && x.IpsNitList!.Any(c=>c.NitIps == x.IpsNit));
            Then()
                .Do(w => invoiceToCheck!.Alerts.Add(CreateAlert()))
                .Do(ctx => OnMatch());
        }
        private static Alert CreateAlert()
        {
            var alert = new Alert
            {
                AlertAction = "SendToQuality",
                AlertNameAction = "Enviar a Calidad",
                AlertType = "Regla por Id Reclamante",
                AlertDescription = "Valida si el nit de la reclamación se encuentra en el archivo matriz",
                AlertMessage = "Asignación a calidad por NIT"
            };

            return alert;
        }
    }
}
