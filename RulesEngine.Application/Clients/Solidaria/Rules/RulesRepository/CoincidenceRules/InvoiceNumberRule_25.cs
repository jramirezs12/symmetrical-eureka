using NRules.Fluent.Dsl;
using RulesEngine.Application.Actions;
using RulesEngine.Domain.Common;
using RulesEngine.Domain.RulesEntities.Solidaria.Entities;

namespace RulesEngine.Application.Clients.Solidaria.Rules.RulesRepository.CoincidenceRules
{
    public class InvoiceNumberRule_25 : Rule, ITrackableRule
    {
        public Action OnMatch { get; set; } = () => { };
        public override void Define()
        {
            InvoiceToCheckSolidaria? invoiceToCheck = default;

            When()
                .Match(() => invoiceToCheck!, x => x.InvoiceNumberF1 != x.InvoiceNumberF2);

            Then()
                .Do(w => invoiceToCheck!.AlertSolidaria.Add(CreateAlert()))
                .Do(ctx => OnMatch());
        }

        private static AlertSolidaria CreateAlert()
        {
            return new AlertSolidaria
            {
                NameAction = "Alerta",
                Type = "Regla de coincidencia",
                Module = "Reclamaciones",
                Description = "El número de factura de la tabla origen es diferente al número de factura de la tabla consulta",
                Message = "El número de factura registrado en el FURIPS I es diferente al número de factura registrado en el FURIPS II",
                Typification = string.Empty,
                HasPriority = false
            };
        }
    }
}
