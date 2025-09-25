using NRules.Fluent.Dsl;
using RulesEngine.Domain.Common;
using RulesEngine.Domain.RulesEntities.Mundial.Entities;
using RulesEngine.Domain.RulesEntities.Solidaria.Entities;

namespace RulesEngine.Application.Clients.Solidaria.Rules.RulesRepository.ServiceCode
{
    /// <summary>
    /// Assigns to Quality if service code matches parameter list
    /// </summary>
    public class QualityServiceCodeRule_5 : Rule
    {
        public Action OnMatch { get; set; } = () => { };
        public override void Define()
        {
            InvoiceToCheckSolidaria? invoiceToCheck = default;

            When()
                .Match(() => invoiceToCheck!, x => x.ListServiceCodes != null
                     && x.ServiceCodeFiles != null && x.ListServiceCodes.Any(code => x.ServiceCodeFiles.Any(file => file.ServiceCode == code)));

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
                AlertType = "Regla por c�digo de servicio",
                AlertDescription = "El c�digo de servicio no se encuentra activo en el par�metro de la etapa II.",
                AlertMessage = "Asignaci�n a calidad por C�digo del servicio."
            };

            return alert;
        }
    }
}
