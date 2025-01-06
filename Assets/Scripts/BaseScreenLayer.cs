using System;

namespace Sklo
{
    public abstract class BaseScreenLayer
    {
        public ScreenLayerView View { get; internal set; }
        
        internal abstract Type ViewType { get; }
        
        public virtual bool Transitional => true;
    }
}