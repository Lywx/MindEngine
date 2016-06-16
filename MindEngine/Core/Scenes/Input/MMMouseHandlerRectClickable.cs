namespace MindEngine.Core.Scenes.Input
{
    using System;
    using Math.Geometry;
    using Microsoft.Xna.Framework;
    using MindEngine.Input.Mouse;
    using MindEngine.Input.Mouse.EventArgs;
    using Mouse;

    public class MMMouseHandlerRectClickable : MMRectangle, IMMMouseClickable, IMMMouseDoubleClickable
    {
        #region Constructors and Finalizer

        public MMMouseHandlerRectClickable()
        {
            this.SetupEventHandlers();
        }

        ~MMMouseHandlerRectClickable()
        {
            this.Dispose(true);
        }

        #endregion

        #region Element State Data

        private bool mouseEnabled;

        /// <summary>
        ///     Gets or sets a value indicating whether this should receive or send any events.
        /// </summary>
        public virtual bool MouseEnabled
        {
            get { return mouseEnabled; }
            set
            {
                var changed = this.mouseEnabled != value;
                if (changed)
                {
                    // This is used to deduce event performance overhead on
                    // an individual frame.
                    if (value)
                    {
                        this.SetupEventHandlers();
                    }
                    else
                    {
                        this.DisposeEventHandlers();
                    }
                }

                mouseEnabled = value;
            }
        }

        #endregion

        #region Update

        public virtual void Update(GameTime time)
        {
            if (this.MouseEnabled)
            {
                this.MouseCacher.FlushInput();
                this.MouseCacher.ClearInput();
            }
        }

        #endregion

        #region Events

        public event EventHandler<EventArgs> MouseEnter = delegate {};

        public event EventHandler<EventArgs> MouseLeave = delegate {};

        /// <summary>
        ///     When mouse is pressed inside frame.
        /// </summary>
        public event EventHandler<EventArgs> MousePress = delegate {};

        public event EventHandler<EventArgs> MousePressLeft = delegate {};

        public event EventHandler<EventArgs> MousePressRight = delegate {};

        /// <summary>
        ///     When mouse is pressed outside frame.
        /// </summary>
        public event EventHandler<EventArgs> MousePressOut = delegate {};

        public event EventHandler<EventArgs> MousePressOutLeft = delegate {};

        public event EventHandler<EventArgs> MousePressOutRight = delegate {};

        /// <summary>
        ///     When mouse is released inside frame.
        /// </summary>
        public event EventHandler<EventArgs> MouseUp = delegate {};

        public event EventHandler<EventArgs> MouseUpLeft = delegate {};

        public event EventHandler<EventArgs> MouseUpRight = delegate {};

        /// <summary>
        ///     When mouse is released outside frame.
        /// </summary>
        public event EventHandler<EventArgs> MouseUpOut = delegate {};

        public event EventHandler<EventArgs> MouseUpOutLeft = delegate {};

        public event EventHandler<EventArgs> MouseUpOutRight = delegate {};

        public event EventHandler<EventArgs> MouseDoubleClick = delegate {};

        public event EventHandler<EventArgs> MouseDoubleClickLeft = delegate {};

        public event EventHandler<EventArgs> MouseDoubleClickRight = delegate {};

        #endregion

        #region Event Handlers

        private void EventBoundChanged(object sender, EventArgs e)
        {
            var mousePosition = this.EngineInput.State.Mouse.Position;

            // Simulate event argument and call event handler method manually
            var mouseEventArgs = new MMMouseEventArgs(MMMouseButtons.None, 0, (int)mousePosition.X, (int)mousePosition.Y, 0);
            this.EventMouseMove(this, mouseEventArgs);
        }

        private void EventMouseDoubleClick(object sender, MMMouseEventArgs e)
        {
            if (this.MouseState.MouseOver)
            {
                if (this.MouseLButton(e))
                {
                    this.MouseState.LDoubleClick();
                    this.MouseState.RClear();

                    if (this.MouseDoubleClickEnabled)
                    {
                        this.MouseCacher.CacheInput(this.OnMouseDoubleClickLeft);
                    }
                }
                if (this.MouseRButton(e))
                {
                    this.MouseState.LClear();
                    this.MouseState.RDoubleClick();

                    if (this.MouseDoubleClickEnabled)
                    {
                        this.MouseCacher.CacheInput(this.OnMouseDoubleClickRight);
                    }
                }
            }
        }

        private void EventMouseDown(object sender, MMMouseEventArgs e)
        {
            if (this.MouseLButton(e))
            {
                this.MouseState.LPress();
                this.MouseState.RClear();

                if (this.MouseState.MouseOver)
                {
                    this.MouseCacher.CacheInput(this.OnMousePressLeft);
                }

                if (!this.MouseState.MouseOver)
                {
                    this.MouseCacher.CacheInput(this.OnMousePressOutLeft);
                }
            }
            else if (this.MouseRButton(e))
            {
                this.MouseState.RPress();
                this.MouseState.LClear();

                if (this.MouseState.MouseOver)
                {
                    this.MouseCacher.CacheInput(this.OnMousePressRight);
                }

                if (!this.MouseState.MouseOver)
                {
                    this.MouseCacher.CacheInput(this.OnMousePressOutRight);
                }
            }
        }

        private void EventMouseMove(object sender, MMMouseEventArgs e)
        {
            if (!this.MouseState.MouseOver
                && this.MouseOverPos(e.Location))
            {
                this.MouseState.Enter();

                this.MouseCacher.CacheInput(this.OnMouseEnter);

                return;
            }

            if (this.MouseState.MouseOver
                && !this.MouseOverPos(e.Location))
            {
                this.MouseState.Leave();

                this.MouseCacher.CacheInput(this.OnMouseLeave);
            }
        }

        private void EventMouseUp(object sender, MMMouseEventArgs e)
        {
            if (this.MouseLButton(e))
            {
                this.MouseState.LRelease();

                if (this.MouseState.MouseOver)
                {
                    this.MouseCacher.CacheInput(this.OnMouseUpLeft);
                }
                else
                {
                    this.MouseCacher.CacheInput(this.OnMouseUpOutLeft);
                }
            }
            else if (this.MouseRButton(e))
            {
                this.MouseState.RRelease();

                if (this.MouseState.MouseOver)
                {
                    this.MouseCacher.CacheInput(this.OnMouseUpRight);
                }
                else
                {
                    this.MouseCacher.CacheInput(this.OnMouseUpOutRight);
                }
            }
        }

        #endregion

        #region Event Initialization

        private void SetupEventHandlers()
        {
            this.AddInputHandlers();
            this.AddChangeHandlers();
        }

        private void AddChangeHandlers()
        {
            this.Move += this.EventBoundChanged;
            this.Resize += this.EventBoundChanged;
        }

        private void AddInputHandlers()
        {
            this.EngineInput.Event.MouseMove -= this.EventMouseMove;
            this.EngineInput.Event.MouseMove += this.EventMouseMove;

            this.EngineInput.Event.MouseUp -= this.EventMouseUp;
            this.EngineInput.Event.MouseUp += this.EventMouseUp;

            this.EngineInput.Event.MouseDown -= this.EventMouseDown;
            this.EngineInput.Event.MouseDown += this.EventMouseDown;

            this.EngineInput.Event.MouseDoubleClick -= this.EventMouseDoubleClick;
            this.EngineInput.Event.MouseDoubleClick += this.EventMouseDoubleClick;
        }

        #endregion

        #region Event On Methods

        protected virtual void OnMouseDoubleClickLeft()
        {
            this.MouseDoubleClick?.Invoke(this, new EventArgs());
            this.MouseDoubleClickLeft?.Invoke(this, new EventArgs());
        }

        protected virtual void OnMouseDoubleClickRight()
        {
            this.MouseDoubleClick?.Invoke(this, new EventArgs());
            this.MouseDoubleClickRight?.Invoke(this, new EventArgs());
        }

        protected virtual void OnMousePressOutLeft()
        {
            this.MousePressOut?.Invoke(this, new EventArgs());
            this.MousePressOutLeft?.Invoke(this, new EventArgs());
        }

        protected virtual void OnMousePressOutRight()
        {
            this.MousePressOut?.Invoke(this, new EventArgs());
            this.MousePressOutRight?.Invoke(this, new EventArgs());
        }

        protected virtual void OnMousePressLeft()
        {
            this.MousePress?.Invoke(this, new EventArgs());
            this.MousePressLeft?.Invoke(this, new EventArgs());
        }

        protected virtual void OnMousePressRight()
        {
            this.MousePress?.Invoke(this, new EventArgs());
            this.MousePressRight?.Invoke(this, new EventArgs());
        }

        protected virtual void OnMouseUpLeft()
        {
            this.MouseUp?.Invoke(this, new EventArgs());
            this.MouseUpLeft?.Invoke(this, new EventArgs());
        }

        protected virtual void OnMouseUpRight()
        {
            this.MouseUp?.Invoke(this, new EventArgs());
            this.MouseUpRight?.Invoke(this, new EventArgs());
        }

        private void OnMouseUpOutRight()
        {
            this.MouseUpOut?.Invoke(this, new EventArgs());
            this.MouseUpOutRight?.Invoke(this, new EventArgs());
        }

        private void OnMouseUpOutLeft()
        {
            this.MouseUpOut?.Invoke(this, new EventArgs());
            this.MouseUpOutLeft?.Invoke(this, new EventArgs());
        }

        protected virtual void OnMouseLeave()
        {
            this.MouseLeave?.Invoke(this, new EventArgs());
        }

        protected virtual void OnMouseEnter()
        {
            this.MouseEnter?.Invoke(this, new EventArgs());
        }

        #endregion

        #region Mouse State Detection

        public bool MouseDoubleClickEnabled { get; set; } = true;

        protected MMInputCacher MouseCacher { get; } = new MMInputCacher();

        public bool MouseOver => this.MouseState.MouseOver;

        public bool MousePressed(MMMouseButton button)
        {
            switch (button)
            {
                case MMMouseButton.Left:
                    return this.MouseState.LButtonPressed;
                case MMMouseButton.Right:
                    return this.MouseState.RButtonPressed;
                default:
                    throw new ArgumentOutOfRangeException(nameof(button), button, null);
            }
        }

        private MMMouseStateAutomata MouseState { get; } = new MMMouseStateAutomata();

        protected bool MouseLButton(MMMouseEventArgs e)
        {
            return e.Button == MMMouseButtons.Left;
        }

        protected bool MouseOverPos(Vector2 position)
        {
            return this.mouseEnabled && this.Bound.Contains(position);
        }

        protected bool MouseRButton(MMMouseEventArgs e)
        {
            return e.Button == MMMouseButtons.Right;
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
                        this.DisposeEvents();

                        // No need to dispose frame change handlers because the
                        // events are disposed in this.DisposeEvents
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
            this.DisposeInputHandlers();
            this.DisposeChangeHandlers();
        }

        private void DisposeChangeHandlers()
        {
            this.Move -= this.EventBoundChanged;
            this.Resize -= this.EventBoundChanged;
        }

        private void DisposeEvents()
        {
            this.MouseEnter = null;
            this.MouseLeave = null;
            this.MousePressLeft = null;
            this.MousePressOutLeft = null;
            this.MouseUpLeft = null;
            this.MouseDoubleClickLeft = null;
            this.MousePressRight = null;
            this.MousePressOutRight = null;
            this.MouseUpRight = null;
            this.MouseDoubleClickRight = null;
        }

        private void DisposeInputHandlers()
        {
            this.EngineInput.Event.MouseMove -= this.EventMouseMove;
            this.EngineInput.Event.MouseUp -= this.EventMouseUp;
            this.EngineInput.Event.MouseDown -= this.EventMouseDown;
            this.EngineInput.Event.MouseDoubleClick -= this.EventMouseDoubleClick;
        }

        #endregion
    }
}
