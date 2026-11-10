using System.Threading;
using Cysharp.Threading.Tasks;
using Rayleigh.Sklo.Layers;

namespace Rayleigh.Sklo.Async.UniTasks
{
    public abstract class AsyncScreenLayer : BaseScreenLayer, IAsyncLayer<UniTask>
    {
        internal UniTask NotifyShown(CancellationToken cancellationToken) => OnShow(cancellationToken);
        UniTask IAsyncLayer<UniTask>.NotifyShown(CancellationToken cancellationToken) => NotifyShown(cancellationToken);

        /// <summary>
        /// Called when the layer is being shown. Override it to play an appearance animation
        /// or run other asynchronous show logic.
        /// </summary>
        /// <remarks>
        /// The screen manager awaits this together with the other screen and layer hooks of the same operation
        /// and completes the operation once all of them finish.
        /// </remarks>
        /// <param name="cancellationToken">
        /// Canceled when the caller wants the operation to finish immediately. It may already be canceled
        /// when this method is called. On cancellation, skip any remaining animations, put the layer into
        /// its final shown state and return normally. Throwing <see cref="System.OperationCanceledException"/>
        /// is allowed, but the screen manager completes the operation anyway and rethrows the exception
        /// to its caller, so leave the layer in that state before throwing.
        /// </param>
        protected virtual UniTask OnShow(CancellationToken cancellationToken) => UniTask.CompletedTask;

        internal UniTask NotifyTransition(CancellationToken cancellationToken) => OnTransition(cancellationToken);
        UniTask IAsyncLayer<UniTask>.NotifyTransition(CancellationToken cancellationToken) =>
            NotifyTransition(cancellationToken);

        /// <summary>
        /// Called when the layer stays displayed while the main screen switches to another screen
        /// that also uses this layer. Override it to animate the layer between the two screens.
        /// </summary>
        /// <remarks>
        /// The screen manager awaits this together with the other screen and layer hooks of the same operation
        /// and completes the operation once all of them finish.
        /// </remarks>
        /// <param name="cancellationToken">
        /// Canceled when the caller wants the operation to finish immediately. It may already be canceled
        /// when this method is called. On cancellation, skip any remaining animations, put the layer into
        /// the state the new screen expects and return normally. Throwing <see cref="System.OperationCanceledException"/>
        /// is allowed, but the screen manager completes the operation anyway and rethrows the exception
        /// to its caller, so leave the layer in that state before throwing.
        /// </param>
        protected virtual UniTask OnTransition(CancellationToken cancellationToken) => UniTask.CompletedTask;
        
        internal UniTask NotifyHidden(CancellationToken cancellationToken) => OnHide(cancellationToken);
        UniTask IAsyncLayer<UniTask>.NotifyHidden(CancellationToken cancellationToken) =>
            NotifyHidden(cancellationToken);

        /// <summary>
        /// Called when the layer is being hidden. Override it to play a disappearance animation
        /// or run other asynchronous hide logic.
        /// </summary>
        /// <remarks>
        /// The screen manager awaits this together with the other screen and layer hooks of the same operation
        /// and completes the operation once all of them finish.
        /// </remarks>
        /// <param name="cancellationToken">
        /// Canceled when the caller wants the operation to finish immediately. It may already be canceled
        /// when this method is called. On cancellation, skip any remaining animations, put the layer into
        /// its final hidden state and return normally. Throwing <see cref="System.OperationCanceledException"/>
        /// is allowed, but the screen manager completes the operation anyway and rethrows the exception
        /// to its caller, so leave the layer in that state before throwing.
        /// </param>
        protected virtual UniTask OnHide(CancellationToken cancellationToken) => UniTask.CompletedTask;
    }
}