using System.Threading;
using System.Threading.Tasks;
using Rayleigh.Sklo.Layers;

namespace Rayleigh.Sklo.Async.Tasks
{
    public abstract class AsyncScreenLayer : BaseScreenLayer, IAsyncLayer<Task>
    {
        internal Task NotifyShown(CancellationToken cancellationToken) => OnShow(cancellationToken);
        Task IAsyncLayer<Task>.NotifyShown(CancellationToken cancellationToken) => NotifyShown(cancellationToken);
        protected virtual Task OnShow(CancellationToken cancellationToken) => Task.CompletedTask;
        
        internal Task NotifyTransition(CancellationToken cancellationToken) => OnTransition(cancellationToken);

        Task IAsyncLayer<Task>.NotifyTransition(CancellationToken cancellationToken) =>
            NotifyTransition(cancellationToken);
        protected virtual Task OnTransition(CancellationToken cancellationToken) => Task.CompletedTask;
        
        internal Task NotifyHidden(CancellationToken cancellationToken) => OnHide(cancellationToken);
        Task IAsyncLayer<Task>.NotifyHidden(CancellationToken cancellationToken) => NotifyHidden(cancellationToken);
        protected virtual Task OnHide(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}