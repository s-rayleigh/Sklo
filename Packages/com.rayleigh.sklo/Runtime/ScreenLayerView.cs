using UnityEngine;

namespace Rayleigh.Sklo
{
    public abstract class ScreenLayerView : MonoBehaviour
    {
        public BaseScreenLayer Layer { get; internal set; }
    }
    
    public abstract class ScreenLayerView<TView, TLayer> : ScreenLayerView
        where TView : ScreenLayerView<TView, TLayer>
        where TLayer : BaseScreenLayer, new()
    {
        public new TLayer Layer => (TLayer)base.Layer;
    }
}