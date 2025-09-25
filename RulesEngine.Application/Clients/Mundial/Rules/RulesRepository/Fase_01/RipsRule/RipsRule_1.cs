using NRules.Fluent.Dsl;
using RulesEngine.Application.Actions;
using RulesEngine.Domain.Common;
using RulesEngine.Domain.RulesEntities.Mundial.Entities;

namespace RulesEngine.Application.Clients.Mundial.Rules.RulesRepository.Fase_01.RipsRule
{
    public class RipsRule_1 : Rule, ITrackableRule
    {
        public Action OnMatch { get; set; } = () => { };
        public override void Define()
        {
            InvoiceToCheck? invoiceToCheck = default;

            When()
                .Match(() => invoiceToCheck!, x => x.IpsNit != x.IpsNitRips
                                                    || x.InvoiceNumberRips != x.InvoiceNumberF1);

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
                AlertType = "Regla de RIPS",
                AlertDescription = "El NIT más número de factura de la tabla origen no coincide con el registrado en la tabla de consulta",
                AlertMessage = "Mostrar mensaje indicando que no fue aportado RIPS"
            };

            return alert;
        }
    }
}
