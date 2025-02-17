using System;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine.Pool;

namespace Rayleigh.Sklo.Tasks
{
    public sealed class AsyncScreenManager : BaseAsyncScreenManager<AsyncScreen>
    {
        #region Main
        public async Task<T> ToggleMain<T>(Action<T> init = null) where T : BaseScreen, new()
        {
            ThrowIfScreenNotSupported<T>();
            var screen = new T();
            init?.Invoke(screen);

            if (screen is Screen syncScreen)
                ToggleMain(syncScreen);
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
                ToggleMain(syncScreen);
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
            
            var tasks = ListPool<Task>.Get();

            try
            {
                if (result.ScreenToHide is not null)
                    tasks.Add(((AsyncScreen)result.ScreenToHide).NotifyHidden());
                if (result.ScreenToShow is not null)
                    tasks.Add(((AsyncScreen)result.ScreenToShow).NotifyShown());

                var transitionalLayers = result.GetTransitionalLayers();
                if (result.LayersToHide is not null)
                {
                    foreach (var layer in result.LayersToHide.Except(transitionalLayers))
                        tasks.Add(((AsyncScreenLayer)layer).NotifyHidden());
                }

                foreach (var layer in transitionalLayers)
                    tasks.Add(((AsyncScreenLayer)layer).NotifyTransition());
                if (result.LayersToShow is not null)
                {
                    foreach (var layer in result.LayersToShow.Except(transitionalLayers))
                        tasks.Add(((AsyncScreenLayer)layer).NotifyShown());
                }
                // TODO: optimize.

                await Task.WhenAll(tasks);
            }
            finally
            {
                ListPool<Task>.Release(tasks);
            }
            
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
                ShowModal(syncScreen);
                return screen;
            }

            var result = BeginShowModal(screen);
            var tasks = ListPool<Task>.Get();

            try
            {
                tasks.Add(((AsyncScreen)result.Screen).NotifyShown());
                foreach (var layer in result.LayersToProcess)
                    tasks.Add(((AsyncScreenLayer)layer).NotifyShown());
                await Task.WhenAll(tasks);
            }
            finally
            {
                ListPool<Task>.Release(tasks);
            }

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
            var tasks = ListPool<Task>.Get();

            try
            {
                tasks.Add(((AsyncScreen)result.Screen).NotifyHidden());
                foreach (var layer in result.LayersToProcess)
                    tasks.Add(((AsyncScreenLayer)layer).NotifyHidden());
                await Task.WhenAll(tasks);
            }
            finally
            {
                ListPool<Task>.Release(tasks);
            }

            EndShowModal(result);
        }

        #endregion Modal
    }
}