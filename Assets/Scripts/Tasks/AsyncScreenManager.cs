using System.Linq;
using System.Threading.Tasks;
using UnityEngine.Pool;

namespace Sklo.Tasks
{
    public sealed class AsyncScreenManager : BaseAsyncScreenManager<AsyncScreen>
    {
        public async Task<T> ToggleMain<T>() where T : BaseScreen, new()
        {
            ThrowIfScreenNotSupported<T>();
            var screen = new T();

            if (screen is Screen syncScreen)
                ToggleMain(syncScreen);
            else
                // TODO: optimize!
                await ToggleMain((AsyncScreen)(object)screen);
            
            return screen;
        }

        public async Task<BaseScreen> ToggleBack()
        {
            if (!TryPopFromHistory(out var baseScreen))
                return null;
            
            if (baseScreen is Screen syncScreen)
                ToggleMain(syncScreen);
            else
                await ToggleMain((AsyncScreen)baseScreen);
            
            return baseScreen;
        }

        private async Task ToggleMain(AsyncScreen screen)
        {
            if (!BeginToggleMain(screen, out var result))
                return;
            
            var tasks = ListPool<Task>.Get();
            
            if (result.ScreenToHide is not null)
                tasks.Add(((AsyncScreen)result.ScreenToHide).NotifyHidden());
            tasks.Add(((AsyncScreen)result.ScreenToShow).NotifyShown());
            
            if (result.LayersToHide is null)
            {
                foreach (var layer in result.LayersToShow)
                    tasks.Add(((AsyncScreenLayer)layer).NotifyShown());
            }
            else
            {
                var transitionalLayers = result.GetTransitionalLayers();
                foreach (var layer in result.LayersToHide.Except(transitionalLayers))
                    tasks.Add(((AsyncScreenLayer)layer).NotifyHidden());
                foreach (var layer in transitionalLayers)
                    tasks.Add(((AsyncScreenLayer)layer).NotifyTransition());
                foreach (var layer in result.LayersToShow.Except(transitionalLayers))
                    tasks.Add(((AsyncScreenLayer)layer).NotifyShown());
                // TODO: optimize.
            }

            await Task.WhenAll(tasks);
            
            EndToggleMain(result);
        }
    }
}