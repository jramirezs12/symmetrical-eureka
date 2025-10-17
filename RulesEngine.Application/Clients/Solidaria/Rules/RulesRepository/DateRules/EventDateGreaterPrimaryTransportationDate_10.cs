using NRules.Fluent.Dsl;
using RulesEngine.Application.Actions;
using RulesEngine.Domain.Common;
using RulesEngine.Domain.RulesEntities.Solidaria.Entities;
using RulesEngine.Domain.ValueObjects;

namespace RulesEngine.Application.Clients.Solidaria.Rules.RulesRepository.DateRules
{
    public class EventDateGreaterPrimaryTransportationDate_10 : Rule, ITrackableRule
    {
        public Action OnMatch { get; set; } = () => { };

        public override void Define()
        {
            InvoiceToCheckSolidaria? invoiceToCheck = default;

            When()
                .Match(() => invoiceToCheck!, x => Date.IsNotNullable(x.EventDate, x.PrimaryTransportationDate) &&
                                                   Date.GreaterThan(x.EventDate, x.PrimaryTransportationDate));

            Then()
                .Do(w => invoiceToCheck!.AlertSolidaria.Add(CreateAlert()))
                .Do(ctx => OnMatch());
        }

        private static AlertSolidaria CreateAlert()
        {
            return new AlertSolidaria
            {
                NameAction = "Devolver Reclamación",
                Type = "Regla lógica de fechas",
                Module = "Reclamaciones",
                Description = "Permite validar si la fecha de ocurrencia del evento es posterior a la fecha de trasporte primario, lo que conlleva a la devolución de la reclamación",
                Message = "Se debe aplicar devolución teniendo en cuenta que el transporte primario no puede ser posterior a la facturación del trasnporte primario. Carta de devolución # xxx",
                Typification = string.Empty,
                HasPriority = false
            };
        }
    }
}
