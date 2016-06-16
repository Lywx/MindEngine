namespace MindEngine.Core.Scenes.Input
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class MMInputCacher 
    {
        private readonly List<Action> cachedInputs = new List<Action>();

        private Action currentInput;

        #region Events

        protected event EventHandler<EventArgs> InputStopped;

        protected event EventHandler<EventArgs> InputStarted;

        #endregion

        #region Event Ons

        private void OnInputStopped()
        {
            this.InputStopped?.Invoke(this, EventArgs.Empty);
        }

        private void OnInputStarted()
        {
            this.InputStarted?.Invoke(this, EventArgs.Empty);
        }

        #endregion

        #region Operations

        public void CacheInput(Action action)
        {
            this.cachedInputs.Add((() => this.TriggerInput(action)));
        }

        /// <summary>
        /// Removes all actions cached.
        /// </summary>
        public void ClearInput()
        {
            this.cachedInputs.Clear();
        }

        /// <summary>
        /// Runs a single action cached.
        /// </summary>
        public void ContinueInput()
        {
            if (this.currentInput == null
                && this.cachedInputs.Count != 0)
            {
                this.currentInput = this.cachedInputs.First();
                this.currentInput();
            }
        }

        /// <summary>
        /// Runs all of actions cached.
        /// </summary>
        public void FlushInput()
        {
            if (this.currentInput == null
                && this.cachedInputs.Count != 0)
            {
                foreach (var action in this.cachedInputs.ToArray())
                {
                    this.currentInput = action;
                    this.currentInput();
                }
            }
        }

        public void QueueInput(Action action)
        {
            if (this.currentInput == null)
            {
                this.currentInput = () => this.TriggerInput(action);
                this.currentInput();
            }
            else
            {
                this.CacheInput(action);
            }
        }

        private void TriggerInput(Action action)
        {
            if (this.cachedInputs.Count == 0)
            {
                this.OnInputStarted();
            }

            action();

            if (this.cachedInputs.Contains(this.currentInput))
            {
                this.cachedInputs.Remove(this.currentInput);
            }

            this.currentInput = null;

            if (this.cachedInputs.Count == 0)
            {
                this.OnInputStopped();
            }
        }

        #endregion
    }
}