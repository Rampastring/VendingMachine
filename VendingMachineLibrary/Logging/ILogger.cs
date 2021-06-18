namespace VendingMachineLibrary.Logging
{
    /// <summary>
    /// Interface for a logger.
    /// </summary>
    public interface ILogger
    {
        /// <summary>
        /// Logs a message.
        /// </summary>
        /// <param name="message">The message to log.</param>
        /// <param name="logLevel">The logging level of the message.</param>
        void Log(string message, LogLevel logLevel);

        /// <summary>
        /// Formats and logs a message.
        /// </summary>
        /// <param name="message">The message to log.</param>
        /// <param name="logLevel">The logging level of the message.</param>
        /// <param name="messageParams">The parameters for formatting the message.</param>
        void Log(string message, LogLevel logLevel, params object[] messageParams);
    }
}
