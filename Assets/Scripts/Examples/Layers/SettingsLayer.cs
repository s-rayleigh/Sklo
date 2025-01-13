using Rayleigh.Sklo;
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
            Debug.Log("show settings layer");
        }

        protected override void OnHide()
        {
            base.OnHide();
            View.BackPressed -= Back;
            Debug.Log("hide settings layer");
        }

        private void Back()
        {
            Debug.Log("back pressed");
            var result = ScreenManager.ToggleBack();
            if (result is null)
                Debug.Log("cannot return back, the history is empty");
        }
    }
}