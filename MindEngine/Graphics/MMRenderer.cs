namespace MindEngine.Graphics
{
    using Core.Contents.Fonts;
    using Core.Contents.Fonts.Alignment;
    using Core.Contents.Fonts.Extensions;
    using Core.Scenes;
    using System.Globalization;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    public class MMRenderer : MMObject, IMMRenderer
    {
        #region Constructors

        public MMRenderer()
        {
        }

        #endregion

        #region Render Data

        private SpriteBatch SpriteBatch => this.GlobalGraphicsDeviceController.SpriteBatch;

        #endregion

        #region Initialization

        public bool Initialized { get; private set; }

        public void Initialize()
        {
            this.Initialized = true;
        }

        #endregion

        #region Batch Operations

        public void Begin()
        {
            this.SpriteBatch.Begin();
        }

        public void Begin(
            BlendState blendState,
            SamplerState samplerState = null,
            DepthStencilState depthStencilState = null,
            RasterizerState rasterizerState = null,
            Effect effect = null,
            Matrix? transformMatrix = null)
        {
            this.SpriteBatch.Begin(SpriteSortMode.Immediate, blendState, samplerState, depthStencilState, rasterizerState, effect, transformMatrix);
        }

        public void End()
        {
            this.SpriteBatch.End();
        }

        #endregion

        #region Draw String

        public void DrawMonospacedString(MMFont font, string str, Vector2 position, Color color, float scale)
        {
            if (string.IsNullOrEmpty(str))
            {
                return;
            }

            var displayable = font.AvailableString(str);

            var CJKCharIndexes = displayable.CJKUniqueCharIndexes();
            var CJKCharAmendedPosition = displayable.CJKUniqueCharAmendedPosition(CJKCharIndexes);

            var isCJKCharExisting = CJKCharIndexes.Count > 0;

            for (var i = 0; i < displayable.Length; ++i)
            {
                var charPosition = isCJKCharExisting ? CJKCharAmendedPosition[i] : i;
                var amendedPosition = position + new Vector2(charPosition * font.MonoData.AsciiSize(scale).X, 0);

                this.DrawMonospacedChar(font, displayable[i], amendedPosition, color, scale);
            }
        }

        /// <param name="font"></param>
        /// <param name="str"></param>
        /// <param name="position"></param>
        /// <param name="color"></param>
        /// <param name="scale"></param>
        /// <param name="halignment"></param>
        /// <param name="valignment"></param>
        /// <param name="leading">Vertical distance from line to line</param>
        public void DrawMonospacedString(MMFont font, string str, Vector2 position, Color color, float scale, HoritonalAlignment halignment, VerticalAlignment valignment, int leading = 0)
        {
            if (string.IsNullOrEmpty(str))
            {
                return;
            }

            if (leading == 0)
            {
                leading = (int)font.MonoData.AsciiSize(scale).Y * 2;
            }

            var lines = str.Split('\n');

            for (var i = 0; i < lines.Length; ++i)
            {
                var line = lines[i];

                var lineString = font.AvailableString(line);
                var lineSize = font.MeasureMonospacedString(lineString, scale);

                var linePosition = position;

                linePosition += new Vector2(0, i * leading);

                if (valignment == VerticalAlignment.Center)
                {
                    linePosition -= new Vector2(0, lineSize.Y / 2);
                }

                if (valignment == VerticalAlignment.Top)
                {
                    linePosition -= new Vector2(0, lineSize.Y);
                }

                if (halignment == HoritonalAlignment.Center)
                {
                    linePosition -= new Vector2(lineSize.X / 2, 0);
                }

                if (halignment == HoritonalAlignment.Left)
                {
                    linePosition -= new Vector2(lineSize.X, 0);
                }

                this.DrawMonospacedString(font, lineString, linePosition, color, scale);
            }
        }

        private void DrawMonospacedChar(MMFont font, char c, Vector2 position, Color color, float scale)
        {
            var str = c.ToString(CultureInfo.InvariantCulture);

            position += font.MonoData.Offset + new Vector2(-font.MeasureString(str, scale).X / 2, 0);

            this.DrawString(font, str, position, color, scale);
        }

        public void DrawString(MMFont font, string str, Vector2 position, Color color, float scale)
        {
            if (string.IsNullOrEmpty(str))
            {
                return;
            }

            this.SpriteBatch.DrawString(font.SpriteData, font.AvailableString(str), position, color, 0f, Vector2.Zero, scale, SpriteEffects.None, 0);
        }

        public void DrawString(MMFont font, string str, Vector2 position, Color color, float scale, HoritonalAlignment halignment, VerticalAlignment valignment, int leading = 0)
        {
            if (string.IsNullOrEmpty(str))
            {
                return;
            }

            if (leading == 0)
            {
                leading = (int)font.MonoData.AsciiSize(scale).Y * 2;
            }

            var lines = str.Split('\n');

            for (var i = 0; i < lines.Length; ++i)
            {
                var line = lines[i];

                var lineString = font.AvailableString(line);
                var lineSize = font.MeasureString(lineString, scale);

                var linePosition = position;

                linePosition += new Vector2(0, i * leading);

                if (valignment == VerticalAlignment.Center)
                {
                    linePosition -= new Vector2(0, lineSize.Y / 2);
                }

                if (valignment == VerticalAlignment.Top)
                {
                    linePosition -= new Vector2(0, lineSize.Y);
                }

                if (halignment == HoritonalAlignment.Center)
                {
                    linePosition -= new Vector2(lineSize.X / 2, 0);
                }

                if (halignment == HoritonalAlignment.Left)
                {
                    linePosition -= new Vector2(lineSize.X, 0);
                }

                this.DrawString(font, lineString, linePosition, color, scale);
            }
       }

        #endregion

        #region Draw Helpers

        public void Draw(Texture2D texture, Rectangle destination, Color color, float depth)
        {
            if (destination.Width > 0 &&
                destination.Height > 0)
            {
                this.SpriteBatch.Draw(texture, destination, null, color, 0.0f, Vector2.Zero, SpriteEffects.None, depth);
            }
        }

        public void Draw(Texture2D texture, Rectangle destination, Rectangle source, Color color, float depth)
        {
            if (source.Width > 0 &&
                source.Height > 0 &&
                destination.Width > 0 &&
                destination.Height > 0)
            {
                this.SpriteBatch.Draw(texture, destination, source, color, 0.0f, Vector2.Zero, SpriteEffects.None, depth);
            }
        }

        public void Draw(Texture2D texture, int x, int y, Color color, float depth)
        {
            this.SpriteBatch.Draw(texture, new Vector2(x, y), null, color, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, depth);
        }

        public void Draw(Texture2D texture, int x, int y, Rectangle source, Color color, float depth)
        {
            if (source.Width > 0 &&
                source.Height > 0)
            {
                this.SpriteBatch.Draw(texture, new Vector2(x, y), source, color, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, depth);
            }
        }

        #endregion
    }
}