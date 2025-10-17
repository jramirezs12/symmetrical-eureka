using NRules.Fluent.Dsl;
using RulesEngine.Domain.Common;
using RulesEngine.Domain.RulesEntities.Solidaria.Entities;

namespace RulesEngine.Application.Clients.Solidaria.Rules.RulesRepository.FuripsI_IIRules
{
    public class FuripsI_IIRule_2 : Rule
    {
        public override void Define()
        {
            InvoiceToCheckSolidaria? invoiceToCheck = default;

            When()
                .Match(() => invoiceToCheck!, x => x.IpsNit != x.IpsNitFurips ||
                                                   x.InvoiceNumberFurips != x.InvoiceNumberF1);

            Then()
                .Do(w => invoiceToCheck!.AlertSolidaria.Add(CreateAlert()));
        }

        private static AlertSolidaria CreateAlert()
        {
            return new AlertSolidaria
            {
                NameAction = "Alerta",
                Type = "Regla de FURIPS I y II",
                Module = "Reclamaciones",
                Description = "El NIT más número de factura de la tabla origen no coincide con el registrado en la tabla de consulta.",
                Message = "No fue aportado FURIPS.",
                Typification = string.Empty,
                HasPriority = false
            };
        }
    }
}