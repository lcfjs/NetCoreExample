using NLog;
using System;
using System.Collections.Generic;
using System.Text;

namespace Example.Core.Log
{
    public class LoggerTools
    {
        #region CONST
        public static readonly string RequestLog = "RequestLog";
        #endregion


        public static LoggerTools GetInstance(string loggerName)
        {
            return new LoggerTools(loggerName);
        }

        private readonly Logger logger;

        private LoggerTools(Logger logger)
        {
            this.logger = logger;
        }

        private LoggerTools(string loggerName)
        {
            this.logger = LogManager.GetLogger(loggerName);
        }

        #region 通用

        public void Info(string message)
        {
            Log(NLog.LogLevel.Info, message, null);
        }
        public void Warn(string message)
        {
            Log(NLog.LogLevel.Warn, message, null);
        }
        public void Error(string message, Exception exception, params object[] parameters)
        {
            Log(NLog.LogLevel.Error, message, exception, parameters);
        }
        public void Fatal(string message, Exception exception, params object[] parameters)
        {
            Log(NLog.LogLevel.Fatal, message, exception, parameters);
        }

        private void Log(NLog.LogLevel logLevel, string message, Exception exception, params object[] parameters)
        {
            LogEventInfo logEventInfo = new LogEventInfo(logLevel, logger.Name, message)
            {
                Exception = exception
            };
            if (parameters?.Length > 0)
            {
                logEventInfo.Properties["url"] = parameters[0];
            }
            logger.Log(logEventInfo);
        }

        #endregion

        #region Request

        public void RequestInfo(string message, params object[] parameters)
        {
            LogEventInfo logEventInfo = new LogEventInfo(NLog.LogLevel.Info, logger.Name, message);
            if (parameters?.Length > 0)
            {
                logEventInfo.Properties["url"] = parameters[0];
                logEventInfo.Properties["request"] = parameters[1];
                logEventInfo.Properties["response"] = parameters[2];
            }
            logger.Log(logEventInfo);
        }

        #endregion
    }
}
