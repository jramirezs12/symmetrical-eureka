using MongoDB.Driver;
using NRules.Fluent.Dsl;
using RulesEngine.Domain.Common;
using RulesEngine.Domain.RulesEntities.Solidaria.Entities;
using RulesEngine.Domain.ValueObjects;

namespace RulesEngine.Application.Clients.Solidaria.Rules.RulesRepository.DuplicatedEventRules
{
    public class SameSoatAndVictimIdDifferentEventDateRule_33 : Rule
    {
        public override void Define()
        {
            InvoiceToCheckSolidaria? invoiceToCheck = default;

            When()
                .Match(() => invoiceToCheck!, x => x.DuplicatedInvoices.Any(d => d.VictimId == x.VictimId && d.SoatNumber == x.SoatNumber
                                                                                            && !Date.IsNullable(x.EventDate)
                                                                                            && Date.Different(x.EventDate, d.EventDate)));

            Then()
                .Do(w => invoiceToCheck!.AlertSolidaria.Add(CreateAlert()));
        }

        private static AlertSolidaria CreateAlert()
        {
            return new AlertSolidaria
            {
                NameAction = "Alerta",
                Type = "Regla duplicidad de siniestro",
                Module = "Reclamaciones",
                Description = "El número de documento de identidad de la víctima y el número de póliza SOAT en la tabla origen es igual al número de documento de identidad de la víctima y el número de póliza SOAT en la tabla de consulta y la fecha de ocurrencia del evento en la tabla de origen es diferente a la fecha de ocurrencia del evento en la tabla de consulta",
                Message = "La víctima ya cuenta con un siniestro con otra fecha de accidente",
                Typification = string.Empty,
                HasPriority = false
            };
        }
    }
}