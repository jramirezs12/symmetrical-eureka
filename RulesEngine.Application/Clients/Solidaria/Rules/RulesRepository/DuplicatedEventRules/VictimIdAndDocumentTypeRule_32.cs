using MongoDB.Driver;
using NRules.Fluent.Dsl;
using RulesEngine.Domain.Common;
using RulesEngine.Domain.RulesEntities.Solidaria.Entities;

namespace RulesEngine.Application.Clients.Solidaria.Rules.RulesRepository.DuplicatedEventRules
{
    public class VictimIdAndDocumentTypeRule_32 : Rule
    {
        public override void Define()
        {
            InvoiceToCheckSolidaria? invoiceToCheck = default;

            When()
                .Match(() => invoiceToCheck!, x => x.DuplicatedInvoices.Any(d => d.VictimId == x.VictimId && d.DocumentType != x.DocumentType));


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
                Description = "El siniestro en la tabla origen es igual al siniestro en la tabla de consulta y el tipo de documento de identidad de la víctima en la tabla origen es diferente al tipo de documento de identidad de la víctima de la tabla de consulta",
                Message = "La víctima tiene un siniestro con otro tipo de documento",
                Typification = string.Empty,
                HasPriority = false
            };
        }
    }
}