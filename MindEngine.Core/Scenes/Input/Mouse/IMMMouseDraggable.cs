namespace MindEngine.Core.Scenes.Input.Mouse
{
    using System;

    public interface IMMMouseDraggable
    {
        bool AllowMove { get; set; }

        bool AllowDrag { get; set; }

        bool AllowDrop { get; set; }

        event EventHandler<EventArgs> MouseDrag;
        
        event EventHandler<EventArgs> MouseMove;

        event EventHandler<EventArgs> MouseDrop;
    }
}