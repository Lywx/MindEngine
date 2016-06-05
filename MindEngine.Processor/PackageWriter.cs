namespace MindEngine.Processor
{
    using Microsoft.Xna.Framework.Content.Pipeline;
    using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;

    [ContentTypeWriter]
    internal class PackageWriter : ContentTypeWriter<PackageXmlDocument>
    {
        protected override void Write(
            ContentWriter output,
            PackageXmlDocument value)
        {
            output.Write(value.InnerXml);
        }

        public override string GetRuntimeType(TargetPlatform targetPlatform)
        {
            return typeof(PackageXmlDocument).AssemblyQualifiedName;
        }

        public override string GetRuntimeReader(TargetPlatform targetPlatform)
        {
            return "MindEngine.Core.Contents.Packages.MMPackageReader, MindEngine.Core";
        }
    }
}