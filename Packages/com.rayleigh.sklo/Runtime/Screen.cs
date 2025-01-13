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

    public abstract class BaseScreen<T> : BaseScreen where T : BaseScreenLayer
    {
        internal override void Compose(LayerManager layerManager, HashSet<BaseScreenLayer> layers, Transform parent) =>
            OnCompose(new ScreenCompositor<T>(layerManager, layers, parent));

        protected abstract void OnCompose(ScreenCompositor<T> compositor);
    }

    public abstract class Screen : BaseScreen<ScreenLayer>
    {
        internal void NotifyShown()
        {
            Displayed = true;
            OnShow();
        }

        protected virtual void OnShow()
        {
        }

        internal void NotifyHidden()
        {
            Displayed = false;
            OnHide();
        }

        protected virtual void OnHide()
        {
        }
    }
}