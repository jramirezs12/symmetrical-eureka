using NRules.Fluent.Dsl;
using RulesEngine.Application.Actions;
using RulesEngine.Domain.Common;
using RulesEngine.Domain.RulesEntities.Mundial.Entities;
using RulesEngine.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RulesEngine.Application.Clients.Mundial.Rules.RulesRepository.Fase_03.InvestigationRules
{
    public class LicensePlateWithMultipleEventsRule36 : Rule, ITrackableRule
    {
        public Action OnMatch { get; set; } = () => { };
        public override void Define()
        {
            InvoiceToCheck? invoiceToCheck = default;

            When()
               .Match(() => invoiceToCheck!, x => x.ValidationAggregationRules_31_40 != null
                    && x.LicensePlate != null
                    && !Date.IsNullable(x.EventDate)
                    && x.ValidationAggregationRules_31_40.LicensePlateCase
                        .Count(c =>
                            c.LicensePlate == x.LicensePlate
                            && c.EventDate != x.EventDate.Value.Value.ToString("yyyy-MM-dd")
                        ) >= x.ValidationAggregationRules_31_40.ParameterRule35And36
                );
            Then()
                .Do(ctx => invoiceToCheck!.Alerts.Add(CreateAlert(invoiceToCheck)))
                .Do(ctx => OnMatch());
        }

        private Alert CreateAlert(InvoiceToCheck invoiceToCheck)
        {
            string typification = invoiceToCheck.TypificationMap.GetValueOrDefault(GetType().Name, "Sin typification");
            bool haspriority = invoiceToCheck.HasPriorityMap.GetValueOrDefault(GetType().Name, false);

            var alert = new Alert
            {
                AlertAction = "Alert",
                AlertNameAction = "Enviar a Investigar",
                AlertType = "Regla de investigación",
                AlertDescription = "El número de placa del vehículo se encuentra en la tabla de consulta con más de un evento",
                AlertMessage = $"Se debe enviar a investigar,evento multiple",
                Typification = typification,
                HasPriority = haspriority
            };

            return alert;
        }
    }
}
