using Rayleigh.Sklo.Layers;

namespace Rayleigh.Sklo.LayerViews
{
    public abstract class ScreenLayerView<TView, TLayer> : ScreenLayerView
        where TView : ScreenLayerView<TView, TLayer>
        where TLayer : BaseScreenLayer, new()
    {
        public new TLayer Layer => (TLayer)base.Layer;
    }
}