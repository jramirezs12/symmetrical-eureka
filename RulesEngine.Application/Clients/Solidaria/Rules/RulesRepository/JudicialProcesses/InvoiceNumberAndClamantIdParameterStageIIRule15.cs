using NRules.Fluent.Dsl;
using RulesEngine.Application.Actions;
using RulesEngine.Domain.Common;
using RulesEngine.Domain.RulesEntities.Solidaria.Entities;

namespace RulesEngine.Application.Clients.Solidaria.Rules.RulesRepository.JudicialProcesses
{
    public class InvoiceNumberAndClamantIdParameterStageIIRule15 : Rule, ITrackableRule
    {
        public Action OnMatch { get; set; } = () => { };
        public override void Define()
        {
            InvoiceToCheckSolidaria? invoiceToCheck = default;

            When()
                .Match(() => invoiceToCheck, x => x.ProcessAndContracts != null &&
                    x.TotalAuthorizedValue.Value > 0 && x.ProcessAndContracts.Any(c => c.ClaimantId == x.IpsNit && c.InvoiceNumber == x.InvoiceNumber
                    && c.Active == true && c.Type == "Procesos judiciales")); ;
            Then()
                .Do(w => invoiceToCheck!.AlertSolidaria.Add(CreateAlert()))
                .Do(ctx => OnMatch());
        }

        private static AlertSolidaria CreateAlert()
        {
            return new AlertSolidaria
            {
                NameAction = "Enviar a Calidad",
                Type = "Regla por procesos judiaciales",
                Module = "Reclamaciones",
                Description = "Valida que la reclamación esté en la tabla paramétrica",
                Message = "La reclamación está registrada con un proceso judicial",
                Typification = string.Empty,
                HasPriority = false
            };
        }
    }
}