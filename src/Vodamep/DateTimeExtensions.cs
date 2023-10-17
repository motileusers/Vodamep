using Google.Protobuf.WellKnownTypes;
using System;
using System.Globalization;

namespace Vodamep
{
    internal static class DateTimeExtensions
    {

        public static DateTime LastDateInMonth(this DateTime date) => new DateTime(date.Year, date.Month, 1).AddMonths(1).AddDays(-1);

        public static bool IsLastDateInMonth(this DateTime date)
        {
            DateTime lastDayInMonth = LastDateInMonth(date);
            if (lastDayInMonth == date)
                return true;
            else
                return false;
        }

        public static DateTime FirstDateInMonth(this DateTime date) => new DateTime(date.Year, date.Month, 1);

        public static bool IsFirstDateInMonth(this DateTime date)
        {
            DateTime firstDayInMonth = FirstDateInMonth(date);
            if (firstDayInMonth == date)
                return true;
            else
                return false;
        }

        public static Timestamp AsTimestamp(this DateTime date) => Timestamp.FromDateTime(date.IgnoreTime());

        public static DateTime AsDate(this Timestamp value) => value?.ToDateTime().IgnoreTime() ?? DateTime.MinValue;       

        public static DateTime IgnoreTime(this DateTime date) => new DateTime(date.Year, date.Month, date.Day, 0, 0, 0, DateTimeKind.Utc);

        /// <summary>
        /// Wandelt einen string im format yyyy-MM-dd in ein Datum
        /// </summary>        
        public static DateTime AsDate(this string value)
        {
            DateTime result;

            // Wir unterstützen genau diese zwei Typen 2001-01-01 und 01.01.2001

            if (value.Contains("-"))
            {
                result = DateTime.ParseExact(value, "yyyy-MM-dd", CultureInfo.InvariantCulture).IgnoreTime();
            }
            else if (value.Contains("."))
            {
                result = DateTime.ParseExact(value, "dd.MM.yyyy", CultureInfo.InvariantCulture).IgnoreTime();
            }
            else
            {
                throw new Exception($"Fehler bei der DateTime Konvertierung von {value}");
            }

            return result;
        } 


    }
}
