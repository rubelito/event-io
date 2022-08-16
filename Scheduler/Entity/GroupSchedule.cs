using System;
namespace Scheduler.Entity
{
    public class GroupSchedule
    {
        public int ParticipantId { get; set; }
        public Group Participant { get; set; }

        public int MeetingId { get; set; }
        public Appointment Meeting { get; set; }
    }
}

