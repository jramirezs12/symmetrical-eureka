﻿using NRules.Fluent.Dsl;
using RulesEngine.Application.Actions;
using RulesEngine.Domain.Common;
using RulesEngine.Domain.RulesEntities.Solidaria.Entities;
using RulesEngine.Domain.ValueObjects;

namespace RulesEngine.Application.Clients.Solidaria.Rules.RulesRepository.PaymentRules
{
    public class GlossValueTotalGlossedValidationStageIIRule6 : Rule, ITrackableRule
    {
        public Action OnMatch { get; set; } = () => { };
        public override void Define()
        {
            InvoiceToCheckSolidaria? invoiceToCheck = default;

            When()
                .Match(() => invoiceToCheck, x => !Currency.IsNullable(x.InvoiceValue) && !Currency.IsNullable(x.TotalGlossedValue) &&
                                                    Equals(x.InvoiceValue, x.TotalGlossedValue));

            Then()
                .Do(w => invoiceToCheck!.AlertSolidaria.Add(CreateAlert()))
                .Do(ctx => OnMatch());
        }

        private AlertSolidaria CreateAlert()
        {
            return new AlertSolidaria
            {
                NameAction = "Enviar a Calidad",
                Type = "Regla de asignación a calidad - por glosa total",
                Module = "Reclamaciones",
                Description = "Valida si el total de la factura es igual al total glosado",
                Message = "Asignación a calidad por Glosa Total",
                Typification = string.Empty,
                HasPriority = false
            };
        }
    }
}