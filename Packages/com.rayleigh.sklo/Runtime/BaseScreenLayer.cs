using System;

namespace Rayleigh.Sklo
{
    public abstract class BaseScreenLayer
    {
        public ScreenLayerView View { get; internal set; }
        
        public DisplayType DisplayType { get; internal set; }
        
        internal abstract Type ViewType { get; }
        
        public virtual bool Transitional => true;
    }
}