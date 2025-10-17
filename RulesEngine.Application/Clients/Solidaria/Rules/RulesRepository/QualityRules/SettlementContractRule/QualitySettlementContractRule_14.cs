using NRules.Fluent.Dsl;
using RulesEngine.Domain.Common;
using RulesEngine.Domain.RulesEntities.Solidaria.Entities;

namespace RulesEngine.Application.Clients.Solidaria.Rules.RulesRepository.SettlementContract
{
    /// <summary>
    /// Assigns to Quality if claim matches settlement contract
    /// </summary>
    public class QualitySettlementContractRule_14 : Rule
    {
        public Action OnMatch { get; set; } = () => { };
        public override void Define()
        {
            InvoiceToCheckSolidaria? invoiceToCheck = default;

            When()
                .Match(() => invoiceToCheck, x => (x.ProcessAndContracts != null) &&
                    (x.TotalAuthorizedValue.Value > 0 && x.ProcessAndContracts.Any(c => c.ClaimantId == x.IpsNit && c.InvoiceNumber == x.InvoiceNumber
                    && c.Active == true && c.Type == "Contratos de transacción")));
            Then()
                .Do(w => invoiceToCheck!.AlertSolidaria.Add(CreateAlert()))
                .Do(ctx => OnMatch());
        }
        private static AlertSolidaria CreateAlert()
        {
            return new AlertSolidaria
            {
                NameAction = "Enviar a Calidad",
                Type = "Regla por Id Reclamante",
                Module = "Reclamaciones",
                Description = "Valida que la reclamación este en la tabla parametrica",
                Message = "La reclamación está registrada como contrato de transacción",
                Typification = string.Empty,
                HasPriority = false
            };
        }
    }
}