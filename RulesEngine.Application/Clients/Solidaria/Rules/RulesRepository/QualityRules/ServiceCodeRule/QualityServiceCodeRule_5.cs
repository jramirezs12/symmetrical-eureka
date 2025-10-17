using NRules.Fluent.Dsl;
using RulesEngine.Domain.Common;
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
                .Do(w => invoiceToCheck!.AlertSolidaria.Add(CreateAlert()))
                .Do(ctx => OnMatch());
        }

        private static AlertSolidaria CreateAlert()
        {
            return new AlertSolidaria
            {
                NameAction = "Enviar a Calidad",
                Type = "Regla por c�digo de servicio",
                Module = "Reclamaciones",
                Description = "El c�digo de servicio no se encuentra activo en el par�metro de la etapa II.",
                Message = "Asignaci�n a calidad por C�digo del servicio.",
                Typification = string.Empty,
                HasPriority = false
            };
        }
    }
}