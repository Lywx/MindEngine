namespace MindEngine.Input.Gamepad
{
    using System;
    using System.Collections.Generic;

    public class MMGamePadState
    {
        public Dictionary<MMGamePadButton, MMGamePadButtonState> Buttons { get; set; } = new Dictionary<MMGamePadButton, MMGamePadButtonState>();

        public MMGamePadStickState Vectors { get; set; }

        public MMGamePadState()
        {
            foreach (var str in Enum.GetNames(typeof(MMGamePadButton)))
            {
                var button = (MMGamePadButton)Enum.Parse(typeof(MMGamePadButton), str);
                var buttonState = new MMGamePadButtonState(button);
                this.Buttons.Add(button, buttonState);
            }
            
        }
    }
}