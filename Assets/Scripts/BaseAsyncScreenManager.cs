using System;
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
            return type.IsSubclassOf(typeof(Screen)) || type.IsSubclassOf(typeof(TScreen));
        }

        protected void ThrowIfScreenNotSupported<T>() where T : BaseScreen
        {
            if (!CheckScreenSupported<T>())
                throw new InvalidOperationException($"Screen {typeof(T).Name} is not supported by this manager.");
        }
    }
}