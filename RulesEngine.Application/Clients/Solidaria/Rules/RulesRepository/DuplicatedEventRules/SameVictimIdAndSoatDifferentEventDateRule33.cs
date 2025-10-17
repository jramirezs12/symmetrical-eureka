using NRules.Fluent.Dsl;
using RulesEngine.Domain.Common;
using RulesEngine.Domain.RulesEntities.Solidaria.Entities;

namespace RulesEngine.Application.Clients.Solidaria.Rules.RulesRepository.DuplicatedEventRules
{
    public class SameVictimIdAndSoatDifferentEventDateRule33 : Rule
    {
        public override void Define()
        {
            InvoiceToCheckSolidaria? invoiceToCheck = default;

            When()
                .Match(() => invoiceToCheck!, x => x.ValidationAggregationRules_31_40 != null && x.ValidationAggregationRules_31_40.IdentificationNumberCase
                                  .Any(c => c.IdentificationNumber == x.VictimId && c.SoatNumber == x.SoatNumber));

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
                Description = "El numero de documento de la victima y el numero de poliza es igua a la tabla consultada pero la fecha de ocurrencia del evento que se está validando es diferente es diferente a la Fecha de ocurrencia del evento en la tabla de consulta",
                Message = "La victima ya cuenta con un siniestro con otra fecha de accidente",
                Typification = string.Empty,
                HasPriority = false
            };
        }
    }
}