using System.Collections.Generic;
using UnityEngine;

namespace Sklo
{
    public sealed class ScreenCompositor<TLayer> where TLayer : BaseScreenLayer
    {
        private readonly LayerManager _layerManager;
        private readonly HashSet<BaseScreenLayer> _layers;
        private readonly Transform _parent;
        
        internal ScreenCompositor(LayerManager layerManager, HashSet<BaseScreenLayer> layers, Transform parent)
        {
            _layerManager = layerManager;
            _layers = layers;
            _parent = parent;
        }
        
        public void AddLayer<T>() where T : TLayer, new()
        {
            var layer = _layerManager.Get<T>(_parent);
            _layers.Add(layer);
        }
    }
}