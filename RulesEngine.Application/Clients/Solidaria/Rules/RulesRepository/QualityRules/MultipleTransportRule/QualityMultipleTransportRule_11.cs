using NRules.Fluent.Dsl;
using RulesEngine.Domain.Common;
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
                Description = "Valida si la placa de veh�culo se encuentra en otra reclamaci�n bajo la misma fecha de accidente",
                Message = "Asignaci�n a calidad por Transporte M�ltiple.",
                Typification = string.Empty,
                HasPriority = false
            };
        }
    }
}