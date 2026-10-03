using System;
using Rayleigh.Sklo.Screens;

namespace Rayleigh.Sklo
{
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