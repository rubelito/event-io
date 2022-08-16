using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using Scheduler.Entity;
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
            return _dbContext.Appointments.Include("Creator").Where(a => a.Creator.UserName == ownerUsername && a.YearMonth == yearMonth).ToList();
        }

        public List<Appointment> GetMeetings(int participantId, string yearMonth)
        {
            return _dbContext.Appointments.Include("Creator")
                .Where(a => a.YearMonth == yearMonth && a.Meetings.Any(m => m.ParticipantId == participantId))
                .ToList();
        }

        public List<Appointment> GetMeetingsThatYoureInvited(int participantId, string yearMonth)
        {
            var query = (from a in _dbContext.Appointments
                         join gm in _dbContext.GroupsMeetings on a.Id equals gm.MeetingId
                         join g in _dbContext.Groups on gm.ParticipantId equals g.Id
                         join ug in _dbContext.UsersGroups on g.Id equals ug.GroupId
                         join u in _dbContext.Users on a.CreatorId equals u.Id
                         where (ug.UserId == participantId && a.YearMonth == yearMonth)
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

                appoint.Title = appointmentToEdit.Title;
                appoint.Details = appointmentToEdit.Details;

                DateTime monthYear = Convert.ToDateTime(appointmentToEdit.Date);
                    
                appoint.YearMonth = monthYear.Month + "/" + monthYear.Year;
                appoint.Date = appointmentToEdit.Date;
                appoint.Time = appointmentToEdit.Time;

                _dbContext.SaveChanges();
                _dbContext.Database.CommitTransaction();
            }
            catch (Exception ex)
            {
                _dbContext.Database.RollbackTransaction();
                throw ex;
            }
        }

        public void ChangeScheduleDate(int appointmentId, string strDate)
        {
            _dbContext.Database.BeginTransaction();
            try
            {
                var appoint = _dbContext.Appointments.FirstOrDefault(a => a.Id == appointmentId);
                if (appoint == null)
                {
                    throw new ArgumentException("Cannot find existing appointment Id=" + appointmentId);
                }

                appoint.Date = strDate;

                DateTime monthYear = Convert.ToDateTime(strDate);

                appoint.YearMonth = monthYear.Month + "/" + monthYear.Year;

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

        public void Dispose()
        {
            //_conn.Close();
            //_conn.Dispose();
        }
    }
}