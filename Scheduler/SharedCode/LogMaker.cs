using System;
using Scheduler.Entity;

namespace Scheduler.SharedCode
{
	public static class LogMaker
	{
		public static ExceptionLog MakeLog(string userName, string action, Exception exception)
		{
            ExceptionLog log = CreateEmptyExceptionLog();
			log.Action = action;
            log.UserName = userName;
			log.Message = exception.Message;
			log.Source = exception.Source != null ? exception.Source : "N/A";
			log.StackTrace = exception.StackTrace != null ? exception.StackTrace : "N/A";

            if (exception.InnerException != null) {
                log.InnerMessage = exception.Message;
                log.InnerSource = exception.InnerException.Source != null ? exception.InnerException.Source : "N/A";
                log.InnerStackTrace = exception.InnerException.StackTrace != null ? exception.InnerException.StackTrace : "N/A";
            }

			log.LogDateTime = DateTime.Now;

            return log;
		}

		private static ExceptionLog CreateEmptyExceptionLog()
		{
            ExceptionLog log = new ExceptionLog();
            log.Action = "N/A";
            log.UserName = "N/A";
            log.Message = "N/A";
            log.Source = "N/A";
            log.StackTrace = "N/A";
            log.LogDateTime = DateTime.Now;

            log.InnerMessage = "N/A";
            log.InnerSource = "N/A";
            log.InnerStackTrace = "N/A";
            
            return log;
        }
    }
}

