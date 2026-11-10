using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
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

        protected static void GetToggleMainTasks(in BeginToggleResult result, List<TTask> tasks,
            CancellationToken cancellationToken)
        {
            // TODO: optimize.
            
            if (result.ScreenToHide is not null)
                tasks.Add(((IAsyncScreen<TTask>)result.ScreenToHide).NotifyHidden(cancellationToken));
            if (result.ScreenToShow is not null)
                tasks.Add(((IAsyncScreen<TTask>)result.ScreenToShow).NotifyShown(cancellationToken));

            var transitionalLayers = result.GetTransitionalLayers();
            if (result.LayersToHide is not null)
            {
                foreach (var layer in result.LayersToHide.Except(transitionalLayers))
                    tasks.Add(((IAsyncLayer<TTask>)layer).NotifyHidden(cancellationToken));
            }

            foreach (var layer in transitionalLayers)
                tasks.Add(((IAsyncLayer<TTask>)layer).NotifyTransition(cancellationToken));
            if (result.LayersToShow is not null)
            {
                foreach (var layer in result.LayersToShow.Except(transitionalLayers))
                    tasks.Add(((IAsyncLayer<TTask>)layer).NotifyShown(cancellationToken));
            }
        }

        protected static void GetShowTasks(in BeginProcessResult result, List<TTask> tasks,
            CancellationToken cancellationToken)
        {
            tasks.Add(((IAsyncScreen<TTask>)result.Screen).NotifyShown(cancellationToken));
            foreach (var layer in result.LayersToProcess)
                tasks.Add(((IAsyncLayer<TTask>)layer).NotifyShown(cancellationToken));
        }

        protected static void GetHideTasks(in BeginProcessResult result, List<TTask> tasks,
            CancellationToken cancellationToken)
        {
            tasks.Add(((IAsyncScreen<TTask>)result.Screen).NotifyHidden(cancellationToken));
            foreach (var layer in result.LayersToProcess)
                tasks.Add(((IAsyncLayer<TTask>)layer).NotifyHidden(cancellationToken));
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