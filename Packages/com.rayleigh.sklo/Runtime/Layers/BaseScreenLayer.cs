using System;
using System.Diagnostics.CodeAnalysis;
using Rayleigh.Sklo.LayerViews;

namespace Rayleigh.Sklo.Layers
{
    public abstract class BaseScreenLayer
    {
        public ScreenLayerView View { get; internal set; }
        
        public DisplayType DisplayType { get; internal set; }
        
        /// <summary>
        /// Dynamic key used to add this layer to the screen and to find a matching view.
        /// Null value indicates a view without a key was requested.
        /// </summary>
        /// <remarks>
        /// Transitions between layers with the same type, but different keys are not allowed because their view
        /// is different.
        /// </remarks>
        [MaybeNull]
        public object Key { get; internal set; }
        
        internal abstract Type ViewType { get; }
        
        public virtual bool Transitional => true;
    }
}