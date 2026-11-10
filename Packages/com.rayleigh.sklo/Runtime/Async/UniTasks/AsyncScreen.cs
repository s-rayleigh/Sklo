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
        
        protected virtual UniTask OnShow(CancellationToken cancellationToken) => UniTask.CompletedTask;
        protected virtual UniTask OnHide(CancellationToken cancellationToken) => UniTask.CompletedTask;
        
        UniTask IAsyncScreen<UniTask>.NotifyHidden(CancellationToken cancellationToken) => NotifyHidden(cancellationToken);
        UniTask IAsyncScreen<UniTask>.NotifyShown(CancellationToken cancellationToken) => NotifyShown(cancellationToken);
    }
}