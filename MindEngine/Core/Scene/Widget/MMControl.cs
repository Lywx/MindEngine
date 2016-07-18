namespace MindEngine.Core.Scene.Widget
{
    using System;
    using System.Diagnostics;
    using Content.Widget;
    using Entity;
    using Microsoft.Xna.Framework;
    using Node;

    public class MMControl : MMNode
    {
        #region Constructors and Finalizer

        protected MMControl()
        {
        }

        #endregion

        #region Entity

        public override string EntityClass => "Control";

        #endregion

        #region Node

        public MMControl RootControl => base.Root as MMControl;

        public MMControl ParentControl
        {
            get { return (MMControl)this.Parent; }
            set { this.Parent = value; }
        }

        public virtual void Add(MMControl child)
        {
            base.Add(child);

            child.UpdateMargins();
        }

        protected override void EndAdd(MMNode child)
        {
            base.EndAdd(child);
            var c = (MMControl)child;
            c.Manager = this.Manager;
        }

        public virtual void Remove(MMControl child)
        {
            base.Remove(child, false);
        }

        public override void OnEnter()
        {
            base.OnEnter();

            // This order allows parent to override children design: you have to 
            // initialize children first
            this.OnDesign();
            this.OnLayout();
        }

        #endregion

        #region Widget

        public MMControlManager Manager { get; set; }

        public virtual string Text { get; set; }

        public MMControlSkin Skin => this.Manager.Skin;

        protected internal virtual void OnSkinChanged(object sender, EventArgs e)
        {
            // TODO
            this.DesignChanged?.Invoke(this, e);
        }

        #endregion

        #region Design

        public MMControlDesign Design { get; set; }

        public event EventHandler DesignChanged = delegate {};

        /// <summary>
        /// You should not call this method manually, except in the OnEnter method.
        /// </summary>
        protected virtual void OnDesign()
        {
            if (this.Manager == null)
            {
                this.Manager = MMControlDefault.Manager;
            }

            // The design is referencing the global manager skins. If the client
            // code would like to change some of design in the runtime, they 
            // need to copy
            this.Design = this.Manager.Skin.Designs[this.EntityClass];

            this.OnDesignLayer();
        }

        public MMDrawEntityCollection<MMControlLayer> Layers { get; set; } = new MMDrawEntityCollection<MMControlLayer>();

        /// <summary>
        /// You should not call this method manually, except in the OnDesign method.
        /// </summary>
        protected virtual void OnDesignLayer()
        {
        }

        /// <summary>
        /// You should not call this method manually, except in the OnEnter method.
        /// </summary>
        protected virtual void OnLayout()
        {
        }

        #endregion

        #region Layout

        private MMBounds bounds = new MMBounds(int.MinValue, int.MinValue, 0, 0);

        public MMBounds Bounds
        {
            get { return this.bounds; }
            set
            {
                // This exception only applies for node which has a parent as 
                // widget: it won't affect to non-widget parent node.
                if (this.RootControl != null)
                {
                    throw new InvalidOperationException("Cannot define bounds for child widget");
                }

                this.bounds = value;
            }
        }

        private MMControlMargins margins = new MMControlMargins(0, 0, 0, 0);

        public MMControlMargins Margins
        {
            get { return this.margins; }
            set
            {
                if (!this.margins.Equals(value))
                {
                    this.margins = value;
                    this.UpdateBounds();
                }
            }
        }

        public MMClipBox ClientArea { get; set; }

        public MMBounds ClientBounds { get; set; }

        public virtual MMControlMargins ClientMargins { get; set; }

        private Rectangle ClippingRectangle(MMControl c)
        {
            var root = c.RootControl;
            var parent = c.ParentControl;

            int x1 = c.Bounds.Left;
            int x2 = c.Bounds.Right;
            int y1 = c.Bounds.Top;
            int y2 = c.Bounds.Bottom;

            while (parent != null)
            {
                int px1 = parent.Bounds.Left;
                int py1 = parent.Bounds.Top;
                int px2 = px1 + parent.Bounds.Width;
                int py2 = py1 + parent.Bounds.Height;

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

                parent = parent.ParentControl;
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

            if (x1 > root.Bounds.Width)
            {
                x1 = root.Bounds.Width;
            }

            if (y1 > root.Bounds.Height)
            {
                y1 = root.Bounds.Height;
            }

            if (dx > root.Bounds.Width)
            {
                dx = root.Bounds.Width;
            }

            if (dy > root.Bounds.Height)
            {
                dy = root.Bounds.Height;
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
            this.Children.Draw<MMControl>((controlParam, timeParam) =>
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
                this.EngineRenderer.DrawRectangle(this.Bounds.Rectangle, Color.Red, 1f);
                this.EngineRenderer.DrawRectangle(this.ClientBounds.Rectangle, Color.DarkRed, 1f);
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
            if (this.Parent != null)
            {
                this.Margins = new MMControlMargins(
                    this.Margins.Left, 
                    this.Margins.Right,
                    this.ParentControl.Bounds.Width - this.Bounds.Width - this.Margins.Left,
                    this.ParentControl.Bounds.Height - this.Bounds.Height - this.Margins.Top);
            }
            else
            {
                this.Margins = new MMControlMargins();
            }
        }

        private void UpdateBounds()
        {
            if (this.RootControl != null)
            {
                this.Bounds.Position = new Point(
                    this.ParentControl.Bounds.X + this.Margins.Left,
                    this.ParentControl.Bounds.Y + this.Margins.Top);
            }
        }

        #endregion
    }
}
