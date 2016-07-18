namespace MindEngine.Core.Service.Process
{
    using System;
    using Microsoft.Xna.Framework;

    public class MMProcessInstant : MMProcess
    {
        private Action Action { get; set; }

        public MMProcessInstant(Action action) 
            : base("Process (Instant)", MMProcessCategory.User)
        {
            this.Action = action;
        }

        public override void OnUpdate(GameTime time)
        {
            this.Action.Invoke();

            this.Exit();
        }
    }
}
