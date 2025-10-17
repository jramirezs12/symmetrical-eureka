using NRules.Fluent.Dsl;
using RulesEngine.Domain.Common;
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
                .Match(() => invoiceToCheck, x => x.Research != null && x.Research.Any(c =>
                    c.Response != null && c.Response.Answer != null &&
                    (c.Response!.Answer!.State!.Name == "No Cubierto" || c.Response!.Answer!.State!.Name == "No cubierto")));

            Then()
                .Do(w => invoiceToCheck.AlertSolidaria.Add(CreateAlert()))
                .Do(ctx => OnMatch());
        }

        private AlertSolidaria CreateAlert()
        {
            return new AlertSolidaria
            {
                NameAction = "Enviar a Calidad",
                Type = "Reglas de investigación",
                Module = "Reclamaciones",
                Description = "Valida si el resultado de la investigación es No cubierto",
                Message = "La reclamación tiene resultados de investigación No Cubiertos",
                Typification = string.Empty,
                HasPriority = false
            };
        }
    }
}