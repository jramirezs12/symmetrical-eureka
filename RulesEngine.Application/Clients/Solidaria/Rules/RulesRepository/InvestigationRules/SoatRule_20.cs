using MongoDB.Driver;
using NRules.Fluent.Dsl;
using RulesEngine.Domain.Common;
using RulesEngine.Domain.RulesEntities.Solidaria.Entities;

namespace RulesEngine.Application.Clients.Solidaria.Rules.RulesRepository.InvestigationRules
{
    public class SoatRule_20 : Rule
    {
        public override void Define()
        {
            InvoiceToCheckSolidaria? invoiceToCheck = default;

            When()
                .Match(() => invoiceToCheck!, x => x.AtypicalEvent != null && x.AtypicalEvent.Any(y => y.SoatNumber == x.SoatNumber));

            Then()
                .Do(w => invoiceToCheck!.AlertSolidaria.Add(CreateAlert(invoiceToCheck)));
        }

        private static AlertSolidaria CreateAlert(InvoiceToCheckSolidaria invoiceToCheck)
        {
            return new AlertSolidaria
            {
                NameAction = "Alerta",
                Type = "Reglas de investigación",
                Module = "Reclamaciones",
                Description = "El número de póliza SOAT coincide con la consulta",
                Message = $"El número de póliza SOAT presenta la siguiente alerta: {invoiceToCheck.AtypicalEvent!.First(x => x.SoatNumber == invoiceToCheck.SoatNumber).AlertDescription}",
                Typification = string.Empty,
                HasPriority = false
            };
        }
    }
}