namespace MindEngine.Core.Service.Process
{
    using Microsoft.Xna.Framework;

    public class MMProcessChain : MMProcess
    {
        public MMProcessChain(params MMProcess[] processes)
            : base("Process (Chain)", MMProcessCategory.User)
        {
            this.Processes = processes;
        }

        private int ProcessIndex { get; set; }

        private MMProcess ProcessCurrent { get; set; }

        private MMProcess[] Processes { get; }

        public override void OnEnter()
        {
            this.AttachNextProcess();
        }

        public override void OnExit()
        {
        }

        public override void OnUpdate(GameTime time)
        {
            if (this.ProcessCurrent.State == MMProcessState.Terminated)
            {
                ++this.ProcessIndex;

                if (this.ProcessIndex < this.Processes.Length)
                {
                    this.AttachNextProcess();
                }
                else
                {
                    this.Exit();
                }
            }
        }

        private void AttachNextProcess()
        {
            this.ProcessCurrent = this.Processes[this.ProcessIndex];
            this.EngineInterop.Process.AttachProcess(this.ProcessCurrent);
        }
    }
}
