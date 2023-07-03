using System.Globalization;
using Scheduler.Entity;
using Scheduler.SharedCode;

namespace Scheduler.Services
{
    public class ScheduleRepeater
    {
        public ScheduleRepeater()
        {
        }

        public List<Appointment> HandleRepeats(List<Appointment> appointments, string yearMonth)
        {
            var withRepeats = appointments.Where(a => a.isRepeat).ToList();
            List<Appointment> repeats = new List<Appointment>();

            foreach (var re in withRepeats)
            {
                if (re.Title == "next month")
                {

                }
                var repeatDates = GetDateRangeForRepeats(re, yearMonth);

                foreach (var rDate in repeatDates)
                {
                    Appointment clonedAppointment = CloneAppointment(re, rDate);
                    clonedAppointment.NumberOfRepeats = repeatDates.Count;
                    repeats.Add(clonedAppointment);
                }
            }

            return repeats;
        }

        public List<DateTime> GetDateRangeForRepeats(Appointment re, string yearMonth)
        {
            List<DateTime> dates = new List<DateTime>();

            if (re.RepeatEnd == RepeatEndEnum.Never)
            {
                DateTime dateOfMonth = DateTime.ParseExact(yearMonth, "M/yyyy", CultureInfo.InvariantCulture);
                int daysInAMonth = DateTime.DaysInMonth(dateOfMonth.Year, dateOfMonth.Month);
                DateTime fullDaysOfCurrentMonth = new DateTime(dateOfMonth.Year, dateOfMonth.Month, daysInAMonth);

                var repeatDates = GenerateDateRange(re.Date, fullDaysOfCurrentMonth, re.RepeatSelection, re.HasNoWeekEnds);
                dates.AddRange(repeatDates);

            }
            else if (re.RepeatEnd == RepeatEndEnum.After)
            {
                var repeatDates = GenerateDateRangeForAfter(re.Date, re.RepeatSelection, re.After, re.HasNoWeekEnds);
                dates.AddRange(repeatDates);

            }
            else if (re.RepeatEnd == RepeatEndEnum.OnDate)
            {
                var repeatDates = GenerateDateRange(re.Date, re.OnDate, re.RepeatSelection, re.HasNoWeekEnds);
                dates.AddRange(repeatDates);
            }

            return dates;
        }

        private List<DateTime> GenerateDateRange(DateTime startDate, DateTime toDate, RepeatSelectionEnum repeatSelection, bool hasNoWeekEnds)
        {
            List<DateTime> repeatDates = new List<DateTime>();

            if (repeatSelection == RepeatSelectionEnum.EveryDay && hasNoWeekEnds)
            {
                var daysDate = DateUtils.GetDaysInRange(startDate, toDate);
                daysDate = DateUtils.RemoveWeekEnds(daysDate);
                repeatDates.AddRange(daysDate);
            }
            else if (repeatSelection == RepeatSelectionEnum.EveryDay && !hasNoWeekEnds)
            {
                var daysDate = DateUtils.GetDaysInRange(startDate, toDate);
                repeatDates.AddRange(daysDate);
            }

            if (repeatSelection == RepeatSelectionEnum.EveryWeek)
            {
                var weeklyDates = DateUtils.GetWeekdayInRange(startDate, toDate, startDate.DayOfWeek);
                repeatDates = weeklyDates;
            }
            else if (repeatSelection == RepeatSelectionEnum.EveryMonth)
            {
                var monthlyDates = DateUtils.GetMonthlydayInRange(startDate, toDate, startDate.Day);
                repeatDates = monthlyDates;
            }
            else if (repeatSelection == RepeatSelectionEnum.EveryYear)
            {
                var yearlyDates = DateUtils.GetYearlydayInRange(startDate, toDate, startDate.Day);
                repeatDates = yearlyDates;
            }

            return repeatDates;
        }

        private List<DateTime> GenerateDateRangeForAfter(DateTime startDate, RepeatSelectionEnum repeatSelection, int after, bool hasNoWeekEnds)
        {
            List<DateTime> repeatDates = new List<DateTime>();

            if (repeatSelection == RepeatSelectionEnum.EveryDay && hasNoWeekEnds)
            {
                var daysDate = DateUtils.GetDaysInRangeForAfter_NoWeekEnds(startDate, after);
                repeatDates.AddRange(daysDate);
            }
            else if (repeatSelection == RepeatSelectionEnum.EveryDay && !hasNoWeekEnds)
            {
                var daysDate = DateUtils.GetDaysInRangeForAfter(startDate, after);
                repeatDates.AddRange(daysDate);
            }

            if (repeatSelection == RepeatSelectionEnum.EveryWeek)
            {
                var weeklyDates = DateUtils.GetWeekdayInRangeForAfter(startDate, startDate.DayOfWeek, after);
                repeatDates = weeklyDates;
            }
            else if (repeatSelection == RepeatSelectionEnum.EveryMonth)
            {
                var monthlyDates = DateUtils.GetMonthlydayInRangeForAfter(startDate, startDate.Day, after);
                repeatDates = monthlyDates;
            }
            else if (repeatSelection == RepeatSelectionEnum.EveryYear)
            {
                var yearlyDates = DateUtils.GetYearlydayInRangeForAfter(startDate, startDate.Day, after);
                repeatDates = yearlyDates;
            }

            return repeatDates;
        }

        private Appointment CloneAppointment(Appointment appointment, DateTime cloneDate)
        {
            Appointment cloned = new Appointment();
            cloned.Id = appointment.Id;
            cloned.Location = appointment.Location;
            cloned.Title = appointment.Title;
            cloned.Color = appointment.Color;
            cloned.Details = appointment.Details;
            cloned.YearMonth = appointment.YearMonth;
            cloned.Date = new DateTime(cloneDate.Year, cloneDate.Month, cloneDate.Day);
            cloned.Time = appointment.Time;
            cloned.EndDateSpan = appointment.EndDateSpan;
            cloned.EndTime = appointment.EndTime;
            cloned.CreatorId = appointment.CreatorId;
            cloned.Creator = appointment.Creator;
            cloned.Type = appointment.Type;
            cloned.IsDone = appointment.IsDone;

            cloned.isRepeat = appointment.isRepeat;
            cloned.RepeatSelection = appointment.RepeatSelection;
            cloned.RepeatEnd = appointment.RepeatEnd;
            cloned.After = appointment.After;
            cloned.OnDate = appointment.OnDate;
            cloned.IsClone = true;
            cloned.IsDeleted = appointment.IsDeleted;
            cloned.HasNoWeekEnds = appointment.HasNoWeekEnds;

            return cloned;
        }
    }
}

