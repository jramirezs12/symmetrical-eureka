using NRules.Fluent.Dsl;
using RulesEngine.Application.Actions;
using RulesEngine.Domain.Common;
using RulesEngine.Domain.RulesEntities.Mundial.Entities;

namespace RulesEngine.Application.Clients.Mundial.Rules.RulesRepository.Fase_01.DuplicatedEventRules
{
    public class SameVictimIdDifferentDocumentTypeRule32 : Rule, ITrackableRule
    {
        public Action OnMatch { get; set; } = () => { };
        public override void Define()
        {
            InvoiceToCheck? invoiceToCheck = default;

           When()
             .Match(() => invoiceToCheck!, x => x.ValidationAggregationRules_31_40 != null
                    && x.VictimId != null
                    && x.DocumentType != null
                    && x.ValidationAggregationRules_31_40.IdentificationNumberCase
                                 .Any(c => c.IdentificationNumber == x.VictimId && c.DocumentType == x.DocumentType));

            Then()
                .Do(w => invoiceToCheck!.Alerts.Add(CreateAlert()))
                .Do(ctx => OnMatch());
        }

        private static Alert CreateAlert()
        {
            var alert = new Alert
            {
                AlertAction = "Alert",
                AlertNameAction = "Alerta",
                AlertType = "Regla de duplicidad de siniestro",
                AlertDescription = "El número de documento de la víctima en la tabla origen es igual al número de documento de la víctima en la tabla consulta y el tipo de documento en la tabla de origen es diferente en la tabla de consulta",
                AlertMessage = "La victima tiene un siniestro con otro tipo de documento"
            };

            return alert;
        }
    }
}
