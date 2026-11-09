using System;
using System.Threading.Tasks;
using Rayleigh.Sklo.LayerViews.Providers;
using Rayleigh.Sklo.ScreenManagers;
using Rayleigh.Sklo.Screens;
using UnityEngine;
using UnityEngine.Pool;
using Screen = Rayleigh.Sklo.Screens.Screen;

namespace Rayleigh.Sklo.Async.Tasks
{
    public class AsyncScreenManager : BaseAsyncScreenManager<AsyncScreen, Task>
    {
        public AsyncScreenManager(ILayerViewProvider layerViewProvider, RectTransform viewsParent)
            : base(layerViewProvider, viewsParent)
        {
        }
        
        #region Main

        public async Task<T> ToggleMain<T>(Action<T> init = null) where T : BaseScreen, new()
        {
            ThrowIfScreenNotSupported<T>();
            var screen = new T();
            init?.Invoke(screen);

            if (screen is Screen syncScreen)
                base.ToggleMain(syncScreen);
            else
                // TODO: optimize!
                await ToggleMain((AsyncScreen)(object)screen);
            
            return screen;
        }

        public async Task<BaseScreen> ToggleBack()
        {
            if (!TryPeekInHistory(out var baseScreen))
                return null;
            
            if (baseScreen is Screen syncScreen)
                base.ToggleMain(syncScreen);
            else
                await ToggleMain((AsyncScreen)baseScreen);
            
            return baseScreen;
        }

        public Task HideMain()
        {
            if (ModalDisplayed)
                throw new InvalidOperationException("Cannot hide the main screen while a modal screen is displayed.");
            return MainDisplayed ? ToggleMain(null) : Task.CompletedTask;
        }

        private async Task ToggleMain(AsyncScreen screen)
        {
            if (!BeginToggleMain(screen, out var result))
                return;
            
            using var _ = ListPool<Task>.Get(out var tasks);
            GetToggleMainTasks(result, tasks);
            await Task.WhenAll(tasks);
            
            EndToggleMain(result);
        }
        
        #endregion Main
        
        #region Modal

        public async Task<T> ShowModal<T>(Action<T> init = null) where T : BaseScreen, new()
        {
            ThrowIfScreenNotSupported<T>();
            var screen = new T();
            init?.Invoke(screen);

            if (screen is Screen syncScreen)
            {
                base.ShowModal(syncScreen);
                return screen;
            }

            var result = BeginShowModal(screen);
            using var _ = ListPool<Task>.Get(out var tasks);
            GetShowTasks(result, tasks);
            await Task.WhenAll(tasks);
            
            EndShowModal(result);
            return screen;
        }

        public new async Task HideModal()
        {
            var screen = ModalScreen;

            if (screen is Screen)
            {
                base.HideModal();
                return;
            }

            var result = BeginHideModal();
            using var _ = ListPool<Task>.Get(out var tasks);
            GetHideTasks(result, tasks);
            await Task.WhenAll(tasks);

            EndHideModal(result);
        }

        #endregion Modal
        
        #region Overlay

        public async Task<T> ShowOverlay<T>(Action<T> init = null) where T : BaseScreen, new()
        {
            ThrowIfScreenNotSupported<T>();
            var screen = new T();
            init?.Invoke(screen);

            if (screen is Screen syncScreen)
            {
                base.ShowOverlay(syncScreen);
                return screen;
            }

            var result = BeginShowOverlay(screen);
            using var _ = ListPool<Task>.Get(out var tasks);
            GetShowTasks(result, tasks);
            await Task.WhenAll(tasks);

            EndShowOverlay(result);
            return screen;
        }

        public new async Task HideOverlay(BaseScreen screen)
        {
            if (screen is Screen syncScreen)
            {
                base.HideOverlay(syncScreen);
                return;
            }

            var result = BeginHideOverlay(screen);
            using var _ = ListPool<Task>.Get(out var tasks);
            GetHideTasks(result, tasks);
            await Task.WhenAll(tasks);

            EndHideOverlay(result);
        }

        public async Task HideAllOverlay()
        {
            using var _1 = ListPool<Task>.Get(out var tasks);
            using var _2 = ListPool<BeginProcessResult>.Get(out var results);
            
            for (var i = OverlayScreens.Count - 1; i >= 0; i--)
            {
                var screen = OverlayScreens[i];

                if (screen is Screen syncScreen)
                {
                    base.HideOverlay(syncScreen);
                    continue;
                }

                var result = BeginHideOverlay(screen);
                GetHideTasks(result, tasks);
                results.Add(result);
            }

            await Task.WhenAll(tasks);

            for (var i = 0; i < results.Count; i++)
                EndHideOverlay(results[i]);
        }
        
        #endregion Overlay
    }
}