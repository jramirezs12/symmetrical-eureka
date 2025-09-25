using NRules.Fluent.Dsl;
using RulesEngine.Domain.Common;
using RulesEngine.Domain.RulesEntities.Mundial.Entities;
using RulesEngine.Domain.RulesEntities.Solidaria.Entities;

namespace RulesEngine.Application.Clients.Solidaria.Rules.RulesRepository.MultipleTransport
{
    /// <summary>
    /// Assigns to Quality if ambulance plate exists in another claim with same accident date
    /// </summary>
    public class QualityMultipleTransportRule_11 : Rule
    {
        public Action OnMatch { get; set; } = () => { };
        public override void Define()
        {
            InvoiceToCheckSolidaria invoiceToCheck = default!;

            When()
                .Match(() => invoiceToCheck, x => x.MultipleTransposrts != null && x.MultipleTransposrts.RadNumbers!.Length >= 1);

            Then()
                .Do(w => invoiceToCheck.Alerts.Add(CreateAlert()))
                .Do(ctx => OnMatch());
        }

        private Alert CreateAlert()
        {
            Alert alert = new Alert
            {
                AlertAction = "SendToQuality",
                AlertNameAction = "Enviar a Calidad",
                AlertType = "Transportes Multiples",
                AlertDescription = "Valida si la placa de vehículo se encuentra en otra reclamación bajo la misma fecha de accidente",
                AlertMessage = "Asignación a calidad por Transporte Múltiple."
            };

            return alert;
        }
    }
}
