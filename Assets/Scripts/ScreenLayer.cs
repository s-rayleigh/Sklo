using System;

namespace Sklo
{
    public abstract class ScreenLayer
    {
        internal virtual Type ViewType { get; }

        internal void NotifyShown() => OnShow();

        protected virtual void OnShow()
        {
            
        }

        internal void NotifyHidden() => OnHide();

        protected virtual void OnHide()
        {
            
        }
    }
    
    public abstract class ScreenLayer<TLayer, TView> : ScreenLayer
        where TLayer : ScreenLayer<TLayer, TView>, new()
        where TView : ScreenLayerView<TView, TLayer>
    {
        protected TView View { get; private set; }
    }
}