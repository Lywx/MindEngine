namespace MindEngine.Core.Service
{
    public class MMEngineDebug : IMMEngineDebug
    {
        public bool Graphics_WidgetPrimitiveEnabled { get; set; } = false;

        public bool Graphics_WidgetClippingDisabled { get; set; } = false;
    }
}