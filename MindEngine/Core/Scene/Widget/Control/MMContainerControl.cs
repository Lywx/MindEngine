namespace MindEngine.Core.Scene.Widget.Control
{
    using Composition;
    using Geometry;
    using Property;

    public interface IMMContainerControl : IMMControl
    {
        void AddControl(MMControl child, bool clientArea);
    }

    public class MMContainerControl : MMControl, IMMContainerControl
    {
        public new class MMComponentRegistration : MMControl.MMComponentRegistration
        {

        }

        public new class MMControlRegistration : MMControl.MMControlRegistration
        {
            
        }

        protected MMContainerControl()
        {
        }

        #region Control

        protected MMControlComposition ControlComposition { get; set; }

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

        public virtual void AddControl(MMControl child, bool clientArea)
        {
            if (clientArea)
            {
                this.ControlClientArea.AttachControl(child);
            }
            else
            {
                base.AttachControl(child);
            }
        }

        public override void RemoveControl(MMControl child)
        {
            base.RemoveControl(child);
            this.ControlClientArea.RemoveControl(child);
        }

        protected override void OnInitControl()
        {
            this.ControlClientArea = new MMClipControl();
            this.ControlClientArea.ControlStyle = this.ControlSkin.ControlStyles[""].As;
            base.AttachControl(this.ControlClientArea);

            base.OnInitControl();
        }

        protected override void OnInitComponent()
        {
            base.OnInitComponent();
        }

        #endregion

        public override MMMargins ControlClientMargins
        {
            get
            {
                return base.ControlClientMargins;
            }
            set
            {
                base.ControlClientMargins = value;
                
                if (this.ControlClientArea != null)
                {
                    this.ControlClientArea.ControlMargins = new MMMargins(
                        this.ControlClientMargins.Left,
                        this.ControlClientMargins.Top,
                        this.ControlClientMargins.Right,
                        this.ControlClientMargins.Bottom);
                }
            }
        }

        protected virtual void UpdateClientMargins()
        {
        }
    }
}