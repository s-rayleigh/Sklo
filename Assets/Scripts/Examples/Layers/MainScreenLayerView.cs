using System;
using UnityEngine.UI;

namespace Sklo.Examples.Layers
{
    public sealed class MainScreenLayerView : ScreenLayerView<MainScreenLayerView, MainScreenLayer>
    {
        public event Action OpenSettingsPressed;
        
        public void OpenSettingsScreen() => OpenSettingsPressed?.Invoke();
    }
}