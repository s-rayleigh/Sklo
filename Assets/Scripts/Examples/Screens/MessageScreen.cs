using Rayleigh.Sklo;
using Rayleigh.Sklo.Layers;
using Rayleigh.Sklo.Screens;
using Sklo.Examples.Data;
using Sklo.Examples.Layers;

namespace Sklo.Examples.Screens
{
    public sealed class MessageScreen : Screen<MessageData>
    {
        protected override void OnCompose(ScreenCompositor<ScreenLayer> compositor, MessageData args)
        {
            compositor.AddLayer<BackgroundLayer>();
            compositor.AddLayer<MessageScreenLayer, MessageData>(args);
        }
    }
}