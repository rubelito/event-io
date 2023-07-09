using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Scheduler.Entity
{
	public class ExceptionLog
	{
		public ExceptionLog()
		{
		}

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
		public string UserName { get; set; }
		public string Action { get; set; }
		public string Message { get; set; }
		public string Source { get; set; }
		public string StackTrace { get; set; }

		public string InnerMessage { get; set; }
		public string InnerSource { get; set; }
		public string InnerStackTrace { get; set; }

		public DateTime LogDateTime { get; set; }
    }
}

