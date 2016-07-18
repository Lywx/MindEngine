namespace MindEngine.Input.Mouse
{
    using System.Diagnostics;
    using Core;
    using Core.Component;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Input;

    public class MMMouseInput : MMCompositeComponent, IMMMouseInput
    {
        /// <summary>
        ///     Sets or gets input offset and ratio when rescaling controls in render target.
        /// </summary>
        public MMMouseOffset MouseOffset { get; set; } = new MMMouseOffset(0, 0);

        public MMMouseState MouseState { get; private set; } = new MMMouseState();

        public MMMouseRecord MouseRecord { get; private set; } = new MMMouseRecord();

        #region Constructors

        public MMMouseInput(MMEngine engine) : base(engine)
        {
        }

        #endregion

        #region Mouse Position Correction

        private Point ScaleMousePosition(Point mousePosition, MMMouseOffset mouseOffset)
        {
            return new Point(
                mousePosition.X - mouseOffset.X,
                mousePosition.Y - mouseOffset.Y);
        }

        private Point BoundMousePosition(Point mousePosition, Rectangle mouseBounds)
        {
            var x = mousePosition.X;
            var y = mousePosition.Y;

            if (x < 0)
            {
                x = 0;
            }

            if (y < 0)
            {
                y = 0;
            }

            if (x >= mouseBounds.Width)
            {
                x = mouseBounds.Width - 1;
            }

            if (y >= mouseBounds.Height)
            {
                y = mouseBounds.Height - 1;
            }

            return new Point(x, y);
        }

        #endregion

        #region Update

        public override void UpdateInput(GameTime time)
        {
            var mouseStateCurrent = Mouse.GetState();

            this.UpdatePosition(mouseStateCurrent);
            this.UpdateWheel(mouseStateCurrent);
            this.UpdateButtons(mouseStateCurrent, time);

            this.MouseRecord.Add(mouseStateCurrent);
        }

        private void UpdatePosition(MouseState mouseStateCurrent)
        {
            var mousePositionCurrent = this.ScaleMousePosition(
                this.BoundMousePosition(
                    mouseStateCurrent.Position,
                    this.Engine.Window.ClientBounds),
                this.MouseOffset);

            var mousePositionPrevious = this.MouseState.Position;
            if (mousePositionCurrent != mousePositionPrevious)
            {
                this.MouseState.PositionDiff = mousePositionCurrent - mousePositionPrevious;
                this.MouseState.Position = mousePositionCurrent;
            }
            else
            {
                this.MouseState.PositionDiff = Point.Zero;
            }
        }

        private void UpdateWheel(MouseState mouseStateCurrent)
        {
            var wheelValueCurrent = mouseStateCurrent.ScrollWheelValue;
            var wheelValuePrevious = this.MouseState.WheelValue;
            if (wheelValueCurrent != wheelValuePrevious)
            {
                this.MouseState.WheelValue = wheelValueCurrent;
                this.MouseState.WheelDirection = wheelValueCurrent < wheelValuePrevious
                                                     ? MMMouseWheelDirection.Down
                                                     : MMMouseWheelDirection.Up;

                this.MouseState.WheelDiff = (wheelValueCurrent - wheelValuePrevious)
                                            / MMMouseSettings.WheelDelta;

            }
            else
            {
                this.MouseState.WheelValue = 0;
                this.MouseState.WheelDirection = MMMouseWheelDirection.None;
                this.MouseState.WheelDiff = 0;
            }
        }

        private void UpdateButton(MMMouseButtonState buttonState, ButtonState buttonPressState, GameTime time)
        {
            var buttonPressedCurrent = buttonPressState == ButtonState.Pressed;
            var buttonPressedPrevious = buttonState.Pressed;

            if (buttonPressedCurrent && !buttonPressedPrevious)
            {
                buttonState.Pressed = true;
                buttonState.PressedDuration = 0;

                buttonState.Down = true;
                buttonState.Up = false;
            }
            else if (!buttonPressedCurrent && buttonPressedPrevious)
            {
                buttonState.Pressed = false;
                buttonState.PressedDuration = 0;

                buttonState.Down = false;
                buttonState.Up = true;
            }
            else if (buttonPressedCurrent /*&& buttonPressedPrevious*/)
            {
                Debug.Assert(time.ElapsedGameTime.Ticks > 0);
                buttonState.PressedDuration += (ulong)time.ElapsedGameTime.Ticks;

                buttonState.Down = false;
            }
            else // (!buttonPressedCurrent && !buttonPressedPrevious) 
            {
                buttonState.Up = false;
            }
        }

        private void UpdateButtons(MouseState mouseStateCurrent, GameTime time)
        {
            this.UpdateButton(this.MouseState.Buttons[MMMouseButton.Left], mouseStateCurrent.LeftButton, time);
            this.UpdateButton(this.MouseState.Buttons[MMMouseButton.Right], mouseStateCurrent.RightButton, time);
            this.UpdateButton(this.MouseState.Buttons[MMMouseButton.Middle], mouseStateCurrent.MiddleButton, time);
            this.UpdateButton(this.MouseState.Buttons[MMMouseButton.XButton1], mouseStateCurrent.XButton1, time);
            this.UpdateButton(this.MouseState.Buttons[MMMouseButton.XButton2], mouseStateCurrent.XButton2, time);
        }

        #endregion Update

        public bool ButtonPressed(MMMouseButton button)
        {
            return this.MouseState.Buttons[button].Pressed;
        }

        public ulong ButtonPressedDuration(MMMouseButton button)
        {
            return this.MouseState.Buttons[button].PressedDuration;
        }

        public bool ButtonDown(MMMouseButton button)
        {
            return this.MouseState.Buttons[button].Down;
        }

        public bool ButtonUp(MMMouseButton button)
        {
            return this.MouseState.Buttons[button].Up;
        }

        public Point Position => this.MouseState.Position;

        public Point PositionDiff => this.MouseState.PositionDiff;

        public bool PositionMove => this.MouseState.PositionDiff.X != 0 || this.MouseState.PositionDiff.Y != 0;

        public MMMouseWheelDirection WheelDirection => this.MouseState.WheelDirection;

        public int WheelDiff => this.MouseState.WheelDiff;

        public bool WheelUp => this.MouseState.WheelDiff > 0;

        public bool WheelDown => this.MouseState.WheelDiff < 0;

        public bool WheelMove => this.MouseState.WheelDiff != 0;
    }
}
