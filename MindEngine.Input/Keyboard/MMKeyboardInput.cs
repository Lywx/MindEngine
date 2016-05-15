namespace MindEngine.Input.Keyboard
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Input;

    public class MMKeyboardInput : IMMKeyboardInput
    {
        private MMKeyboardBinding<MMInputAction> keyboardBinding;

        private KeyboardState keyboardCurrent;

        private KeyboardState keyboardPrevious;

        #region Constructors

        public MMKeyboardInput()
        {
            this.keyboardBinding = new MMKeyboardBinding<MMInputAction>();
            
        }

        public MMKeyboardInput(MMKeyboardBinding<MMInputAction> keyboardBinding)
        {
            if (keyboardBinding == null)
            {
                throw new ArgumentNullException(nameof(keyboardBinding));
            }

            this.keyboardBinding = keyboardBinding;
        }

        #endregion Constructors

        #region Update

        public void UpdateInput(GameTime time)
        {
            this.keyboardPrevious = this.keyboardCurrent;
            this.keyboardCurrent = Keyboard.GetState();
        }

        #endregion Update

        #region Modifier States

        public bool KeyAltDown
            =>
                this.keyboardCurrent.IsKeyDown(Keys.LeftAlt)
                || this.keyboardCurrent.IsKeyDown(Keys.RightAlt);

        public bool KeyCtrlDown
            =>
                this.keyboardCurrent.IsKeyDown(Keys.LeftControl)
                || this.keyboardCurrent.IsKeyDown(Keys.RightControl);

        public bool KeyShiftDown
            =>
                this.keyboardCurrent.IsKeyDown(Keys.LeftShift)
                || this.keyboardCurrent.IsKeyDown(Keys.RightShift);

        private bool AreModifiersReady(KeyValuePair<Keys, List<Keys>> binding)
        {
            return this.AreModifiersEmpty(binding) || this.AreModifiersPressed(binding);
        }

        private bool AreModifiersPressed(KeyValuePair<Keys, List<Keys>> binding)
        {
            return binding.Value.All(this.KeyPressed);
        }

        private bool AreModifiersEmpty(KeyValuePair<Keys, List<Keys>> binding)
        {
            return binding.Value.Count == 0;
        }

        #endregion 

        #region Action States

        /// <summary>
        ///     Check if an action has been pressed.
        /// </summary>
        public bool ActionPressed(MMInputAction action)
        {
            return this.IsKeyboardBindingPressed(this.keyboardBinding[action]);
        }

        /// <summary>
        ///     Check if an action was just performed in the most recent update.
        /// </summary>
        public bool ActionTriggered(MMInputAction action)
        {
            return this.IsKeyboardBindingTriggered(this.keyboardBinding[action]);
        }

        #endregion

        #region Keyboard Binding States

        /// <summary>
        ///     Check if an action map has been pressed.
        /// </summary>
        private bool IsKeyboardBindingPressed(MMKeyboardCombination combination)
        {
            return
                combination.Any(binding => this.KeyPressed(binding.Key)
                                           && this.AreModifiersReady(binding));
        }

        /// <summary>
        ///     Check if an action map has been triggered this frame.
        /// </summary>
        private bool IsKeyboardBindingTriggered(MMKeyboardCombination combination)
        {
            return
                combination.Any(
                    binding => this.KeyTriggered(binding.Key)
                               && this.AreModifiersReady(binding));
        }

        #endregion

        #region Keyboard States

        /// <summary>
        ///     Check if a key is pressed.
        /// </summary>
        public bool KeyPressed(Keys key)
        {
            return this.keyboardCurrent.IsKeyDown(key);
        }

        /// <summary>
        ///     Check if a key was just pressed in the most recent update.
        /// </summary>
        public bool KeyTriggered(Keys key)
        {
            return this.keyboardCurrent.IsKeyDown(key)
                   && !this.keyboardPrevious.IsKeyDown(key);
        }

        #endregion 
    }
}