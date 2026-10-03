namespace Rayleigh.Sklo.Layers
{
    public abstract class ScreenLayer : BaseScreenLayer
    {
        internal void NotifyShown() => OnShow();

        protected virtual void OnShow()
        {
        }

        internal void NotifyTransition() => OnTransition();

        protected virtual void OnTransition()
        {
        }
        
        internal void NotifyHidden() => OnHide();

        protected virtual void OnHide()
        {
        }
    }
}