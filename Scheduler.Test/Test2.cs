using System.Globalization;
using Microsoft.AspNetCore.Http;
using MySql.Data.MySqlClient;
using Scheduler.Entity;
using Scheduler.Models;
using Scheduler.Services;
using Scheduler.SharedCode;

namespace Scheduler.Test
{
    public class Test2
    {
        public Test2()
        {
        }

        [Test]
        public void Test_GetDaysInRange()
        {
            DateTime from = new DateTime(2022, 9, 9);
            DateTime to = new DateTime(2022, 10, 10);

            var dates = DateUtils.GetDaysInRange(from, to);
        }

        [Test]
        public void Test_GetMonthlydayInRange()
        {
            DateTime from = new DateTime(2022, 10, 9);
            DateTime to = new DateTime(2022, 12, 8);

            var dates = DateUtils.GetMonthlydayInRange(from, to, 9);
        }

        [Test]
        public void Test_GetYearlydayInRange()
        {
            DateTime from = new DateTime(2022, 9, 9);
            DateTime to = new DateTime(2025, 10, 9);

            var dates = DateUtils.GetYearlydayInRange (from, to, 9);
        }

        [Test]
        public void Test_GetYearlydayInRangeWithAfter()
        {
            DateTime from = new DateTime(2022, 9, 9);

            var dates = DateUtils.GetYearlydayInRangeForAfter(from, 9, 10);
        }

        [Test]
        public void Test_GetDaysInRangeWithAfter()
        {
            DateTime from = new DateTime(2022, 9, 22);
            int after = 35;

            var dates = DateUtils.GetDaysInRangeForAfter(from, after);
        }

        [Test]
        public void Test_GetWeekdayInRange()
        {
            DateTime from = new DateTime(2022, 9, 6);
            DateTime to = new DateTime(2022, 11, 15);

            var dates = DateUtils.GetWeekdayInRange(from, to, DayOfWeek.Tuesday);
        }

        [Test]
        public void Test_GetWeekDaysInRangeWithAfter()
        {
            DateTime from = new DateTime(2022, 9, 6);
            int after = 9;

            var dates = DateUtils.GetWeekdayInRangeForAfter(from, DayOfWeek.Tuesday, after);
        }

        [Test]
        public void TestRepeat()
        {
            string yearMonth = "9/2022";

            List<EventModel> results = new List<EventModel>();

            AppointmentRepository _appointmentRepository = new AppointmentRepository();

            var ownedEvents = _appointmentRepository.GetAppointments("john", yearMonth);

            var events = _appointmentRepository.GetMeetings(4, yearMonth);
            var groupInvites = _appointmentRepository.GetMeetingsThatYoureInvited(4, yearMonth);
            events = events.UnionBy(ownedEvents, e => e.Id).ToList(); //Combine and remove duplicates;
            events = events.UnionBy(groupInvites, e => e.Id).ToList();

            var ownedEventsWithRepeats = _appointmentRepository.GetAppointmentsWithRepeats("john");

            var eventsWithRepeats = _appointmentRepository.GetMeetingsWithRepeat(4);
            var groupInvitesWithRepeats = _appointmentRepository.GetMeetingsThatYoureInvitedWithRepeats(4);
            eventsWithRepeats = eventsWithRepeats.UnionBy(ownedEventsWithRepeats, e => e.Id).ToList(); //Combine and remove duplicates;
            eventsWithRepeats = eventsWithRepeats.UnionBy(groupInvitesWithRepeats, e => e.Id).ToList();

            eventsWithRepeats = RemoveFutureRepeats(eventsWithRepeats, yearMonth);

            ScheduleRepeater repeatGenerator = new ScheduleRepeater();
            var repeats = repeatGenerator.HandleRepeats(eventsWithRepeats, yearMonth);

            events.AddRange(repeats);
            results = ClassConverter.ConvertToEventModel(events);
        }

        private List<Appointment> RemoveFutureRepeats(List<Appointment> apps, string yearMonth)
        {
            DateTime dateOfMonth = DateTime.ParseExact(yearMonth, "M/yyyy", CultureInfo.InvariantCulture);
            dateOfMonth = new DateTime(dateOfMonth.Year, dateOfMonth.Month, DateTime.DaysInMonth(dateOfMonth.Year, dateOfMonth.Month));
            return apps.Where(e => e.Date <= dateOfMonth).ToList();
        }

        [Test]
        public void TestEditRepeat()
        {
            string yearMonth = "9/2022";

            List<EventModel> results = new List<EventModel>();

            AppointmentRepository _appointmentRepository = new AppointmentRepository();

            var ownedEvents = _appointmentRepository.GetAppointments("john", yearMonth);

            var events = _appointmentRepository.GetMeetings(4, yearMonth);
            var groupInvites = _appointmentRepository.GetMeetingsThatYoureInvited(4, yearMonth);
            events = events.UnionBy(ownedEvents, e => e.Id).ToList(); //Combine and remove duplicates;
            events = events.UnionBy(groupInvites, e => e.Id).ToList();

            var ownedEventsWithRepeats = _appointmentRepository.GetAppointmentsWithRepeats("john");

            var eventsWithRepeats = _appointmentRepository.GetMeetingsWithRepeat(4);
            var groupInvitesWithRepeats = _appointmentRepository.GetMeetingsThatYoureInvitedWithRepeats(4);
            eventsWithRepeats = eventsWithRepeats.UnionBy(ownedEventsWithRepeats, e => e.Id).ToList(); //Combine and remove duplicates;
            eventsWithRepeats = eventsWithRepeats.UnionBy(groupInvitesWithRepeats, e => e.Id).ToList();

            eventsWithRepeats = RemoveFutureRepeats(eventsWithRepeats, yearMonth);

            ScheduleRepeater repeatGenerator = new ScheduleRepeater();
            var repeats = repeatGenerator.HandleRepeats(eventsWithRepeats, yearMonth);
            ApplyEditRepeats(repeats);

            events.AddRange(repeats);
            results = ClassConverter.ConvertToEventModel(events);
        }

        private void ApplyEditRepeats(List<Appointment> apps)
        {
            AppointmentRepository _appointmentRepository = new AppointmentRepository();
            var repeatEdits = new List<RepeatEdit>();

            var withRepeats = apps.Where(a => a.isRepeat).Select(a => a.Id).Distinct().ToList();

            foreach(var r in withRepeats)
            {
                var editRepeats = _appointmentRepository.GetRepeatEdits(r);
                foreach (var er in editRepeats)
                {
                    var ap = apps.FirstOrDefault(wr => wr.Id == er.AppointmentId && wr.Date == er.OriginalDate);
                    ap.RepeatEdit = er;
                }
            }

            var hello = repeatEdits;
        }

        [Test]
        public void TestGetImage()
        {
            UserRepository userRepository = new UserRepository();

            var userPicture = userRepository.GetUserPicture(4);
        }
    }
}

