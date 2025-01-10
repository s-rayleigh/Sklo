using Sklo.Examples.Screens;
using UnityEngine;

namespace Sklo.Examples
{
    [DefaultExecutionOrder(1)]
    public sealed class ExampleController : MonoBehaviour
    {
        [SerializeField]
        private ScreenManager _screenManager;

        private void Awake()
        {
            _screenManager.ToggleMain<SplashScreen>();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.H) && _screenManager.MainDisplayed)
                _screenManager.HideMain();
            
            if (Input.GetKeyDown(KeyCode.S))
                _screenManager.ToggleMain<SplashScreen>();
        }
    }
}