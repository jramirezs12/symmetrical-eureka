using NRules.Fluent.Dsl;
using RulesEngine.Application.Actions;
using RulesEngine.Domain.Common;
using RulesEngine.Domain.RulesEntities.Mundial.Entities;

namespace RulesEngine.Application.Clients.Mundial.Rules.RulesRepository.Fase_02.ServiceCodeRule
{
    public class ServiceCodeActiveParameterStageIIRule5 : Rule, ITrackableRule
    {
        public Action OnMatch { get; set; } = () => { };
        public override void Define()
        {
            InvoiceToCheck? invoiceToCheck = default;

            When()
                .Match(() => invoiceToCheck!, x => x.ListServiceCodes != null
                     && x.ServiceCodeFiles != null && x.ListServiceCodes.Any(code =>x.ServiceCodeFiles.Any(file => file.ServiceCode == code)));

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
                AlertType = "Regla por código de servicio",
                AlertDescription = "El código de servicio no se encuentra activo en el parámetro de la etapa II.",
                AlertMessage = "Asignación a calidad por Código del servicio."
            };

            return alert;
        }
    }
}
