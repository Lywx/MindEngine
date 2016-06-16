namespace MindEngine.Core.Scenes.Input.Mouse
{
    using System;

    public class MMMouseState
    {
        public DateTime Stamp { get; set; }

        public MMMouseState()
        {
            this.Stamp = DateTime.Now;
        }
    }
}