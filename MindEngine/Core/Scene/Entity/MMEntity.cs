namespace MindEngine.Core.Scene.Entity
{
    using System;
    using System.Collections.Generic;

    public interface IMMEntity 
    {
        #region Fast RTTI

        string EntityClass { get; }

        int EntityId { get; }

        string EntityName { get; }

        #endregion

        #region Easy Connection

        void AttachConnection(MMEntity entity);

        void RemoveConnection(MMEntity entity);

        void RemoveConnections();

        #endregion
    }

    [Serializable]
    public class MMEntity : MMObject, IMMEntity
    {
        #region Fast RTTI

        public int EntityId { get; private set; } = MMEntityManager.Singleton.EntityCount++;

        public virtual string EntityClass { get; set; } = "Entity";

        public virtual string EntityName
        {
            get { return $"{this.EntityClass} {this.EntityId}"; }
            set { }
        }

        #endregion

        #region Constructors and Finalizer

        protected MMEntity()
        {
        }

        #endregion

        #region Easy Behavior

        public List<MMEntity> EntitiesKnown { get; set; } = new List<MMEntity>();

        public void AttachConnection(MMEntity entity)
        {
            this.EntitiesKnown.Add(entity);
        }

        public void RemoveConnection(MMEntity entity)
        {
            if (this.EntitiesKnown.Contains(entity))
            {
                this.EntitiesKnown.Remove(entity);
            }
        }

        public void RemoveConnections()
        {
            this.EntitiesKnown.Clear();
        }

        #endregion
    }
}