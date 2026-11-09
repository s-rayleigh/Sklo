using Cysharp.Threading.Tasks;
using Rayleigh.Sklo.Layers;

namespace Rayleigh.Sklo.Async.UniTasks
{
    public abstract class AsyncScreenLayer : BaseScreenLayer, IAsyncLayer<UniTask>
    {
        public UniTask NotifyShown() => OnShow();

        protected virtual UniTask OnShow() => UniTask.CompletedTask;

        public UniTask NotifyTransition() => OnTransition();

        protected virtual UniTask OnTransition() => UniTask.CompletedTask;
        
        public UniTask NotifyHidden() => OnHide();
        
        protected virtual UniTask OnHide() => UniTask.CompletedTask;
    }
}