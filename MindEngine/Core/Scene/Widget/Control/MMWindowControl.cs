namespace MindEngine.Core.Scene.Widget.Control
{
    using Component;
    using Geometry;
    using Microsoft.Xna.Framework;

    public class MMWindowControl : MMModalControl
    {
        public new class MMComponentRegistration : MMModalControl.MMComponentRegistration
        {
            public virtual string Caption           { get; set; } = "Window.Caption";

            public virtual string CaptionIcon       { get; set; } = "Window.Caption.Icon";

            public virtual string Shadow            { get; set; } = "Window.Shadow";

            public virtual string FrameTop          { get; set; } = "Window.Frame.Top";

            public virtual string FrameLeft         { get; set; } = "Window.Frame.Left";

            public virtual string FrameRight        { get; set; } = "Window.Frame.Right";

            public virtual string FrameBottom       { get; set; } = "Window.Frame.Bottom";

            public virtual string ClientFrameTop    { get; set; } = "Window.Client.Frame.Top";

            public virtual string ClientFrameLeft   { get; set; } = "Window.Client.Frame.Left";

            public virtual string ClientFrameRight  { get; set; } = "Window.Client.Frame.Right";

            public virtual string ClientFrameBottom { get; set; } = "Window.Client.Frame.Bottom";
        }

        public new class MMControlRegistration : MMModalControl.MMControlRegistration
        {
            public virtual string CaptionButtonClose { get; set; } = "Window.Caption.Button.Close";
        }

        private class MMControlRegistration_CaptionButtonClose : MMButtonControl.MMControlRegistration
        {
            
        }

        private class MMComponentRegistration_CaptionButtonClose : MMButtonControl.MMComponentRegistration
        {
            public override string Image { get; set; } = "Window.Caption.Button.Close.Image";
        }

        protected MMWindowControl(MMControlRegistration controlRegistration, MMComponentRegistration componentRegistration)
            : base(controlRegistration, componentRegistration)
        {

        }

        #region Entity

        public override string EntityClass => "Window";

        #endregion

        #region Component

        protected MMCaptionComponent CaptionComponent { get; set; }

        #endregion

        #region Control

        private MMControlRegistration controlRegistration;

        public new MMControlRegistration ControlRegistration
        {
            get
            {
                if (this.controlRegistration == null)
                {
                    this.controlRegistration = new MMControlRegistration();
                }

                return this.controlRegistration;
            }
            set
            {
                this.controlRegistration = value;
                base.ControlRegistration = value;
            }
        }

        protected MMButtonControl CaptionButtonClose { get; set; }

        #endregion

        #region Element

        private MMComponentRegistration componentRegistration;

        public new MMComponentRegistration ComponentRegistration
        {
            get
            {
                if (this.componentRegistration == null)
                {
                    this.componentRegistration = new MMComponentRegistration();
                }

                return this.componentRegistration;
            }
            set
            {
                this.componentRegistration = value;
                base.ComponentRegistration = value;
            }
        }

        #endregion

        protected override void OnInitControl()
        {
            base.OnInitControl();

            this.CaptionButtonClose = new MMButtonControl(
                new MMControlRegistration_CaptionButtonClose(), 
                new MMComponentRegistration_CaptionButtonClose());
            this.CaptionButtonClose.ControlStyle = this.ControlSkin.ControlStyles[this.ControlRegistration.CaptionButtonClose];

                //new MMButtonControl.MMControlRegistration() {}, 
                //new MMButtonControl.MMComponentRegistration()
                //{
                //    Image = this.ControlRegistration.CaptionButtonClose
                //}
            this.AddControl(this.CaptionButtonClose);
        }

        protected override void OnInitComponent()
        {
            this.CaptionComponent = new MMCaptionComponent(this);
            this.CaptionComponent.ElementRegistration = new MMCaptionComponent.MMElementRegistration
            {
                Image = this.ComponentRegistration.Caption

            };

            this.ControlComponents.Add(this.CaptionComponent);
        }

        protected override void OnInitLayout()
        {
            this.CaptionButtonClose.ControlMargins = new MMMargins(
                this.ControlBounds.Width - this.CaptionButtonClose.Bounds.Width,
                0, 0, this.ControlBounds.Height - this.CaptionButtonClose.ControlBounds.Height
                );

            base.OnInitLayout();
        }

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
            var layout = this.ControlStyle.LayoutProperty;

            if (this.BorderEnabled
                && this.CaptionEnabled)
            {
                this.ControlClientMargins = new MMMargins(layout.ClientMargins.Left, this.CaptionComponent.Height, layout.ClientMargins.Right, layout.ClientMargins.Bottom);
            }
            else if (this.BorderEnabled
                     && !this.CaptionEnabled)
            {
                this.ControlClientMargins = new MMMargins(layout.ClientMargins.Left, layout.ClientMargins.Top, layout.ClientMargins.Right, layout.ClientMargins.Bottom);
            }
            else // (!this.BorderEnabled)
            {
                this.ControlClientMargins = new MMMargins(0, 0, 0, 0);
            }

            // TODO: move it elsewhere
            if (this.CaptionButtonClose != null)
            {
                this.CaptionButtonClose.UpdateEnabled = this.CaptionButtonClose.UpdateEnabled && this.CaptionEnabled;
            }

            base.UpdateClientMargins();
        }

        #endregion

        #region Draw and Update

        protected override void DrawInternal(GameTime time)
        {
            base.DrawInternal(time);
        }

        #endregion
    }
}
