using System;
using Scheduler.Entity;

namespace Scheduler.Models
{
	public class EventModel
	{
		public int Id { get; set; }
		public string Location { get; set; }
		public string Title { get; set; }
		public string Color { get; set; }
		public string Details { get; set; }

		public string ActualYearMonth { get; set; }
		public string YearMonth { get; set; }

		public string OriginalDate { get; set; }
		public string Date { get; set; }

		public string Time { get; set; }
		public bool IsRepeat { get; set; }
        public int CreatorId { get; set; }
		public string CreatedBy { get; set; }
		public bool IsOwner { get; set; }

		public bool IsDeleted { get; set; }
		public AppointmentType Type { get; set; }
		public bool IsDone { get; set; }

		public bool HasEdit { get; set; }

		public RepeatSelectionEnum RepeatSelection { get; set; }
		public RepeatEndEnum RepeatEnd { get; set; }

		public int After { get; set; }
		public string OnDate { get; set; }
		public bool IsClone { get; set; }
		public int NumberOfRepeats { get; set; }
    }
}