using System;
namespace Scheduler.Interfaces
{
	public interface IActivityLoggerFile
	{
        public void LogErrorToFiile(string message);
        public void LogActivityToFile(string message);
    }
}

