using System;
using System.Diagnostics.Metrics;
using MySqlX.XDevAPI.Common;

namespace Scheduler.SharedCode
{
    public static class DateUtils
    {
        public static List<DateTime> GetDaysInRange(DateTime from, DateTime to)
        {
            var result = new List<DateTime>();

            while (from <= to)
            {
                var newDate = new DateTime(from.Year, from.Month, from.Day);
                result.Add(newDate);
                from = from.AddDays(1);
            }

            return result;
        }

        public static List<DateTime> GetDaysInRangeForAfter(DateTime from, int after)
        {
            var result = new List<DateTime>();

            for (int i = 1; i <= after; i++)
            {
                var newDate = new DateTime(from.Year, from.Month, from.Day);
                result.Add(newDate);
                from = from.AddDays(1);
            }

            return result;
        }

        public static List<DateTime> GetWeekdayInRange(DateTime from, DateTime to, DayOfWeek day)
        {
            const int daysInWeek = 7;
            var result = new List<DateTime>();
            var daysToAdd = ((int)day - (int)from.DayOfWeek + daysInWeek) % daysInWeek;

            do
            {
                from = from.AddDays(daysToAdd);
                if (from <= to)
                {
                    result.Add(from);
                    daysToAdd = daysInWeek;
                }
            } while (from < to);

            return result;
        }

        public static List<DateTime> GetWeekdayInRangeForAfter(DateTime from, DayOfWeek day, int after)
        {
            const int daysInWeek = 7;
            var result = new List<DateTime>();
            var daysToAdd = ((int)day - (int)from.DayOfWeek + daysInWeek) % daysInWeek;

            for (int i = 1; i <= after; i++)
            {
                from = from.AddDays(daysToAdd);
                result.Add(from);
                daysToAdd = daysInWeek;
            }

            return result;
        }

        public static List<DateTime> GetMonthlydayInRange(DateTime from, DateTime to, int monthDay)
        {
            var result = new List<DateTime>();

            while (from <= to)
            {
                var newDate = new DateTime(from.Year, from.Month, monthDay);
                result.Add(newDate);
                from = from.AddMonths(1);
            }

            return result;
        }

        public static List<DateTime> GetMonthlydayInRangeForAfter(DateTime from, int monthDay, int after)
        {
            var result = new List<DateTime>();

            for (int i = 1; i <= after; i++)
            {
                var newDate = new DateTime(from.Year, from.Month, monthDay);
                result.Add(newDate);
                from = from.AddMonths(1);
            }

            return result;
        }

        public static List<DateTime> GetYearlydayInRange(DateTime from, DateTime to, int monthDay)
        {
            var result = new List<DateTime>();

            while (from <= to)
            {
                var newDate = new DateTime(from.Year, from.Month, monthDay);
                result.Add(newDate);
                from = from.AddYears(1);
            }

            return result;
        }

        public static List<DateTime> GetYearlydayInRangeForAfter(DateTime from, int monthDay, int after)
        {
            var result = new List<DateTime>();

            for (int i = 1; i <= after; i++)
            {
                var newDate = new DateTime(from.Year, from.Month, monthDay);
                result.Add(newDate);
                from = from.AddYears(1);
            }

            return result;
        }
    }
}

