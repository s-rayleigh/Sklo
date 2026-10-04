using System;
using System.Collections.Generic;
using Rayleigh.Sklo.Extensions;
using Rayleigh.Sklo.Layers;
using Rayleigh.Sklo.LayerViews;
using UnityEngine;

namespace Rayleigh.Sklo
{
    internal class LayerManager
    {
        private readonly struct ReturnedLayer
        {
            public readonly BaseScreenLayer Layer;
            public readonly object Key;
            public readonly ulong OperationId;
            
            public ReturnedLayer(BaseScreenLayer layer, object key, ulong operationId)
            {
                Layer = layer;
                OperationId = operationId;
                Key = key;
            }
        }
        
        private readonly ILayerViewProvider _viewProvider;
        private readonly List<ReturnedLayer> _returnedLayers;
        
        public LayerManager(ILayerViewProvider viewProvider)
        {
            _viewProvider = viewProvider;
            _returnedLayers = new List<ReturnedLayer>();
        }
        
        public BaseScreenLayer Get<T>(object key, ulong opId, RectTransform parent) where T : BaseScreenLayer, new()
        {
            var layerType = typeof(T);
            if (TryConsumeTransitionalLayer(layerType, key, opId, out var layer))
                return layer;
            
            layer = new T();
            layer.Key = key;
            
            var viewType = layer.ViewType;
            var view = _viewProvider.Get(new LayerViewRequest(viewType, key), parent);
            
            if (view is null)
            {
                throw new KeyNotFoundException(
                    $"No view of type '{viewType.Name}' found for a layer type '{typeof(T).Name}' and key '{key}'.");
            }
            
            if (!viewType.IsInstanceOfType(view))
            {
                throw new InvalidOperationException($"Layer view provider returned view type '{view.GetType().Name}'" +
                                                    $" instead of requested '{viewType.Name} type.");
            }

            var viewTransform = view.transform;
            
            // Set the parent if provider ignored the "parent" argument.
            if (viewTransform.parent != parent)
                viewTransform.SetParent(parent, false);
            
            // Useful if the implemented pooling does not re-parent the object.
            viewTransform.SetAsLastSibling();
            
            // Cross-link the layer and view.
            layer.View = view;
            view.Layer = layer;
            
            return layer;
        }

        public void Return(BaseScreenLayer layer, ulong opId) =>
            _returnedLayers.Add(new ReturnedLayer(layer, layer.Key, opId));

        public void ReleaseUnused(ulong opId)
        {
            for (var i = 0; i < _returnedLayers.Count; i++)
            {
                var returnedLayer = _returnedLayers[i];
                if (returnedLayer.OperationId != opId)
                    continue;
                ReleaseLayer(returnedLayer.Layer);
                _returnedLayers.RemoveAtSwapBack(i);
            }
        }

        protected virtual void ReleaseLayer(BaseScreenLayer layer)
        {
            _viewProvider.Release(layer.View);
            
            var viewTransform = layer.View.transform;
            if (viewTransform.parent is not null)
                viewTransform.SetParent(null);
            
            layer.View = null;
            layer.Key = null;
        }

        private bool TryConsumeTransitionalLayer(Type layerType, object key, ulong opId, out BaseScreenLayer layer)
        {
            for (var i = 0; i < _returnedLayers.Count; i++)
            {
                var returnedLayer = _returnedLayers[i];
                
                if (!returnedLayer.Layer.Transitional)
                    continue;
                
                var keyEquals = returnedLayer.Key is null && key is null ||
                                returnedLayer.Key is not null && returnedLayer.Key.Equals(key);

                if (returnedLayer.Layer.GetType() != layerType || !keyEquals || returnedLayer.OperationId != opId)
                    continue;
                
                // "Consume" the layer.
                _returnedLayers.RemoveAtSwapBack(i);
                
                layer = returnedLayer.Layer;
                return true;
            }
            
            layer = null;
            return false;
        }
    }
}