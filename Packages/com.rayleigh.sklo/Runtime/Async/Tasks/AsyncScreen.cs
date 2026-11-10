using System.Threading;
using System.Threading.Tasks;
using Rayleigh.Sklo.Screens;

namespace Rayleigh.Sklo.Async.Tasks
{
    public abstract class AsyncScreen : BaseScreen<AsyncScreenLayer>, IAsyncScreen<Task>
    {
        internal Task NotifyShown(CancellationToken cancellationToken)
        {
            Displayed = true;
            return OnShow(cancellationToken);
        }

        internal Task NotifyHidden(CancellationToken cancellationToken)
        {
            Displayed = false;
            return OnHide(cancellationToken);
        }
        
        /// <summary>
        /// Called when the screen is being shown. Override it to play an appearance animation
        /// or run other asynchronous show logic.
        /// </summary>
        /// <remarks>
        /// The screen manager awaits this together with the other screen and layer hooks of the same operation
        /// and completes the operation once all of them finish.
        /// </remarks>
        /// <param name="cancellationToken">
        /// Canceled when the caller wants the operation to finish immediately. It may already be canceled
        /// when this method is called. On cancellation, skip any remaining animations, put the screen into
        /// its final shown state and return normally. Throwing <see cref="System.OperationCanceledException"/>
        /// is allowed, but the screen manager completes the operation anyway and rethrows the exception
        /// to its caller, so leave the screen in that state before throwing.
        /// </param>
        protected virtual Task OnShow(CancellationToken cancellationToken) => Task.CompletedTask;

        /// <summary>
        /// Called when the screen is being hidden. Override it to play a disappearance animation
        /// or run other asynchronous hide logic.
        /// </summary>
        /// <remarks>
        /// The screen manager awaits this together with the other screen and layer hooks of the same operation
        /// and completes the operation once all of them finish.
        /// </remarks>
        /// <param name="cancellationToken">
        /// Canceled when the caller wants the operation to finish immediately. It may already be canceled
        /// when this method is called. On cancellation, skip any remaining animations, put the screen into
        /// its final hidden state and return normally. Throwing <see cref="System.OperationCanceledException"/>
        /// is allowed, but the screen manager completes the operation anyway and rethrows the exception
        /// to its caller, so leave the screen in that state before throwing.
        /// </param>
        protected virtual Task OnHide(CancellationToken cancellationToken) => Task.CompletedTask;
        
        Task IAsyncScreen<Task>.NotifyHidden(CancellationToken cancellationToken) => NotifyHidden(cancellationToken);
        Task IAsyncScreen<Task>.NotifyShown(CancellationToken cancellationToken) => NotifyShown(cancellationToken);
    }
}