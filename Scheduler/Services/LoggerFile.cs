using Scheduler.Interfaces;
using Scheduler.SharedCode;

namespace Scheduler.Services
{
    public class LoggerFile : IActivityLoggerFile
	{
        string logFilePath;

		public LoggerFile()
		{
            this.logFilePath = StaticConfig.LogFilePath;
        }

        public void LogActivityToFile(string message)
        {
            throw new NotImplementedException();
        }

        public void LogErrorToFiile(string message)
        {
            throw new NotImplementedException();

        }
    }
}

