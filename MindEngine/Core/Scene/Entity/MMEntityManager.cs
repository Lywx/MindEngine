namespace MindEngine.Core.Scene.Entity
{
    using System.Runtime.Serialization;

    [DataContract]
    public class MMEntityManager
    {
        public static MMEntityManager Singleton { get; set; } = new MMEntityManager();

        [DataMember]
        public int EntityCount { get; set; } = 0;

        public MMEntityManager()
        {
        }
    }
}