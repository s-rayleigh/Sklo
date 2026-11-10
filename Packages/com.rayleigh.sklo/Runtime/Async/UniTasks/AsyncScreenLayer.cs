using System.Threading;
using Cysharp.Threading.Tasks;
using Rayleigh.Sklo.Layers;

namespace Rayleigh.Sklo.Async.UniTasks
{
    public abstract class AsyncScreenLayer : BaseScreenLayer, IAsyncLayer<UniTask>
    {
        public UniTask NotifyShown(CancellationToken cancellationToken) => OnShow(cancellationToken);
        UniTask IAsyncLayer<UniTask>.NotifyShown(CancellationToken cancellationToken) => NotifyShown(cancellationToken);
        protected virtual UniTask OnShow(CancellationToken cancellationToken) => UniTask.CompletedTask;

        public UniTask NotifyTransition(CancellationToken cancellationToken) => OnTransition(cancellationToken);

        UniTask IAsyncLayer<UniTask>.NotifyTransition(CancellationToken cancellationToken) =>
            NotifyTransition(cancellationToken);
        protected virtual UniTask OnTransition(CancellationToken cancellationToken) => UniTask.CompletedTask;
        
        public UniTask NotifyHidden(CancellationToken cancellationToken) => OnHide(cancellationToken);

        UniTask IAsyncLayer<UniTask>.NotifyHidden(CancellationToken cancellationToken) =>
            NotifyHidden(cancellationToken);
        protected virtual UniTask OnHide(CancellationToken cancellationToken) => UniTask.CompletedTask;
    }
}