using System;
using System.Collections.Generic;
using System.Text;
using VendingMachineLibrary.Logging;

namespace VendingMachineConsole
{
    /// <summary>
    /// A logger implementation that outputs log lines to the standard output stream, with timestamps!
    /// </summary>
    class AmazingCustomLogger : ILogger
    {
        public void Log(string message, LogLevel logLevel)
        {
            Console.WriteLine($"{DateTime.Now} [{logLevel}] WITH CUSTOM LOGGING! {message}");
        }

        public void Log(string message, LogLevel logLevel, params object[] messageParams)
        {
            Console.WriteLine($"{DateTime.Now} [{logLevel}] WITH CUSTOM LOGGING! {string.Format(message, messageParams)}");
        }
    }
}
