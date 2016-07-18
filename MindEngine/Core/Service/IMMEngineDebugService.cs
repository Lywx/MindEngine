namespace MindEngine.Core.Service
{
    public interface IMMEngineDebugService
    {
        /// <summary>
        /// Draw widget bounds.
        /// </summary>
        bool Graphics_WidgetPrimitiveEnabled { get; set; }

        /// <summary>
        /// Draw widget without clipping.
        /// </summary>
        bool Graphics_WidgetClippingDisabled { get; set; }
    }
}