namespace MindEngine.Core.Scene.Entity
{
    using System;
    using System.Runtime.Serialization;
    using Core;
    using Microsoft.Xna.Framework;

    [DataContract]
    public class MMDrawEntity : MMUpdateEntity, IMMDrawableOperations, IComparable<MMDrawEntity>
    {
        #region Constructors

        protected MMDrawEntity()
        {
        }

        ~MMDrawEntity()
        {
            this.Dispose(true);
        }

        #endregion

        #region State Data

        private bool drawEnabled = true;

        public virtual bool DrawEnabled
        {
            get
            {
                return this.drawEnabled;
            }

            set
            {
                var changed = this.drawEnabled != value;
                if (changed)
                {
                    this.drawEnabled = value;
                    this.OnDrawEnabledChanged();
                }
            }
        }

        #endregion

        #region Comparison

        public int CompareTo(MMDrawEntity other)
        {
            return this.DrawOrder.CompareTo(other.DrawOrder);
        }

        #endregion

        #region Draw

        private int drawOrder;

        /// <summary>
        /// Z Order.
        /// </summary>
        public int DrawOrder
        {
            get
            {
                return this.drawOrder;
            }

            set
            {
                var changed = this.drawOrder != value;
                if (changed)
                {
                    this.drawOrder = value;
                    this.OnDrawOrderChanged();
                }
            }
        }

        /// <summary>
        /// Standard draw routine.
        /// </summary>
        /// <remarks>
        /// It is recommended not to call SpriteBatch.Begin and SpriteBatch.End 
        /// in this method and its override version.
        /// </remarks>>
        /// <param name="time"></param>
        public virtual void Draw(GameTime time)
        {
            if (this.DrawEnabled)
            {
                this.DrawInternal(time);
            }
        }

        protected virtual void DrawInternal(GameTime time) { }

        #endregion

        #region Events

        public event EventHandler<MMDrawOrderChangedEventArgs> DrawOrderChanged;

        public event EventHandler<MMDrawEnabledChangedEventArgs> DrawEnabledChanged;

        #endregion

        #region Event Ons

        protected virtual void OnDrawOrderChanged()
        {
            this.DrawOrderChanged?.Invoke(this, new MMDrawOrderChangedEventArgs(this.DrawOrder));
        }

        protected virtual void OnDrawEnabledChanged()
        {
            this.DrawEnabledChanged?.Invoke(this, new MMDrawEnabledChangedEventArgs(this.DrawEnabled));
        }

        #endregion

        #region IDisposable

        private bool IsDisposed { get; set; }

        protected override void Dispose(bool disposing)
        {
            try
            {
                if (disposing)
                {
                    if (!this.IsDisposed)
                    {
                        this.DisposeEventHandlers();
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
                base.Dispose(disposing);
            }
        }

        private void DisposeEventHandlers()
        {
            this.DrawOrderChanged = null;
            this.DrawEnabledChanged = null;
        }

        #endregion
    }
}