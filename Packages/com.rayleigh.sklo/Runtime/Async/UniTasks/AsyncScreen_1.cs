using Rayleigh.Sklo.Screens;

namespace Rayleigh.Sklo.Async.UniTasks
{
    /// <summary>
    /// An async screen that is shown with arguments of type <typeparamref name="TArgs"/>.
    /// </summary>
    /// <remarks>
    /// Show it with screen manager overloads that accept arguments. The screen keeps its arguments,
    /// so it is composed with the same arguments when it is shown again from the history.
    /// </remarks>
    /// <typeparam name="TArgs">Type of the arguments the screen expects.</typeparam>
    public abstract class AsyncScreen<TArgs> : AsyncScreen, IScreenWithArgs<TArgs>, IScreenArgsReceiver<TArgs>
    {
        private bool _hasArgs;

        /// <summary>
        /// Arguments the screen was shown with.
        /// </summary>
        protected TArgs Args { get; private set; }

        bool IScreenArgsReceiver.HasArgs => _hasArgs;

        void IScreenArgsReceiver<TArgs>.SetArgs(TArgs args)
        {
            Args = args;
            _hasArgs = true;
        }

        protected sealed override void OnCompose(ScreenCompositor<AsyncScreenLayer> compositor) =>
            OnCompose(compositor, Args);

        /// <summary>
        /// Adds layers to the screen.
        /// </summary>
        /// <param name="compositor">Compositor used to add layers.</param>
        /// <param name="args">Arguments the screen was shown with.</param>
        protected abstract void OnCompose(ScreenCompositor<AsyncScreenLayer> compositor, TArgs args);
    }
}
