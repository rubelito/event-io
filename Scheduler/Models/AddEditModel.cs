using System;
using Scheduler.Entity;

namespace Scheduler.Models
{
    public class AddEditModel
    {
        public EventModel Appointment { get; set; }
        public List<int> MemberIds { get; set; }
        public List<int> GroupIds { get; set; }
    }
}