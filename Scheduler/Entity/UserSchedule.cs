using System;
namespace Scheduler.Entity
{
    public class UserSchedule
    {
        public int ParticipantId { get; set; }
        public User Participant { get; set; }

        public int MeetingId { get; set; }
        public Appointment Meeting { get; set; }
    }
}