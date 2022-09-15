﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Scheduler.Entity;

namespace Scheduler.Entity
{
	public class Appointment
	{
		public Appointment()
		{
			Location = "";
			Title = "";
			Details = "";
			YearMonth = "";
			Time = "";
			After = 0;

			Meetings = new List<UserSchedule>();
			GroupsParticipants = new List<GroupSchedule>();
		}

		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }
		public string Location { get; set; }
		public string Title { get; set; }
		public string Details { get; set; }
		public string YearMonth { get; set; }
		public DateTime Date { get; set; }
		public string Time { get; set; }
		public int CreatorId { get; set; }
		public User Creator { get; set; }

		public List<UserSchedule> Meetings { get; set; }
		public List<GroupSchedule> GroupsParticipants { get; set; }

		public bool isRepeat { get; set; }
		public RepeatSelectionEnum RepeatSelection { get; set; }

		public RepeatEndEnum RepeatEnd { get; set; }
		public int After { get; set; }
		public DateTime OnDate { get; set; }

        [NotMapped]
		public RepeatEdit RepeatEdit { get; set; }

		public bool IsDone { get; set; }
        [NotMapped]
		public bool IsClone { get; set; }
        [NotMapped]
		public bool IsDeleted { get; set; }
        [NotMapped]
		public int NumberOfRepeats { get; set; }
	}
}