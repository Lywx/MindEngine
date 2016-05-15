namespace MindEngine.Processor
{
    using Microsoft.Xna.Framework.Content.Pipeline;

    [ContentImporter(".xml", DisplayName = "Package Importer - Meta Mind")]
    internal class PackageImporter : ContentImporter<PackageXmlDocument>
    {
        public override PackageXmlDocument Import(string filename, ContentImporterContext context)
        {
            var doc = new PackageXmlDocument();
            doc.Load(filename);

            return doc;
        }
    }
}