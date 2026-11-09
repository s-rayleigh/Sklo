using Cysharp.Threading.Tasks;
using Rayleigh.Sklo.Screens;

namespace Rayleigh.Sklo.Async.UniTasks
{
    public abstract class AsyncScreen : BaseScreen<AsyncScreenLayer>, IAsyncScreen<UniTask>
    {
        public UniTask NotifyShown()
        {
            Displayed = true;
            return OnShow();
        }

        public UniTask NotifyHidden()
        {
            Displayed = false;
            return OnHide();
        }
        
        protected virtual UniTask OnShow() => UniTask.CompletedTask;
        protected virtual UniTask OnHide() => UniTask.CompletedTask;
    }
}