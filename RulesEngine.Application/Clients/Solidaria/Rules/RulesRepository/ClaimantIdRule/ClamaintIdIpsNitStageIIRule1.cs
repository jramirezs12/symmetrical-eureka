using NRules.Fluent.Dsl;
using RulesEngine.Domain.Common;
using RulesEngine.Domain.RulesEntities.Mundial.Entities;
using RulesEngine.Domain.RulesEntities.Solidaria.Entities;

namespace RulesEngine.Application.Clients.Solidaria.Rules.RulesRepository.ClaimantIdRule
{
    public class ClamaintIdIpsNitStageIIRule1 : Rule
    {
        public override void Define()
        {
            InvoiceToCheck? invoiceToCheck = default;

            When()
                .Match(() => invoiceToCheck, x => x.IpsNitList!.Any(c=>c.NitIps == x.IpsNit));
            Then()
                .Do(w => invoiceToCheck!.Alerts.Add(CreateAlert()));
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
