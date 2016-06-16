namespace MindEngine.Core.Scenes.Input.Mouse
{
    using System;

    public interface IMMMouseClickable
    {
        bool MouseEnabled { get; set; }

        bool MouseOver { get; }

        bool MousePressed (MMMouseButton button);

        #region Mouse General

        event EventHandler<EventArgs> MouseEnter;

        event EventHandler<EventArgs> MouseLeave;

        event EventHandler<EventArgs> MousePress;

        event EventHandler<EventArgs> MousePressOut;

        event EventHandler<EventArgs> MouseUp;

        event EventHandler<EventArgs> MouseUpOut;

        #endregion Mouse General

        #region Mouse Left Buttons

        event EventHandler<EventArgs> MousePressLeft;

        event EventHandler<EventArgs> MousePressOutLeft;

        event EventHandler<EventArgs> MouseUpLeft;

        event EventHandler<EventArgs> MouseUpOutLeft;

        #endregion Mouse Left Buttons

        #region Mouse Right Buttons

        event EventHandler<EventArgs> MousePressRight;

        event EventHandler<EventArgs> MousePressOutRight;

        event EventHandler<EventArgs> MouseUpRight;

        event EventHandler<EventArgs> MouseUpOutRight;

        #endregion Mouse Right Buttons
        
    }
}