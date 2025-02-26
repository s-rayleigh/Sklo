using Rayleigh.Sklo;
using Sklo.Examples.Layers;
using Sklo.Examples.Screens;
using UnityEngine;

namespace Sklo.Examples
{
    [DefaultExecutionOrder(1)]
    public sealed class ExampleController : MonoBehaviour
    {
        [SerializeField]
        private ScreenManager _screenManager;

        private DebugScreen _debugScreen;
        
        private void Awake()
        {
            _screenManager.BeforeLayerShow += (layer, _) =>
            {
                if (layer is IScreenManagerHolderLayer holder)
                    holder.ScreenManager = _screenManager;
            };
            _debugScreen = _screenManager.ShowOverlay<DebugScreen>();
            _screenManager.ToggleMain<SplashScreen>();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.H) && _screenManager.MainDisplayed)
                _screenManager.HideMain();
            
            if (Input.GetKeyDown(KeyCode.S))
                _screenManager.ToggleMain<SplashScreen>();
            
            if (Input.GetKeyDown(KeyCode.D))
                _debugScreen.ToggleDebugInfo();
        }
    }
}