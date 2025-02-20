using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

namespace Rayleigh.Sklo
{
    public abstract class BaseScreen
    {
        internal readonly Dictionary<Type, BaseScreenLayer> Layers = new();
        
        public bool Displayed { get; protected set; }
        
        public DisplayType DisplayType { get; protected set; }
        
        public Transform ParentContainer { get; internal set; }
        
        public bool Destroyed { get; private set; }

        public virtual bool ExcludeFromHistory => false;

        /// <summary>
        /// Priority used for sorting when the screen is shown in overlay mode.
        /// </summary>
        public virtual short OverlayPriority => 0; 

        internal BaseScreen()
        {
            
        }

        internal virtual void Compose(LayerManager layerManager, HashSet<BaseScreenLayer> layers, Transform parent)
        {
            ParentContainer = parent;
        }
        
        [return: MaybeNull]
        protected T GetLayer<T>() where T : BaseScreenLayer => (T)Layers.GetValueOrDefault(typeof(T));
        
        public bool SameAs([MaybeNull] BaseScreen other) => other is not null && other.GetType() == GetType();
    }
}