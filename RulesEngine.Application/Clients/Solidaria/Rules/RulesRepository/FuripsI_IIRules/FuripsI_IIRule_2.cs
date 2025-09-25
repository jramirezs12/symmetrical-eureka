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
                .Do(w => invoiceToCheck!.Alerts.Add(CreateAlert()));
        }

        private static Alert CreateAlert()
        {
            var alert = new Alert
            {
                AlertAction = "Alert",
                AlertNameAction = "Alerta",
                AlertType = "Regla de FURIPS I y II",
                AlertDescription = "El NIT más número de factura de la tabla origen no coincide con el registrado en la tabla de consulta.",
                AlertMessage = "No fue aportado FURIPS."
            };

            return alert;
        }
    }
}
