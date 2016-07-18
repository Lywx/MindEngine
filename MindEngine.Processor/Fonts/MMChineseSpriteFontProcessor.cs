namespace MindEngine.Processor.Fonts
{
    using System.ComponentModel;
    using System.IO;
    using Microsoft.Xna.Framework.Content.Pipeline;
    using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
    using Microsoft.Xna.Framework.Content.Pipeline.Processors;

    /// <summary>
    /// This class will be instantiated by the MonoGame Framework Content Builder
    /// to apply custom processing to content data, converting an object of
    /// type TInput to TOutput. The input and output types may be the same if
    /// the processor wishes to alter data without changing its type.
    ///
    /// This should be part of a Content Pipeline Extension Library project.
    /// </summary>
    [ContentProcessor(DisplayName = "Chinese Sprite Font Description - Mind Engine")]
    public class MMChineseSpriteFontProcessor : FontDescriptionProcessor
    {
        public override SpriteFontContent Process(FontDescription input, ContentProcessorContext context)
        {
            var fullPath = Path.GetFullPath(this.CharacterSet);

            context.AddDependency(fullPath);

            var letters = File.ReadAllText(fullPath, System.Text.Encoding.UTF8);

            foreach (var c in letters)
            {
                input.Characters.Add(c);
            }

            return base.Process(input, context);
        }

        [DefaultValue("Chinese5931.txt")]
        [DisplayName("Chinese Character Set File")]
        [Description("The characters in this file will be automatically added to the font.")]
        public string CharacterSet { get; } = @"Fonts\Characters\Chinese5931.txt";
    }
}