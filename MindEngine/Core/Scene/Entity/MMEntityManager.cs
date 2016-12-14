namespace MindEngine.Core.Scene.Entity
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Threading;

    internal interface IMMEntityManager
    {
        void RegisterEntity(MMEntity entity);

        MMEntity GetEntity(int entityId);
    }

    [DataContract]
    public class MMEntityManager : IMMEntityManager
    {
        #region Constructors and Finalizer

        public MMEntityManager()
        {
        }

        #endregion

        #region Singleton

        private static MMEntityManager singleton;

        public static MMEntityManager Singleton
        {
            get
            {
                // Thread safe initialization
                if (singleton == null)
                {
                    Interlocked.CompareExchange<MMEntityManager>(ref singleton, new MMEntityManager(), null);
                }

                return singleton; 
            }
        }

        #endregion

        private int EntityCount { get; set; } = 0;

        private Dictionary<int, MMEntity> EntityTable { get; set; } = new Dictionary<int, MMEntity>();

        public void RegisterEntity(MMEntity entity)
        {
            entity.EntityId = Singleton.EntityCount++;

            this.EntityTable.Add(entity.EntityId, entity);
        }

        public MMEntity GetEntity(int entityId)
        {
            return this.EntityTable[entityId];
        }
    }
}