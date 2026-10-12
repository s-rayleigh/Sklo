using System;

namespace Rayleigh.Sklo.LayerViews
{
    public readonly struct LayerViewRequest
    {
        public readonly Type ViewType;
        public readonly object Key;

        public LayerViewRequest(Type viewType, object key = null)
        {
            ViewType = viewType;
            Key = key;
        }
    }
}