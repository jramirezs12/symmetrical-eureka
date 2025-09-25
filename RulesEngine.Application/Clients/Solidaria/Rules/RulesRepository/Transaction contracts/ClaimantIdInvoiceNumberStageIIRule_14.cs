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
                .Do(w => invoiceToCheck!.Alerts.Add(CreateAlert()));
        }
        private static Alert CreateAlert()
        {
            var alert = new Alert
            {
                AlertAction = "SendToQuality",
                AlertNameAction = "Enviar a Calidad",
                AlertType = "Regla por Id Reclamante",
                AlertDescription = "Valida que la reclamación este en la tabla parametrica",
                AlertMessage = "La reclamación está registrada como contrato de transacción"
            };

            return alert;
        }
    }
}
