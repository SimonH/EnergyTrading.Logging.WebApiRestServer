using EnergyTrading.Contracts.Logging;

namespace EnergyTrading.Logging.WebApiRestServer.Logic
{
    public class LogMessageHandler
    {
        public void AddLogMessage(LogMessage message)
        {
            var hasException = message.Exception != null;
            var hasParams = message.Params != null && message.Params.Length > 0;
            if (hasException)
            {
                LogWithException(message);
            }
            else if (hasParams)
            {
                LogWithFormat(message);
            }
            else
            {
                LogMessage(message);
            }
        }

        private ILogger GetLogger(LogMessage logMessage)
        {
            return LoggerProvider.GetLogger(logMessage.CreatingType);
        }

        private void LogWithException(LogMessage logMessage)
        {
            switch (logMessage.Level.ToLowerInvariant())
            {
                case "debug":
                    GetLogger(logMessage).Debug(logMessage.Message, logMessage.Exception);
                    break;
                case "error":
                    GetLogger(logMessage).Error(logMessage.Message, logMessage.Exception);
                    break;
                case "fatal":
                    GetLogger(logMessage).Fatal(logMessage.Message, logMessage.Exception);
                    break;
                case "info":
                    GetLogger(logMessage).Info(logMessage.Message, logMessage.Exception);
                    break;
                case "warn":
                    GetLogger(logMessage).Warn(logMessage.Message, logMessage.Exception);
                    break;
            }
        }

        private void LogWithFormat(LogMessage logMessage)
        {
            switch (logMessage.Level.ToLowerInvariant())
            {
                case "debug":
                    GetLogger(logMessage).DebugFormat(logMessage.Message, logMessage.Params);
                    break;
                case "error":
                    GetLogger(logMessage).ErrorFormat(logMessage.Message, logMessage.Params);
                    break;
                case "fatal":
                    GetLogger(logMessage).FatalFormat(logMessage.Message, logMessage.Params);
                    break;
                case "info":
                    GetLogger(logMessage).InfoFormat(logMessage.Message, logMessage.Params);
                    break;
                case "warn":
                    GetLogger(logMessage).WarnFormat(logMessage.Message, logMessage.Params);
                    break;
            }
        }

        private void LogMessage(LogMessage logMessage)
        {
            switch (logMessage.Level.ToLowerInvariant())
            {
                case "debug":
                    GetLogger(logMessage).Debug(logMessage.Message);
                    break;
                case "error":
                    GetLogger(logMessage).Error(logMessage.Message);
                    break;
                case "fatal":
                    GetLogger(logMessage).Fatal(logMessage.Message);
                    break;
                case "info":
                    GetLogger(logMessage).Info(logMessage.Message);
                    break;
                case "warn":
                    GetLogger(logMessage).Warn(logMessage.Message);
                    break;
            }
        }
    }
}