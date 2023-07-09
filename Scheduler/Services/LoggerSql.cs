
using Scheduler.Entity;
using Scheduler.Interfaces;
namespace Scheduler.Services
{
	public class LoggerSql : IActivityLoggerSql
    {
        private SchedulerDbContext _dbContext;

        public LoggerSql() 
		{
            _dbContext = new SchedulerDbContext();
        }

        public void LogActivityToDb(string message)
        {
            throw new NotImplementedException();
        }

        public void LogErrorToDb(ExceptionLog log)
        {
            _dbContext.ExceptionLogs.Add(log);
            _dbContext.SaveChanges();
        }
    }
}

