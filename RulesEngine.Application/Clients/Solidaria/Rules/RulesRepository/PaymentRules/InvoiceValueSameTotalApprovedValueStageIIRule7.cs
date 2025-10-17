using NRules.Fluent.Dsl;
using RulesEngine.Application.Actions;
using RulesEngine.Domain.Common;
using RulesEngine.Domain.RulesEntities.Solidaria.Entities;
using RulesEngine.Domain.ValueObjects;

namespace RulesEngine.Application.Clients.Solidaria.Rules.RulesRepository.PaymentRules
{
    public class InvoiceValueSameTotalApprovedValueStageIIRule7 : Rule, ITrackableRule
    {
        public Action OnMatch { get; set; } = () => { };
        public override void Define()
        {
            InvoiceToCheckSolidaria? invoiceToCheck = default;

            When()
                .Match(() => invoiceToCheck, x => x.InvoiceValue.Value > 0 && x.TotalAuthorizedValue.Value > 0 && Equals(x.InvoiceValue, x.TotalAuthorizedValue));

            Then()
                .Do(w => invoiceToCheck!.AlertSolidaria.Add(CreateAlert()))
                .Do(ctx => OnMatch());
        }

        private AlertSolidaria CreateAlert()
        {
            return new AlertSolidaria
            {
                NameAction = "Enviar a Calidad",
                Type = "Regla de asignación a calidad - por pago total",
                Module = "Reclamaciones",
                Description = "Valida si el total de la factura es igual al total autorizado",
                Message = "Asignación a calidad por Pago Total",
                Typification = string.Empty,
                HasPriority = false
            };
        }
    }
}