using System.Collections.Generic;
using Rayleigh.Sklo.Layers;
using UnityEngine;

namespace Rayleigh.Sklo
{
    public sealed class ScreenCompositor<TLayer> where TLayer : BaseScreenLayer
    {
        private readonly ulong _opId;
        private readonly LayerManager _layerManager;
        private readonly HashSet<BaseScreenLayer> _layers;
        private readonly RectTransform _parent;

        internal ScreenCompositor(LayerManager layerManager, HashSet<BaseScreenLayer> layers, RectTransform parent,
            ulong opId)
        {
            _opId = opId;
            _layerManager = layerManager;
            _layers = layers;
            _parent = parent;
        }
        
        public void AddLayer<T>(object key = null) where T : TLayer, new()
        {
            var layer = _layerManager.Get<T>(key, _opId, _parent);
            _layers.Add(layer);
        }
    }
}