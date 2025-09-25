using ErrorOr;
using Microsoft.VisualBasic;
using MongoDB.Driver.Core.Operations;
using RulesEngine.Domain.Invoices.Entities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RulesEngine.Domain.ValueObjects
{
    public record Currency
    {
        public decimal? Value { get; init; }
        private readonly bool _IsFailedConversion = false;

        private Currency(decimal value)
        {
            Value = value;
        }

        private Currency(decimal? value, bool isFailedConversion)
        {
            Value = value;
            _IsFailedConversion = isFailedConversion;
        }

        public static Currency Create(string value)
        {
            if (string.IsNullOrEmpty(value))
                return new(null, false);
            else if (decimal.TryParse(value, out decimal result))
                return new(result);
            else
                return new(null, true);
        }

        public static bool IsFailedConversion(Currency currencyObject) => currencyObject._IsFailedConversion;

        public static bool IsNullable(Currency currencyObject)
        {
            if (currencyObject?.Value is null)
                return true;
            return false;
        }

        public static bool IsNullable(Currency currencyObject1, Currency currencyObject2, Currency currencyObject3)
        {
            if (currencyObject1?.Value is null && currencyObject2?.Value is null && currencyObject3?.Value is null)
                return true;
            return false;
        }

        public static int Length(Currency currencyObject)
        {
            if (currencyObject?.Value is not null)
                return currencyObject.Value.ToString()!.Length;
            return 0;
        }
    }
}
