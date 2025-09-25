using NRules.Fluent.Dsl;
using RulesEngine.Application.Actions;
using RulesEngine.Domain.Common;
using RulesEngine.Domain.RulesEntities.Mundial.Entities;

namespace RulesEngine.Application.Clients.Mundial.Rules.RulesRepository.Fase_01.PaymentRules
{
    public class NitAndInvoiceRule_27 : Rule, ITrackableRule
    {
        public Action OnMatch { get; set; } = () => { };
        public override void Define()
        {
            InvoiceToCheck? invoiceToCheck = default;

            When()
                .Match(() => invoiceToCheck!, x => x.IpsNit != x.IpsNitF2 || x.InvoiceNumberF1 != x.InvoiceNumberF2);

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
                AlertType = "Regla de coincidencia",
                AlertDescription = "El nIT más número de factura registrado de la tabla origen se encuentra en la tabla consulta bajo otro número de radicado",
                AlertMessage = "La reclamación ya se encuentra en otro radicado"
            };

            return alert;
        }
    }
}
