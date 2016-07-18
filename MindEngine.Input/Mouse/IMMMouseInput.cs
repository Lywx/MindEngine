namespace MindEngine.Input.Mouse
{
    using Core;
    using Microsoft.Xna.Framework;

    public interface IMMMouseInput : IMMInputtableOperations
    {
        bool ButtonPressed(MMMouseButton button);

        ulong ButtonPressedDuration(MMMouseButton button);

        bool ButtonDown(MMMouseButton button);

        bool ButtonUp(MMMouseButton button);

        Point Position { get; }

        bool PositionMove { get; }

        Point PositionDiff { get; }

        /// <summary>
        ///     Mouse wheel scroll direction.
        /// </summary>
        MMMouseWheelDirection WheelDirection { get; }

        int WheelDiff { get; }

        bool WheelUp { get; }

        bool WheelDown { get; }

        bool WheelMove { get; }
    }
}