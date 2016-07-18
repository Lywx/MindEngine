namespace MindEngine.Core.Service
{
    using System;

    public class MMEngineDebugService : IMMEngineDebugService
    {
        private IMMEngineDebug Debug { get; set; }

        public MMEngineDebugService(IMMEngineDebug debug)
        {
            if (debug == null)
            {
                throw new ArgumentNullException(nameof(debug));
            }

            this.Debug = debug;
        }

        public bool Graphics_WidgetPrimitiveEnabled
        {
            get { return this.Debug.Graphics_WidgetPrimitiveEnabled; }
            set { this.Debug.Graphics_WidgetPrimitiveEnabled = value; }
        }

        public bool Graphics_WidgetClippingDisabled
        {
            get { return this.Debug.Graphics_WidgetClippingDisabled; }
            set { this.Debug.Graphics_WidgetClippingDisabled = value; }
        }
    }
}
