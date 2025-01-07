using System;

namespace Sklo
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
    
    public abstract class ScreenLayer<TLayer, TView> : ScreenLayer
        where TLayer : ScreenLayer<TLayer, TView>, new()
        where TView : ScreenLayerView<TView, TLayer>
    {
        internal override Type ViewType => typeof(TView);
        protected new TView View => (TView)base.View;
    }
}