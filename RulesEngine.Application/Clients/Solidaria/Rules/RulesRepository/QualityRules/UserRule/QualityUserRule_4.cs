using NRules.Fluent.Dsl;
using RulesEngine.Domain.Common;
using RulesEngine.Domain.RulesEntities.Mundial.Entities;
using RulesEngine.Domain.RulesEntities.Solidaria.Entities;

namespace RulesEngine.Application.Clients.Solidaria.Rules.RulesRepository.User
{
    /// <summary>
    /// Assigns to Quality if user matches parameter list
    /// </summary>
    public class QualityUserRule_4 : Rule
    {
        public Action OnMatch { get; set; } = () => { };
        public override void Define()
        {
            InvoiceToCheckSolidaria? invoiceToCheck = default;

            When()
                .Match(() => invoiceToCheck, x => x.AllowedUsers != null && x!.AllowedUsers!.Any(c => c.UserAccount == x.UserClaim));
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
                AlertType = "Regla por Usuario",
                AlertDescription = "Valida si el usuario se encuentra en el archivo matriz",
                AlertMessage = "Asignación a calidad por Usuario"
            };

            return alert;
        }
    }
}
