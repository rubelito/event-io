using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
			Date = "";
			Time = "";

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
		public string Date { get; set; }
		public string Time { get; set; }
		public int CreatorId { get; set; }
		public User Creator { get; set; }

		public List<UserSchedule> Meetings { get; set; }
		public List<GroupSchedule> GroupsParticipants { get; set; }
	}
}