using NRules.Fluent.Dsl;
using RulesEngine.Domain.Common;
using RulesEngine.Domain.RulesEntities.Solidaria.Entities;

namespace RulesEngine.Application.Clients.Solidaria.Rules.RulesRepository.IpsRules
{
    public class HabilitationCodeProviderRule30 : Rule
    {
        public override void Define()
        {
            InvoiceToCheckSolidaria? invoiceToCheck = default;

            When()
               .Match(() => invoiceToCheck!, x => x.ProviderData == null || string.IsNullOrEmpty(x.ProviderData.HabilitationCode));

            Then()
                .Do(ctx => invoiceToCheck!.AlertSolidaria.Add(CreateAlert()));
        }

        private static AlertSolidaria CreateAlert()
        {
            return new AlertSolidaria
            {
                NameAction = "Alerta",
                Type = "Regla de IPS",
                Module = "Reclamaciones",
                Description = "El código de habilitación del prestador no existe en la consulta",
                Message = "IPS no se encuentra habilitada en el REPS",
                Typification = string.Empty,
                HasPriority = false
            };
        }
    }
}