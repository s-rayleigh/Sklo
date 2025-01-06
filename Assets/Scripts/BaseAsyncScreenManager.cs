using UnityEngine;

namespace Sklo
{
    [DisallowMultipleComponent]
    public abstract class BaseAsyncScreenManager<TScreen> : BaseScreenManager
        where TScreen : BaseScreen
    {
        protected bool CheckScreenSupported<T>() where T : BaseScreen
        {
            var type = typeof(T);
            return type == typeof(Screen) || type == typeof(T);
        }
    }
}