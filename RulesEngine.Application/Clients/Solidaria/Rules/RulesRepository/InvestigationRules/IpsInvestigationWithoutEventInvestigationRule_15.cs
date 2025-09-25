using NRules.Fluent.Dsl;
using RulesEngine.Domain.Common;
using RulesEngine.Domain.RulesEntities.Solidaria.Entities;
using RulesEngine.Domain.ValueObjects;

namespace RulesEngine.Application.Clients.Solidaria.Rules.RulesRepository.InvestigationRules
{
    public class IpsInvestigationWithoutEventInvestigationRule_15 : Rule
    {
        public override void Define()
        {
            InvoiceToCheckSolidaria? invoiceToCheck = default;

            When()
                .Match(() => invoiceToCheck!, x => x.IpsInvestigation != null
                                                    && x.IpsInvestigation.IpsNit == x.IpsNit
                                                    && Date.IsNullable(x.InvestigationResponseDate));

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
                AlertNameAction = "Enviar a Investigar",
                AlertType = "Reglas de investigación",
                AlertDescription = "El NIT de la IPS en la tabla de origen es igua al NIT de la IPS en la tabla de consulta  y el siniestro no tiene resultado de investigación",
                AlertMessage = "Se debe enviar a investigar, IPS se encuentra en la matriz de IPS en esquema de investigación y no tiene investigación asociada",
                Typification = typification,
                HasPriority = haspriority,
            };

            return alert;
        }
    }
}
