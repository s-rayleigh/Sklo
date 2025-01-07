using System.Threading.Tasks;
using LayerType = Sklo.Tasks.AsyncScreenLayer;

namespace Sklo.Tasks
{
    public abstract class AsyncScreen : BaseScreen<LayerType>
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