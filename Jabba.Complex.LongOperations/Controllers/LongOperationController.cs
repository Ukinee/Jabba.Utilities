using Jabba.Complex.LongOperations.Handlers;
using Jabba.Complex.LongOperations.Utils;
using Microsoft.Extensions.Logging;

namespace Jabba.Complex.LongOperations.Controllers;

public class LongOperationController
{
    private static readonly IReadOnlySet<LongOperationState> InvalidStartStates = new HashSet<LongOperationState>()
    {
        LongOperationState.Disposed,
        LongOperationState.Starting,
        LongOperationState.Running,
        LongOperationState.StoppingImmediately,
        LongOperationState.StoppingSoftly,
    };

    private static readonly IReadOnlySet<LongOperationState> IgnoredStopStates = new HashSet<LongOperationState>()
    {
        LongOperationState.StoppingSoftly,
        LongOperationState.StoppingImmediately,
        LongOperationState.Stopped,
        LongOperationState.Failed,
        LongOperationState.WaitingForStart,
        LongOperationState.Completed,
    };

    private static readonly IReadOnlySet<LongOperationState> InvalidStopStates = new HashSet<LongOperationState>()
    {
        LongOperationState.Disposed,
    };

    private readonly Dictionary<string, LongOperationHandlerBase> _longOperationHandlers = new Dictionary<string, LongOperationHandlerBase>();
    private readonly ILogger<LongOperationController> _logger;

    public LongOperationController(ILogger<LongOperationController> logger)
    {
        _logger = logger;
    }

    public void Add(LongOperationHandlerBase handler)
    {
        _longOperationHandlers.Add(handler.Name, handler);
    }

    public void Remove(string name)
    {
        _longOperationHandlers.Remove(name);
    }

    public void Start(string name)
    {
        LongOperationHandlerBase handler = _longOperationHandlers[name];

        if (InvalidStartStates.Contains(handler.State) == false)
        {
            _logger.LongOperationController_InvalidStartState(name, handler.State);

            throw new InvalidOperationException($"{nameof(LongOperationHandlerBase)} with name {name} has invalid state for start. Current state {handler.State}");
        }

        handler.Start();
    }

    public async Task StopSoftly(string name, TimeSpan timeout)
    {
        LongOperationHandlerBase handler = _longOperationHandlers[name];

        if (InvalidStopStates.Contains(handler.State))
        {
            _logger.LongOperationController_InvalidStopState(name, handler.State);
        }

        if (IgnoredStopStates.Contains(handler.State))
        {
            _logger.LongOperationController_AlreadyStoppedState(name, handler.State);

            return;
        }

        await handler.StopSoftly(timeout);
    }

    public void StopImmediately(string name)
    {
        LongOperationHandlerBase handler = _longOperationHandlers[name];

        if (InvalidStopStates.Contains(handler.State))
        {
            _logger.LongOperationController_InvalidStopState(name, handler.State);
        }

        if (IgnoredStopStates.Contains(handler.State))
        {
            _logger.LongOperationController_AlreadyStoppedState(name, handler.State);
            _logger.LogInformation($"{nameof(LongOperationHandlerBase)} with name {{Name}} has ignored state for stop. Current state {{State}}", name, handler.State);

            return;
        }

        handler.StopImmediately();
    }
}
