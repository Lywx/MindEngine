namespace MindEngine.Core.Service
{
    public class MMEngineNullDebug : IMMEngineDebug
    {
        public bool Graphics_WidgetPrimitiveEnabled { get { return false; } set {} }
        public bool Graphics_WidgetClippingDisabled { get { return false; } set {} }
    }
}