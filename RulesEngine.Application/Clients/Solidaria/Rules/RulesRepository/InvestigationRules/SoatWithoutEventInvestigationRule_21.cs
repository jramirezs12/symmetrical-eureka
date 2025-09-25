using MongoDB.Driver;
using NRules.Fluent.Dsl;
using RulesEngine.Domain.Common;
using RulesEngine.Domain.RulesEntities.Solidaria.Entities;

namespace RulesEngine.Application.Clients.Solidaria.Rules.RulesRepository.InvestigationRules
{
    public class SoatWithoutEventInvestigationRule_21 : Rule
    {
        public override void Define()
        {
            InvoiceToCheckSolidaria? invoiceToCheck = default;

            When()
                .Match(() => invoiceToCheck!, x => x.AtypicalEvent != null
                                                && x.AtypicalEvent.Any(y => y.SoatNumber == x.SoatNumber
                                                //&& siniestro sin investigación
                                                ));

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
                AlertDescription = "El número de póliza SOAT en la tabla de origen es igual a la placa en la tabla de consulta y no tiene resultado de investigación asociado al siniestro",
                AlertMessage = "Se debe enviar a investigar por alerta de atipicidad",
                Typification = typification,
                HasPriority = haspriority
            };

            return alert;
        }
    }
}
