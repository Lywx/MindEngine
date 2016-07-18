namespace MindEngine.Input.Keyboard
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Xna.Framework.Input;

    /// <summary>
    /// A sequence of keyboard state record. This class would b
    /// </summary>
    [Serializable]
    public class MMKeyboardRecord : List<KeyboardState>
    {
        public MMKeyboardRecord()
        {
        }
    }
}