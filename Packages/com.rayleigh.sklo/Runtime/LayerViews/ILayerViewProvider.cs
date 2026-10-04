using UnityEngine;

namespace Rayleigh.Sklo.LayerViews
{
    /// <summary>
    /// Provides methods for retrieving and releasing instances of <see cref="ScreenLayerView"/>.
    /// </summary>
    /// <remarks>
    /// This allows for custom sources of layer view instances (like Addressables)
    /// and control over their lifecycle (pooling).
    /// </remarks>
    public interface ILayerViewProvider
    {
        public ScreenLayerView Get(LayerViewRequest request, RectTransform parent);
        public void Release(ScreenLayerView view);
    }
}