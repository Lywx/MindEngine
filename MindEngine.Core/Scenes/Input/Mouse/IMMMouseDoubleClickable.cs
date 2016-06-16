namespace MindEngine.Core.Scenes.Input.Mouse
{
    using System;

    /// <summary>
    ///     Interface supporting basic mouse picking operation and selection.
    /// </summary>
    public interface IMMMouseDoubleClickable
    {
        bool MouseDoubleClickEnabled { get; set; }

        #region Mouse Left Buttons

        event EventHandler<EventArgs> MouseDoubleClickLeft;

        #endregion Mouse Left Buttons

        #region Mouse Right Buttons

        event EventHandler<EventArgs> MouseDoubleClickRight;

        #endregion Mouse Right Buttons

        #region Mouse General

        event EventHandler<EventArgs> MouseDoubleClick;

        #endregion Mouse General
    }
}
