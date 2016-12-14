namespace MindEngine.Core.Scene.Widget.Component
{
    using System;
    using System.Diagnostics;
    using Content.Font;
    using Control;
    using Element;
    using Element.Style;
    using Entity;
    using Microsoft.Xna.Framework;

    /// <summary>
    /// Control component usually doesn't have a input relative function. But 
    /// it still provides a update function for customized update and transition.
    /// </summary>
    /// <remarks>
    /// Control component doesn't allow children components.
    /// </remarks>
    public class MMControlComponent : MMEntityNode
    {
        public class MMElementRegistration
        {
            public string DebugText      = "Debug.Text";

            public string DebugPrimitive = "Debug.Primitive";
        }

        protected MMControlComponent(MMControl controlParent)
        {
            if (controlParent == null)
            {
                throw new ArgumentNullException(nameof(controlParent));
            }

            this.ControlParent = controlParent;
        }

        protected MMControlComponent(MMControl controlParent, MMElementRegistration elementRegistration) 
            : this(controlParent)
        {
            if (elementRegistration == null)
            {
                throw new ArgumentNullException(nameof(elementRegistration));
            }

            this.ElementRegistration = elementRegistration;
        }

        #region Node

        protected override void OnInitScene()
        {
            // Control component doesn't allow children components.
            this.OnInitComponent();
            this.OnInitElement();
        }

        protected virtual void OnInitElement()
        {
            this.Element_DefaultTextStyle = (MMTextStyle)this.ControlParent.ControlSkin.ElementStyles[this.ElementRegistration.DebugText];
            this.Element_DefaultTextFont = this.EngineInterop.Asset.Fonts[this.Element_DefaultTextStyle.TextProperty.FontName];

            this.Element_DefaultPrimitiveStyle =  (MMPrimitiveStyle)this.ControlParent.ControlSkin.ElementStyles[this.ElementRegistration.DebugPrimitive];
        }

        protected virtual void OnInitComponent()
        {
        }

        #endregion

        #region Control

        public MMControl ControlParent { get; set; }

        public Rectangle ControlBounds => this.ControlParent.ControlBounds;

        #endregion

        #region Component

        public Rectangle ComponentBounds(Rectangle controlBounds, MMElementStyle controlElement)
        {
            controlBounds.Offset(controlElement.Offset);
            controlBounds.Size = controlElement.Size;
            return controlBounds;
        }

        public Vector2 ComponentPosition(Rectangle controlBounds, MMElementStyle controlElement)
        {
            controlBounds.Offset(controlElement.Offset);
            return controlBounds.Location.ToVector2();
        }

        #endregion

        #region Element

        private MMElementRegistration elementRegistration;

        public MMElementRegistration ElementRegistration
        {
            get
            {
                if (this.elementRegistration == null)
                {
                    this.elementRegistration = new MMElementRegistration();
                }

                return this.elementRegistration;
            }

            set { this.elementRegistration = value; }
        }

        protected MMFont Element_DefaultTextFont { get; private set; }

        protected MMTextStyle Element_DefaultTextStyle { get; private set; }

        protected MMPrimitiveStyle Element_DefaultPrimitiveStyle { get; private set; }

        #endregion

        #region Draw

        protected override void DrawInternal(GameTime time)
        {
            this.DrawComponentPrimitive(time);
            this.DrawComponent(time);
            this.DrawComponentProperty(time);
        }

        protected virtual void DrawComponent(GameTime time) {}

        [Conditional("DEBUG")]
        protected virtual void DrawComponentPrimitive(GameTime time)
        {
            if (this.EngineDebug.Graphics_WidgetPrimitiveEnabled)
            {
                this.EngineRenderer.DrawRectangle(
                    this.ComponentBounds(this.ControlBounds, 
                    this.Element_DefaultPrimitiveStyle), 
                    this.Element_DefaultPrimitiveStyle.LineColor, 
                    this.Element_DefaultPrimitiveStyle.LineThick);
            }
        }

        [Conditional("DEBUG")]
        protected virtual void DrawComponentProperty(GameTime time)
        {
        }

        #endregion

        #region Update

        protected override void UpdateInternal(GameTime time)
        {
            base.UpdateInternal(time);
        }

        #endregion
    }
}
