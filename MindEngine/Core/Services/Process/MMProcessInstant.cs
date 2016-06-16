using System;

namespace MindEngine.Core.Services.Process
{
    public class MMProcessInstant : MMProcess
    {
        private Action Action { get; set; }

        public MMProcessInstant(Action action) 
            : base("Process (Instant)", MMProcessCategory.User)
        {
            this.Action = action;
        }

        public override void OnEnter()
        {
            this.Action.Invoke();
        }
    }
}
