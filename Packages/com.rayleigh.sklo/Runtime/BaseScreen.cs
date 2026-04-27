using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

namespace Rayleigh.Sklo
{
    public abstract class BaseScreen
    {
        internal readonly Dictionary<Type, BaseScreenLayer> InternalLayers = new();
        
        public bool Displayed { get; protected set; }
        
        public DisplayType DisplayType { get; internal set; }
        
        public Transform ParentContainer { get; internal set; }
        
        public bool Destroyed { get; private set; }

        public virtual bool ExcludeFromHistory => false;

        /// <summary>
        /// Priority used for sorting when the screen is shown in overlay mode.
        /// </summary>
        public virtual short OverlayPriority => 0;

        public IEnumerable<BaseScreenLayer> Layers => InternalLayers.Values; 
        
        internal BaseScreen()
        {
            
        }

        internal virtual void Compose(LayerManager layerManager, HashSet<BaseScreenLayer> layers, Transform parent,
            ulong opId)
        {
            ParentContainer = parent;
        }
        
        [return: MaybeNull]
        protected T GetLayer<T>() where T : BaseScreenLayer => (T)InternalLayers.GetValueOrDefault(typeof(T));
        
        public bool SameAs([MaybeNull] BaseScreen other) => other is not null && other.GetType() == GetType();
    }
}