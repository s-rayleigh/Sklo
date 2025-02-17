using Rayleigh.Sklo;
using Sklo.Examples.Layers.Views;
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
        }

        protected override void OnHide()
        {
            View.OpenSettingsPressed -= OpenSettingsScreen;
            base.OnHide();
        }

        private void OpenSettingsScreen() => ScreenManager.ToggleMain<SettingsScreen>();
    }
}