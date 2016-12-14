namespace MindEngine.Core.Scene.Entity
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
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

        void AttachProcess(MMProcess process);

        void DetachProcess(string processName);

        void GetProcess(string processName);

        void AttachListener(MMEventListener listener);

        void DetachListener(string listenerName);

        void GetListener(string listenerName);

        #endregion

        #region Entity Behaviors and Components

        /// <summary>
        /// This is only used for the versatile usage of the entity. This is 
        /// often used to implement something like "screen shaking" or many 
        /// different things without creating a whole lot of subclass. 
        /// 
        /// It employs the multiple controllers "pattern" that I've seen in the 
        /// Wild Magic code.
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

        #region Constructors and Finalizer

        protected MMEntity()
        {
            MMEntityManager.Singleton.RegisterEntity(this);
        }

        #endregion

        #region Entity Service

        private Dictionary<string, MMProcess> EntityProcesses { get; set; } = new Dictionary<string, MMProcess>();

        public void AttachProcess(MMProcess process)
        {
            this.EntityProcesses.Add(process.Name, process);
            process.OnExit();
            TODO;
            this.EngineInterop.Process.AttachProcess(process);
            process.Enter();
            
        }

        public void DetachProcess(string processName)
        {
            throw new NotImplementedException();
        }

        public void GetProcess(string processName)
        {
            throw new NotImplementedException();
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

        private Dictionary<string, MMEntityComponent> EntityComponents { get; set; } = new Dictionary<string, MMEntityComponent>();

        public void AddComponent(int updateIndex, MMEntityComponent component)
        {
            TODO;
            this.EntityComponents.Add(component.ComponentName, component);
        }

        public void RemoveComponent(string componentName)
        {
            this.EntityComponents.Remove(componentName);
        }

        public void RemoveComponents()
        {
            this.EntityComponents.Clear();
        }

        public T GetComponent<T>(string componentName) where T : MMEntityComponent
        {
            return this.EntityComponents[componentName] as T;
        }

        public void AttachBehavior(int updateIndex, Action<GameTime> updateAction)
        {
            MMEntityComponent entityComponent;

            if (!this.EntityComponents.TryGetValue(MMEntityUpdateBehavior.Name, out entityComponent))
            {
                // Update component has the top update priority.
                entityComponent = new MMEntityUpdateComponent(0);
                this.EntityComponents.Add(entityComponent.ComponentName, entityComponent);
            }

            var updateComponent = (MMEntityUpdateComponent)entityComponent;
            updateComponent.AddBehavior(new MMEntityUpdateBehavior(updateIndex, updateAction));
        }

        public void DetachBehavior(int updateIndex)
        {
            MMEntityComponent entityComponent;
            if (!this.EntityComponents.TryGetValue(MMEntityUpdateBehavior.Name, out entityComponent))
            {
                return;
            }

            var updateComponent = (MMEntityUpdateComponent)entityComponent;
            updateComponent.RemoveBehavior(updateIndex);
        }

        public void DetachBehaviors()
        {
            MMEntityComponent entityComponent;
            if (!this.EntityComponents.TryGetValue(MMEntityUpdateBehavior.Name, out entityComponent))
            {
                return;
            }

            var updateComponent = (MMEntityUpdateComponent)entityComponent;
            updateComponent.ClearBehaviors();
        }

        #endregion

        #region Entity Update

        protected override void UpdateInternal(GameTime time)
        {
            foreach (var behavior in this.EntityComponents.Values.ToArray())
            {
                behavior.Update(time);
            }
        }

        #endregion
    }
}