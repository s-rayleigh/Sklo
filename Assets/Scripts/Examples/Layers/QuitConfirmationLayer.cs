using Rayleigh.Sklo;
using Rayleigh.Sklo.Layers;
using Rayleigh.Sklo.ScreenManagers;
using Sklo.Examples.Layers.Views;
using UnityEngine;

namespace Sklo.Examples.Layers
{
    public sealed class QuitConfirmationLayer : ScreenLayer<QuitConfirmationLayer, QuitConfirmationLayerView>,
        IScreenManagerHolderLayer
    {
        public ScreenManager ScreenManager { get; set; }

        public void HideScreen()
        {
            if (DisplayType is DisplayType.Modal)
                ScreenManager.HideModal();
            else
                Debug.Log($"Cannot hide quit confirmation screen, unsupported display type {DisplayType}.", View);
        }

        public void CloseApp()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.ExitPlaymode();
#else
            Application.Quit();
#endif
        }
    }
}