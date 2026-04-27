using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using Object = UnityEngine.Object;

namespace Rayleigh.Sklo
{
    internal class LayerManager
    {
        private readonly ScreenLayerView[] _layerViewPrefabs;
        private readonly Dictionary<(ulong opId, Type layerType), BaseScreenLayer> _returnedLayers;
        
        public LayerManager(ScreenLayerView[] layerViewPrefabs)
        {
            _layerViewPrefabs = layerViewPrefabs;
            _returnedLayers = new();
        }
        
        public T Get<T>(Transform parent, ulong opId) where T : BaseScreenLayer, new()
        {
            var type = typeof(T);
            
            // TODO: search 
            if (_returnedLayers.TryGetValue((opId, type), out var returnedLayer) && returnedLayer.Transitional)
            {
                _returnedLayers.Remove((opId, type));
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

            // TODO: virtual method so it can be overriden to use a pool instead.
            layer.View = Object.Instantiate(viewPrefab, parent, false);
            layer.View.Layer = layer;
            
            return layer;
        }

        public void Return(BaseScreenLayer layer, ulong opId) => _returnedLayers.Add((opId, layer.GetType()), layer);

        public void ReleaseUnused(ulong opId)
        {
            using var _ = ListPool<(ulong, Type)>.Get(out var toRemove);
            
            foreach (var ((layerOpId, layerType), layer) in _returnedLayers)
            {
                if (layerOpId != opId)
                    continue;
                ReleaseLayer(layer);
                toRemove.Add((layerOpId, layerType));
            }

            for (var i = 0; i < toRemove.Count; i++)
                _returnedLayers.Remove(toRemove[i]);
        }

        protected virtual void ReleaseLayer(BaseScreenLayer layer)
        {
            Object.Destroy(layer.View.gameObject);
            layer.View = null;
        }
    }
}