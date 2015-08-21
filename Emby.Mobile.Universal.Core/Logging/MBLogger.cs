using System;
using System.Text;
using Emby.Mobile.Core.Interfaces;
using MediaBrowser.Model.Logging;

namespace Emby.Mobile.Universal.Core.Logging
{
    public class MBLogger : ILogger
    {
        private readonly ILog _logger;

        public MBLogger(ILog logger)
        {
            _logger = logger;
        }

        public void Info(string message, params object[] paramList)
        {
            _logger.Info(message, paramList);
        }

        public void Error(string message, params object[] paramList)
        {
            _logger.Error(message, paramList);
        }

        public void Warn(string message, params object[] paramList)
        {
            _logger.Warning(message, paramList);
        }

        public void Debug(string message, params object[] paramList)
        {
            _logger.Debug(message, paramList);
        }

        public void Fatal(string message, params object[] paramList)
        {
            _logger.Fatal(message, paramList);
        }

        public void FatalException(string message, Exception exception, params object[] paramList)
        {
            _logger.Fatal(message, paramList);
            _logger.FatalException(message, exception);
        }

        public void Log(LogSeverity severity, string message, params object[] paramList)
        {
            _logger.Info(message, paramList);
        }

        public void ErrorException(string message, Exception exception, params object[] paramList)
        {
            _logger.Error(message, paramList);
            _logger.ErrorException(message, exception);
        }

        public void LogMultiline(string message, LogSeverity severity, StringBuilder additionalContent)
        {
            
        }
    }
}
