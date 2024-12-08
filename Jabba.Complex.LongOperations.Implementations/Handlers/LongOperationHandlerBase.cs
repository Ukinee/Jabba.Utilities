using System.ComponentModel;
using Jabba.Complex.LongOperations.Abstractions;
using Jabba.Complex.LongOperations.Utils;
using Microsoft.Extensions.Logging;
using Utilities.Frameworks.ComponentModel;

namespace Jabba.Complex.LongOperations.Handlers
{
    public abstract class LongOperationHandlerBase : NotifyPropertyChanged, ITaskHandler, IDisposable
    {
        private readonly ILogger<LongOperationHandlerBase> _logger;

        private readonly CancellationTokenSource _immediateCancellationTokenSource = new CancellationTokenSource();
        private readonly CancellationTokenSource _softCancellationTokenSource = new CancellationTokenSource();

        private LongOperationState _state = LongOperationState.WaitingForStart;
        private Task? _longOperationTask;

        private object _lock = new object();

        private string _stage = string.Empty;
        private bool _isDisposed;

        protected LongOperationHandlerBase(ILogger<LongOperationHandlerBase> logger, string name)
        {
            _logger = logger;
            Name = name;
        }

        ~LongOperationHandlerBase()
        {
            Dispose(false);
        }

        public string Name { get; }

        public LongOperationState State
        {
            get => _state;
            private set
            {
                ObjectDisposedException.ThrowIf(_isDisposed, this);

                if (SetField(ref _state, value))
                {
                    _logger.LongOperationHandler_StateChanged(Name, value);
                }
            }
        }

        public string Stage
        {
            get => _stage;
            set
            {
                ObjectDisposedException.ThrowIf(_isDisposed, this);

                if (string.IsNullOrEmpty(value))
                {
                    value = string.Empty;
                }

                if (SetField(ref _stage, value))
                {
                    _logger.LongOperationHandler_StageChanged(Name, value);
                }
            }
        }

        public void Start()
        {
            lock (_lock)
            {
                ObjectDisposedException.ThrowIf(_isDisposed, this);

                if (_longOperationTask is { IsCompleted: false })
                {
                    throw new InvalidOperationException("The task is already running.");
                }

                State = LongOperationState.Starting;

                _longOperationTask = Task.Run
                (
                    async () =>
                    {
                        try
                        {
                            State = LongOperationState.Running;
                            await Run(_immediateCancellationTokenSource.Token, _softCancellationTokenSource.Token);
                            State = LongOperationState.Completed;
                        }
                        catch (Exception exception)
                        {
                            _logger.LogException(Name, State, exception, nameof(Run));
                            State = LongOperationState.Failed;
                        }
                    }
                );
            }
        }

        public async Task StopSoftly(TimeSpan immediateCancelAfter)
        {
            ObjectDisposedException.ThrowIf(_isDisposed, this);

            State = LongOperationState.StoppingSoftly;

            _logger.LongOperationHandler_StoppingSoft(Name);

            try
            {
                await _softCancellationTokenSource.CancelAsync();

                if (_longOperationTask is { IsCompleted: false })
                {
                    await Task.WhenAny(_longOperationTask, Task.Delay(immediateCancelAfter));

                    if (StopIfNecessary(immediateCancelAfter) == false)
                    {
                        State = LongOperationState.Stopped;
                    }
                }
            }
            catch (Exception exception)
            {
                _logger.LogException(Name, State, exception, nameof(StopSoftly));

                throw;
            }
        }

        public void StopImmediately()
        {
            lock (_lock)
            {
                ObjectDisposedException.ThrowIf(_isDisposed, this);

                State = LongOperationState.StoppingImmediately;

                _logger.LongOperationHandler_StoppingImmediately(Name);

                try
                {
                    _immediateCancellationTokenSource.Cancel();

                    if (_longOperationTask is { IsCompleted: false })
                    {
                        throw new InvalidAsynchronousStateException($"{Name} operation must be completed after {nameof(_immediateCancellationTokenSource)}.Cancel(); call");
                    }
                }
                catch (Exception exception)
                {
                    _logger.LogException(Name, State, exception, nameof(StopImmediately));

                    throw;
                }

                State = LongOperationState.Stopped;
            }
        }

        protected abstract Task Run(CancellationToken softToken, CancellationToken forceToken);

        private bool StopIfNecessary(TimeSpan immediateCancelAfter)
        {
            if (_longOperationTask.IsCompleted)
            {
                return false;
            }

            _logger.LongOperationHandler_DidntStopSoftly(Name, immediateCancelAfter);
            StopImmediately();

            return true;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing == false)
                return;

            StopImmediately();

            State = LongOperationState.Disposed;
            _isDisposed = true;

            _immediateCancellationTokenSource.Dispose();
            _softCancellationTokenSource.Dispose();
        }

        public void Dispose()
        {
            if (_isDisposed)
                return;

            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
