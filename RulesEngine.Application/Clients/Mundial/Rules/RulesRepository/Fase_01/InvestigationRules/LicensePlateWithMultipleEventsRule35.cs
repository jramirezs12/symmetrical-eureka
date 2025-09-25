using MongoDB.Driver;
using NRules.Fluent.Dsl;
using RulesEngine.Application.Actions;
using RulesEngine.Domain.Common;
using RulesEngine.Domain.Invoices.Entities;
using RulesEngine.Domain.Invoices.Repositories;
using RulesEngine.Domain.Primitives;
using RulesEngine.Domain.RulesEntities.Mundial.Entities;
using RulesEngine.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RulesEngine.Application.Clients.Mundial.Rules.RulesRepository.Fase_01.InvestigationRules
{
    public class LicensePlateWithMultipleEventsRule35 : Rule, ITrackableRule
    {
        public Action OnMatch { get; set; } = () => { };
        public override void Define()
        {
            InvoiceToCheck? invoiceToCheck = default;

            When()
               .Match(() => invoiceToCheck!, x => x.ValidationAggregationRules_31_40 != null && x.LicensePlate != null
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

        private static Alert CreateAlert(InvoiceToCheck invoice)
        {
            var validations = invoice.ValidationAggregationRules_31_40;
            int eventCount = validations.LicensePlateCase.Count(x => invoice.LicensePlate == x.LicensePlate && invoice.EventDate.Value.Value.ToString("yyyy-MM-dd") != x.EventDate);

            var alert = new Alert
            {
                AlertAction = "Alert",
                AlertNameAction = "Alerta",
                AlertType = "Regla de investigación",
                AlertDescription = "El número de placa del vehículo se encuentra en la tabla de consulta con más de un evento",
                AlertMessage = $"Para esta placa existen {eventCount} Eventos"
            };

            return alert;
        }
    }
}
