using NRules.Fluent.Dsl;
using RulesEngine.Domain.Common;
using RulesEngine.Domain.RulesEntities.Solidaria.Entities;

namespace RulesEngine.Application.Clients.Solidaria.Rules.RulesRepository.DuplicatedEventRules
{
    public class SameVictimIdDifferentDocumentTypeRule32 : Rule
    {
        public override void Define()
        {
            InvoiceToCheckSolidaria? invoiceToCheck = default;

            When()
                .Match(() => invoiceToCheck!, x => x.ValidationAggregationRules_31_40 != null && x.ValidationAggregationRules_31_40.IdentificationNumberCase
                              .Any(c => c.IdentificationNumber == x.VictimId && c.DocumentType == x.DocumentType));

            Then()
                .Do(w => invoiceToCheck!.AlertSolidaria.Add(CreateAlert()));
        }

        private static AlertSolidaria CreateAlert()
        {
            return new AlertSolidaria
            {
                NameAction = "Alerta",
                Type = "Regla de duplicidad de siniestro",
                Module = "Reclamaciones",
                Description = "El número de documento de la víctima en la tabla origen es igual al número de documento de la víctima en la tabla consulta y el tipo de documento en la tabla de origen es diferente en la tabla de consulta",
                Message = "La victima tiene un siniestro con otro tipo de documento",
                Typification = string.Empty,
                HasPriority = false
            };
        }
    }
}