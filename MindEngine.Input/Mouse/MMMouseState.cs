namespace MindEngine.Input.Mouse
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Xna.Framework;

    public class MMMouseState
    {
        public MMMouseState()
        {
            foreach (var str in Enum.GetNames(typeof(MMMouseButton)))
            {
                var button = (MMMouseButton)Enum.Parse(typeof(MMMouseButton), str);
                var buttonState = new MMMouseButtonState(button);
                this.Buttons.Add(button, buttonState);
            }
        }

        public Dictionary<MMMouseButton, MMMouseButtonState> Buttons { get; set; } = new Dictionary<MMMouseButton, MMMouseButtonState>();

        public Point Position { get; set; }

        public Point PositionDiff { get; set; }

        public int WheelDiff { get; set; }

        public int WheelValue { get; set; }

        public MMMouseWheelDirection WheelDirection { get; set; }
    }
}