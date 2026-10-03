using System;
using System.Collections;
using Rayleigh.Sklo;
using Rayleigh.Sklo.Layers;
using Sklo.Examples.Layers.Views;
using Sklo.Examples.Screens;
using UnityEngine;

namespace Sklo.Examples.Layers
{
    public sealed class SplashLayer : ScreenLayer<SplashLayer, SplashLayerView>, IScreenManagerHolderLayer
    {
        private const float ShowDuration = 3f;
        
        public ScreenManager ScreenManager { get; set; }

        protected override void OnShow()
        {
            base.OnShow();
            View.StartCoroutine(SplashDelay());
        }

        private IEnumerator SplashDelay()
        {
            var time = ShowDuration;
            while (time > 0f)
            {
                time -= Time.deltaTime;
                View.SetTimeLeft(TimeSpan.FromSeconds(time));
                yield return null;
            }

            ScreenManager.ToggleMain<MainScreen>();
        }
    }
}