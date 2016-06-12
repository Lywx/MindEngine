namespace MindEngine.Core.Sessions
{
    using System.IO;
    using System.Runtime.Serialization;
    using System.Xml;
    using IO.Directory;

    [DataContract]
    public sealed class MMSession<TData, TController> : IMMSession<TData, TController>
        where TData : IMMSessionData, new()
        where TController : IMMSessionController<TData>, new()
    {
        #region Constructors

        private MMSession()
        {
            this.Data = new TData();
        }

        #endregion Constructors

        #region Singleton

        private static MMSession<TData, TController> Singleton { get; set; }

        #endregion Singleton

        #region Session Update

        public void Update()
        {
            this.Controller.Update();
            this.Data.Update();
        }

        #endregion Update

        #region File Data

        [DataMember]
        public static readonly string SaveFilename = "Session.xml";

        public static string SaveFilePath => MMDirectoryManager.SavePath(SaveFilename);

        #endregion

        #region Session Data 

        [DataMember]
        public TData Data { get; set; }

        /// <summary>
        ///     Controller is more like a not saved data container for session-wide
        ///     storage.
        /// </summary>
        public TController Controller { get; set; }

        #endregion

        #region Session Creation

        private static void CreateSession()
        {
            Singleton = new MMSession<TData, TController>
            {
                Controller = CreateController()
            };
        }

        private static TController CreateController()
        {
            return new TController
            {
                Data = Singleton.Data
            };
        }

        private static MMSession<TData, TController> GetSessionFromDerializer(DataContractSerializer deserializer, XmlDictionaryReader reader)
        {
            return (MMSession<TData, TController>)deserializer.ReadObject(reader, true);
        }

        #endregion

        #region Session Save and Load

        public void Save()
        {
            var serializer = new DataContractSerializer(typeof(MMSession<TData, TController>), null, int.MaxValue, false, true, null);
            try
            {
                using (var file = File.Create(SaveFilePath))
                {
                    serializer.WriteObject(file, Singleton);
                }
            }
            catch (IOException)
            {
                // Skip because there may be other instance trying to save
            }
        }

        public static MMSession<TData, TController> Load()
        {
            if (File.Exists(SaveFilePath))
            {
                // Auto-backup the old file
                File.Copy(SaveFilePath, SaveFilePath + ".bak", true);

                // Load from save
                Load(SaveFilePath);
            }
            else if (File.Exists(SaveFilePath + ".bak"))
            {
                // Load from the backup
                Load(SaveFilePath + ".bak");
            }
            else
            {
                CreateSession();
            }

            return Singleton;
        }

        private static void Load(string path)
        {
            using (var file = File.OpenRead(path))
            {
                try
                {
                    var deserializer = new DataContractSerializer(typeof(MMSession<TData, TController>));
                    using (var reader = XmlDictionaryReader.CreateTextReader(file, new XmlDictionaryReaderQuotas()))
                    {
                        Singleton = GetSessionFromDerializer(deserializer, reader);
                        Singleton.Controller = CreateController();
                    }
                }
                catch (SerializationException)
                {
                    CreateSession();
                }
                catch (XmlException)
                {
                    CreateSession();
                }
            }
        }

        #endregion
    }
}
