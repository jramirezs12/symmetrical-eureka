using NRules.Fluent.Dsl;
using RulesEngine.Domain.Common;
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
                .Do(w => invoiceToCheck!.AlertSolidaria.Add(CreateAlert()))
                .Do(ctx => OnMatch());
        }
        private static AlertSolidaria CreateAlert()
        {
            return new AlertSolidaria
            {
                NameAction = "Enviar a Calidad",
                Type = "Regla por Usuario",
                Module = "Reclamaciones",
                Description = "Valida si el usuario se encuentra en el archivo matriz",
                Message = "Asignación a calidad por Usuario",
                Typification = string.Empty,
                HasPriority = false
            };
        }
    }
}