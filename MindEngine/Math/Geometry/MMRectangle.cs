namespace MindEngine.Math.Geometry
{
    using System;
    using Core;
    using Core.Scenes;
    using Microsoft.Xna.Framework;

    /// <remarks>
    ///     The reason this class inherits from MMInputEntity is that this class
    /// </remarks>
    public class MMRectangle : MMObject, IMMRectangle
    {
        #region Events

        public event EventHandler<MMShapeBoundChangedEventArgs> Move = delegate {};

        public event EventHandler<MMShapeBoundChangedEventArgs> Resize = delegate {};

        #endregion

        #region Event On

        protected virtual void OnShapeMove(Rectangle boundPrevious, Rectangle boundCurrent)
        {
            this.Move?.Invoke(this, new MMShapeBoundChangedEventArgs(MMShapeEvent.Shape_Move, boundPrevious, boundCurrent));
        }

        protected virtual void OnShapeResize(Rectangle boundPrevious, Rectangle boundCurrent)
        {
            this.Resize?.Invoke(this, new MMShapeBoundChangedEventArgs(MMShapeEvent.Shape_Resize, boundPrevious, boundCurrent));
        }

        #endregion

        #region Element Geometry Data

        /// <remarks>
        ///     Initialized to have a size of a pixel. Location is outside the screen.
        /// </remarks>
        private Rectangle bound = new Rectangle(
            int.MinValue,
            int.MinValue,
            0,
            0);

        public virtual Rectangle Bound
        {
            get { return this.bound; }

            set
            {
                var deltaLocation = (value.Location - this.bound.Location).ToVector2();
                var deltaSize = (value.Size - this.bound.Size).ToVector2();

                var hasMoved = deltaLocation.Length() > 0f;
                var hasResized = deltaSize.Length() > 0;

                if (hasMoved)
                {
                    this.OnShapeMove(this.bound, value);
                }

                if (hasResized)
                {
                    this.OnShapeResize(this.bound, value);
                }

                this.bound = value;
            }
        }

        public virtual Point Center
        {
            get { return this.Bound.Center; }
            set { this.Bound = new Rectangle(value, this.Size); }
        }

        public virtual int Height
        {
            get { return this.Bound.Height; }
            set
            {
                this.Bound = new Rectangle(
                    this.Bound.X,
                    this.Bound.Y,
                    this.Bound.Width,
                    value);
            }
        }

        public virtual Point Position
        {
            get { return this.Bound.Location; }
            set
            {
                this.Bound = new Rectangle(
                    value.X,
                    value.Y,
                    this.Bound.Width,
                    this.Bound.Height);
            }
        }

        public virtual Point Size
        {
            get { return new Point(this.Bound.Width, this.Bound.Height); }
            set
            {
                this.Bound = new Rectangle(
                    this.Center,
                    value);
            }
        }

        public virtual int Width
        {
            get { return this.Bound.Width; }
            set
            {
                this.Bound = new Rectangle(
                    this.Bound.X,
                    this.Bound.Y,
                    value,
                    this.Bound.Height);
            }
        }

        public virtual int X
        {
            get { return this.Bound.X; }
            set
            {
                this.Bound = new Rectangle(
                    value,
                    this.Bound.Y,
                    this.Bound.Width,
                    this.Bound.Height);
            }
        }

        public virtual int Y
        {
            get { return this.Bound.Y; }
            set
            {
                this.Bound = new Rectangle(
                    this.Bound.X,
                    value,
                    this.Bound.Width,
                    this.Bound.Height);
            }
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
            finally
            {
            }
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
