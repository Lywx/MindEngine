namespace MindEngine.Core.Services.Process
{
    using System;
    using Microsoft.Xna.Framework;

    public class MMProcessChain : MMProcess
    {
        public MMProcessChain(MMProcess first, MMProcess second)
            : base(first.Name, first.Category)
        {
            if (first == null)
            {
                throw new ArgumentNullException(nameof(first));
            }

            if (second == null)
            {
                throw new ArgumentNullException(nameof(second));
            }

            this.First = first;
            this.Second = second;
        }

        public MMProcess First { get; set; }

        public MMProcess Second { get; set; }

        public override void OnEnter()
        {
            this.First.OnEnter();
        }

        public override void OnExit()
        {
            this.First.OnExit();

            this.EngineInterop.Process.AttachProcess(this.Second);
        }

        public override void OnUpdate(GameTime time)
        {
            this.First.OnUpdate(time);
        }

        public override void OnWait(GameTime time)
        {
            this.First.OnWait(time);
        }
    }
}
