using NRules.Fluent.Dsl;
using RulesEngine.Application.Actions;
using RulesEngine.Domain.Common;
using RulesEngine.Domain.RulesEntities.Solidaria.Entities;
using RulesEngine.Domain.ValueObjects;

namespace RulesEngine.Application.Clients.Solidaria.Rules.RulesRepository.DateRules
{
    public class InvoiceMAOSDateGreaterClaimDate_43 : Rule, ITrackableRule
    {
        public Action OnMatch { get; set; } = () => { };

        public override void Define()
        {
            InvoiceToCheckSolidaria? invoiceToCheck = default;

            When()
                .Match(() => invoiceToCheck!, x => Date.IsNotNullable(x.InvoiceMAOSDate, x.ClaimDate) && 
                                                   Date.GreaterThan(x.InvoiceMAOSDate, x.ClaimDate));

            Then()
                .Do(w => invoiceToCheck!.AlertSolidaria.Add(CreateAlert()))
                .Do(ctx => OnMatch());
        }

        private static AlertSolidaria CreateAlert()
        {
            return new AlertSolidaria
            {
                NameAction = "Devolver Reclamación",
                Type = "Regla lógica de fechas",
                Module = "Reclamaciones",
                Description = "Permite validar si la fecha de factura proveedor MAOS es posterior a la fecha de aviso de la reclamación, lo que conlleva a la devolución de la reclamación",
                Message = "Se debe aplicar objeción parcial a los items que contengan MAOS teniendo en cuenta que la fecha de factura de proveedor MAOS no puede ser mayor a la fecha de aviso de la reclamación. Código objeción parcial # xx",
                Typification = string.Empty,
                HasPriority = false
            };
        }
    }
}
