namespace MindEngine.Core.Scene.Widget.Component
{
    using Content.Texture;
    using Control;
    using Element;
    using Element.Style;
    using Microsoft.Xna.Framework;

    public class MMCaptionComponent : MMControlComponent
    {
        public new class MMElementRegistration : MMControlComponent.MMElementRegistration
        {
            public string Image { get; set; } = "Window.Caption.Image";

            public string Text { get; set; } = "Window.Caption.Text";
        }

        public MMCaptionComponent(MMControl controlParent) 
            : base(controlParent)
        {
            
        }

        public MMCaptionComponent(MMControl controlParent, MMElementRegistration elementRegistration)
            : base(controlParent, elementRegistration)
        {
            this.ElementRegistration = elementRegistration;
        }

        #region Element

        private MMElementRegistration elementRegistration;

        public new MMElementRegistration ElementRegistration
        {
            get
            {
                if (this.elementRegistration == null)
                {
                    this.elementRegistration = new MMElementRegistration();
                }

                return this.elementRegistration;
            }
            set
            {
                this.elementRegistration = value;
                base.ElementRegistration = value;
            }
        }

        protected MMImage Element_Image { get; set; }

        protected MMImageStyle Element_ImageStyle { get; set; }

        #endregion

        protected override void OnInitComponent()
        {
            base.OnInitComponent();

            if (this.Element_ImageStyle == null)
            {
                this.Element_ImageStyle = (MMImageStyle)this.ControlParent.ControlSkin.ElementStyles[this.ElementRegistration.Image];
            }

            this.Element_Image = EngineInterop.Asset.Texture[this.Element_ImageStyle.ImageProperty.ImageName];
        }

        protected override void DrawComponent(GameTime time)
        {
            base.DrawComponent(time);

            // TODO
        }
    }
}
