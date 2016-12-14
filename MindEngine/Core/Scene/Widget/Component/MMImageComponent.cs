namespace MindEngine.Core.Scene.Widget.Component
{
    using Content.Texture;
    using Control;
    using Element;
    using Element.Style;
    using Microsoft.Xna.Framework;

    public class MMImageComponent : MMControlComponent
    {
        public new class MMElementRegistration : MMControlComponent.MMElementRegistration
        {
            public string Image = "DefaultImage";
        }

        public MMImageComponent(MMControl controlParent) 
            : base(controlParent)
        {
            
        }

        public MMImageComponent(MMControl controlParent, MMElementRegistration elementRegistration) 
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

        protected MMImageStyle Element_ImageStyle { get; set; }

        protected MMImage Element_Image { get; private set; }

        #endregion

        #region Component

        protected override void OnInitElement()
        {
            base.OnInitElement();

            // You could load layer manually 
            if (this.Element_ImageStyle == null)
            {
                this.Element_ImageStyle = (MMImageStyle)this.ControlParent.ControlSkin.ElementStyles[this.ElementRegistration.Image];
            }

            this.Element_Image = this.EngineInterop.Asset.Texture[this.Element_ImageStyle.ImageProperty.ImageName];
        }

        protected override void OnInitComponent()
        {
            base.OnInitComponent();
        }

        #endregion

        #region Draw

        protected override void DrawInternal(GameTime time)
        {
            base.DrawInternal(time);
        }

        protected override void DrawComponent(GameTime time)
        {
            base.DrawComponent(time);

            if (this.Element_ImageStyle.ImageProperty != null)
            {
                this.EngineRenderer.Draw(
                    this.Element_Image.Resource,
                    this.ComponentBounds(this.ControlBounds, this.Element_ImageStyle),
                    this.Element_ImageStyle.ImageProperty.ImageBaseColor, 0f);
            }
        }

        protected override void DrawComponentProperty(GameTime time)
        {
            base.DrawComponentProperty(time);

            if (this.Engine.Debug.Graphics_WidgetPrimitiveEnabled)
            {
                this.EngineRenderer.DrawMonospacedString(
                    this.Element_DefaultTextFont,
                    $"Image = {this.Element_ImageStyle.ImageProperty.ImageName}\n" +
                    $"Opacity = {this.Element_ImageStyle.ImageProperty.ImageOpacity}\n" +
                    $"Color = {this.Element_ImageStyle.ImageProperty.ImageColor(this.Element_ImageStyle.ImageProperty.ImageOpacity)}",
                    this.ComponentPosition(this.ControlBounds, this.Element_DefaultTextStyle),
                    this.Element_DefaultTextStyle.TextProperty.FontBaseColor,
                    this.Element_DefaultTextStyle.TextProperty.FontBaseSize,
                    this.Element_DefaultTextStyle.TextProperty.TextHorizontalAlignment,
                    this.Element_DefaultTextStyle.TextProperty.TextVerticalAlignment,
                    this.Element_DefaultTextStyle.TextProperty.FontBaseLeading);
            }
        }

        #endregion
    }
}