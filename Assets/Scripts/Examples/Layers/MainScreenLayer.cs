using Rayleigh.Sklo;
using Sklo.Examples.Screens;
using UnityEngine;

namespace Sklo.Examples.Layers
{
    public sealed class MainScreenLayer : ScreenLayer<MainScreenLayer, MainScreenLayerView>, IScreenManagerHolderLayer 
    {
        public ScreenManager ScreenManager { get; set; }
        
        protected override void OnShow()
        {
            base.OnShow();
            View.OpenSettingsPressed += OpenSettingsScreen;
            Debug.Log("show main screen layer");
        }

        protected override void OnHide()
        {
            View.OpenSettingsPressed -= OpenSettingsScreen;
            base.OnHide();
            Debug.Log("hide main screen layer");
        }

        private void OpenSettingsScreen() => ScreenManager.ToggleMain<SettingsScreen>();
    }
}