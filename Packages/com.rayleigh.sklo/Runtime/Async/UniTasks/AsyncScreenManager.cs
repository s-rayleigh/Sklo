using System;
using System.Threading;
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

        public async UniTask<T> ToggleMain<T>(CancellationToken cancellationToken = default)
            where T : BaseScreen, new()
        {
            ThrowIfScreenNotSupported<T>();
            var screen = new T();
            await ToggleMainScreen(screen, cancellationToken);
            return screen;
        }

        public async UniTask<T> ToggleMain<T, TArgs>(TArgs args, CancellationToken cancellationToken = default)
            where T : BaseScreen, IScreenWithArgs<TArgs>, new()
        {
            ThrowIfScreenNotSupported<T>();
            var screen = CreateScreen<T, TArgs>(args);
            await ToggleMainScreen(screen, cancellationToken);
            return screen;
        }

        public async UniTask<BaseScreen> ToggleBack(CancellationToken cancellationToken = default)
        {
            if (!TryPeekInHistory(out var baseScreen))
                return null;

            await ToggleMainScreen(baseScreen, cancellationToken);
            return baseScreen;
        }

        public UniTask HideMain(CancellationToken cancellationToken = default)
        {
            if (ModalDisplayed)
                throw new InvalidOperationException("Cannot hide the main screen while a modal screen is displayed.");
            return MainDisplayed ? ToggleMainScreen(null, cancellationToken) : UniTask.CompletedTask;
        }

        private async UniTask ToggleMainScreen(BaseScreen screen, CancellationToken cancellationToken)
        {
            if (screen is Screen syncScreen)
            {
                base.ToggleMain(syncScreen);
                return;
            }
            
            if (!BeginToggleMain(screen, out var result))
                return;
            
            using var _ = ListPool<UniTask>.Get(out var tasks);
            try
            {
                GetToggleMainTasks(result, tasks, cancellationToken);
                await UniTask.WhenAll(tasks);
            }
            finally
            {
                EndToggleMain(result);
            }
        }
        
        #endregion Main
        
        #region Modal

        public async UniTask<T> ShowModal<T>(CancellationToken cancellationToken = default)
            where T : BaseScreen, new()
        {
            ThrowIfScreenNotSupported<T>();
            var screen = new T();
            await ShowModalScreen(screen, cancellationToken);
            return screen;
        }

        public async UniTask<T> ShowModal<T, TArgs>(TArgs args, CancellationToken cancellationToken = default)
            where T : BaseScreen, IScreenWithArgs<TArgs>, new()
        {
            ThrowIfScreenNotSupported<T>();
            var screen = CreateScreen<T, TArgs>(args);
            await ShowModalScreen(screen, cancellationToken);
            return screen;
        }

        private async UniTask ShowModalScreen(BaseScreen screen, CancellationToken cancellationToken)
        {
            if (screen is Screen syncScreen)
            {
                base.ShowModal(syncScreen);
                return;
            }

            var result = BeginShowModal(screen);
            using var _ = ListPool<UniTask>.Get(out var tasks);
            try
            {
                GetShowTasks(result, tasks, cancellationToken);
                await UniTask.WhenAll(tasks);
            }
            finally
            {
                EndShowModal(result);
            }
        }

        public async UniTask HideModal(CancellationToken cancellationToken = default)
        {
            var screen = ModalScreen;

            if (screen is Screen)
            {
                base.HideModal();
                return;
            }

            var result = BeginHideModal();
            using var _ = ListPool<UniTask>.Get(out var tasks);
            try
            {
                GetHideTasks(result, tasks, cancellationToken);
                await UniTask.WhenAll(tasks);
            }
            finally
            {
                EndHideModal(result);
            }
        }

        #endregion Modal
        
        #region Overlay

        public async UniTask<T> ShowOverlay<T>(CancellationToken cancellationToken = default)
            where T : BaseScreen, new()
        {
            ThrowIfScreenNotSupported<T>();
            var screen = new T();
            await ShowOverlayScreen(screen, cancellationToken);
            return screen;
        }

        public async UniTask<T> ShowOverlay<T, TArgs>(TArgs args, CancellationToken cancellationToken = default)
            where T : BaseScreen, IScreenWithArgs<TArgs>, new()
        {
            ThrowIfScreenNotSupported<T>();
            var screen = CreateScreen<T, TArgs>(args);
            await ShowOverlayScreen(screen, cancellationToken);
            return screen;
        }

        private async UniTask ShowOverlayScreen(BaseScreen screen, CancellationToken cancellationToken)
        {
            if (screen is Screen syncScreen)
            {
                base.ShowOverlay(syncScreen);
                return;
            }

            var result = BeginShowOverlay(screen);
            using var _ = ListPool<UniTask>.Get(out var tasks);
            try
            {
                GetShowTasks(result, tasks, cancellationToken);
                await UniTask.WhenAll(tasks);
            }
            finally
            {
                EndShowOverlay(result);
            }
        }

        public async UniTask HideOverlay(BaseScreen screen, CancellationToken cancellationToken = default)
        {
            if (screen is Screen syncScreen)
            {
                base.HideOverlay(syncScreen);
                return;
            }

            var result = BeginHideOverlay(screen);
            using var _ = ListPool<UniTask>.Get(out var tasks);
            try
            {
                GetHideTasks(result, tasks, cancellationToken);
                await UniTask.WhenAll(tasks);
            }
            finally
            {
                EndHideOverlay(result);
            }
        }

        public async UniTask HideAllOverlay(CancellationToken cancellationToken = default)
        {
            using var _1 = ListPool<UniTask>.Get(out var tasks);
            using var _2 = ListPool<BeginProcessResult>.Get(out var results);
            
            try
            {
                for (var i = OverlayScreens.Count - 1; i >= 0; i--)
                {
                    var screen = OverlayScreens[i];

                    if (screen is Screen syncScreen)
                    {
                        base.HideOverlay(syncScreen);
                        continue;
                    }

                    var result = BeginHideOverlay(screen);
                    // Record the result before invoking hooks, so it gets ended even if a hook throws synchronously.
                    results.Add(result);
                    GetHideTasks(result, tasks, cancellationToken);
                }

                await UniTask.WhenAll(tasks);
            }
            finally
            {
                for (var i = 0; i < results.Count; i++)
                    EndHideOverlay(results[i]);
            }
        }
        
        #endregion Overlay
    }
}