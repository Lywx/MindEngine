namespace MindEngine.Processor
{
    using Microsoft.Xna.Framework.Content.Pipeline;
    using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;

    [ContentTypeWriter]
    internal class MMPackageWriter : ContentTypeWriter<MMPackageXmlDocument>
    {
        protected override void Write(
            ContentWriter output,
            MMPackageXmlDocument value)
        {
            output.Write(value.InnerXml);
        }

        public override string GetRuntimeType(TargetPlatform targetPlatform)
        {
            return typeof(MMPackageXmlDocument).AssemblyQualifiedName;
        }

        public override string GetRuntimeReader(TargetPlatform targetPlatform)
        {
            return "MindEngine.Core.Content.Package.MMPackageReader, MindEngine.Core";
        }
    }
}