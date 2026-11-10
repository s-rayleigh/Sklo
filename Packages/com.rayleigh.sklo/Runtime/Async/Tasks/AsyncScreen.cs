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
        
        protected virtual Task OnShow(CancellationToken cancellationToken) => Task.CompletedTask;
        protected virtual Task OnHide(CancellationToken cancellationToken) => Task.CompletedTask;
        
        Task IAsyncScreen<Task>.NotifyHidden(CancellationToken cancellationToken) => NotifyHidden(cancellationToken);
        Task IAsyncScreen<Task>.NotifyShown(CancellationToken cancellationToken) => NotifyShown(cancellationToken);
    }
}