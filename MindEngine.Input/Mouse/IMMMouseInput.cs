namespace MindEngine.Input.Mouse
{
    using Core;
    using Microsoft.Xna.Framework;

    public interface IMMMouseInput : IMMInputtableOperations
    {
        #region Button States

        bool ButtonLeftClicked { get; }

        bool ButtonRightClicked { get; }

        #endregion

        #region Wheel States

        bool WheelDownScrolled { get; }

        bool WheelUpScrolled { get; }

        /// <summary>
        /// Integer value for wheel scrolling. Positive when scrolled down.
        /// Negative when scrolled down.
        /// </summary>
        int WheelScroll { get; }

        #endregion

        #region Position State

        Vector2 Position { get; }

        #endregion
    }
}
