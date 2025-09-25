using NRules.Fluent.Dsl;
using RulesEngine.Domain.Common;
using RulesEngine.Domain.RulesEntities.Mundial.Entities;
using RulesEngine.Domain.RulesEntities.Solidaria.Entities;

namespace RulesEngine.Application.Clients.Solidaria.Rules.RulesRepository.ClaimantId
{
    /// <summary>
    /// Assigns to Quality if claimant ID matches parameter list
    /// </summary>
    public class QualityClaimantIdRule_1 : Rule
    {
        public Action OnMatch { get; set; } = () => { };
        public override void Define()
        {
            InvoiceToCheckSolidaria? invoiceToCheck = default;

            When()
                .Match(() => invoiceToCheck, x => x.IpsNitList != null && x.IpsNitList!.Any(c => c.NitIps == x.IpsNit));
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
