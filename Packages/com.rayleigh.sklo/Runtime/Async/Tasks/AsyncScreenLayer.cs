using System.Threading.Tasks;
using Rayleigh.Sklo.Layers;

namespace Rayleigh.Sklo.Async.Tasks
{
    public abstract class AsyncScreenLayer : BaseScreenLayer, IAsyncLayer<Task>
    {
        public Task NotifyShown() => OnShow();

        protected virtual Task OnShow() => Task.CompletedTask;

        public Task NotifyTransition() => OnTransition();

        protected virtual Task OnTransition() => Task.CompletedTask;
        
        public Task NotifyHidden() => OnHide();
        
        protected virtual Task OnHide() => Task.CompletedTask;
    }
}