using System;
using Rayleigh.Sklo.LayerViews;

namespace Rayleigh.Sklo.Async.Tasks
{
    public abstract class AsyncScreenLayer<TLayer, TView> : AsyncScreenLayer
        where TLayer : AsyncScreenLayer<TLayer, TView>, new()
        where TView : ScreenLayerView<TView, TLayer>
    {
        internal override Type ViewType => typeof(TView);
        protected new TView View => (TView)base.View;
    }
}