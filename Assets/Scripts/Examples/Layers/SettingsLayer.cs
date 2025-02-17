using Rayleigh.Sklo;
using Sklo.Examples.Layers.Views;
using UnityEngine;

namespace Sklo.Examples.Layers
{
    public sealed class SettingsLayer : ScreenLayer<SettingsLayer, SettingsLayerView>, IScreenManagerHolderLayer
    {
        public ScreenManager ScreenManager { get; set; }
        
        protected override void OnShow()
        {
            base.OnShow();
            View.BackPressed += Back;
        }

        protected override void OnHide()
        {
            base.OnHide();
            View.BackPressed -= Back;
        }

        private void Back()
        {
            var result = ScreenManager.ToggleBack();
            if (result is null)
                Debug.Log("cannot return back, the history is empty");
        }
    }
}