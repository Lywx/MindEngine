namespace MindEngine.Core.Scene.Widget.Control
{
    using System;
    using System.Diagnostics;
    using System.Linq;
    using Component;
    using Entity;
    using Geometry;
    using Microsoft.Xna.Framework;
    using Object;
    using Style;

    internal interface IMMControl
    {
        #region Layout

        Rectangle ControlBounds { get; set; }

        MMMargins ControlMargins { get; set; }

        MMClipControl ControlClientArea { get; set; }

        Rectangle ControlClientBounds { get; set; }

        MMMargins ControlClientMargins { get; set; }

        #endregion

        #region Design

        MMControlSkin ControlSkin { get; }

        MMControlStyle ControlStyle { get; set; }

        MMControlComponentList ControlComponents { get; set; }

        MMControlSettings ControlSettings { get; }

        #endregion

        #region Control Hierarchy Semantics 

        MMControlManager ControlManager { get; set; }

        MMControl ControlParent { get; set; }

        MMControl ControlRoot { get; }

        void AttachControl(MMControl child);

        void RemoveControl(MMControl child);

        #endregion
    }

    public class MMControl : MMGameObject, IMMControl
    {
        public class MMControlRegistration {}

        public class MMComponentRegistration {}

        #region Constructors and Finalizer

        protected MMControl()
        {
            
        }

        #endregion

        #region Entity

        public override string EntityClass => "Control";

        #endregion

        #region Node

        public MMControl ControlRoot => base.NodeRoot as MMControl;

        public MMControl ControlParent
        {
            get { return (MMControl)this.NodeParent; }
            set { this.NodeParent = value; }
        }

        public virtual void AttachControl(MMControl child)
        {
            base.AttachNode(child);

            child.UpdateMargins();
        }

        public virtual void RemoveControl(MMControl child)
        {
            base.RemoveNode(child, false);
        }

        protected override void OnAttachNodeEnd(MMEntityNode child)
        {
            base.OnAttachNodeEnd(child);
            var c = (MMControl)child;
            c.ControlManager = this.ControlManager;
        }

        protected override void OnInitScene()
        {
            // Call children to initialize first. This order allows parent to 
            // override children design: you have to initialize children first.
            this.OnInitControl();
            this.OnInitComponent();
            this.OnInitLayout();
        }

        #endregion

        #region Control 

        public MMControlManager ControlManager { get; set; }

        public MMControlSkin ControlSkin => this.ControlManager.Skin;

        public MMControlSettings ControlSettings => this.ControlManager.Settings;

        /// <summary>
        /// Control style this control are referencing.
        /// </summary>
        /// <remarks>
        /// The design is referencing the global manager skins. If the client
        /// code would like to change some of design in the runtime, they 
        /// need to copy the style they want to use.
        /// </remarks>
        public MMControlStyle ControlStyle { get; set; }

        public MMControlComponentList ControlComponents { get; set; } = new MMControlComponentList();

        private MMComponentRegistration componentRegistration;

        public MMComponentRegistration ComponentRegistration
        {
            get
            {
                if (this.componentRegistration == null)
                {
                    this.componentRegistration = new MMComponentRegistration();
                }

                return this.componentRegistration;
            }
            set { this.componentRegistration = value; }
        }

        private MMControlRegistration controlRegistration;

        public MMControlRegistration ControlRegistration
        {
            get
            {
                if (this.controlRegistration == null)
                {
                    this.controlRegistration = new MMControlRegistration();
                }

                return this.controlRegistration;
            }
            set { this.controlRegistration = value; }
        }

        /// <summary>
        /// You should always call base.OnInitControl() after you finished 
        /// adding new child control.
        /// </summary>
        protected virtual void OnInitControl()
        {
            if (this.ControlManager == null)
            {
                this.ControlManager = MMControlDefault.Manager;
            }

            if (this.ControlStyle == null)
            {
                this.ControlStyle = this.ControlManager.Skin.ControlStyles[this.EntityClass];
            }

            foreach (var node in this.NodeChildren.ToArray())
            {
                var control = node as MMControl;
                control?.OnInitControl();
            }
        }

        /// <summary>
        /// You should always call base.OnInitComponent() after you finished 
        /// adding new child component.
        /// </summary>
        protected virtual void OnInitComponent()
        {
            foreach (var node in this.NodeChildren.ToArray())
            {
                var control = node as MMControl;
                control?.OnInitComponent();
            }
        }

        protected virtual void OnInitLayout()
        {
            foreach (var node in this.NodeChildren.ToArray())
            {
                var control = node as MMControl;
                control?.OnInitLayout();
            }
        }

        #endregion

        #region Layout

        private Rectangle controlBounds = new Rectangle(int.MinValue, int.MinValue, 0, 0);

        public Rectangle ControlBounds
        {
            get { return this.controlBounds; }
            set
            {
                // This exception only applies for node which has a parent as 
                // widget: it won't affect to non-widget parent node.
                if (this.ControlRoot != null)
                {
                    throw new InvalidOperationException("Cannot define bounds for child widget");
                }

                this.controlBounds = value;
            }
        }

        private MMMargins controlMargins = new MMMargins(0, 0, 0, 0);

        public MMMargins ControlMargins
        {
            get { return this.controlMargins; }
            set
            {
                if (!this.controlMargins.Equals(value))
                {
                    this.controlMargins = value;
                    this.UpdateBounds();
                }
            }
        }

        public MMClipControl ControlClientArea { get; set; }

        public Rectangle ControlClientBounds { get; set; }

        public virtual MMMargins ControlClientMargins { get; set; }

        private Rectangle ClippingRectangle(MMControl c)
        {
            var root = c.ControlRoot;
            var parent = c.ControlParent;

            int x1 = c.ControlBounds.Left;
            int x2 = c.ControlBounds.Right;
            int y1 = c.ControlBounds.Top;
            int y2 = c.ControlBounds.Bottom;

            while (parent != null)
            {
                int px1 = parent.ControlBounds.Left;
                int py1 = parent.ControlBounds.Top;
                int px2 = px1 + parent.ControlBounds.Width;
                int py2 = py1 + parent.ControlBounds.Height;

                if (x1 < px1)
                {
                    x1 = px1;
                }

                if (y1 < py1)
                {
                    y1 = py1;
                }

                if (x2 > px2)
                {
                    x2 = px2;
                }

                if (y2 > py2)
                {
                    y2 = py2;
                }

                parent = parent.ControlParent;
            }

            int dx = x2 - x1;
            int dy = y2 - y1;

            if (x1 < 0)
            {
                x1 = 0;
            }

            if (y1 < 0)
            {
                y1 = 0;
            }

            if (dx < 0)
            {
                dx = 0;
            }

            if (dy < 0)
            {
                dy = 0;
            }

            if (x1 > root.ControlBounds.Width)
            {
                x1 = root.ControlBounds.Width;
            }

            if (y1 > root.ControlBounds.Height)
            {
                y1 = root.ControlBounds.Height;
            }

            if (dx > root.ControlBounds.Width)
            {
                dx = root.ControlBounds.Width;
            }

            if (dy > root.ControlBounds.Height)
            {
                dy = root.ControlBounds.Height;
            }

            return new Rectangle(x1, y1, dx, dy);
        }

        private void SetClippingRectangle(MMControl control)
        {
            this.EngineGraphicsDeviceController.ScissorRectangleEnabled = true;
            this.EngineGraphicsDeviceController.ScissorRectangle = this.ClippingRectangle(control);

            this.ClippingRectangleDebug(control);
        }

        [Conditional("DEBUG")]
        private void ClippingRectangleDebug(MMControl control)
        {
            if (this.EngineDebug.Graphics_WidgetClippingDisabled)
            {
                this.EngineGraphicsDeviceController.ScissorRectangleEnabled = false;
            }
        }

        #endregion

        #region Draw 

        protected override void DrawInternal(GameTime time)
        {
            // The organization of the method is highly dependent on the rule 
            // that each control would render itself in one pair of begin / end
            // semantics. The structure of the method just establish that rule
            // as a primary rationale behind.
            //
            // The reason behind this organization is for rendering each control
            // with individual scissor test. The scissor test is against the 
            // bounds of the control.

            this.EngineRenderer.Begin();
            this.DrawControl(time);
            this.EngineRenderer.End();

            this.DrawChildrenControl(time);
        }

        protected virtual void DrawControl(GameTime time)
        {
            this.DrawControlPrimitive(time);
            this.DrawControlProperty(time);

            this.DrawControlLayer(time);
        }

        private void DrawChildrenControl(GameTime time)
        {
            this.NodeChildren.Draw<MMControl>((controlParam, timeParam) =>
            {
                this.SetClippingRectangle(controlParam);

                this.EngineRenderer.Begin();
                controlParam.DrawControl(time);
                this.EngineRenderer.End();

                controlParam.DrawChildrenControl(time);
            }, time);
        }

        protected virtual void DrawControlLayer(GameTime time)
        {
        }

        #endregion

        #region Draw Debug

        [Conditional("DEBUG")]
        protected virtual void DrawControlPrimitive(GameTime time)
        {
            if (this.EngineDebug.Graphics_WidgetPrimitiveEnabled)
            {
                this.EngineRenderer.DrawRectangle(this.ControlBounds, Color.Red, 1f);
                this.EngineRenderer.DrawRectangle(this.ControlClientBounds, Color.DarkRed, 1f);
            }
        }

        [Conditional("DEBUG")]
        protected virtual void DrawControlProperty(GameTime time)
        {
            
        }

        #endregion

        #region Update

        protected override void UpdateInternal(GameTime time)
        {
            base.UpdateInternal(time);
        }

        private void UpdateMargins()
        {
            if (this.ControlParent != null)
            {
                this.ControlMargins = new MMMargins(
                    this.ControlMargins.Left, 
                    this.ControlMargins.Right,
                    this.ControlParent.ControlBounds.Width - this.ControlBounds.Width - this.ControlMargins.Left,
                    this.ControlParent.ControlBounds.Height - this.ControlBounds.Height - this.ControlMargins.Top);
            }
            else
            {
                this.ControlMargins = new MMMargins();
            }
        }

        private void UpdateBounds()
        {
            if (this.ControlRoot != null)
            {
                this.ControlBounds = new Rectangle(
                    this.ControlParent.ControlBounds.X + this.ControlMargins.Left,
                    this.ControlParent.ControlBounds.Y + this.ControlMargins.Top,
                    this.ControlBounds.Width, 
                    this.ControlBounds.Height);
            }
        }

        #endregion
    }
}
