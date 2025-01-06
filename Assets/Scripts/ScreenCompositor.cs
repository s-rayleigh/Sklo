using System.Collections.Generic;

namespace Sklo
{
    public sealed class ScreenCompositor
    {
        private readonly LayerManager _layerManager;
        private readonly HashSet<BaseScreenLayer> _layers;
        
        internal ScreenCompositor(LayerManager layerManager, HashSet<BaseScreenLayer> layers)
        {
            _layerManager = layerManager;
            _layers = layers;
        }
        
        public void AddLayer<T>() where T : ScreenLayer, new()
        {
            var layer = _layerManager.Get<T>();
            _layers.Add(layer);
        }
    }
}