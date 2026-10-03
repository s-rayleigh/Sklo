using System;
using Rayleigh.Sklo.LayerViews;

namespace Rayleigh.Sklo.Layers
{
    public abstract class BaseScreenLayer
    {
        public ScreenLayerView View { get; internal set; }
        
        public DisplayType DisplayType { get; internal set; }
        
        internal abstract Type ViewType { get; }
        
        public virtual bool Transitional => true;
    }
}