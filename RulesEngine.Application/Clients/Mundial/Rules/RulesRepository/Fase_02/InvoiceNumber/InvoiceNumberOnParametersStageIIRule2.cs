using NRules.Fluent.Dsl;
using RulesEngine.Application.Actions;
using RulesEngine.Domain.Common;
using RulesEngine.Domain.RulesEntities.Mundial.Entities;

namespace RulesEngine.Application.Clients.Mundial.Rules.RulesRepository.Fase_02.InvoiceNumber
{
    public class InvoiceNumberOnParametersStageIIRule2 : Rule, ITrackableRule
    {

        public Action OnMatch { get; set; } = () => { };
        public override void Define()
        {
            InvoiceToCheck? invoiceToCheck = default;

            When()
                .Match(() => invoiceToCheck!, x => x.InvoiceNumberFile != null && !string.IsNullOrEmpty(x.InvoiceNumber) && x.InvoiceNumberFile.Any(n => x.InvoiceNumber == n.InvoiceNumber));

            Then()
                .Do(w => invoiceToCheck!.Alerts.Add(CreateAlert()))
                .Do(ctx => OnMatch());
        }

        private static Alert CreateAlert()
        {
            var alert = new Alert
            {
                AlertAction = "SendToQuality",
                AlertNameAction = "Enviar a Calidad",
                AlertType = "Regla por número de factura",
                AlertDescription = "Si el número de factura registrado en la reclamación corresponde al mismo número de factura activo en los parámetros, la cuenta se debe asignar a calidad.",
                AlertMessage = "Asignación a calidad por Factura."
            };

            return alert;
        }
    }
}
