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
        
        public bool Destroyed { get; private set; }

        public virtual bool ExcludeFromHistory => false;

        internal BaseScreen()
        {
            
        }

        internal abstract void Compose(LayerManager layerManager, HashSet<BaseScreenLayer> layers, Transform parent);
        
        [return: MaybeNull]
        protected T GetLayer<T>() where T : BaseScreenLayer => (T)Layers.GetValueOrDefault(typeof(T));
        
        public bool SameAs([MaybeNull] BaseScreen other) => other is not null && other.GetType() == GetType();
    }
}