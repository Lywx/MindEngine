namespace MindEngine.Core.Scene.Entity
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Xna.Framework;
    using Service.Event;
    using Service.Process;

    public interface IMMEntity : IMMEntityNode
    {
        #region Entity Identification

        string EntityClass { get; }

        int EntityId { get; }

        string EntityName { get; }

        List<string> EntityTag { get; set; }

        #endregion

        #region Entity Service

        void AttachProcess(MMProcess process, bool processEnter, bool processRun);

        void DetachProcess(string processName, bool processExit);

        MMProcess GetProcess(string processName);

        void AttachListener(MMEventListener listener);

        void DetachListener(string listenerName);

        void GetListener(string listenerName);

        #endregion

        #region Entity Behaviors and Components

        /// <summary>
        ///     This is only used for the versatile usage of the entity. This is
        ///     often used to implement something like "screen shaking" or many
        ///     different things without creating a whole lot of subclass.
        ///     It employs the multiple controllers "pattern" that I've seen in the
        ///     Wild Magic code.
        /// </summary>
        void AttachBehavior(int updateIndex, Action<GameTime> updateAction);

        void DetachBehavior(int updateIndex);

        void DetachBehaviors();

        void AddComponent(int updateIndex, MMEntityComponent component);

        void RemoveComponent(string componentName);

        void RemoveComponents();

        T GetComponent<T>(string componentName) where T : MMEntityComponent;

        #endregion
    }

    [Serializable]
    public class MMEntity : MMEntityNode, IMMEntity
    {
        #region Constructors and Finalizer

        protected MMEntity()
        {
            MMEntityManager.Singleton.RegisterEntity(this);
        }

        #endregion

        #region Entity Update

        protected override void UpdateInternal(GameTime time)
        {
            this.EntityComponents.Update(time);
        }

        #endregion

        #region Entity Identification

        public int EntityId { get; internal set; }

        public virtual string EntityClass { get; set; } = "Entity";

        public virtual string EntityName
        {
            get { return $"{this.EntityClass} {this.EntityId}"; }
            set { }
        }

        public List<string> EntityTag { get; set; } = new List<string>();

        #endregion

        #region Entity Service

        private Dictionary<string, MMProcess> EntityProcesses { get; } = new Dictionary<string, MMProcess>();

        private Dictionary<string, MMEventListener> EntityListeners { get; set; } = new Dictionary<string, MMEventListener>();

        public void AttachProcess(MMProcess process, bool processEnter, bool processRun)
        {
            this.EntityProcesses.Add(process.Name, process);

            EngineInterop.Process.AttachProcess(process);

            if (processEnter)
            {
                process.Enter();

                if (processRun)
                {
                    process.Run();
                }
            }
            else if (processRun)
            {
                throw new InvalidOperationException("Process does not allow to enter before trying to run.");
            }

            process.Exited += (sender, args) => this.DetachProcess(process.Name, false);
        }

        public void DetachProcess(string processName, bool processExit)
        {
            var process = this.GetProcess(processName);
            if (process != null)
            {
                if (processExit)
                {
                    process.Exit();
                }

                this.EntityProcesses.Remove(processName);
            }
        }

        public MMProcess GetProcess(string processName)
        {
            MMProcess process;
            if (this.EntityProcesses.TryGetValue(processName, out process))
            {
                return process;
            }

            return null;
        }

        public void AttachListener(MMEventListener listener)
        {
            throw new NotImplementedException();
        }

        public void DetachListener(string listenerName)
        {
            throw new NotImplementedException();
        }

        public void GetListener(string listenerName)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Entity Behavior and Component

        private MMEntityUpdatableList<MMEntityComponent> EntityComponents { get; } = new MMEntityUpdatableList<MMEntityComponent>();

        public void AddComponent(int updateIndex, MMEntityComponent component)
        {
            if (updateIndex == 0)
            {
                throw new InvalidOperationException($"0 update index is taken by {MMEntityUpdateBehavior.Name} component.");
            }

            component.UpdateOrder = updateIndex;
            this.EntityComponents.Add(component);
        }

        public void RemoveComponent(string componentName)
        {
            var component = GetComponent<MMEntityComponent>(componentName);
            if (component != null)
            {
                this.EntityComponents.Remove(component);
            }
        }

        public void RemoveComponents()
        {
            this.EntityComponents.Clear();
        }

        public T GetComponent<T>(string componentName) where T : MMEntityComponent
        {
            return (T)this.EntityComponents.Find(component => component.ComponentName == componentName);
        }

        public void AttachBehavior(int updateIndex, Action<GameTime> updateAction)
        {
            var updateComponent = this.GetComponent<MMEntityUpdateComponent>(MMEntityUpdateBehavior.Name);
            if (updateComponent == null)
            {
                updateComponent = new MMEntityUpdateComponent();

                // Update component has the top update priority.
                this.AddComponent(0, updateComponent);
            }

            updateComponent.AttachBehavior(new MMEntityUpdateBehavior(updateIndex, updateAction));
        }

        public void DetachBehavior(int updateIndex)
        {
            var updateComponent = this.GetComponent<MMEntityUpdateComponent>(MMEntityUpdateBehavior.Name);
            if (updateComponent == null)
            {
                return;
            }

            updateComponent.DetachBehavior(updateIndex);
        }

        public void DetachBehaviors()
        {
            var updateComponent = this.GetComponent<MMEntityUpdateComponent>(MMEntityUpdateBehavior.Name);
            if (updateComponent == null)
            {
                return;
            }

            updateComponent.ClearBehaviors();
        }

        #endregion
    }
}
