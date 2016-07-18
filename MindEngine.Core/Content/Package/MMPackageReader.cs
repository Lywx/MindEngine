namespace MindEngine.Core.Content.Package
{
    using Microsoft.Xna.Framework.Content;

    public class MMPackageReader : ContentTypeReader<MMPackageXmlDocument>
    {
        /// <param name="input"></param>
        /// <param name="existingInstance">If is null, create a new XML document to load input</param>
        /// <returns></returns>
        protected override MMPackageXmlDocument Read(ContentReader input, MMPackageXmlDocument existingInstance)
        {
            if (existingInstance == null)
            {
                var doc = new MMPackageXmlDocument();
                doc.LoadXml(input.ReadString());

                return doc;
            }

            existingInstance.LoadXml(input.ReadString());
            return existingInstance;
        }
    }
}