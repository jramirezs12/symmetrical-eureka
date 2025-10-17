using NRules.Fluent.Dsl;
using RulesEngine.Domain.Common;
using RulesEngine.Domain.RulesEntities.Solidaria.Entities;

namespace RulesEngine.Application.Solidaria.Rules.RulesRepository.Transaction_contracts
{
    public class ClaimantIdInvoiceNumberStageIIRule_14 : Rule
    {
        public override void Define()
        {
            InvoiceToCheckSolidaria? invoiceToCheck = default;

            When()
                .Match(() => invoiceToCheck, x => (x.ProcessAndContracts != null) &&
                    (x.TotalAuthorizedValue.Value > 0 && x.ProcessAndContracts.Any(c => c.ClaimantId == x.IpsNit && c.InvoiceNumber == x.InvoiceNumber
                    && c.Active == true && c.Type == "Contratos de transacción")));
            Then()
                .Do(w => invoiceToCheck!.AlertSolidaria.Add(CreateAlert()));
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