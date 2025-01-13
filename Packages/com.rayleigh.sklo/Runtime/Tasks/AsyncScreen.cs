using System.Threading.Tasks;

namespace Rayleigh.Sklo.Tasks
{
    public abstract class AsyncScreen : BaseScreen<AsyncScreenLayer>
    {
        internal Task NotifyShown()
        {
            Displayed = true;
            return OnShow();
        }

        internal Task NotifyHidden()
        {
            Displayed = false;
            return OnHide();
        }
        
        protected virtual Task OnShow() => Task.CompletedTask;
        protected virtual Task OnHide() => Task.CompletedTask;
    }
}