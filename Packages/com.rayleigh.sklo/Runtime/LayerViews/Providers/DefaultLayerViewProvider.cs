using UnityEngine;

namespace Rayleigh.Sklo.LayerViews.Providers
{
    /// <summary>
    /// Default layer view provider implementation that automatically added by the host
    /// if no other layer view provider is present on the same game object.
    /// Ignores a key in the layer view request.
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class DefaultLayerViewProvider : LayerViewProvider
    {
        [SerializeField]
        private ScreenLayerView[] _viewPrefabs;
        
        public override ScreenLayerView Get(LayerViewRequest request, RectTransform parent)
        {
            for (var i = 0; i < _viewPrefabs.Length; i++)
            {
                var viewPrefab = _viewPrefabs[i];
                var prefabType = viewPrefab.GetType();

                if (request.ViewType != prefabType)
                    continue;
                
                return Instantiate(viewPrefab, parent, false);
            }

            return null;
        }

        public override void Release(ScreenLayerView view)
        {
            Destroy(view.gameObject);
        }
    }
}