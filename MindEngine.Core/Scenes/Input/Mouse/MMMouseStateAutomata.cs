namespace MindEngine.Core.Scenes.Input.Mouse
{
    /// <summary>
    /// Implemented as a pushdown automata.
    /// </summary>
    public class MMMouseStateAutomata
    {
        private MMMouseButtonHistory LButtonHistory { get; set; } = new MMMouseButtonHistory();

        private MMMouseButtonHistory RButtonHistory { get; set; } = new MMMouseButtonHistory();

        private MMMousePositionHistory PositionHistory { get; set; } = new MMMousePositionHistory();

        #region States

        public bool LButtonDoubleClicked => this.ButtonDoubleClicked(this.LButtonHistory);

        public bool LButtonPressed => this.ButtonPressed(this.LButtonHistory);

        public bool LButtonReleased => this.ButtonReleased(this.LButtonHistory);

        public bool MouseOver => this.PositionHistory.Count != 0 && this.PositionHistory.Peek() is MMMousePositionOver;

        public bool RButtonDoubleClicked => this.ButtonDoubleClicked(this.RButtonHistory);

        public bool RButtonPressed => this.ButtonPressed(this.RButtonHistory);

        public bool RButtonReleased => this.ButtonReleased(this.RButtonHistory);

        private bool ButtonDoubleClicked(MMMouseButtonHistory button)
        {
            return button.Count != 0 && button.Peek() is MMMouseButtonDoubleClick;
        }

        private bool ButtonPressed(MMMouseButtonHistory button)
        {
            return button.Count != 0 && button.Peek() is MMMouseButtonPress;
        }

        private bool ButtonReleased(MMMouseButtonHistory button)
        {
            return button.Count == 0;
        }

        #endregion

        #region Operations

        protected void Clear(MMMouseButtonHistory button)
        {
            button.Clear();
        }

        public void LClear()
        {
            this.Clear(this.LButtonHistory);
        }

        public void RClear()
        {
            this.Clear(this.RButtonHistory);
        }

        protected void DoubleClick(MMMouseButtonHistory button)
        {
            this.Clear(button);
            button.Push(new MMMouseButtonDoubleClick());
        }

        public void LDoubleClick()
        {
            this.DoubleClick(this.LButtonHistory);
        }

        public void RDoubleClick()
        {
            this.DoubleClick(this.RButtonHistory);
        }

        protected void Press(MMMouseButtonHistory button)
        {
            button.Push(new MMMouseButtonPress());
        }

        public void LPress()
        {
            this.Press(this.LButtonHistory);
        }

        public void RPress()
        {
            this.Press(this.RButtonHistory);
        }

        protected void Release(MMMouseButtonHistory button)
        {
            if (button.Count != 0)
            {
                button.Pop();
            }
        }

        public void LRelease()
        {
            this.Release(this.LButtonHistory);
        }

        public void RRelease()
        {
            this.Release(this.RButtonHistory);
        }

        public void Enter()
        {
            this.PositionHistory.Push(new MMMousePositionOver());
        }

        public void Leave()
        {
            this.PositionHistory.Pop();
        }

        #endregion
    }
}