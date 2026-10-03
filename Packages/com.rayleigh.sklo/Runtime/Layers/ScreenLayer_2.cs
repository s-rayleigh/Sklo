using System;
using Rayleigh.Sklo.LayerViews;

namespace Rayleigh.Sklo.Layers
{
    public abstract class ScreenLayer<TLayer, TView> : ScreenLayer
        where TLayer : ScreenLayer<TLayer, TView>, new()
        where TView : ScreenLayerView<TView, TLayer>
    {
        internal override Type ViewType => typeof(TView);
        protected new TView View => (TView)base.View;
    }
}