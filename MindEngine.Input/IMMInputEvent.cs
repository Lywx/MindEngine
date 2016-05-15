namespace MindEngine.Input
{
    using System;
    using Core.Components;
    using Keyboard.EventArgs;
    using Mouse.EventArgs;

    /// <summary>
    ///     Provide event based input handling. The advantages of event based input
    ///     is that you don't need to manage low level state in your code. You can
    ///     use nicely implemented event to encapsulate logic.
    /// </summary>
    public interface IMMInputEvent : IMMCompositeComponent
    {
        /// <summary>
        ///     Event raised when a character has been entered.
        /// </summary>
        event EventHandler<MMKeyInputEventArgs> CharEntered;

        /// <summary>
        ///     Event raised when a key has been pressed down. May fire multiple
        ///     times due to keyboard repeat.
        /// </summary>
        event EventHandler<MMKeyEventArgs> KeyDown;

        /// <summary>
        ///     Event raised when a key has been released.
        /// </summary>
        event EventHandler<MMKeyEventArgs> KeyUp;

        /// <summary>
        ///     Event raised when a char key has been pressed.
        /// </summary>
        event EventHandler<MMKeyPressEventArgs> KeyPress;

        /// <summary>
        ///     Event raised when a mouse button has been double clicked.
        /// </summary>
        event EventHandler<MMMouseEventArgs> MouseDoubleClick;

        /// <summary>
        ///     Event raised when a mouse button is pressed.
        /// </summary>
        event EventHandler<MMMouseEventArgs> MouseDown;

        /// <summary>
        ///     Event raised when the mouse has hovered in the same location for a short period of time.
        /// </summary>
        event EventHandler MouseHover;

        /// <summary>
        ///     Event raised when the mouse changes location.
        /// </summary>
        event EventHandler<MMMouseEventArgs> MouseMove;

        /// <summary>
        ///     Event raised when a mouse button is released.
        /// </summary>
        event EventHandler<MMMouseEventArgs> MouseUp;

        /// <summary>
        ///     Event raised when the mouse wheel has been moved.
        /// </summary>
        event EventHandler<MMMouseEventArgs> MouseScroll;
    }
}
