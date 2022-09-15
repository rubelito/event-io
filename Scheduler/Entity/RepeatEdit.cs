using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Scheduler.Entity
{
    public class RepeatEdit
    {
        public RepeatEdit()
        {
            Title = "";
            Location = "";
            Details = "";
            Time = "";
            IsDeleted = false;
            IsDone = false;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int AppointmentId { get; set; }
        public DateTime OriginalDate { get; set; }
        public DateTime EditedDate { get; set; }

        public string Title { get; set; }
        public string Location { get; set; }
        public string Details { get; set; }
        public string Time { get; set; }

        public bool IsDeleted { get; set; }
        public bool IsDone { get; set; }
    }
}

