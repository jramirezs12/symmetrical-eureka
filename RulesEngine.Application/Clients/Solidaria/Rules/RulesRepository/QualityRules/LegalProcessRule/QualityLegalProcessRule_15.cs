using NRules.Fluent.Dsl;
using RulesEngine.Domain.Common;
using RulesEngine.Domain.RulesEntities.Mundial.Entities;
using RulesEngine.Domain.RulesEntities.Solidaria.Entities;

namespace RulesEngine.Application.Clients.Solidaria.Rules.RulesRepository.LegalProcess
{
    /// <summary>
    /// Assigns to Quality if claim is linked to a legal process
    /// </summary>
    public class QualityLegalProcessRule_15 : Rule
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
                .Do(w => invoiceToCheck!.Alerts.Add(CreateAlert()))
                .Do(ctx => OnMatch());
        }

        private static Alert CreateAlert()
        {
            var alert = new Alert
            {
                AlertAction = "SendToQuality",
                AlertNameAction = "Enviar a Calidad",
                AlertType = "Regla por procesos judiaciales",
                AlertDescription = "Valida que la reclamación este en la tabla parametrica",
                AlertMessage = "La reclamación está registrada con un proceso judicial"
            };

            return alert;
        }
    }
}
