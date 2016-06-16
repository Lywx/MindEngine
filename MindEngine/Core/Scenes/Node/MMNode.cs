namespace MindEngine.Core.Scenes.Node
{
    using System;
    using System.Linq;
    using Microsoft.Xna.Framework;
    using Entity;

    public interface IMMNode : IMMEntity, IMMUpdateableOperations
    {
        #region Common Draw Property

        bool DrawEnabled { get; set; }

        Color DrawColor { get; set; }

        byte DrawOpacity { get; set; }

        byte DrawDepth { get; set; }

        #endregion

        #region Common Screen Property

        Vector2 Position { get; set; }

        Vector2 Size { get; set; }

        #endregion

        #region Hierarchy Semantics

        void Add(MMNode child);

        void Remove(MMNode child, bool disposing);

        void OnEnter();

        void OnExit();

        MMNode Parent { get; set; }

        MMNode Root { get; set; }

        MMNodeCollection Children { get; }

        #endregion
    }

    /// <summary>
    /// Nodes are built with support to a wide range of common properties defined 
    /// in facade interface. It provides action scheduling and some other features 
    /// to enable easy and fast implementation of commonly seen operations in the games.
    /// </summary>

    [Serializable]
    public class MMNode : MMDrawEntity, IMMNode
    {
        public MMNode(string entityClass) 
            : base(entityClass)
        {
            this.Root = this;
        }

        #region Physical Data

        public Vector2 Position { get; set; }

        public Quaternion Roation { get; set; }

        public Vector2 Size { get; set; }

        #endregion

        #region Graphical Data

        public Color DrawColor { get; set; }

        public byte DrawOpacity { get; set; }

        public byte DrawDepth { get; set; }

        private int DrawOrderCurrent { get; set; }

        #endregion

        #region Organization Data and Operations

        public MMNode Parent { get; set; } = null;

        public MMNode Root { get; set; } 

        public MMNodeCollection Children { get; set; } = new MMNodeCollection();

        public virtual void Add(MMNode child)
        {
            if (child == null)
            {
                throw new ArgumentNullException(nameof(child));
            }

            if (child == this)
            {
                throw new InvalidOperationException("Can not add node to itself.");
            }

            if (child.Parent != null)
            {
                throw new InvalidOperationException("Node is already added. It can't be added again.");
            }

            this.Children.Add(child);

            child.DrawOrder = this.DrawOrderCurrent++;
            child.Parent = this;

            child.OnEnter();
        }

        public virtual void Remove(MMNode child, bool disposing)
        {
            if (child == null)
            {
                return;
            }

            if (this.Children.Contains(child))
            {
                this.Detach(child, disposing);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="node">
        /// </param>
        /// <param name="disposing">
        /// If you don't do cleanup, the child's actions will not get removed
        /// and the its ScheduledSelectors dictionary will not get released!
        /// </param>
        private void Detach(MMNode node, bool disposing)
        {
            if (this.BehaviorsEnabled)
            {
                node.OnExit();
            }

            this.Children.Remove(node);
            node.Parent = node;

            if (disposing)
            {
                node.Dispose();
            }
        }

        #endregion

        #region Transition Operations

        public virtual void OnEnter()
        {
            foreach (var child in this.Children.ToArray())
            {
                child.OnEnter();
            }
        }

        public virtual void OnExit()
        {
            // TODO
            //this.NodePause();
            //this.NodeRunning = false;

            foreach (var child in this.Children.ToArray())
            {
                child.OnExit();
            }
        }

        #endregion

        // TODO
        //#region Schedule Data and Operations

        //public bool NodeRunning { get; private set; }

        //public bool NodePaused => !this.NodeRunning;

        //public void NodeSchedule()
        //{
        //    this.NodeSchedule(0);
        //}

        //public void NodeSchedule(int priority)
        //{
        //    this.NodeActionScheduler.Schedule(this, priority, this.NodePaused);
        //}

        //public void NodeSchedule(Action<float> selector)
        //{
        //    this.Schedule(selector, 0.0f, CCSchedulePriority.RepeatForever, 0.0f);
        //}

        //public void NodeSchedule(Action<float> selector, float interval)
        //{
        //    this.Schedule(selector, interval, CCSchedulePriority.RepeatForever, 0.0f);
        //}

        //public void Schedule(Action<float> selector, float interval, uint repeat, float delay)
        //{
        //    if (selector == null)
        //    {
        //        throw new ArgumentNullException(nameof(selector));
        //    }

        //    if (interval < 0)
        //    {
        //        throw new ArgumentOutOfRangeException(nameof(interval), "Interval must be positive");
        //    }

        //    this.NodeActionScheduler.Schedule(selector, this, interval, repeat, delay, this.NodePaused);
        //}

        //public void NodeScheduleOnce(Action<float> selector, float delay)
        //{
        //    this.Schedule(selector, 0.0f, 0, delay);
        //}

        //public void NodeUnschedule()
        //{
        //    this.NodeActionScheduler.Unschedule(this);
        //}

        //public void NodeUnschedule(Action<float> selector)
        //{
        //    if (selector == null)
        //    {
        //        return;
        //    }

        //    this.NodeActionScheduler.Unschedule(selector, this);
        //}

        //public void NodeUnscheduleAll()
        //{
        //    this.NodeActionScheduler.UnscheduleAll(this);
        //}

        //public void NodeResume()
        //{
        //    this.NodeActionScheduler.ResumeTarget(this);
        //    this.NodeActionManager.ResumeTarget(this);

        //    if (EventDispatcher != null)
        //        EventDispatcher.Resume(this);
        //}

        //public void NodePause()
        //{
        //    this.NodeActionScheduler.PauseTarget(this);
        //    this.NodeActionManager.PauseTarget(this);

        //    if (EventDispatcher != null)
        //    {
        //        EventDispatcher.Pause(this);
        //    }
        //}

        //#endregion

        #region Update

        public override void Update(GameTime time)
        {
            if (this.UpdateEnabled)
            {
                foreach (var child in this.Children.ToArray())
                {
                    child.Update(time);
                }
            }
        }

        public virtual bool UpdateInput(GameTime time, MMNodeParams @params)
        {
            if (this.UpdateEnabled)
            {
                if (this.Children.ToArray().Any(child => child.UpdateInput(time, @params)))
                {
                    return true;
                }

                return true;
            }

            return false;
        }

        #endregion

        #region IDisposable

        private bool IsDisposed { get; set; }

        protected override void Dispose(bool disposing)
        {
            try
            {
                if (disposing)
                {
                    if (!this.IsDisposed)
                    {
                    }

                    this.IsDisposed = true;
                }
            }
            catch
            {
                // Ignored
            }
            finally
            {
                base.Dispose(disposing);
            }
        }

        #endregion
    }
}