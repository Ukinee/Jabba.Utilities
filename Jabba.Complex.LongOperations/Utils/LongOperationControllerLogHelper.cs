using Microsoft.Extensions.Logging;

namespace Jabba.Complex.LongOperations.Utils
{
    public static partial class LogHelper
    {
        public static void LongOperationController_InvalidStartState(this ILogger logger, string name, LongOperationState state)
        {
            logger.LogError
            (
                "LongOperationHandlerBase with name {Name} has invalid state for start. Current state {State}",
                name,
                state
            );
        }

        public static void LongOperationController_InvalidStopState(this ILogger logger, string name, LongOperationState state)
        {
            logger.LogWarning
            (
                "LongOperationHandlerBase with name {Name} has invalid state for stop. Current state {State}",
                name,
                state
            );
        }

        public static void LongOperationController_AlreadyStoppedState(this ILogger logger, string name, LongOperationState state)
        {
            logger.LogError
            (
                "LongOperationHandlerBase with name {Name} has stopped. Current state {State}",
                name,
                state
            );
        }
    }
}
