using System.Globalization;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using Scheduler.Entity;
using Scheduler.Models;
using Scheduler.SharedCode;

namespace Scheduler.Services
{
    public class AppointmentRepository
    {
        private MySqlConnection _conn;
        private SchedulerDbContext _dbContext;

        public AppointmentRepository()
        {
            //_conn = new MySqlConnection(StaticConfig.ConStr);
            //_dbContext = new SchedulerDbContext(_conn, false);
            //_dbContext.Database.CreateIfNotExists();
            //_conn.Open();

            _dbContext = new SchedulerDbContext();
        }

        public List<Appointment> GetAppointments(string ownerUsername, string yearMonth)
        {
            return _dbContext.Appointments.Include("Creator").Where(a => a.Creator.UserName == ownerUsername && a.YearMonth == yearMonth && a.isRepeat == false).ToList();
        }

        public List<Appointment> GetAppointmentsWithRepeats(string ownerUsername)
        {
            return _dbContext.Appointments.Include("Creator").Where(a => a.Creator.UserName == ownerUsername && a.isRepeat == true).ToList();
        }

        public List<Appointment> GetMeetings(int participantId, string yearMonth)
        {
            return _dbContext.Appointments.Include("Creator")
                .Where(a => a.isRepeat == false && a.YearMonth == yearMonth && a.Meetings.Any(m => m.ParticipantId == participantId))
                .ToList();
        }

        public List<Appointment> GetMeetingsWithRepeat(int participantId)
        {
            return _dbContext.Appointments.Include("Creator")
                .Where(a => a.isRepeat == true && a.Meetings.Any(m => m.ParticipantId == participantId))
                .ToList();
        }

        public List<Appointment> GetMeetingsThatYoureInvited(int participantId, string yearMonth)
        {
            var query = (from a in _dbContext.Appointments
                         join gm in _dbContext.GroupsMeetings on a.Id equals gm.MeetingId
                         join g in _dbContext.Groups on gm.ParticipantId equals g.Id
                         join ug in _dbContext.UsersGroups on g.Id equals ug.GroupId
                         join u in _dbContext.Users on a.CreatorId equals u.Id
                         where (ug.UserId == participantId && a.YearMonth == yearMonth && a.isRepeat == false)
                         select new { appoint = a, creator = u});

            var appointments = query.ToList();
            List<Appointment> result = new List<Appointment>();

            foreach (var a in appointments)
            {
                a.appoint.Creator = a.creator;
                result.Add(a.appoint);
            }

            return result;
        }

        public List<Appointment> GetMeetingsThatYoureInvitedWithRepeats(int participantId)
        {
            var query = (from a in _dbContext.Appointments
                         join gm in _dbContext.GroupsMeetings on a.Id equals gm.MeetingId
                         join g in _dbContext.Groups on gm.ParticipantId equals g.Id
                         join ug in _dbContext.UsersGroups on g.Id equals ug.GroupId
                         join u in _dbContext.Users on a.CreatorId equals u.Id
                         where (ug.UserId == participantId && a.isRepeat ==  true)
                         select new { appoint = a, creator = u });

            var appointments = query.ToList();
            List<Appointment> result = new List<Appointment>();

            foreach (var a in appointments)
            {
                a.appoint.Creator = a.creator;
                result.Add(a.appoint);
            }

            return result;
        }

        public Appointment GetAppointmentById(int id)
        {
            return _dbContext.Appointments.FirstOrDefault(a => a.Id == id);
        }

        public Appointment AddAppointment(string ownerUsername, Appointment appointment)
        {
            _dbContext.Database.BeginTransaction();
            try
            {
                var user = _dbContext.Users.FirstOrDefault(u => u.UserName == ownerUsername);
                user.Appointments.Add(appointment);
                _dbContext.Appointments.Add(appointment);
                _dbContext.SaveChanges();
                _dbContext.Database.CommitTransaction();
                return appointment;
            }
            catch (Exception ex)
            {
                _dbContext.Database.RollbackTransaction();
                throw ex;
            }
        }

        public void EditAppointment(Appointment appointmentToEdit)
        {
            _dbContext.Database.BeginTransaction();
            try
            {
                var appoint = _dbContext.Appointments.FirstOrDefault(a => a.Id == appointmentToEdit.Id);
                if (appoint == null)
                {
                    throw new ArgumentException("Cannot find existing appointment Id=" + appointmentToEdit.Id);
                }

                appoint.Location = appointmentToEdit.Location;
                appoint.Title = appointmentToEdit.Title;
                appoint.Details = appointmentToEdit.Details;

                DateTime monthYear = Convert.ToDateTime(appointmentToEdit.Date);

                if (!appointmentToEdit.IsClone)
                {
                    appoint.Date = appointmentToEdit.Date;
                }

                appoint.YearMonth = monthYear.Month + "/" + monthYear.Year;
                appoint.Time = appointmentToEdit.Time;
                appoint.isRepeat = appointmentToEdit.isRepeat;
                appoint.RepeatSelection = appointmentToEdit.RepeatSelection;
                appoint.RepeatEnd = appointmentToEdit.RepeatEnd;
                appoint.After = appointmentToEdit.After;
                appoint.OnDate = appointmentToEdit.OnDate;
                appoint.IsDone = appointmentToEdit.IsDone;

                _dbContext.SaveChanges();
                _dbContext.Database.CommitTransaction();
            }
            catch (Exception ex)
            {
                _dbContext.Database.RollbackTransaction();
                throw ex;
            }
        }

        public void EditAllRepeatAppointment(EventModel ev)
        {
            try
            {
                var appoint = _dbContext.Appointments.FirstOrDefault(a => a.Id == ev.Id);
                if (appoint == null)
                {
                    throw new ArgumentException("Cannot find existing appointment Id=" + ev.Id);
                }
                var originalDate = DateTime.ParseExact(ev.OriginalDate, "MM/dd/yyyy", CultureInfo.InvariantCulture);
                var editRepeat = _dbContext.RepeatEdits.FirstOrDefault(r => r.AppointmentId == ev.Id && r.OriginalDate == originalDate);

                if (editRepeat != null) {
                    editRepeat.Title = ev.Title;
                    editRepeat.Location = ev.Location;
                    editRepeat.Details = ev.Details;
                    editRepeat.EditedDate = DateTime.ParseExact(ev.Date, "MM/dd/yyyy", CultureInfo.InvariantCulture);
                    editRepeat.Time = ev.Time;
                    editRepeat.IsDeleted = ev.IsDeleted;
                    editRepeat.IsDone = ev.IsDone;

                    _dbContext.SaveChanges();
                }
                else
                {
                    var newEditRepeat = new RepeatEdit();
                    newEditRepeat.AppointmentId = ev.Id;
                    newEditRepeat.Title = ev.Title;
                    newEditRepeat.Location = ev.Location;
                    newEditRepeat.Details = ev.Details;
                    newEditRepeat.OriginalDate = originalDate;
                    newEditRepeat.EditedDate = DateTime.ParseExact(ev.Date, "MM/dd/yyyy", CultureInfo.InvariantCulture);
                    newEditRepeat.Time = ev.Time;
                    newEditRepeat.IsDeleted = ev.IsDeleted;
                    newEditRepeat.IsDone = ev.IsDone;

                    _dbContext.RepeatEdits.Add(newEditRepeat);
                    _dbContext.SaveChanges();
                }

            }
            catch (Exception ex)
            {
                //_dbContext.Database.RollbackTransaction();
                throw ex;
            }
        }

        public void MarkRepeatAsDeleted(int appointmentId, DateTime originalDate)
        {
            try
            {
                var appoint = _dbContext.Appointments.FirstOrDefault(a => a.Id == appointmentId);
                if (appoint == null)
                {
                    throw new ArgumentException("Cannot find existing appointment Id=" + appointmentId);
                }
                var editRepeat = _dbContext.RepeatEdits.FirstOrDefault(r => r.AppointmentId == appointmentId && r.OriginalDate == originalDate);

                if (editRepeat != null)
                {
                    editRepeat.IsDeleted = true;

                    _dbContext.SaveChanges();
                }
                else
                {
                    var newEditRepeat = new RepeatEdit();
                    newEditRepeat.AppointmentId = appointmentId;
                    newEditRepeat.OriginalDate = originalDate;
                    newEditRepeat.EditedDate = originalDate;
                    newEditRepeat.IsDeleted = true;

                    _dbContext.RepeatEdits.Add(newEditRepeat);
                    _dbContext.SaveChanges();
                }

            }
            catch (Exception ex)
            {
                //_dbContext.Database.RollbackTransaction();
                throw ex;
            }
        }

        public void EditAppointmentRepeatSettings(EventModel model)
        {
            var originalEvent = _dbContext.Appointments.FirstOrDefault(a => a.Id == model.Id);

            originalEvent.RepeatSelection = model.RepeatSelection;
            originalEvent.RepeatEnd = model.RepeatEnd;
            originalEvent.After = model.After;
            originalEvent.OnDate = DateTime.ParseExact(model.OnDate, "MM/dd/yyyy", CultureInfo.InvariantCulture);

            _dbContext.SaveChanges();
        }

        public void DeleteRepeats(int appointmentId)
        {
            var repeats = _dbContext.RepeatEdits.Where(r => r.AppointmentId == appointmentId).ToList();

            if (repeats.Count > 0)
            {
                _dbContext.RepeatEdits.RemoveRange(repeats);
                _dbContext.SaveChanges();
            }
        }

        public void ChangeScheduleDate(int appointmentId, DateTime newDate)
        {
            _dbContext.Database.BeginTransaction();
            try
            {
                var appoint = _dbContext.Appointments.FirstOrDefault(a => a.Id == appointmentId);
                if (appoint == null)
                {
                    throw new ArgumentException("Cannot find existing appointment Id=" + appointmentId);
                }

                appoint.Date = newDate;

                appoint.YearMonth = newDate.Month + "/" + newDate.Year;

                _dbContext.SaveChanges();
                _dbContext.Database.CommitTransaction();
            }
            catch (Exception ex)
            {
                _dbContext.Database.RollbackTransaction();
                throw ex;
            }
        }

        public void DeleteAppointment(string ownerUsername, int idToDelete)
        {
            _dbContext.Database.BeginTransaction();
            try
            {
                DeleteAllUserAsignedToAppointment(idToDelete);
                var appointToDelete = _dbContext.Appointments.FirstOrDefault(a => a.Id == idToDelete && a.Creator.UserName == ownerUsername);
                _dbContext.Appointments.Remove(appointToDelete);

                _dbContext.SaveChanges();
                _dbContext.Database.CommitTransaction();
            }
            catch (Exception ex)
            {
                _dbContext.Database.RollbackTransaction();
                throw ex;
            }
        }

        public void AssignUserToAppointments(List<int> participantsIds, int appointmentId)
        {
            try
            {
                List<UserSchedule> meetings = new List<UserSchedule>();
                foreach (int id in participantsIds)
                {
                    var participant = _dbContext.Users.FirstOrDefault(u => u.Id == id);
                    if (participant != null)
                    {
                        var newMeeting = new UserSchedule();
                        newMeeting.Participant = participant;
                        meetings.Add(newMeeting);
                    }
                }

                var meeting = _dbContext.Appointments.FirstOrDefault(a => a.Id == appointmentId);
                meeting.Meetings.AddRange(meetings);

                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void AssignGroupToAppointments(List<int> groupIds, int appointmentId)
        {
            try
            {
                List<GroupSchedule> meetings = new List<GroupSchedule>();
                foreach (int id in groupIds)
                {
                    var group = _dbContext.Groups.FirstOrDefault(u => u.Id == id);
                    if (group != null)
                    {
                        var newMeeting = new GroupSchedule();
                        newMeeting.Participant = group;
                        meetings.Add(newMeeting);
                    }
                }

                var meeting = _dbContext.Appointments.FirstOrDefault(a => a.Id == appointmentId);
                meeting.GroupsParticipants.AddRange(meetings);

                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void DeleteAllUserAsignedToAppointment(int appointmentId)
        {
            try
            {
                var meetingsToRemove = _dbContext.Meetings.Where(m => m.MeetingId == appointmentId).ToList();
                _dbContext.Meetings.RemoveRange(meetingsToRemove);
                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void DeleteAllGroupAsignedToAppointment(int appointmentId)
        {
            try
            {
                var meetingsToRemove = _dbContext.GroupsMeetings.Where(m => m.MeetingId == appointmentId).ToList();
                _dbContext.GroupsMeetings.RemoveRange(meetingsToRemove);
                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<User> GetAllAttendeesByAppointment(int appointmentId)
        {
            return _dbContext.Users.Where(u => u.Meetings.Any(m => m.MeetingId == appointmentId)).ToList();
        }

        public List<Group> GetAllGroupAttendessByAppointment(int appontmentId)
        {
            return _dbContext.Groups.Where(u => u.Meetings.Any(m => m.MeetingId == appontmentId)).ToList();
        }

        public List<RepeatEdit> GetRepeatEdits(int appointmentId)
        {
            return _dbContext.RepeatEdits.Where(r => r.AppointmentId == appointmentId).ToList();
        }

        public void Dispose()
        {
            //_conn.Close();
            //_conn.Dispose();
        }
    }
}