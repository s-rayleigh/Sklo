using UnityEngine;

namespace Sklo
{
    public abstract class ScreenLayerView : MonoBehaviour
    {
        
    }
    
    public abstract class ScreenLayerView<TView, TScreen> : ScreenLayerView
        where TView : ScreenLayerView<TView, TScreen>
        where TScreen : ScreenLayer<TScreen, TView>, new()
    {
        public TScreen Screen { get; private set; }
    }
}