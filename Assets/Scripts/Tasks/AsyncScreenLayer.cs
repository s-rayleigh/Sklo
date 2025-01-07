using System.Threading.Tasks;

namespace Sklo.Tasks
{
    public abstract class AsyncScreenLayer : BaseScreenLayer
    {
        internal Task NotifyShown() => OnShow();

        protected virtual Task OnShow() => Task.CompletedTask;

        internal Task NotifyTransition() => OnTransition();

        protected virtual Task OnTransition() => Task.CompletedTask;
        
        internal Task NotifyHidden() => OnHide();
        
        protected virtual Task OnHide() => Task.CompletedTask;
    }
}