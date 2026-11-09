using System;
using System.Collections.Generic;
using System.Linq;
using Rayleigh.Sklo.Async;
using Rayleigh.Sklo.LayerViews.Providers;
using Rayleigh.Sklo.Screens;
using UnityEngine;
using Screen = Rayleigh.Sklo.Screens.Screen;

namespace Rayleigh.Sklo.ScreenManagers
{
    public abstract class BaseAsyncScreenManager<TScreen, TTask> : BaseScreenManager where TScreen : BaseScreen
    {
        protected BaseAsyncScreenManager(ILayerViewProvider layerViewProvider, RectTransform viewsParent)
            : base(layerViewProvider, viewsParent)
        {
        }

        protected static void GetToggleMainTasks(in BeginToggleResult result, List<TTask> tasks)
        {
            // TODO: optimize.
            
            if (result.ScreenToHide is not null)
                tasks.Add(((IAsyncScreen<TTask>)result.ScreenToHide).NotifyHidden());
            if (result.ScreenToShow is not null)
                tasks.Add(((IAsyncScreen<TTask>)result.ScreenToShow).NotifyShown());

            var transitionalLayers = result.GetTransitionalLayers();
            if (result.LayersToHide is not null)
            {
                foreach (var layer in result.LayersToHide.Except(transitionalLayers))
                    tasks.Add(((IAsyncLayer<TTask>)layer).NotifyHidden());
            }

            foreach (var layer in transitionalLayers)
                tasks.Add(((IAsyncLayer<TTask>)layer).NotifyTransition());
            if (result.LayersToShow is not null)
            {
                foreach (var layer in result.LayersToShow.Except(transitionalLayers))
                    tasks.Add(((IAsyncLayer<TTask>)layer).NotifyShown());
            }
        }

        protected static void GetShowTasks(in BeginProcessResult result, List<TTask> tasks)
        {
            tasks.Add(((IAsyncScreen<TTask>)result.Screen).NotifyShown());
            foreach (var layer in result.LayersToProcess)
                tasks.Add(((IAsyncLayer<TTask>)layer).NotifyShown());
        }
        
        protected static void GetHideTasks(in BeginProcessResult result, List<TTask> tasks)
        {
            tasks.Add(((IAsyncScreen<TTask>)result.Screen).NotifyHidden());
            foreach (var layer in result.LayersToProcess)
                tasks.Add(((IAsyncLayer<TTask>)layer).NotifyHidden());
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