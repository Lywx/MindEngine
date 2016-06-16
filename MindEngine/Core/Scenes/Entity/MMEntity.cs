namespace MindEngine.Core.Scenes.Entity
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

        #endregion
    }

    [Serializable]
    public class MMEntity : MMObject, IMMEntity
    {
        #region Fast RTTI

        public int EntityId { get; private set; } = MMEntityManager.Singleton.EntityCount++;

        public string EntityClass { get; private set; }

        public string EntityName => $"{this.EntityClass} {this.EntityId}";

        #endregion

        #region Constructors and Finalizer

        protected MMEntity(string entityClass)
        {
            if (entityClass == null)
            {
                throw new ArgumentNullException(nameof(entityClass));
            }

            this.EntityClass = entityClass;
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

        #endregion
    }
}