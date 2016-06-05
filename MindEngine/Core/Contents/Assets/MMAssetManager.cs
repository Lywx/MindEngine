namespace MindEngine.Core.Contents.Assets
{
    using System;
    using System.IO;
    using System.Xml;
    using Components;
    using Extensions;
    using Fonts;
    using IO.Directory;
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Graphics;
    using NLog;
    using Packages;
    using Parser;
    using Texture;

    public class MMAssetManager : MMCompositeComponent, IMMAssetManager
    {
        #region Logger

        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        #endregion

        #region Dependency

        protected ContentManager Content => this.Engine.Content;

        #endregion

        #region Asset Data

        public IMMFontManager Fonts { get; private set; }

        public IMMTextureManager Texture { get; private set; }

        private MMAssetDictionary Assets { get; set; } = new MMAssetDictionary();

        //private Controls

        #endregion

        #region Constructors

        public MMAssetManager(MMEngine engine) : base(engine)
        {
            this.Fonts   = new MMFontManager(engine);
            this.Texture = new MMTextureManager(engine);
        }

        #endregion

        #region Path

        private string AssetPath(string catalog, string asset) => Path.Combine(this.AssetCatalogPath(catalog), asset);

        /// <summary>
        /// The path used in Load method in ContentManager, which doesn't include ".\\Content\" prefix.
        /// </summary>
        private string AssetCatalogPath(string catalog) => catalog + @"\";

        #endregion

        #region Initialization

        public override void Initialize()
        {
            base.Initialize();
            this.Fonts.Initialize();
            this.Texture.Initialize();
        }

        #endregion

        #region Load and Unload 

        protected override void LoadContent()
        {
            base.LoadContent();

            // Load engine persistent asset
            this.LoadPackage("Engine.Persistent");
        }

        public void LoadPackage(string packageName, bool async = false)
        {
            this.ReadPackage(packageName);

            foreach (var font in this.Assets[packageName].Fonts.Values)
            {
                this.LoadFont(font, async);
            }

            foreach (var texture in this.Assets[packageName].Texture.Values)
            {
                this.LoadImage(texture, async);
            }
        }

        private void LoadFont(MMFontAsset font, bool async = false)
        {
            var spriteFontPath = this.AssetPath("Fonts", font.Asset);

            if (async)
            {
                this.Content.LoadAsync<SpriteFont>(
                    spriteFontPath,
                    spriteFont => font.Resource = spriteFont);
            }
            else
            {
                font.Resource = this.Content.Load<SpriteFont>(spriteFontPath);
            }

            this.Fonts.Add(font);
        }

        private void LoadImage(MMImageAsset image, bool async = false)
        {
            var texturePath = this.AssetPath("Texture", image.Asset);

            if (async)
            {
                this.Content.LoadAsync<Texture2D>(
                    texturePath,
                    texture2D => image.Resource = texture2D);
            }
            else
            {
                image.Resource = this.Content.Load<Texture2D>(texturePath);
            }

            this.Texture.Add(image);
        }

        public void UnloadPackage(string packageName)
        {
            foreach (var font in this.Assets[packageName].Fonts.Values)
            {
                this.UnloadFont(font);
            }

            foreach (var texture in this.Assets[packageName].Texture.Values)
            {
                this.UnloadImage(texture);
            }
        }

        private void UnloadFont(MMFontAsset font)
        {
            this.Assets.GetFont(font.Name).Resource = null;
            this.Fonts.Remove(font);
        }

        private void UnloadImage(MMImageAsset image)
        {
            this.Assets.GetTexture(image.Name).Resource = null;
            this.Texture.Remove(image);
        }

        #endregion

        #region XML Package Operations

        public void ReadPackage(string packageName)
        {
            if (this.Assets.ContainsKey(packageName))
            {
                return;
            }

            var packageDocument = this.Content.Load<MMPackageXmlDocument>(packageName);

            try
            {
                var e = packageDocument["Package"];
                if (e != null)
                {
                    var package = new MMPackageAsset(packageName, packageName);
                    this.Assets.Add(packageName, package);

                    this.ReadFonts(package, e);
                    this.ReadImages(package, e);
                }
                else
                {
                    logger.Error($@"Unable to load package file: cannot find ""Package"" element in {packageName}.");
                }
            }
            catch (Exception e)
            {
                throw new Exception($"Unable to load package file. {e.Message}");
            }
        }

        private void ReadFonts(MMPackageAsset package, XmlElement packageElement)
        {
            var fontList = this.ReadAssetList(packageElement, "Fonts", "Font");
            if (fontList?.Count > 0)
            {
                foreach (XmlElement fontElement in fontList)
                {
                    var fontName = MMXmlReader.ReadAttributeString(fontElement, "Name", null, true);
                    var fontSize = MMXmlReader.ReadAttributeInt(fontElement, "Size", 0, true);
                    var fontAsset = MMXmlReader.ReadAttributeString(fontElement, "Asset", null, true);

                    var font = new MMFontAsset(fontName, fontAsset, fontSize);
                    if (!package.Fonts.ContainsKey(font.Name))
                    {
                        package.Add(font);
                    }
                }
            }
        }

        private void ReadImages(MMPackageAsset package, XmlElement packageElement)
        {
            var imageList = this.ReadAssetList(packageElement, "Texture", "Image");
            if (imageList?.Count > 0)
            {
                foreach (XmlElement imageElement in imageList)
                {
                    var imageName = MMXmlReader.ReadAttributeString(imageElement, "Name", null, true);
                    var imageAsset = MMXmlReader.ReadAttributeString(imageElement, "Asset", null, true);
                    var imageDesign = this.ReadImageDesign(imageElement);

                    var image = new MMImageAsset(imageName, imageAsset, imageDesign);

                    if (!package.Texture.ContainsKey(image.Name))
                    {
                        package.Add(image);
                    }
                }
            }
        }

        private MMImageDesign ReadImageDesign(XmlElement imageElement)
        {
            var designElement = imageElement["Design"];

            var screenWidth = MMXmlReader.ReadAttributeInt(designElement, "ScreenWidth", 0, true);
            var screenHeight = MMXmlReader.ReadAttributeInt(designElement, "ScreenHeight", 0, true);

            return new MMImageDesign(screenWidth, screenHeight);
        }

        private XmlNodeList ReadAssetList(XmlElement contentElement, string catalogName, string itemName)
        {
            var catalogElement = contentElement[catalogName];
            if (catalogElement == null)
            {
                logger.Warn($@"Failed to load catalog ""{catalogName}""");

                return null;
            }

            var itemList = catalogElement.GetElementsByTagName(itemName);
            return itemList;
        }

        #endregion

        #region IDisposable

        private bool IsDisposed { get; set; }

        protected override void Dispose(bool disposing)
        {
            try
            {
                if (disposing)
                {
                    if (!this.IsDisposed)
                    {
                        this.Fonts?.Dispose();
                        this.Fonts = null;
                    }

                    this.IsDisposed = true;
                }
            }
            catch
            {
                // Ignored
            }
            finally
            {
                base.Dispose(disposing);
            }
        }

        #endregion
    }
}