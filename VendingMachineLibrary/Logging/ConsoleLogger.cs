using System;

namespace VendingMachineLibrary.Logging
{
    /// <summary>
    /// A logger implementation that outputs log lines to the standard output stream.
    /// </summary>
    public class ConsoleLogger : ILogger
    {
        public ConsoleLogger()
        {
        }

        public void Log(string message, LogLevel logLevel)
        {
            Console.WriteLine($"[{logLevel}] {message}");
        }

        public void Log(string message, LogLevel logLevel, params object[] messageParams)
        {
            Console.WriteLine($"[{logLevel}] {string.Format(message, messageParams)}");
        }
    }
}
