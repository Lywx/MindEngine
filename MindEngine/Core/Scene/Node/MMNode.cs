namespace MindEngine.Core.Scene.Node
{
    using System;
    using System.Linq;
    using Entity;
    using Microsoft.Xna.Framework;

    public interface IMMNode : IMMEntity, IMMUpdateableOperations
    {
        #region Common Draw Property

        bool DrawEnabled { get; set; }

        float DrawDepth { get; set; }

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
        protected MMNode() 
        {
            this.Root = this;
        }

        #region Entity

        public override string EntityClass => "Node";

        #endregion

        #region Graphics

        public float DrawDepth { get; set; } = 1f;

        private int DrawOrderCurrent { get; set; }

        #endregion

        #region Organization

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

            this.BeginAdd(child);
            this.Children.Add(child);
            this.EndAdd(child);

            child.OnEnter();
        }

        protected virtual void BeginAdd(MMNode child)
        {
            child.Parent?.Remove(child, false);
        }

        protected virtual void EndAdd(MMNode child)
        {
            child.Parent = this;
            child.Root = this.Root;
            child.DrawOrder = this.DrawOrderCurrent++;
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
        /// <param name="child">
        /// </param>
        /// <param name="disposing">
        /// If you don't do cleanup, the child's actions will not get removed
        /// and the its ScheduledSelectors dictionary will not get released!
        /// </param>
        private void Detach(MMNode child, bool disposing)
        {
            if (this.UpdateEnabled)
            {
                child.OnExit();
            }

            this.BeginDetach(child);
            this.Children.Remove(child);
            this.EndDetach(child);

            if (disposing)
            {
                child.Dispose();
            }
        }

        protected virtual void BeginDetach(MMNode child)
        {
            
        }

        protected virtual void EndDetach(MMNode child)
        {
            child.Root = child;
            child.Parent = null;
        }

        #endregion

        #region Transition

        public virtual void OnEnter()
        {
            foreach (var child in this.Children.ToArray())
            {
                child.OnEnter();
            }
        }

        public virtual void OnExit()
        {
            foreach (var child in this.Children.ToArray())
            {
                child.OnExit();
            }
        }

        #endregion

        #region Draw and Update

        protected override void DrawInternal(GameTime time)
        {
            this.Children.Draw(time);
        }

        protected override void UpdateInternal(GameTime time)
        {
            base.UpdateInternal(time);

            foreach (var child in this.Children.ToArray())
            {
                child.Update(time);
            }
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