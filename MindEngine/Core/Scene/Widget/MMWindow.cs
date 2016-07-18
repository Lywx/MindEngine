namespace MindEngine.Core.Scene.Widget
{
    using Content.Widget;
    using Microsoft.Xna.Framework;

    public class MMWindow : MMModalContainer
    {
        #region Entity

        public override string EntityClass => "Window";

        #endregion

        #region Layers

        /// The following layers are default design used in the window. To 
        /// implement customized behavior

        private readonly string layerWindowCaptionString     = "Window.Caption";

        private MMControlLayer layerWindowCaption;

        private readonly string layerWindowIconString        = "Window.Icon";

        private readonly string layerWindowFrameTopString    = "Window.FrameTop";

        private readonly string layerWindowFrameLeftString   = "Window.FrameLeft";

        private readonly string layerWindowFrameRightString  = "Window.FrameRight";

        private readonly string layerWindowFrameBottomString = "Window.FrameBottom";

        private readonly string layerWindowShadowString      = "Window.Shadow";

        private readonly string skinButtonString             = "Window.Caption.CloseButton";

        private readonly string layerClientFrameTopString    = "Client.FrameTop";

        private readonly string layerClientFrameLeftString   = "Client.FrameLeft";

        private readonly string layerClientFrameRightString  = "Client.FrameRight";

        private readonly string layerClientFrameBottomString = "Client.FrameBottom";

        private readonly string layerButtonString = "Control";

        #endregion

        #region Widget

        #region Layout

        private bool captionEnabled;

        public virtual bool CaptionEnabled
        {
            get { return this.captionEnabled; }
            set
            {
                this.captionEnabled = value;
                this.UpdateClientMargins();
            }
        }

        private bool borderEnabled;

        public virtual bool BorderEnabled
        {
            get { return this.borderEnabled; }
            set
            {
                this.borderEnabled = value;
                this.UpdateClientMargins();
            }
        }

        protected override void UpdateClientMargins()
        {
            var layout = this.Design.Layout;

            if (this.BorderEnabled
                && this.CaptionEnabled)
            {
                this.ClientMargins = new MMControlMargins(layout.ClientMargins.Left, this.layerWindowCaption.Height, layout.ClientMargins.Right, layout.ClientMargins.Bottom);
            }
            else if (this.BorderEnabled
                     && !this.CaptionEnabled)
            {
                this.ClientMargins = new MMControlMargins(layout.ClientMargins.Left, layout.ClientMargins.Top, layout.ClientMargins.Right, layout.ClientMargins.Bottom);
            }
            else // (!this.BorderEnabled)
            {
                this.ClientMargins = new MMControlMargins(0, 0, 0, 0);
            }

            // TODO: move it elsewhere
            if (this.buttonClose != null)
            {
                this.buttonClose.UpdateEnabled = this.buttonClose.UpdateEnabled && this.CaptionEnabled;
            }

            base.UpdateClientMargins();
        }

        #endregion

        #endregion

        private MMButton buttonClose;

        public override void OnEnter()
        {
            this.buttonClose = new MMButton();

            this.Add(this.buttonClose);

            base.OnEnter();
        }

        protected override void OnDesignLayer()
        {
            this.layerWindowCaption = this.Skin.Layers[this.layerWindowCaptionString];
            this.layerButtonClose = this.Skin.Layers[this.layerWindowCloseButtonString];
            this.layerButtonClose = this.Skin.Layers[this.]
        }

        protected override void OnLayout()
        {
            this.buttonClose.Margins = new MMControlMargins(
                this.Bounds.Width - 0, // this.buttonClose.Bounds.Width,
                0, 0, this.Bounds.Height - this.buttonClose.Bounds.Height
                );

            base.OnLayout();
        }

        #region Draw and Update

        protected override void DrawInternal(GameTime time)
        {
            base.DrawInternal(time);
        }

        #endregion
    }
}
