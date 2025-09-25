using NRules.Fluent.Dsl;
using RulesEngine.Application.Actions;
using RulesEngine.Domain.Common;
using RulesEngine.Domain.RulesEntities.Mundial.Entities;

namespace RulesEngine.Application.Clients.Mundial.Rules.RulesRepository.Fase_01.FuripsI_IIRules
{
    public class FuripsI_IIRule_2 : Rule, ITrackableRule
    {
        public Action OnMatch { get; set; } = () => { };
        public override void Define()
        {
            InvoiceToCheck? invoiceToCheck = default;

            When()
                .Match(() => invoiceToCheck!, x => x.IpsNit != x.IpsNitFurips ||
                                                   x.InvoiceNumberFurips != x.InvoiceNumberF1);
            
            Then()
                .Do(w => invoiceToCheck!.Alerts.Add(CreateAlert()))
                .Do(ctx => OnMatch());
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
