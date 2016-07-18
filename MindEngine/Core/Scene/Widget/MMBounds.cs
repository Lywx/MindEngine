namespace MindEngine.Core.Scene.Widget
{
    using System;
    using Math.Geometry;
    using Microsoft.Xna.Framework;

    public interface IMMBounds : IMMShape
    {
    }

    /// <remarks>
    ///     The reason this class inherits from MMInputEntity is that this class
    /// </remarks>
    public class MMBounds : IMMBounds
    {
        public MMBounds(int x, int y, int width, int height)
        {
            this.rectangle = new Rectangle(x, y, width, height);
        }

        #region Events

        public event EventHandler<MMShapeBoundChangedEventArgs> Move = delegate {};

        public event EventHandler<MMShapeBoundChangedEventArgs> Resize = delegate {};

        #endregion

        #region Shape Event On

        protected virtual void OnShapeMove(Rectangle boundPrevious, Rectangle boundCurrent)
        {
            this.Move?.Invoke(this, new MMShapeBoundChangedEventArgs(MMShapeEvent.Shape_Move, boundPrevious, boundCurrent));
        }

        protected virtual void OnShapeResize(Rectangle boundPrevious, Rectangle boundCurrent)
        {
            this.Resize?.Invoke(this, new MMShapeBoundChangedEventArgs(MMShapeEvent.Shape_Resize, boundPrevious, boundCurrent));
        }

        #endregion

        #region Shape

        /// <remarks>
        ///     Initialized to have a size of a pixel. Location is outside the screen.
        /// </remarks>
        private Rectangle rectangle;

        public Rectangle Rectangle
        {
            get { return this.rectangle; }

            set
            {
                var deltaLocation = (value.Location - this.rectangle.Location).ToVector2();
                var deltaSize = (value.Size - this.rectangle.Size).ToVector2();

                var hasMoved = deltaLocation.Length() > 0f;
                var hasResized = deltaSize.Length() > 0;

                if (hasMoved)
                {
                    this.OnShapeMove(this.rectangle, value);
                }

                if (hasResized)
                {
                    this.OnShapeResize(this.rectangle, value);
                }

                this.rectangle = value;
            }
        }

        public virtual int Bottom
        {
            get { return this.Rectangle.Bottom; }
            set { this.Rectangle = new Rectangle(this.X, value - this.Height, this.Width, this.Height); }
        }

        public virtual int Left
        {
            get { return this.Rectangle.Left; }
            set { this.X = value; }
        }

        public virtual int Right
        {
            get { return this.Rectangle.Right; }
            set { this.Rectangle = new Rectangle(value - this.Width, this.Y, this.Width, this.Height); }
        }

        public virtual int Top
        {
            get { return this.Rectangle.Top; }
            set { this.Y = value; }
        }

        public virtual Point Center
        {
            get { return this.Rectangle.Center; }
            set { this.Rectangle = new Rectangle(new Point(value.X - this.Size.X / 2, value.Y - this.Size.Y / 2), this.Size); }
        }

        public virtual int Height
        {
            get { return this.Rectangle.Height; }
            set { this.Rectangle = new Rectangle(this.X, this.Y, this.Width, value); }
        }

        public virtual Point Position
        {
            get { return this.Rectangle.Location; }
            set { this.Rectangle = new Rectangle(value.X, value.Y, this.Width, this.Height); }
        }

        public virtual Point Size
        {
            get { return new Point(this.Width, this.Height); }
            set { this.Rectangle = new Rectangle(this.X, this.Y, value.X, value.Y); }
        }

        public virtual int Width
        {
            get { return this.Rectangle.Width; }
            set { this.Rectangle = new Rectangle(this.X, this.Y, value, this.Height); }
        }

        public virtual int X
        {
            get { return this.Rectangle.X; }
            set { this.Rectangle = new Rectangle(value, this.Y, this.Width, this.Height); }
        }

        public virtual int Y
        {
            get { return this.Rectangle.Y; }
            set { this.Rectangle = new Rectangle(this.X, value, this.Width, this.Height); }
        }

        #endregion

        #region IDisposable

        private bool IsDisposed { get; set; }

        public void Dispose()
        {
            this.Dispose(true);

            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            try
            {
                if (disposing)
                {
                    if (!this.IsDisposed)
                    {
                        this.DisposeEvents();
                    }

                    this.IsDisposed = true;
                }
            }
            catch
            {
                // Ignored
            }
            finally {}
        }

        private void DisposeEvents()
        {
            this.DisposeShapeChangeEvents();
        }

        private void DisposeShapeChangeEvents()
        {
            this.Move = null;
            this.Resize = null;
        }

        #endregion
    }
}
