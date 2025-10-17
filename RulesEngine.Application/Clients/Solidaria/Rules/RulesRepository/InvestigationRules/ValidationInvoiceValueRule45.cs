using NRules.Fluent.Dsl;
using RulesEngine.Domain.Common;
using RulesEngine.Domain.RulesEntities.Solidaria.Entities;

namespace RulesEngine.Application.Clients.Solidaria.Rules.RulesRepository.InvestigationRules
{
    public class ValidationInvoiceValueRule45 : Rule
    {
        public override void Define()
        {
            InvoiceToCheckSolidaria? invoiceToCheck = default;
            When()
                    .Match(() => invoiceToCheck!, x => x.IpsPhoneVerification != null
                                && x.IpsPhoneVerification.Any(c => x.IpsNit == c.NitIps
                                && x.InvoiceValue.Value > x.InvoicePhoneVerificationValue.Value));
            Then()
                .Do(ctx => invoiceToCheck!.AlertSolidaria.Add(CreateAlert()));
        }

        private static AlertSolidaria CreateAlert()
        {
            return new AlertSolidaria
            {
                NameAction = "Alerta",
                Type = "Regla de investigación",
                Module = "Reclamaciones",
                Description = "El NIT coincide y el valor de la factura es mayor al valor permitido",
                Message = "Verificación telefónica",
                Typification = string.Empty,
                HasPriority = false
            };
        }
    }
}