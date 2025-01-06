using System;
using System.Collections.Generic;
using Object = UnityEngine.Object;

namespace Sklo
{
    internal class LayerManager
    {
        private readonly ScreenLayerView[] _layerViewPrefabs;
        private readonly Dictionary<Type, BaseScreenLayer> _returnedLayers;
        
        public LayerManager(ScreenLayerView[] layerViewPrefabs)
        {
            _layerViewPrefabs = layerViewPrefabs;
            _returnedLayers = new Dictionary<Type, BaseScreenLayer>();
        }
        
        public T Get<T>() where T : BaseScreenLayer, new()
        {
            var type = typeof(T);
            
            if (_returnedLayers.TryGetValue(type, out var returnedLayer) && returnedLayer.Transitional)
            {
                _returnedLayers.Remove(type);
                return (T)returnedLayer;
            }
            
            var layer = new T();
            var viewType = layer.ViewType;
            ScreenLayerView viewPrefab = null;
            
            for (var i = 0; i < _layerViewPrefabs.Length; i++)
            {
                var item = _layerViewPrefabs[i];
                if (item.GetType() != viewType)
                    continue;
                viewPrefab = item;
                break;
            }
            
            if (viewPrefab is null)
                throw new Exception($"No view prefab found for a layer of type {typeof(T).Name}");

            // TODO: set parent object for a view (still need to think what may be a parent for the layer view.
            // TODO: virtual method so it can be overriden to use a pool instead.
            layer.View = Object.Instantiate(viewPrefab);
            
            return layer;
        }

        public void Return(BaseScreenLayer layer) => _returnedLayers.Add(layer.GetType(), layer);

        public void ReleaseUnused()
        {
            foreach (var layer in _returnedLayers.Values)
                ReleaseLayer(layer);
            _returnedLayers.Clear();
        }

        protected virtual void ReleaseLayer(BaseScreenLayer layer)
        {
            Object.Destroy(layer.View);
            layer.View = null;
        }
    }
}