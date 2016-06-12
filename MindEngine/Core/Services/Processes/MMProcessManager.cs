namespace MindEngine.Core.Services.Processes
{
    using System.Collections.Generic;
    using Debug;
    using Microsoft.Xna.Framework;

    public class MMProcessManager : GameComponent, IMMProcessManager
    {
        #region Process Data

        private List<IMMProcessManagerItem> processes = new List<IMMProcessManagerItem>();

        #endregion Process Data

        #region Constructors

        public MMProcessManager(MMEngine engine)
            : base(engine) {}

        #endregion Constructors

        #region Deconstruction

        ~MMProcessManager()
        {
            this.Dispose(true);
        }

        #endregion Deconstruction

        #region Update 

        public override void Update(GameTime time)
        {
            using (new MMDebugBlockTimer())
            {
                var i = 0;

                // TODO(Wuxiang): Add priority semantics
                while (i < this.processes.Count)
                {
                    using (var process = this.processes[i])
                    {
                        if (process.IsUninitialized)
                        {
                            process.OnInit();
                        }

                        if (process.IsAlive)
                        {
                            switch (process.State)
                            {
                                case MMProcessState.Running:
                                {
                                    process.Update(time);
                                    break;
                                }

                                case MMProcessState.Paused:
                                {
                                    break;
                                }
                            }
                        }

                        if (process.IsDead)
                        {
                            switch (process.State)
                            {
                                case MMProcessState.Succeeded:
                                {
                                    process.OnSucceed();

                                    // Support process chaining
                                    var chainedProcess = process.RemoveChild<IMMProcessManagerItem>();
                                    if (chainedProcess != null)
                                    {
                                        this.processes.Add(chainedProcess);
                                    }

                                    break;
                                }

                                case MMProcessState.Failed:
                                {
                                    process.OnFail();
                                    break;
                                }

                                case MMProcessState.Aborted:
                                {
                                    process.OnAbort();
                                    break;
                                }
                            }

                            this.processes.Remove(process);

                            // Totally remove process's existing implication
                            using (var child = process.RemoveChild<IMMProcessManagerItem>())
                            {
                                child?.OnAbort();
                            }

                            process.Dispose();
                        }
                        else
                        {
                            i += 1;
                        }
                    }
                }
            }
        }

        #endregion Update

        #region IDisposable

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.processes?.Clear();
                this.processes = null;
            }

            base.Dispose(disposing);
        }

        #endregion

        #region Operations

        public void AbortProcesses(bool immediate)
        {
            var i = 0;
            while (i < this.processes.Count)
            {
                var process = this.processes[i];

                process.Abort();

                if (immediate)
                {
                    process.OnAbort();

                    this.processes.Remove(process);
                }
                else
                {
                    i += 1;
                }
            }
        }

        public void AttachProcess(IMMProcessManagerItem process)
        {
            this.processes.Add(process);
        }

        #endregion Operations
    }
}
