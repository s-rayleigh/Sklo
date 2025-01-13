using System.Collections;
using Rayleigh.Sklo;
using Sklo.Examples.Screens;
using UnityEngine;

namespace Sklo.Examples.Layers
{
    public class SplashLayer : ScreenLayer<SplashLayer, SplashLayerView>, IScreenManagerHolderLayer
    {
        public ScreenManager ScreenManager { get; set; }
        
        protected override void OnHide()
        {
            base.OnHide();
            Debug.Log("splash layer hide");
        }

        protected override void OnTransition()
        {
            base.OnTransition();
            Debug.Log("splash layer transition");
        }

        protected override void OnShow()
        {
            base.OnShow();
            Debug.Log("splash layer show");
            View.StartCoroutine(SplashDelay());
        }

        private IEnumerator SplashDelay()
        {
            yield return new WaitForSeconds(2f);
            ScreenManager.ToggleMain<MainScreen>();
        }
    }
}