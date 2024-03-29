﻿using System;
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
            Color = "";
            Time = "";
            EndDateSpan = 0;
            EndTime = "";
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
        public string Color { get; set; }
        public string Details { get; set; }
        public string Time { get; set; }
        public int EndDateSpan { get; set; }
        public string EndTime { get; set; }

        public bool IsDeleted { get; set; }
        public bool IsDone { get; set; }
    }
}

