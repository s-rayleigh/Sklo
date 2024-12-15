using System;
using System.Collections.Generic;

namespace Sklo
{
    internal class LayerManager
    {
        private List<ScreenLayer> _layersInUse;
        
        public LayerManager()
        {
            _layersInUse = new List<ScreenLayer>();
        }
        
        public ScreenLayer Get<T>() where T : ScreenLayer
        {
            throw new NotImplementedException();
        }

        public void Recycle(ScreenLayer layer)
        {
            throw new NotImplementedException();
        }
    }
}