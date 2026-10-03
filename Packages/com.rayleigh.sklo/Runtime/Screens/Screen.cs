using Rayleigh.Sklo.Layers;

namespace Rayleigh.Sklo.Screens
{
    public abstract class Screen : BaseScreen<ScreenLayer>
    {
        internal void NotifyShown()
        {
            Displayed = true;
            OnShow();
        }

        protected virtual void OnShow()
        {
        }

        internal void NotifyHidden()
        {
            Displayed = false;
            OnHide();
        }

        protected virtual void OnHide()
        {
        }
    }
}