using System.Threading;
using Cysharp.Threading.Tasks;
using Rayleigh.Sklo.Screens;

namespace Rayleigh.Sklo.Async.UniTasks
{
    public abstract class AsyncScreen : BaseScreen<AsyncScreenLayer>, IAsyncScreen<UniTask>
    {
        public UniTask NotifyShown(CancellationToken cancellationToken)
        {
            Displayed = true;
            return OnShow(cancellationToken);
        }

        public UniTask NotifyHidden(CancellationToken cancellationToken)
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
        protected virtual UniTask OnShow(CancellationToken cancellationToken) => UniTask.CompletedTask;

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
        protected virtual UniTask OnHide(CancellationToken cancellationToken) => UniTask.CompletedTask;
        
        UniTask IAsyncScreen<UniTask>.NotifyHidden(CancellationToken cancellationToken) => NotifyHidden(cancellationToken);
        UniTask IAsyncScreen<UniTask>.NotifyShown(CancellationToken cancellationToken) => NotifyShown(cancellationToken);
    }
}