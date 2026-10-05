using System;
using Rayleigh.Sklo.LayerViews.Providers;
using UnityEngine;

namespace Rayleigh.Sklo.ScreenManagers.Hosts
{
    public abstract class ScreenManagerHost<T> : MonoBehaviour where T : BaseScreenManager
    {
        [SerializeField]
        private RectTransform _viewsParent;
        
        public T Manager { get; private set; }
        
        protected virtual void Reset()
        {
            if (!TryGetComponent<ILayerViewProvider>(out _))
                gameObject.AddComponent<DefaultLayerViewProvider>();
        }

        protected virtual void Awake()
        {
            if (!TryGetComponent(out ILayerViewProvider provider))
                throw new InvalidOperationException("Layer view provider must be present on the same object.");
            var parent = _viewsParent ?? transform as RectTransform;
            if (parent is null)
                throw new InvalidOperationException("View parent must be set.");
            Manager = CreateManager(provider, parent);
        }
        
        protected abstract T CreateManager(ILayerViewProvider viewProvider, RectTransform viewsParent);
    }
}