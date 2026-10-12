using Rayleigh.Sklo.Layers;
using Rayleigh.Sklo.ScreenManagers;
using Sklo.Examples.Layers.Views;
using UnityEngine;

namespace Sklo.Examples.Layers
{
    public sealed class SettingsLayer : ScreenLayer<SettingsLayer, SettingsLayerView>, IScreenManagerHolderLayer
    {
        public ScreenManager ScreenManager { get; set; }

        public void ToggleBack()
        {
            var result = ScreenManager.ToggleBack();
            if (result is null)
                Debug.Log("cannot return back, the history is empty");
        }
    }
}