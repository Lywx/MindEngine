#if WINDOWS

namespace MindEngine.Input
{
    using System;
    using System.Windows.Forms;
    using Core;
    using Core.Components;
    using Keyboard.EventArgs;
    using Microsoft.Xna.Framework;
    using Mouse.EventArgs;

    public class MMInputEventWin32 : MMCompositeComponent, IMMInputEvent
    {
        private readonly GameWindow window;

        private Form windowForm;

        #region Constructors

        public MMInputEventWin32(MMEngine engine)
            : base(engine)
        {
            if (engine == null)
            {
                throw new ArgumentNullException(nameof(engine));
            }

            this.window = engine.Window;
        }

        #endregion Constructors

        #region Initialization

        public override void Initialize()
        {
            this.windowForm = this.GetWindowForm();

            this.RegisterKeyInputHandlers();
            this.RegisterMouseInputHandlers();

            base.Initialize();
        }

        #endregion

        #region Update

        public override void UpdateInput(GameTime time) {}

        #endregion

        #region Windows Forms

        /// <summary>
        ///     Force application to find current application window controls with System.Windows.Forms (Windows)
        /// </summary>
        /// <remarks>
        ///     Has to be called after GraphicsManager initialization, because by then the windows
        ///     form is constructed.
        /// </remarks>
        /// >
        private Form GetWindowForm()
        {
            return (Form)Control.FromHandle(Application.OpenForms[0].Handle);
        }

        #endregion

        #region Events

        /// <summary>
        ///     Event raised when a character has been entered.
        /// </summary>
        public event EventHandler<MMKeyInputEventArgs> CharEntered = delegate {};

        /// <summary>
        ///     Event raised when a key has been pressed down. May fire multiple times due to keyboard repeat.
        /// </summary>
        public event EventHandler<MMKeyEventArgs> KeyDown = delegate {};

        /// <summary>
        ///     Event raised when a key has been released.
        /// </summary>
        public event EventHandler<MMKeyEventArgs> KeyUp = delegate {};

        /// <summary>
        ///     Event raised when a key has been pressed.
        /// </summary>
        public event EventHandler<MMKeyPressEventArgs> KeyPress = delegate {};

        /// <summary>
        ///     Event raised when a mouse button has been double clicked.
        /// </summary>
        public event EventHandler<MMMouseEventArgs> MouseDoubleClick = delegate {};

        /// <summary>
        ///     Event raised when a mouse button is pressed.
        /// </summary>
        public event EventHandler<MMMouseEventArgs> MouseDown = delegate {};

        /// <summary>
        ///     Event raised when the mouse has hovered in the same location for a short period of time.
        /// </summary>
        public event EventHandler MouseHover = delegate {};

        /// <summary>
        ///     Event raised when the mouse changes location.
        /// </summary>
        public event EventHandler<MMMouseEventArgs> MouseMove = delegate {};

        /// <summary>
        ///     Event raised when a mouse button is released.
        /// </summary>
        public event EventHandler<MMMouseEventArgs> MouseUp = delegate {};

        /// <summary>
        ///     Event raised when the mouse wheel has been moved.
        /// </summary>
        public event EventHandler<MMMouseEventArgs> MouseScroll = delegate {};

        #endregion

        #region Event Registration

        private void RegisterKeyInputHandlers()
        {
            this.windowForm.KeyUp += this.OnKeyUp;
            this.windowForm.KeyDown += this.OnKeyDown;
            this.windowForm.KeyPress += this.OnKeyPress;

            // Text is processed separately
            this.window.TextInput += this.OnCharEntered;
        }

        private void RegisterMouseInputHandlers()
        {
            this.windowForm.MouseUp += this.OnMouseUp;
            this.windowForm.MouseDown += this.OnMouseDown;

            this.windowForm.MouseHover += this.OnMouseHover;
            this.windowForm.MouseMove += this.OnMouseMove;
            this.windowForm.MouseWheel += this.OnMouseWheel;

            this.windowForm.MouseDoubleClick += this.OnMouseDoubleClick;
        }

        #endregion

        #region Event On

        private void OnCharEntered(object sender, TextInputEventArgs args)
        {
            this.CharEntered?.Invoke(sender, args);
        }

        private void OnKeyPress(object sender, KeyPressEventArgs args)
        {
            this.KeyPress?.Invoke(sender, args);
        }

        private void OnKeyDown(object sender, KeyEventArgs args)
        {
            this.KeyDown?.Invoke(sender, args);
        }

        private void OnKeyUp(object sender, KeyEventArgs args)
        {
            this.KeyUp?.Invoke(sender, args);
        }

        private void OnMouseWheel(object sender, MouseEventArgs args)
        {
            this.MouseScroll?.Invoke(sender, args);
        }

        private void OnMouseMove(object sender, MouseEventArgs args)
        {
            this.MouseMove?.Invoke(sender, args);
        }

        private void OnMouseHover(object sender, EventArgs args)
        {
            this.MouseHover?.Invoke(sender, args);
        }

        private void OnMouseDown(object sender, MouseEventArgs args)
        {
            this.MouseDown?.Invoke(sender, args);
        }

        private void OnMouseUp(object sender, MouseEventArgs args)
        {
            this.MouseUp?.Invoke(sender, args);
        }

        private void OnMouseDoubleClick(object sender, MouseEventArgs args)
        {
            this.MouseDoubleClick?.Invoke(sender, args);
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
                        this.DisposeHandlers();
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

        private void DisposeHandlers()
        {
            this.DisposeTextInputHandler();
            this.DisposeKeyInputHandlers();
            this.DisposeMouseInputHandlers();
        }

        private void DisposeTextInputHandler()
        {
            this.window.TextInput -= this.OnCharEntered;
        }

        private void DisposeKeyInputHandlers()
        {
            this.windowForm.KeyUp -= this.OnKeyUp;
            this.windowForm.KeyDown -= this.OnKeyDown;
            this.windowForm.KeyPress -= this.OnKeyPress;
        }

        private void DisposeMouseInputHandlers()
        {
            this.windowForm.MouseUp -= this.OnMouseUp;
            this.windowForm.MouseDown -= this.OnMouseDown;

            this.windowForm.MouseHover -= this.OnMouseHover;
            this.windowForm.MouseMove -= this.OnMouseMove;
            this.windowForm.MouseWheel -= this.OnMouseWheel;

            this.windowForm.MouseDoubleClick -= this.OnMouseDoubleClick;
        }

        private void DisposeEvents()
        {
            this.DisposeTextInputEvent();
            this.DisposeKeyInputEvents();
            this.DisposeMouseInputEvents();
        }

        private void DisposeMouseInputEvents()
        {
            this.MouseUp = null;
            this.MouseDown = null;

            this.MouseDoubleClick = null;

            this.MouseHover = null;
            this.MouseMove = null;
            this.MouseScroll = null;
        }

        private void DisposeKeyInputEvents()
        {
            this.KeyUp = null;
            this.KeyPress = null;
            this.KeyDown = null;
        }

        private void DisposeTextInputEvent()
        {
            this.CharEntered = null;
        }

        #endregion
    }
}

#endif
