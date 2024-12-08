using Microsoft.Extensions.Logging;

namespace Jabba.Complex.LongOperations.Utils
{
    public static partial class LogHelper
    {
        public static void LongOperationHandler_StageChanged(this ILogger logger, string name, string newValue)
        {
            logger.LogInformation("[{HandlerName}] job changed to {State}", name, newValue);
        }

        public static void LongOperationHandler_StateChanged(this ILogger logger, string name, LongOperationState newValue)
        {
            logger.LogInformation("[{HandlerName}] state changed to {State}", name, newValue);
        }

        public static void LongOperationHandler_StoppingSoft(this ILogger logger, string name)
        {
            logger.LogInformation("[{HandlerName}] handler ordered to soft stop", name);
        }
        
        public static void LongOperationHandler_DidntStopSoftly(this ILogger logger, string name, TimeSpan timeout)
        {
            logger.LogInformation("[{HandlerName}] handler didn't stop softly in {Timeout}", name, timeout);
        }

        public static void LongOperationHandler_StoppingImmediately(this ILogger logger, string name)
        {
            logger.LogInformation("[{HandlerName}] handler ordered to force stop", name);
        }

        public static void LogException(this ILogger logger, string handlerName, LongOperationState state, Exception ex, string operation)
        {
            logger.LogError(ex, "[{HandlerName}] failed during {Operation}. Current state: {State}", handlerName, operation, state);
        }
    }
}
