using NRules.Fluent.Dsl;
using RulesEngine.Domain.Common;
using RulesEngine.Domain.RulesEntities.Mundial.Entities;
using RulesEngine.Domain.RulesEntities.Solidaria.Entities;

namespace RulesEngine.Application.Clients.Solidaria.Rules.RulesRepository.InvestigationNotCovered
{
    /// <summary>
    /// Assigns to Quality if investigation result is Not Covered
    /// </summary>
    public class QualityInvestigationNotCoveredRule_12 : Rule
    {
        public Action OnMatch { get; set; } = () => { };
        public override void Define()
        {
            InvoiceToCheckSolidaria invoiceToCheck = default!;

            When()
                .Match(() => invoiceToCheck, x => x.Research != null && x.Research.Any(c => c.Response != null && c.Response.Answer != null && c.Response!.Answer!.State!.Name == "No Cubierto" || c.Response != null && c.Response.Answer != null && c.Response!.Answer!.State!.Name == "No cubierto"));


            Then()
                .Do(w => invoiceToCheck.Alerts.Add(CreateAlert()))
                .Do(ctx => OnMatch());
        }

        private Alert CreateAlert()
        {
            Alert alert = new Alert
            {
                AlertAction = "SendToQuality",
                AlertNameAction = "Enviar a Calidad",
                AlertType = "Reglas de investigación",
                AlertDescription = "Valida si el resultado de la investigación es No cubierto",
                AlertMessage = "La reclamación tiene resultados de investigación No Cubiertos"
            };

            return alert;
        }
    }
}
