using NRules.Fluent.Dsl;
using RulesEngine.Domain.Common;
using RulesEngine.Domain.RulesEntities.Mundial.Entities;
using RulesEngine.Domain.RulesEntities.Solidaria.Entities;
using RulesEngine.Domain.ValueObjects;

namespace RulesEngine.Application.Clients.Solidaria.Rules.RulesRepository.TotalGloss
{
    /// <summary>
    /// Assigns to Quality if total gloss equals invoice total
    /// </summary>
    public class QualityTotalGlossRule_6 : Rule
    {
        public Action OnMatch { get; set; } = () => { };
        public override void Define()
        {
            InvoiceToCheckSolidaria? invoiceToCheck = default;

            When()
                .Match(() => invoiceToCheck, x => !Currency.IsNullable(x.InvoiceValue) && !Currency.IsNullable(x.TotalGlossedValue) &&
                                                    Equals(x.InvoiceValue, x.TotalGlossedValue));

            Then()
                .Do(w => invoiceToCheck!.Alerts.Add(CreateAlert()))
                .Do(ctx => OnMatch());
        }

        private Alert CreateAlert()
        {
            Alert alert = new Alert
            {
                AlertAction = "SendToQuality",
                AlertNameAction = "Enviar a Calidad",
                AlertType = "Regla de asignación a calidad - por glosa total",
                AlertDescription = "Valida si el total de la factura es igual al total glosado",
                AlertMessage = "Asignación a calidad por Glosa Total"
            };

            return alert;
        }
    }
}
