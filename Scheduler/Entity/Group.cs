using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Scheduler.Entity
{
    public class Group
    {
        public Group() {
            Meetings = new List<GroupSchedule>();
            Members = new List<UserGroup>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string GroupName { get; set; }
        public string Description { get; set; }
        public int OwnerId { get; set; }
        public User Owner { get; set; }
        public bool Active { get; set; }

        public ICollection<GroupSchedule> Meetings { get; set; }
        public ICollection<UserGroup> Members { get; set; }
    }
}

