using NRules.Fluent.Dsl;
using RulesEngine.Domain.Common;
using RulesEngine.Domain.RulesEntities.Solidaria.Entities;

namespace RulesEngine.Application.Clients.Solidaria.Rules.RulesRepository.PaymentRules
{
    public class NitAndInvoiceRule_27 : Rule
    {
        public override void Define()
        {
            InvoiceToCheckSolidaria? invoiceToCheck = default;

            When()
                .Match(() => invoiceToCheck!, x => x.IpsNit != x.IpsNitF2 || x.InvoiceNumberF1 != x.InvoiceNumberF2);

            Then()
                .Do(w => invoiceToCheck!.AlertSolidaria.Add(CreateAlert()));
        }

        private static AlertSolidaria CreateAlert()
        {
            return new AlertSolidaria
            {
                NameAction = "Alerta",
                Type = "Regla de coincidencia",
                Module = "Reclamaciones",
                Description = "El NIT y/o número de factura en la tabla de origen difiere del de la consulta",
                Message = "La reclamación ya se encuentra en otro radicado",
                Typification = string.Empty,
                HasPriority = false
            };
        }
    }
}