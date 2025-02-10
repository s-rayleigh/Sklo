namespace Rayleigh.Sklo
{
    public abstract class Screen : BaseScreen<ScreenLayer>
    {
        internal void NotifyShown(DisplayType displayType)
        {
            Displayed = true;
            DisplayType = displayType;
            OnShow();
        }

        protected virtual void OnShow()
        {
        }

        internal void NotifyHidden()
        {
            Displayed = false;
            DisplayType = DisplayType.None;
            OnHide();
        }

        protected virtual void OnHide()
        {
        }
    }
}