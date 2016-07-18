namespace MindEngine.Core
{
    using Component;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    public interface IMMGraphicsDeviceController : IMMGameComponent
    {
        SpriteBatch SpriteBatch { get; }

        //TODO(Wuxiang)
        //MMViewportAdapter ViewportAdapter { get; set; }

        Viewport Viewport { get; }

        Rectangle ScissorRectangle { get; set; }

        /// <reference>
        /// http://gamedev.stackexchange.com/questions/24697/how-to-clip-cut-off-text-in-a-textbox 
        /// </reference>
        /// <summary>
        /// Whether enable scissor in rasterization. 
        /// </summary>
        /// <remarks>
        /// Set it to true, before using the scissor. After drawing using sprite
        /// batch, you could set it to false before sprite batch end.
        /// </remarks>
        bool ScissorRectangleEnabled { get; set; }

        bool VertexColorEnabled { get; set; }

        bool TextureEnabled { get; set; }

        void SetRenderTarget(RenderTarget2D target);

        void RestoreRenderTarget();

        void MatrixMultiply(Matrix matrixIn);

        void MatrixPush();

        void MatrixPop();

        void MatrixSetIdentity();
    }
}