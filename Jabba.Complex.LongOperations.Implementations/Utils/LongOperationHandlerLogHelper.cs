using Microsoft.Extensions.Logging;

namespace Jabba.Complex.LongOperations.Utils
{
    public static partial class LogHelper
    {
        public static void LongOperationHandler_StageEnd(this ILogger logger, string name, string stage, TimeSpan elapsed)
        {
            logger.LogInformation("[{HandlerName}] stopped stage {Stage}. Elapsed: {Elapsed}", name, stage, elapsed);
        }
        
        public static void LongOperationHandler_StageStart(this ILogger logger, string name, string stage)
        {
            logger.LogInformation("[{HandlerName}] started new stage {Stage}", name, stage);
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
