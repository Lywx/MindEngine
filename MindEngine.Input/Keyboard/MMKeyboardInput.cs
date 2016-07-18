namespace MindEngine.Input.Keyboard
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Input;

    public class MMKeyboardInput : IMMKeyboardInput
    {
        private MMKeyboardBinding<MMInputAction> KeyboardBinding { get; set; }

        private MMKeyboardRecord KeyboardRecord { get; set; } = new MMKeyboardRecord();

        private MMKeyboardState KeyboardState { get; set; } = new MMKeyboardState();

        #region Constructors

        public MMKeyboardInput()
        {
            this.KeyboardBinding = new MMKeyboardBinding<MMInputAction>();
        }

        public MMKeyboardInput(MMKeyboardBinding<MMInputAction> keyboardBinding)
        {
            if (keyboardBinding == null)
            {
                throw new ArgumentNullException(nameof(keyboardBinding));
            }

            this.KeyboardBinding = keyboardBinding;
        }

        #endregion Constructors

        #region Update

        public void UpdateInput(GameTime time)
        {
            var keyboardStateCurrent = Keyboard.GetState();

            this.UpdateKeys(keyboardStateCurrent, time);

            this.KeyboardRecord.Add(keyboardStateCurrent);
        }

        private void UpdateKeys(KeyboardState keyboardStateCurrent, GameTime time)
        {
            foreach (var keyState in this.KeyboardState.Values)
            {
                var keyPressedCurrent = keyboardStateCurrent.IsKeyDown(keyState.Key);
                var keyPressedPrevious = keyState.Pressed;

                // Update press or release change and related events
                if (keyPressedCurrent && !keyPressedPrevious)
                {
                    keyState.Pressed = true;
                    keyState.PressedDuration = 0;

                    keyState.Down = true;
                    keyState.Up = false;
                }
                else if (!keyPressedCurrent && keyPressedPrevious)
                {
                    keyState.Pressed = false;
                    keyState.PressedDuration = 0;

                    keyState.Down = false;
                    keyState.Up = true;
                }
                else if (keyPressedCurrent /*&& keyPressedPrevious*/)
                {
                    Debug.Assert(time.ElapsedGameTime.Ticks > 0);
                    keyState.PressedDuration += (ulong)time.ElapsedGameTime.Ticks;

                    keyState.Down = false;
                }
                else // (!keyPressedCurrent && !keyPressedPrevious) 
                {
                    keyState.Up = false;
                }
            }
        }

        #endregion Update

        #region Action States

        public bool ActionPressed(MMInputAction action)
        {
            return this.KeyCombinationPressed(this.KeyboardBinding[action]);
        }

        public ulong ActionPressedDuration(MMInputAction action)
        {
            return this.KeyCombinationPressedDuration(this.KeyboardBinding[action]);
        }

        public bool ActionDown(MMInputAction action)
        {
            return this.KeyCombinationDown(this.KeyboardBinding[action]);
        }

        public bool ActionUp(MMInputAction action)
        {
            return this.KeyCombinationUp(this.KeyboardBinding[action]);
        }

        #endregion

        #region Modifier States

        public ulong ModifierPressedDuration(List<Keys> keys)
        {
            return keys.Min(key => this.KeyPressedDuration(key));
        }

        public bool ModifierPressed(List<Keys> keys)
        {
            return keys.Count == 0 || keys.All(this.KeyPressed);
        }

        #endregion 

        #region Keyboard Binding States

        /// <summary>
        ///     Check if an action map has been pressed.
        /// </summary>
        public bool KeyCombinationPressed(MMKeyCombination combination)
        {
            return combination.Any(binding => this.KeyPressed(binding.Key)
                                              && this.ModifierPressed(binding.Value));
        }

        public ulong KeyCombinationPressedDuration(MMKeyCombination combination)
        {
            // Maximal amount in different bindings
            return combination.Max(

                // Minimal amount in different keys
                binding => Math.Min(
                this.KeyPressedDuration(binding.Key),
                this.ModifierPressedDuration(binding.Value)));
        }

        /// <summary>
        ///     Check if an action map has been triggered this frame.
        /// </summary>
        public bool KeyCombinationDown(MMKeyCombination combination)
        {
            return combination.Any(
                binding => this.KeyDown(binding.Key) && this.ModifierPressed(binding.Value));
        }

        public bool KeyCombinationUp(MMKeyCombination combination)
        {
            return combination.Any(
                binding => this.KeyDown(binding.Key) && this.ModifierPressed(binding.Value));
        }

        #endregion

        #region Keyboard States

        public bool KeyPressed(Keys key)
        {
            return this.KeyboardState[key].Pressed;
        }

        public ulong KeyPressedDuration(Keys key)
        {
            return this.KeyboardState[key].PressedDuration;
        }

        public bool KeyDown(Keys key)
        {
            return this.KeyboardState[key].Down;
        }

        public bool KeyUp(Keys key)
        {
            return this.KeyboardState[key].Up;
        }

        #endregion 
    }
}