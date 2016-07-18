namespace MindEngine.Input.Keyboard
{
    using System;
    using System.Collections.Generic;
    using Core;
    using Microsoft.Xna.Framework.Input;

    public interface IMMKeyboardInput : IMMInputtableOperations
    {
        #region Key States

        /// <summary>
        /// Check if a key is pressed.
        /// </summary>
        bool KeyPressed(Keys key);

        /// <summary>
        /// Return the duration in ticks of a key has been pressed.
        /// </summary>
        ulong KeyPressedDuration(Keys key);

        /// <summary>
        /// Check if a key was just pressed in the most recent update.
        /// </summary>
        bool KeyDown(Keys key);

        /// <summary>
        /// Check if a key was just released in the most recent update.
        /// </summary>
        bool KeyUp(Keys key);

        #endregion

        #region Key Combination States

        bool KeyCombinationDown(MMKeyCombination combination);

        bool KeyCombinationPressed(MMKeyCombination combination);

        ulong KeyCombinationPressedDuration(MMKeyCombination combination);

        bool KeyCombinationUp(MMKeyCombination combination);

        #endregion

        #region Modifier States

        bool ModifierPressed(List<Keys> keys);

        ulong ModifierPressedDuration(List<Keys> keys);

        #endregion

        #region Action States

        bool ActionPressed(MMInputAction action);

        ulong ActionPressedDuration(MMInputAction action);

        bool ActionDown(MMInputAction action);

        bool ActionUp(MMInputAction action);

        #endregion
    }
}