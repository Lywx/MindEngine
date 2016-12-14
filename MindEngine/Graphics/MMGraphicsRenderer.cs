namespace MindEngine.Graphics
{
    using System;
    using System.Globalization;
    using Core;
    using Core.Component;
    using Core.Content.Font;
    using Core.Content.Font.Extensions;
    using Core.Content.Text;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Primtives2D;

    public class MMGraphicsRenderer : MMCompositeComponent, IMMGraphicsRenderer
    {

        #region Constructors and Finalizer

        public MMGraphicsRenderer(MMEngine engine, IMMGraphicsDeviceController deviceController)
            : base(engine)
        {
            if (deviceController == null)
            {
                throw new ArgumentNullException(nameof(deviceController));
            }

            this.DeviceController = deviceController;
        }

        #endregion

        #region Render Data

        public IMMGraphicsDeviceController DeviceController { get; }

        private SpriteBatch SpriteBatch => this.DeviceController.SpriteBatch;

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

        /// <param name="font"></param>
        /// <param name="str"></param>
        /// <param name="position"></param>
        /// <param name="color"></param>
        /// <param name="scale"></param>
        /// <param name="halignment"></param>
        /// <param name="valignment"></param>
        /// <param name="leading">Vertical distance from line to line</param>
        public void DrawMonospacedString(MMFont font, string str, Vector2 position, Color color, float scale, MMHorizontalAlignment halignment = MMHorizontalAlignment.Right, MMVerticalAlignment valignment = MMVerticalAlignment.Bottom, int leading = 0)
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

                if (valignment == MMVerticalAlignment.Center)
                {
                    linePosition -= new Vector2(0, lineSize.Y / 2);
                }

                if (valignment == MMVerticalAlignment.Top)
                {
                    linePosition -= new Vector2(0, lineSize.Y);
                }

                if (halignment == MMHorizontalAlignment.Center)
                {
                    linePosition -= new Vector2(lineSize.X / 2, 0);
                }

                if (halignment == MMHorizontalAlignment.Left)
                {
                    linePosition -= new Vector2(lineSize.X, 0);
                }

                this.DrawMonospacedOneLineString(font, lineString, linePosition, color, scale);
            }
        }

        public void DrawMonospacedString(MMFont font, string str, Point position, Color color, float scale, MMHorizontalAlignment halignment = MMHorizontalAlignment.Right, MMVerticalAlignment valignment = MMVerticalAlignment.Bottom, int leading = 0)
        {
            this.DrawMonospacedString(font, str, position.ToVector2(), color, scale, halignment, valignment, leading);
        }

        private void DrawMonospacedOneLineString(MMFont font, string str, Vector2 position, Color color, float scale)
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

        private void DrawMonospacedChar(MMFont font, char c, Vector2 position, Color color, float scale)
        {
            var str = c.ToString(CultureInfo.InvariantCulture);

            position += font.MonoData.Offset + new Vector2(-font.MeasureString(str, scale).X / 2, 0);

            this.DrawOneLineString(font, str, position, color, scale);
        }

        public void DrawString(MMFont font, string text, Point position, Color color, float scale, MMHorizontalAlignment halignment = MMHorizontalAlignment.Right, MMVerticalAlignment valignment = MMVerticalAlignment.Bottom, int leading = 0)
        {
            this.DrawString(font, text, position.ToVector2(), color, scale, halignment, valignment, leading);
        }

        public void DrawString(MMFont font, string str, Vector2 position, Color color, float scale, MMHorizontalAlignment halignment = MMHorizontalAlignment.Right, MMVerticalAlignment valignment = MMVerticalAlignment.Bottom, int leading = 0)
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

                if (valignment == MMVerticalAlignment.Center)
                {
                    linePosition -= new Vector2(0, lineSize.Y / 2);
                }

                if (valignment == MMVerticalAlignment.Top)
                {
                    linePosition -= new Vector2(0, lineSize.Y);
                }

                if (halignment == MMHorizontalAlignment.Center)
                {
                    linePosition -= new Vector2(lineSize.X / 2, 0);
                }

                if (halignment == MMHorizontalAlignment.Left)
                {
                    linePosition -= new Vector2(lineSize.X, 0);
                }

                this.DrawOneLineString(font, lineString, linePosition, color, scale);
            }
        }

        private void DrawOneLineString(MMFont font, string str, Vector2 position, Color color, float scale)
        {
            if (string.IsNullOrEmpty(str))
            {
                return;
            }

            this.SpriteBatch.DrawString(font.SpriteData, font.AvailableString(str), position, color, 0f, Vector2.Zero, scale, SpriteEffects.None, 0);
        }

        #endregion

        public void DrawRectangle(Rectangle rectangle, Color color, float thickness = 1f)
        {
            Primitives2D.DrawRectangle(this.SpriteBatch, rectangle, color, thickness);
        }
        
        public void DrawRectangle(Rectangle rectangle, Vector2 position, Vector2 size, Color color, float thickness = 1f)
        {
            Primitives2D.DrawRectangle(this.SpriteBatch, position, size, color, thickness);
        }

        #region Draw Helpers

        public void Draw(Texture2D texture, Vector2 position, float depth)
        {
            this.Draw(texture, position, null, Color.White, depth);
        }

        public void Draw(Texture2D texture, int x, int y, Color color, float depth)
        {
            this.Draw(texture, x, y, null, color, depth);
        }

        public void Draw(Texture2D texture, int x, int y, Rectangle? source, Color color, float depth)
        {
            this.Draw(texture, new Vector2(x, y), source, color, depth);
        }

        public void Draw(Texture2D texture, Vector2 position, Vector2 size, Color color, float depth)
        {
            this.Draw(texture, new Rectangle(position.ToPoint(), size.ToPoint()), color, depth);
        }

        private void Draw(Texture2D texture, Vector2 position, Rectangle? source, Color color, float depth)
        {
            this.SpriteBatch.Draw(texture, position, source, color, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, depth);
        }

        public void Draw(Texture2D texture, Rectangle destination, Color color, float depth)
        {
            this.SpriteBatch.Draw(texture, destination, null, color, 0.0f, Vector2.Zero, SpriteEffects.None, depth);
        }

        public void Draw(Texture2D texture, Rectangle destination, Rectangle? source, Color color, float depth)
        {
            this.SpriteBatch.Draw(texture, destination, source, color, 0.0f, Vector2.Zero, SpriteEffects.None, depth);
        }

        #endregion
    }
}
