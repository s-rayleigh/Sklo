using System;
using Cysharp.Threading.Tasks;
using Rayleigh.Sklo.LayerViews.Providers;
using Rayleigh.Sklo.ScreenManagers;
using Rayleigh.Sklo.Screens;
using UnityEngine;
using UnityEngine.Pool;
using Screen = Rayleigh.Sklo.Screens.Screen;

namespace Rayleigh.Sklo.Async.UniTasks
{
    public class AsyncScreenManager : BaseAsyncScreenManager<AsyncScreen, UniTask>
    {
        public AsyncScreenManager(ILayerViewProvider layerViewProvider, RectTransform viewsParent)
            : base(layerViewProvider, viewsParent)
        {
        }
        
        #region Main

        public async UniTask<T> ToggleMain<T>(Action<T> init = null) where T : BaseScreen, new()
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

        public async UniTask<BaseScreen> ToggleBack()
        {
            if (!TryPeekInHistory(out var baseScreen))
                return null;
            
            if (baseScreen is Screen syncScreen)
                base.ToggleMain(syncScreen);
            else
                await ToggleMain((AsyncScreen)baseScreen);
            
            return baseScreen;
        }

        public UniTask HideMain()
        {
            if (ModalDisplayed)
                throw new InvalidOperationException("Cannot hide the main screen while a modal screen is displayed.");
            return MainDisplayed ? ToggleMain(null) : UniTask.CompletedTask;
        }

        private async UniTask ToggleMain(AsyncScreen screen)
        {
            if (!BeginToggleMain(screen, out var result))
                return;
            
            using var _ = ListPool<UniTask>.Get(out var tasks);
            GetToggleMainTasks(result, tasks);
            await UniTask.WhenAll(tasks);
            
            EndToggleMain(result);
        }
        
        #endregion Main
        
        #region Modal

        public async UniTask<T> ShowModal<T>(Action<T> init = null) where T : BaseScreen, new()
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
            using var _ = ListPool<UniTask>.Get(out var tasks);
            GetShowTasks(result, tasks);
            await UniTask.WhenAll(tasks);
            
            EndShowModal(result);
            return screen;
        }

        public new async UniTask HideModal()
        {
            var screen = ModalScreen;

            if (screen is Screen)
            {
                base.HideModal();
                return;
            }

            var result = BeginHideModal();
            using var _ = ListPool<UniTask>.Get(out var tasks);
            GetHideTasks(result, tasks);
            await UniTask.WhenAll(tasks);

            EndHideModal(result);
        }

        #endregion Modal
        
        #region Overlay

        public async UniTask<T> ShowOverlay<T>(Action<T> init = null) where T : BaseScreen, new()
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
            using var _ = ListPool<UniTask>.Get(out var tasks);
            GetShowTasks(result, tasks);
            await UniTask.WhenAll(tasks);

            EndShowOverlay(result);
            return screen;
        }

        public new async UniTask HideOverlay(BaseScreen screen)
        {
            if (screen is Screen syncScreen)
            {
                base.HideOverlay(syncScreen);
                return;
            }

            var result = BeginHideOverlay(screen);
            using var _ = ListPool<UniTask>.Get(out var tasks);
            GetHideTasks(result, tasks);
            await UniTask.WhenAll(tasks);

            EndHideOverlay(result);
        }

        public async UniTask HideAllOverlay()
        {
            using var _1 = ListPool<UniTask>.Get(out var tasks);
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

            await UniTask.WhenAll(tasks);

            for (var i = 0; i < results.Count; i++)
                EndHideOverlay(results[i]);
        }
        
        #endregion Overlay
    }
}