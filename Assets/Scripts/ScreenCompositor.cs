using System;

namespace Sklo
{
    public sealed class ScreenCompositor
    {
        private readonly LayerManager _layerManager;
        
        internal ScreenCompositor(LayerManager layerManager)
        {
            _layerManager = layerManager;
        }
        
        public void AddLayer<T>() where T : ScreenLayer, new()
        {
            // TODO: prevent using the same layer twice.
            var layer = _layerManager.Get<T>();
            
            throw new NotImplementedException();
        }
    }
}