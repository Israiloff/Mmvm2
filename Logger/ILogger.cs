using System;
using JetBrains.Annotations;

namespace Israiloff.Cashbox.Component.Logger
{
    public interface ILogger
    {
        [StringFormatMethod("message")]
        void Trace(string message, params object[] args);

        [StringFormatMethod("message")]
        void Debug(string message, params object[] args);

        [StringFormatMethod("message")]
        void Info(string message, params object[] args);

        [StringFormatMethod("message")]
        void Warn(string message, params object[] args);

        [StringFormatMethod("message")]
        void Error(string message, params object[] args);

        void Error(Exception ex);

        [StringFormatMethod("message")]
        void Error(Exception ex, string message, params object[] args);

        [StringFormatMethod("message")]
        void Fatal(string message, params object[] args);

        void Fatal(Exception ex);

        [StringFormatMethod("message")]
        void Fatal(Exception ex, string message, params object[] args);
    }
}