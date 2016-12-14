namespace MindEngine.Core.Scene.Entity
{
    using System;
    using System.Linq;
    using Microsoft.Xna.Framework;

    public interface IMMEntityNode : IMMEntityDrawable
    {
        #region Hierarchy Semantics

        void AddNode(MMEntityNode child);

        void RemoveNode(MMEntityNode child, bool disposing);

        void RemoveNodes(bool disposing);

        MMEntityNode NodeParent { get; set; }

        MMEntityNode NodeRoot { get; set; }

        MMEntityNodeList NodeChildren { get; }

        #endregion

        #region Scene Semantics

        void OnEnterScene();

        void OnExitScene();

        #endregion
    }

    /// <summary>
    /// Nodes are built in order to support to a basic scene graph system.
    /// </summary>
    [Serializable]
    public class MMEntityNode : MMEntityDrawable, IMMEntityNode
    {
        protected MMEntityNode() 
        {
            this.NodeRoot = this;
        }

        #region Hierarchy Semantics

        public MMEntityNode NodeParent { get; set; } = null;

        public MMEntityNode NodeRoot { get; set; } 

        public MMEntityNodeList NodeChildren { get; set; } = new MMEntityNodeList();

        public virtual void AddNode(MMEntityNode child)
        {
            if (child == null)
            {
                throw new ArgumentNullException(nameof(child));
            }

            if (child == this)
            {
                throw new InvalidOperationException("Can not add node to itself.");
            }

            if (child.NodeParent != null)
            {
                throw new InvalidOperationException("Node is already added. It can't be added again.");
            }

            this.AttachNode(child);
        }

        private void AttachNode(MMEntityNode child)
        {
            this.OnAttachNodeBegin(child);
            this.NodeChildren.Add(child);
            this.OnAttachNodeEnd(child);

            if (this.UpdateEnabled && child.UpdateEnabled)
            {
                child.OnInitScene();
                child.OnEnterScene();
            }
        }

        protected virtual void OnAttachNodeBegin(MMEntityNode child)
        {
            child.NodeParent?.RemoveNode(child, false);
        }

        protected virtual void OnAttachNodeEnd(MMEntityNode child)
        {
            child.NodeParent = this;
            child.NodeRoot = this.NodeRoot;
            child.DrawOrder = this.NodeDrawOrderCurrent++;
        }

        public virtual void RemoveNode(MMEntityNode child, bool disposing)
        {
            if (child == null)
            {
                return;
            }

            if (this.NodeChildren.Contains(child))
            {
                this.DetachNode(child, disposing);
            }
        }

        public void RemoveNodes(bool disposing)
        {
            for (int i = 0; i < this.NodeChildren.Count; --i)
            {
                this.RemoveNode(this.NodeChildren[i], disposing);
            }

            this.NodeDrawOrderCurrent = 0;
        }

        private void DetachNode(MMEntityNode child, bool disposing)
        {
            if (this.UpdateEnabled && child.UpdateEnabled)
            {
                child.OnExitScene();
            }

            this.OnDetachNodeBegin(child);
            this.NodeChildren.Remove(child);
            this.OnDetachNodeEnd(child);

            if (disposing)
            {
                child.Dispose();
            }
        }

        protected virtual void OnDetachNodeBegin(MMEntityNode child)
        {
            
        }

        protected virtual void OnDetachNodeEnd(MMEntityNode child)
        {
            child.NodeRoot = child;
            child.NodeParent = null;
        }

        #endregion

        #region Scene Semantics

        private int NodeDrawOrderCurrent { get; set; } = 0;

        protected virtual void BringNodeToFront(int times = 1)
        {
            var nodeList = this.NodeParent.NodeChildren.ItemsOf<MMEntityDrawable>();

            // When this node is NOT already the front-most node
            var nodeIndex = nodeList.IndexOf(this);
            var nodeIndexLast = nodeList.Count - 1;
            if (nodeIndex < nodeIndexLast)
            {
                var timesPossible = Math.Min(nodeIndexLast - nodeIndex, times);

                // Bubble sorting style swapping
                for (var i = 0; i < timesPossible; ++i)
                {
                    // node that is just in front of this node
                    var nodeFront = nodeList[nodeIndex++];

                    var swapDrawOrder = nodeFront.DrawOrder;
                    nodeFront.DrawOrder = this.DrawOrder;
                    this.DrawOrder = swapDrawOrder;
                }

                // Update draw collection order
                nodeList.Rebuild();
            }
        }

        protected virtual void BringNodeToBack(int times = 1)
        {
            var nodeList = this.NodeParent.NodeChildren.ItemsOf<MMEntityDrawable>();

            // When this node is NOT already the front-most node
            var nodeIndex = nodeList.IndexOf(this);
            var nodeIndexFirst = 0;
            if (nodeIndex > nodeIndexFirst)
            {
                var timesPossible = Math.Min(nodeIndex - nodeIndexFirst, times);

                // Bubble sorting style swapping
                for (var i = 0; i < timesPossible; ++i)
                {
                    // node that is just in front of this node
                    var nodeFront = nodeList[nodeIndex--];

                    var swapDrawOrder = nodeFront.DrawOrder;
                    nodeFront.DrawOrder = this.DrawOrder;
                    this.DrawOrder = swapDrawOrder;
                }

                // Update draw collection order
                nodeList.Rebuild();
            }
        }

        /// <remarks>
        /// Post-order tree traversal.
        /// </remarks> 
        protected virtual void OnInitScene()
        {
            foreach (var child in this.NodeChildren.ToArray())
            {
                child.OnInitScene();
            }
        }

        /// <remarks>
        /// Post-order tree traversal.
        /// </remarks> 
        public virtual void OnEnterScene()
        {
            foreach (var child in this.NodeChildren.ToArray())
            {
                child.OnEnterScene();
            }
        }

        /// <remarks>
        /// Post-order tree traversal.
        /// </remarks> 
        public virtual void OnExitScene()
        {
            foreach (var child in this.NodeChildren.ToArray())
            {
                child.OnExitScene();
            }
        }

        #endregion

        #region Draw and Update

        protected override void DrawInternal(GameTime time)
        {
            this.NodeChildren.Draw(time);
        }

        protected override void UpdateInternal(GameTime time)
        {
            base.UpdateInternal(time);

            foreach (var child in this.NodeChildren.ToArray())
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