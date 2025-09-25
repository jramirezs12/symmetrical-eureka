using NRules.Fluent.Dsl;
using RulesEngine.Application.Actions;
using RulesEngine.Domain.Common;
using RulesEngine.Domain.RulesEntities.Mundial.Entities;

namespace RulesEngine.Application.Clients.Mundial.Rules.RulesRepository.Fase_02.MultipleTransport
{
    public class LicencePlateOnAnotherClaimStageIIRule11 : Rule, ITrackableRule
    {
        public Action OnMatch { get; set; } = () => { };
        public override void Define()
        {
            InvoiceToCheck invoiceToCheck = default!;

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
