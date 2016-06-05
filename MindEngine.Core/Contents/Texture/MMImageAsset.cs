namespace MindEngine.Core.Contents.Texture
{
    using Assets;
    using Microsoft.Xna.Framework.Graphics;

    public class MMImageAsset : MMAsset
    {
        public MMImageAsset(string name, string asset, MMImageDesign design) 
            : base(name, asset)
        {
            this.Design = design;
        }

        #region Resource

        public MMImageDesign Design { get; set; }

        public Texture2D Resource { get; set; }

        #endregion

        #region Conversion

        public MMImage ToImage()
        {
            return new MMImage(this.Design, this.Resource);
        }

        #endregion
    }
}
