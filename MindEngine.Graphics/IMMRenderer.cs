namespace MindEngine.Graphics
{
    using Core.Components;
    using Core.Contents.Fonts;
    using Core.Contents.Fonts.Alignment;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    public interface IMMRendererTextureOperaions
    {
        void Draw(Texture2D texture, int x, int y, Color color, float depth);

        void Draw(Texture2D texture, int x, int y, Rectangle? source, Color color, float depth);

        void Draw(Texture2D texture, Vector2 position, float depth);

        void Draw(Texture2D texture, Vector2 position, Vector2 size, Color color, float depth);

        void Draw(Texture2D texture, Rectangle destination, Rectangle? source, Color color, float depth);

        void Draw(Texture2D texture, Rectangle destination, Color color, float depth);
    }

    public interface IMMRendererStringOperations
    {
        /// <summary>
        ///     Draws the left-top mono-spaced text at particular position.
        /// </summary>
        void DrawMonospacedString(MMFont font, string str, Vector2 position, Color color, float scale);

        void DrawMonospacedString(MMFont font, string str, Vector2 position, Color color, float scale, HoritonalAlignment halignment, VerticalAlignment valignment, int leading = 0);

        /// <summary>
        ///     Draws the left-top text at particular position.
        /// </summary>
        void DrawString(MMFont font, string str, Vector2 position, Color color, float scale);

        void DrawString(MMFont font, string str, Vector2 position, Color color, float scale, HoritonalAlignment halignment, VerticalAlignment valignment, int leading = 0);
    }

    public interface IMMRendererBatchOperations
    {
        void Begin();

        void Begin(
            BlendState blendState,
            SamplerState samplerState = null,
            DepthStencilState depthStencilState = null,
            RasterizerState rasterizerState = null,
            Effect effect = null,
            Matrix? transformMatrix = null);

        void End();
    }

    public interface IMMRenderer : IMMDrawableComponent,
        IMMRendererStringOperations,
        IMMRendererTextureOperaions,
        IMMRendererBatchOperations
    {
    }
}
