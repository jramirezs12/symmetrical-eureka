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
                .Do(w => invoiceToCheck!.AlertSolidaria.Add(CreateAlert(invoiceToCheck)));
        }

        private AlertSolidaria CreateAlert(InvoiceToCheckSolidaria invoiceToCheck)
        {
            string typification = invoiceToCheck.TypificationMap.GetValueOrDefault(GetType().Name, "Sin typification");
            bool haspriority = invoiceToCheck.HasPriorityMap.GetValueOrDefault(GetType().Name, false);

            return new AlertSolidaria
            {
                NameAction = "Enviar a investigar",
                Type = "Reglas de investigación",
                Module = "Reclamaciones",
                Description = "El SOAT coincide y no hay investigación asociada al siniestro",
                Message = "Se debe enviar a investigar por alerta de atipicidad",
                Typification = typification,
                HasPriority = haspriority
            };
        }
    }
}