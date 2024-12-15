using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Sklo
{
    public abstract class BaseScreen
    {
        private readonly Dictionary<Type, ScreenLayer> _layers = new();
        
        public bool Displayed { get; private set; }
        
        public bool Destroyed { get; private set; }

        public virtual bool ExcludeFromHistory => false;

        internal BaseScreen()
        {
            
        }
        
        internal void Compose(ScreenCompositor compositor) => OnCompose(compositor);
        
        protected abstract void OnCompose(ScreenCompositor compositor);
        
        [return: MaybeNull]
        protected T GetLayer<T>() where T : ScreenLayer => (T)_layers.GetValueOrDefault(typeof(T));
        
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

    public abstract class Screen : BaseScreen
    {
        
    }

    public abstract class ParametricScreen<T> : BaseScreen where T : unmanaged
    {
        protected T Parameter { get; private set; }
        
        internal void SetParameter(T parameter)
        {
            Parameter = parameter;
        }
    }
}