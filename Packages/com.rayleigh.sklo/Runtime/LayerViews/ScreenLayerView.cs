using Rayleigh.Sklo.Layers;
using UnityEngine;

namespace Rayleigh.Sklo.LayerViews
{
    public abstract class ScreenLayerView : MonoBehaviour
    {
        public BaseScreenLayer Layer { get; internal set; }
    }
}