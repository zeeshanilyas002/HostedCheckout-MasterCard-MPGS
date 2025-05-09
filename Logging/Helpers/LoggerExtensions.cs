using Microsoft.Extensions.Logging;

namespace GlobalPay.Logging.Helpers
{
    public static class LoggerExtensions
    {
        public static void LogOperation(this ILogger logger, string operation, string method, string? message = null, Exception? exception = null)
        {
            var logMessage = $"Operation: {operation} | Method: {method} | Message: {message}";

            if (exception != null)
            {
                logger.LogError(exception, logMessage, operation, method, message);
            }
            else
            {
                logger.LogInformation(logMessage, operation, method, message);
            }
        }
    }
}
