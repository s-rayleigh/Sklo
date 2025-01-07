using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Sklo
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

        internal abstract void Compose(LayerManager layerManager, HashSet<BaseScreenLayer> layers);
        
        [return: MaybeNull]
        protected T GetLayer<T>() where T : BaseScreenLayer => (T)Layers.GetValueOrDefault(typeof(T));
        
        public bool SameAs(BaseScreen other) => other.GetType() == GetType();
    }

    public abstract class BaseScreen<T> : BaseScreen where T : BaseScreenLayer
    {
        internal override void Compose(LayerManager layerManager, HashSet<BaseScreenLayer> layers) =>
            OnCompose(new ScreenCompositor<T>(layerManager, layers));

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