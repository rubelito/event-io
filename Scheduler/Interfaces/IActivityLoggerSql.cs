
using Scheduler.Entity;

namespace Scheduler.Interfaces
{
	public interface IActivityLoggerSql
	{
		public void LogErrorToDb(ExceptionLog log);
		public void LogActivityToDb(string message);
	}
}