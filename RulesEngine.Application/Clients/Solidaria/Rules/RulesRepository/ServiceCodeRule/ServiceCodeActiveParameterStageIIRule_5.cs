using NRules.Fluent.Dsl;
using RulesEngine.Domain.Common;
using RulesEngine.Domain.RulesEntities.Solidaria.Entities;

namespace RulesEngine.Application.Clients.Solidaria.Rules.RulesRepository.ServiceCodeRule
{
    public class ServiceCodeActiveParameterStageIIRule_5 : Rule
    {
        public override void Define()
        {
            InvoiceToCheckSolidaria? invoiceToCheck = default;

            When()
                .Match(() => invoiceToCheck!, x => x.ListServiceCodes!.Any(code => x.ServiceCodeFiles!.Any(file => file.ServiceCode == code)));

            Then()
                .Do(w => invoiceToCheck!.Alerts.Add(CreateAlert()));
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
