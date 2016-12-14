namespace MindEngine.Core.Scene.Widget.Control
{
    using Component;

    public class MMButtonControl : MMControl
    {
        public new class MMControlRegistration : MMControl.MMControlRegistration
        {
        }

        public new class MMComponentRegistration : MMControl.MMComponentRegistration
        {
            public virtual string Image { get; set; } = "Button.Image";
        }

        public MMButtonControl()
        {
            
        }

        #region Entity

        public override string EntityClass => "Button";

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


        #endregion

        #region Component

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

        protected override void OnInitComponent()
        {
            base.OnInitComponent();

            this.ImageComponent = new MMImageComponent(this, (MMImageComponent.MMElementRegistration)[this.ComponentRegistration.Image]);

            this.ControlComponents.Add(this.ImageComponent);
        }

        public MMImageComponent ImageComponent { get; set; }
    }
}
