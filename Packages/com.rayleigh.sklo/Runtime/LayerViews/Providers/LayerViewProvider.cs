using UnityEngine;

namespace Rayleigh.Sklo.LayerViews.Providers
{
    public abstract class LayerViewProvider : MonoBehaviour, ILayerViewProvider
    {
        public abstract ScreenLayerView Get(LayerViewRequest request, RectTransform parent);

        public abstract void Release(ScreenLayerView view);
    }
}