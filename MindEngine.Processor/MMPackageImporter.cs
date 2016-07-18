namespace MindEngine.Processor
{
    using Microsoft.Xna.Framework.Content.Pipeline;

    [ContentImporter(".xml", DisplayName = "Package Importer - Mind Engine")]
    internal class MMPackageImporter : ContentImporter<MMPackageXmlDocument>
    {
        public override MMPackageXmlDocument Import(string filename, ContentImporterContext context)
        {
            var doc = new MMPackageXmlDocument();
            doc.Load(filename);

            return doc;
        }
    }
}