using NRules.Fluent.Dsl;
using RulesEngine.Application.Actions;
using RulesEngine.Domain.Common;
using RulesEngine.Domain.RulesEntities.Solidaria.Entities;

namespace RulesEngine.Application.Clients.Solidaria.Rules.RulesRepository.MultipleTransport
{
    public class LicencePlateOnAnotherClaimStageIIRule11 : Rule, ITrackableRule
    {
        public Action OnMatch { get; set; } = () => { };
        public override void Define()
        {
            InvoiceToCheckSolidaria invoiceToCheck = default!;

            When()
                .Match(() => invoiceToCheck, x => x.MultipleTransposrts != null && x.MultipleTransposrts.RadNumbers!.Length >= 1);

            Then()
                .Do(w => invoiceToCheck.AlertSolidaria.Add(CreateAlert()))
                .Do(ctx => OnMatch());
        }

        private AlertSolidaria CreateAlert()
        {
            return new AlertSolidaria
            {
                NameAction = "Enviar a Calidad",
                Type = "Transportes Multiples",
                Module = "Reclamaciones",
                Description = "Valida si la placa de vehículo se encuentra en otra reclamación bajo la misma fecha de accidente",
                Message = "Asignación a calidad por Transporte Múltiple.",
                Typification = string.Empty,
                HasPriority = false
            };
        }
    }
}