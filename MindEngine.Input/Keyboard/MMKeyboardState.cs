namespace MindEngine.Input.Keyboard
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Xna.Framework.Input;

    public class MMKeyboardState : Dictionary<Keys, MMKeyState> 
    {
        public MMKeyboardState()
        {
            foreach (var str in Enum.GetNames(typeof(Keys)))
            {
                var key = (Keys)Enum.Parse(typeof(Keys), str);
                var keyState = new MMKeyState(key);
                this.Add(key, keyState);
            }
        }
    }
}