using System;
using Rayleigh.Sklo;
using TMPro;
using UnityEngine;

namespace Sklo.Examples.Layers
{
    public class SplashLayerView : ScreenLayerView<SplashLayerView, SplashLayer>
    {
        [SerializeField]
        private TMP_Text _timeText;

        private string _timeTextPattern;
        
        private void Awake() => _timeTextPattern = _timeText.text;

        public void SetTimeLeft(TimeSpan timeLeft) => _timeText.text = string.Format(_timeTextPattern, timeLeft);
    }
}