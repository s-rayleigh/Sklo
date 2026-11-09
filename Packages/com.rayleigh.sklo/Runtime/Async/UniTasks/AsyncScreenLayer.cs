using Cysharp.Threading.Tasks;
using Rayleigh.Sklo.Layers;

namespace Rayleigh.Sklo.Async.UniTasks
{
    public abstract class AsyncScreenLayer : BaseScreenLayer, IAsyncLayer<UniTask>
    {
        public UniTask NotifyShown() => OnShow();
        UniTask IAsyncLayer<UniTask>.NotifyShown() => NotifyShown();
        protected virtual UniTask OnShow() => UniTask.CompletedTask;

        public UniTask NotifyTransition() => OnTransition();
        UniTask IAsyncLayer<UniTask>.NotifyTransition() => NotifyTransition();
        protected virtual UniTask OnTransition() => UniTask.CompletedTask;
        
        public UniTask NotifyHidden() => OnHide();
        UniTask IAsyncLayer<UniTask>.NotifyHidden() => NotifyHidden();
        protected virtual UniTask OnHide() => UniTask.CompletedTask;
    }
}