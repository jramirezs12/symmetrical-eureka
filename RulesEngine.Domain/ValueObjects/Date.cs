using System.Globalization;

namespace RulesEngine.Domain.ValueObjects
{
    public record Date
    {
        private const string _Format1 = "dd/MM/yyyy HH:mm:ss.fff";
        private const string _Format2 = "yyyy-MM-dd'T'HH:mm:ss'Z'";
        private const string _Format3 = "MM/dd/yyyy h:mm:ss tt";
        private const string _Format4 = "dd/MM/yyyy";
        private const string _Format5 = "yyyy-MM-dd'T'HH:mm:ss.fff'Z'";
        private const string _Format6 = "M/d/yyyy h:mm:ss tt";
        private const string _Format7 = "yyyy-MM-dd HH:mm:ss";

        public DateTime? Value { get; init; }
        private readonly bool _IsFailedConversion = false;

        private Date(DateTime value)
        {
            Value = value;
        }

        private Date(DateTime? value, bool isFailedConversion)
        {
            Value = value;
            _IsFailedConversion = isFailedConversion;
        }

        public static Date Create(string dateTime)
        {
            try
            {
                if (string.IsNullOrEmpty(dateTime))
                    return new(null, false);
                else if (DateTime.TryParseExact(dateTime, _Format1, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date1))
                    return new(date1);
                else if (DateTime.TryParseExact(dateTime, _Format2, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date2))
                    return new(date2);
                else if (DateTime.TryParseExact(dateTime, _Format3, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date3))
                    return new(date3);
                else if (DateTime.TryParseExact(dateTime, _Format4, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date4))
                    return new(date4);
                else if (DateTime.TryParseExact(dateTime, _Format5, CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal, out DateTime date5))
                    return new(date5);
                else if (DateTime.TryParseExact(dateTime, _Format6, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date6))
                    return new(date6);
                else if (DateTime.TryParseExact(dateTime, _Format7, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date7))
                    return new(date7);
                else
                    return new(null, true);
            }
            catch
            {
                throw new Exception($"Ocurió un error al intentar convertir la fecha {dateTime} a un formato válido");
            }
        }

        public static Date ConvertToUtcFormattedDate(object? dateInput)
        {
            if (dateInput == null || string.IsNullOrEmpty(dateInput.ToString())) return Create(null!);

            string rawDate = dateInput.ToString() ?? string.Empty;

            if (DateTime.TryParse(rawDate, out DateTime parsedDate))
            {
                var utcDate = parsedDate.ToUniversalTime();
                string formattedUtcDate = utcDate.ToString("MM/dd/yyyy h:mm:ss tt", CultureInfo.InvariantCulture);
                return Create(formattedUtcDate);
            }

            return Create(null!);
        }

        public static bool IsFailedConversion(Date dateObject) => dateObject._IsFailedConversion;

        public static bool Equal(Date dateObject, DateTime? date)
        {
            if (dateObject.Value is not null && dateObject._IsFailedConversion != true && date is not null)
                if (dateObject.Value == date)
                    return true;
            return false;
        }

        public static bool Different(Date dateObject, DateTime? date)
        {
            if (dateObject.Value is not null && dateObject._IsFailedConversion != true && date is not null)
                if (dateObject.Value != date)
                    return true;
            return false;
        }

        public static bool GreaterThan(Date date1, Date date2)
        {
            if (date1.Value is not null && date1._IsFailedConversion != true && date2.Value is not null && date1._IsFailedConversion != true)
                if (date1.Value > date2.Value)
                    return true;
            return false;
        }

        public static bool IsNullable(Date date)
        {
            if (date?.Value is null)
                return true;
            return false;
        }

        public static bool IsNullable(Date date1, Date date2)
        {
            if (date1?.Value is null || date2?.Value is null)
                return true;
            return false;
        }

        public static bool IsNotNullable(Date date1, Date date2)
        {
            if (date1?.Value is not null && date2?.Value is not null)
                return true;
            return false;
        }

        public static bool IsNotNullable(Date date1, DateTime? date2)
        {
            if (date1?.Value is not null && date2 is not null)
                return true;
            return false;
        }
    }
}
