using MongoDB.Driver;
using NRules.Fluent.Dsl;
using RulesEngine.Domain.Common;
using RulesEngine.Domain.RulesEntities.Solidaria.Entities;

namespace RulesEngine.Application.Clients.Solidaria.Rules.RulesRepository.InvestigationRules
{
    public class IpsCatastrophicEventRule_22 : Rule
    {
        public override void Define()
        {
            InvoiceToCheckSolidaria? invoiceToCheck = default;

            When()
                .Match(() => invoiceToCheck!, x => x.CatastrophicEvent != null && x.CatastrophicEvent!.Any(y => y.SoatNumber == x.SoatNumber));

            Then()
                .Do(w => invoiceToCheck!.Alerts.Add(CreateAlert(invoiceToCheck)));
        }

        private Alert CreateAlert(InvoiceToCheckSolidaria invoiceToCheck)
        {
            string typification = invoiceToCheck.TypificationMap.GetValueOrDefault(GetType().Name, "Sin typification");
            bool haspriority = invoiceToCheck.HasPriorityMap.GetValueOrDefault(GetType().Name, false);
            var alert = new Alert
            {
                AlertAction = "SendToInvestigation",
                AlertNameAction = "Enviar a investigar",
                AlertType = "Reglas de investigación",
                AlertDescription = "La póliza en la tabla de origen es igual a la póliza en la tabla de consulta y no tiene resultado de investigación asociado al siniestro",
                AlertMessage = $"Se debe enviar a investigar, caso catastrofico",
                Typification = typification,
                HasPriority = haspriority
            };

            return alert;
        }
    }
}
