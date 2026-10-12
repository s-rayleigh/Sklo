using Rayleigh.Sklo.ScreenManagers;
using Rayleigh.Sklo.ScreenManagers.Hosts;
using Sklo.Examples.Layers;
using Sklo.Examples.Screens;
using UnityEngine;

namespace Sklo.Examples
{
    [DefaultExecutionOrder(1)]
    public sealed class ExampleController : MonoBehaviour
    {
        [SerializeField]
        private ScreenManagerHost _screenManagerHost;

        private DebugScreen _debugScreen;
        private TestOverlayScreen _testOverlayScreen;

        private ScreenManager ScreenManager => _screenManagerHost.Manager;
        
        private void Awake()
        {
            ScreenManager.BeforeLayerShow += (layer, _) =>
            {
                if (layer is IScreenManagerHolderLayer holder)
                    holder.ScreenManager = ScreenManager;
            };
            _debugScreen = ScreenManager.ShowOverlay<DebugScreen>();
            ScreenManager.ToggleMain<SplashScreen>();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.H) && ScreenManager.MainDisplayed)
                ScreenManager.HideMain();
            
            if (Input.GetKeyDown(KeyCode.S))
                ScreenManager.ToggleMain<SplashScreen>();
            
            if (Input.GetKeyDown(KeyCode.D))
                _debugScreen.ToggleDebugInfo();

            if (Input.GetKeyDown(KeyCode.O))
            {
                if (_testOverlayScreen is null)
                {
                    _testOverlayScreen = ScreenManager.ShowOverlay<TestOverlayScreen>();
                }
                else
                {
                    ScreenManager.HideOverlay(_testOverlayScreen);
                    _testOverlayScreen = null;
                }
            }
        }
    }
}