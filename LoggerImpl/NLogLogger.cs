using System;
using System.Linq;
using Mmvm.Serializer;
using NLog;

namespace Mmvm.Logger.Impl
{
    public class NLogLogger : ILogger
    {
        #region Constructors

        public NLogLogger(Type type, ISerializer serializer)
        {
            Serializer = serializer;
            Logger = LogManager.GetLogger(type.FullName);
        }

        #endregion

        #region Private properties

        private global::NLog.Logger Logger { get; }

        private ISerializer Serializer { get; }

        #endregion

        #region ILogger impl

        public void Trace(string message, params object[] args)
        {
            Log(LogLevel.Trace, message, args);
        }

        public void Debug(string message, params object[] args)
        {
            Log(LogLevel.Debug, message, args);
        }

        public void Info(string message, params object[] args)
        {
            Log(LogLevel.Info, message, args);
        }

        public void Warn(string message, params object[] args)
        {
            Log(LogLevel.Warn, message, args);
        }

        public void Error(string message, params object[] args)
        {
            Log(LogLevel.Error, message, args);
        }

        public void Error(Exception ex)
        {
            Log(LogLevel.Error, null, null, ex);
        }

        public void Error(Exception ex, string message, params object[] args)
        {
            Log(LogLevel.Error, message, args, ex);
        }

        public void Fatal(string message, params object[] args)
        {
            Log(LogLevel.Fatal, message, args);
        }

        public void Fatal(Exception ex)
        {
            Log(LogLevel.Fatal, null, null, ex);
        }

        public void Fatal(Exception ex, string message, params object[] args)
        {
            Log(LogLevel.Fatal, message, args, ex);
        }

        #endregion

        #region Methods

        private void Log(LogLevel level, string message, object[] args, Exception ex = null)
        {
            if (!Logger.IsEnabled(level))
            {
                return;
            }

            Logger.Log(typeof(NLogLogger),
                ex == null
                    ? new LogEventInfo(level, Logger.Name, null, message, SerializeArgs(args))
                    : new LogEventInfo(level, Logger.Name, null, message, SerializeArgs(args), ex));
        }

        private object[] SerializeArgs(object[] args)
        {
            return Serializer == null
                ? args
                : args
                    .Select(arg => arg is string || arg is Enum ? arg : Serializer.Serialize(arg))
                    .ToArray();
        }

        #endregion
    }
}