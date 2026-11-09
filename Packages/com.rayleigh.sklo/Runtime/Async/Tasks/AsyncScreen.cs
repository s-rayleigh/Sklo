using System.Threading.Tasks;
using Rayleigh.Sklo.Screens;

namespace Rayleigh.Sklo.Async.Tasks
{
    public abstract class AsyncScreen : BaseScreen<AsyncScreenLayer>, IAsyncScreen<Task>
    {
        public Task NotifyShown()
        {
            Displayed = true;
            return OnShow();
        }

        public Task NotifyHidden()
        {
            Displayed = false;
            return OnHide();
        }
        
        protected virtual Task OnShow() => Task.CompletedTask;
        protected virtual Task OnHide() => Task.CompletedTask;
    }
}