using Scheduler.Entity;
using Scheduler.Models;

namespace Scheduler.Interfaces
{
    public interface IAppointmentRepository
    {
        Appointment AddAppointment(string ownerUsername, Appointment appointment);
        void AddAppointmentWithAttendees(string ownerUsername, Appointment appointment, List<int> memberIds, List<int> groupIds);
        void AssignGroupToAppointments(List<int> groupIds, int appointmentId);
        void AssignUserToAppointments(List<int> participantsIds, int appointmentId);
        void ChangeScheduleDate(int appointmentId, DateTime newDate);
        void DeleteAllGroupAsignedToAppointment(int appointmentId);
        void DeleteAllUserAsignedToAppointment(int appointmentId);
        void DeleteAppointment(string ownerUsername, int idToDelete);
        void DeleteAllRepeats(int appointmentId);
        void DeleteRepeat(int appointmentId);
        void Dispose();
        void EditAllRepeatAppointment(EventModel ev);
        void EditAppointment(Appointment appointmentToEdit);
        void EditAppointmentWithAttendees(Appointment appointment, List<int> memberIds, List<int> groupIds);
        void EditAppointmentRepeatSettings(EventModel model);
        void EditRepeatWithAttendees(EventModel model, List<int> memberIds, List<int> groupIds);
        List<User> GetAllAttendeesByAppointment(int appointmentId);
        List<Group> GetAllGroupAttendessByAppointment(int appontmentId);
        Appointment GetAppointmentById(int id);
        List<Appointment> GetAppointments(string ownerUsername, string yearMonth);
        List<Appointment> GetAppointmentsWithRepeats(string ownerUsername);
        List<Appointment> GetMeetings(int participantId, string yearMonth);
        List<Appointment> GetMeetingsThatYoureInvited(int participantId, string yearMonth);
        List<Appointment> GetMeetingsThatYoureInvitedWithRepeats(int participantId);
        List<Appointment> GetMeetingsWithRepeat(int participantId);
        List<RepeatEdit> GetRepeatEdits(int appointmentId);
        void MarkRepeatAsDeleted(int appointmentId, DateTime originalDate);
    }
}