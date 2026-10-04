using System;
using Rayleigh.Sklo.LayerViews;
using Rayleigh.Sklo.Screens;
using UnityEngine;
using Screen = Rayleigh.Sklo.Screens.Screen;

namespace Rayleigh.Sklo
{
    public abstract class BaseAsyncScreenManager<TScreen> : BaseScreenManager
        where TScreen : BaseScreen
    {
        protected BaseAsyncScreenManager(ILayerViewProvider layerViewProvider, RectTransform viewsParent)
            : base(layerViewProvider, viewsParent)
        {
        }

        protected bool CheckScreenSupported<T>() where T : BaseScreen
        {
            var type = typeof(T);
            return type.IsSubclassOf(typeof(Screen)) || type.IsSubclassOf(typeof(TScreen));
        }

        protected void ThrowIfScreenNotSupported<T>() where T : BaseScreen
        {
            if (!CheckScreenSupported<T>())
                throw new InvalidOperationException($"Screen {typeof(T).Name} is not supported by this manager.");
        }
    }
}