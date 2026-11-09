using System.Threading.Tasks;
using Rayleigh.Sklo.Layers;

namespace Rayleigh.Sklo.Async.Tasks
{
    public abstract class AsyncScreenLayer : BaseScreenLayer, IAsyncLayer<Task>
    {
        internal Task NotifyShown() => OnShow();
        Task IAsyncLayer<Task>.NotifyShown() => NotifyShown();
        protected virtual Task OnShow() => Task.CompletedTask;
        
        internal Task NotifyTransition() => OnTransition();
        Task IAsyncLayer<Task>.NotifyTransition() => NotifyTransition();
        protected virtual Task OnTransition() => Task.CompletedTask;
        
        internal Task NotifyHidden() => OnHide();
        Task IAsyncLayer<Task>.NotifyHidden() => NotifyHidden();
        protected virtual Task OnHide() => Task.CompletedTask;
    }
}