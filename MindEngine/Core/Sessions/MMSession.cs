namespace MindEngine.Core.Sessions
{
    using System.IO;
    using System.Runtime.Serialization;
    using System.Xml;
    using IO.Directory;

    [DataContract]
    public sealed class MMSession<TData> : IMMSession<TData>
        where TData : IMMSessionData, new()
    {
        #region Constructors and Finalizer

        private MMSession()
        {
            this.Data = new TData();
        }

        #endregion Constructors

        #region Singleton

        private static MMSession<TData> Singleton { get; set; }

        #endregion Singleton

        #region Session Update

        public void Initialize()
        {
            this.Data.Initialize();
        }

        public void Update()
        {
            this.Data.Update();
        }

        #endregion Update

        #region Session Data 

        [DataMember]
        public static readonly string FileName = "Session.xml";

        public static string FilePath => MMDirectoryManager.SavePath(FileName);

        [DataMember]
        public TData Data { get; set; }

        #endregion

        #region Session Creation

        private static void Create()
        {
            Singleton = new MMSession<TData>();
        }

        private static MMSession<TData> Deserialize(DataContractSerializer deserializer, XmlDictionaryReader reader)
        {
            return (MMSession<TData>)deserializer.ReadObject(reader, true);
        }

        #endregion

        #region Session Save and Load

        public void Save()
        {
            var serializer = new DataContractSerializer(typeof(MMSession<TData>), null, int.MaxValue, false, true, null);
            try
            {
                using (var file = File.Create(FilePath))
                {
                    serializer.WriteObject(file, Singleton);
                }
            }
            catch (IOException)
            {
                // Skip because there may be other instance trying to save
            }
        }

        public static MMSession<TData> Load()
        {
            if (File.Exists(FilePath))
            {
                // Auto-backup the old file
                File.Copy(FilePath, FilePath + ".bak", true);

                // Load from save
                Load(FilePath);
            }
            else if (File.Exists(FilePath + ".bak"))
            {
                // Load from the backup
                Load(FilePath + ".bak");
            }
            else
            {
                Create();
            }

            return Singleton;
        }

        private static void Load(string path)
        {
            using (var file = File.OpenRead(path))
            {
                try
                {
                    var deserializer = new DataContractSerializer(typeof(MMSession<TData>));
                    using (var reader = XmlDictionaryReader.CreateTextReader(file, new XmlDictionaryReaderQuotas()))
                    {
                        Singleton = Deserialize(deserializer, reader);
                    }
                }
                catch (SerializationException)
                {
                    Create();
                }
                catch (XmlException)
                {
                    Create();
                }
            }
        }

        #endregion
    }
}
