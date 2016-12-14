namespace MindEngine.Graphics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Forms;

    public class MMGraphicsDeviceContext
    {
        public MMGraphicsDeviceContext(List<MMGraphicsDeviceResolution> resolutionsAvailable)
        {
            if (resolutionsAvailable == null)
            {
                throw new ArgumentNullException(nameof(resolutionsAvailable));
            }

            this.ResolutionsAvailable = resolutionsAvailable;
        }

        public Screen[] ScreensAvailable => Screen.AllScreens;

        public Screen   ScreenPrimary => this.ScreensAvailable.First(e => e.Primary);

        public List<MMGraphicsDeviceResolution> ResolutionsAvailable { get; set; }
    }
}