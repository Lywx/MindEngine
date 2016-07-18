namespace MindEngine.Core.Content.Asset
{
    using System;
    using System.IO;
    using System.Xml;
    using Component;
    using Content.Extension;
    using Content.Package;
    using Cursor;
    using Font;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Graphics;
    using NLog;
    using Parser;
    using Texture;

    // TODO: The NLOG config file is not setup yet.
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

        public IMMCursorManager Cursors { get; private set; }

        private MMAssetDictionary Assets { get; set; } = new MMAssetDictionary();

        #endregion

        #region Constructors

        public MMAssetManager(MMEngine engine) : base(engine)
        {
            this.Fonts   = new MMFontManager(engine);
            this.Texture = new MMTextureManager(engine);
            this.Cursors = new MMCursorManager(engine);
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
            this.Cursors.Initialize();
        }

        #endregion

        #region Load and Unload 

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

            foreach (var cursor in this.Assets[packageName].Cursors.Values)
            {
                this.LoadCursor(cursor, async);
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

        private void LoadCursor(MMCursorAsset cursor, bool async = false)
        {
            if (async)
            {
                this.Content.LoadAsync<Texture2D>(this.AssetPath("Cursors", cursor.Design.NormalSelect)       , texture2D => cursor.Resource.NormalSelect        = texture2D);
                this.Content.LoadAsync<Texture2D>(this.AssetPath("Cursors", cursor.Design.HelpSelect)         , texture2D => cursor.Resource.HelpSelect          = texture2D);
                this.Content.LoadAsync<Texture2D>(this.AssetPath("Cursors", cursor.Design.WorkingInBackground), texture2D => cursor.Resource.WorkingInBackground = texture2D);
                this.Content.LoadAsync<Texture2D>(this.AssetPath("Cursors", cursor.Design.Busy)               , texture2D => cursor.Resource.Busy                = texture2D);
                this.Content.LoadAsync<Texture2D>(this.AssetPath("Cursors", cursor.Design.PrecisionSelect)    , texture2D => cursor.Resource.PrecisionSelect     = texture2D);
                this.Content.LoadAsync<Texture2D>(this.AssetPath("Cursors", cursor.Design.TextSelect)         , texture2D => cursor.Resource.TextSelect          = texture2D);
                this.Content.LoadAsync<Texture2D>(this.AssetPath("Cursors", cursor.Design.Handwriting)        , texture2D => cursor.Resource.Handwriting         = texture2D);
                this.Content.LoadAsync<Texture2D>(this.AssetPath("Cursors", cursor.Design.Unavailable)        , texture2D => cursor.Resource.Unavailable         = texture2D);
                this.Content.LoadAsync<Texture2D>(this.AssetPath("Cursors", cursor.Design.VerticalResize)     , texture2D => cursor.Resource.VerticalResize      = texture2D);
                this.Content.LoadAsync<Texture2D>(this.AssetPath("Cursors", cursor.Design.HorizontalResize)   , texture2D => cursor.Resource.HorizontalResize    = texture2D);
                this.Content.LoadAsync<Texture2D>(this.AssetPath("Cursors", cursor.Design.DiagonalResize1)    , texture2D => cursor.Resource.DiagonalResize1     = texture2D);
                this.Content.LoadAsync<Texture2D>(this.AssetPath("Cursors", cursor.Design.DiagonalResize2)    , texture2D => cursor.Resource.DiagonalResize2     = texture2D);
                this.Content.LoadAsync<Texture2D>(this.AssetPath("Cursors", cursor.Design.Move)               , texture2D => cursor.Resource.Move                = texture2D);
                this.Content.LoadAsync<Texture2D>(this.AssetPath("Cursors", cursor.Design.AlternativeSelect)  , texture2D => cursor.Resource.AlternativeSelect   = texture2D);
                this.Content.LoadAsync<Texture2D>(this.AssetPath("Cursors", cursor.Design.LinkSelect)         , texture2D => cursor.Resource.LinkSelect          = texture2D);
            }
            else
            {
                cursor.Resource.NormalSelect        = this.Content.Load<Texture2D>(this.AssetPath("Cursors", cursor.Design.NormalSelect));
                cursor.Resource.HelpSelect          = this.Content.Load<Texture2D>(this.AssetPath("Cursors", cursor.Design.HelpSelect));
                cursor.Resource.WorkingInBackground = this.Content.Load<Texture2D>(this.AssetPath("Cursors", cursor.Design.WorkingInBackground));
                cursor.Resource.Busy                = this.Content.Load<Texture2D>(this.AssetPath("Cursors", cursor.Design.Busy));
                cursor.Resource.PrecisionSelect     = this.Content.Load<Texture2D>(this.AssetPath("Cursors", cursor.Design.PrecisionSelect));
                cursor.Resource.TextSelect          = this.Content.Load<Texture2D>(this.AssetPath("Cursors", cursor.Design.TextSelect));
                cursor.Resource.Handwriting         = this.Content.Load<Texture2D>(this.AssetPath("Cursors", cursor.Design.Handwriting));
                cursor.Resource.Unavailable         = this.Content.Load<Texture2D>(this.AssetPath("Cursors", cursor.Design.Unavailable));
                cursor.Resource.VerticalResize      = this.Content.Load<Texture2D>(this.AssetPath("Cursors", cursor.Design.VerticalResize));
                cursor.Resource.HorizontalResize    = this.Content.Load<Texture2D>(this.AssetPath("Cursors", cursor.Design.HorizontalResize));
                cursor.Resource.DiagonalResize1     = this.Content.Load<Texture2D>(this.AssetPath("Cursors", cursor.Design.DiagonalResize1));
                cursor.Resource.DiagonalResize2     = this.Content.Load<Texture2D>(this.AssetPath("Cursors", cursor.Design.DiagonalResize2));
                cursor.Resource.Move                = this.Content.Load<Texture2D>(this.AssetPath("Cursors", cursor.Design.Move));
                cursor.Resource.AlternativeSelect   = this.Content.Load<Texture2D>(this.AssetPath("Cursors", cursor.Design.AlternativeSelect));
                cursor.Resource.LinkSelect          = this.Content.Load<Texture2D>(this.AssetPath("Cursors", cursor.Design.LinkSelect));
            }

            this.Cursors.Add(cursor);
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

            foreach (var cursor in this.Assets[packageName].Cursors.Values)
            {
                this.UnloadCursor(cursor);
            }
        }

        private void UnloadFont(MMFontAsset asset)
        {
            asset.Dispose();
            this.Fonts.Remove(asset);
        }

        private void UnloadImage(MMImageAsset asset)
        {
            asset.Resource.Dispose();
            this.Texture.Remove(asset);
        }

        private void UnloadCursor(MMCursorAsset asset)
        {
            asset.Resource.Dispose();
            this.Cursors.Remove(asset);
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
                    this.ReadCursors(package, e);
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

            var designWidth = MMXmlReader.ReadAttributeInt(designElement, "DesignWidth", 0, true);
            var designHeight = MMXmlReader.ReadAttributeInt(designElement, "DesignHeight", 0, true);
            var screenWidth = MMXmlReader.ReadAttributeInt(designElement, "ScreenWidth", 0, true);
            var screenHeight = MMXmlReader.ReadAttributeInt(designElement, "ScreenHeight", 0, true);

            return new MMImageDesign(designWidth, designHeight, screenWidth, screenHeight);
        }

        private void ReadCursors(MMPackageAsset package, XmlElement packageElement)
        {
            var cursorList = this.ReadAssetList(packageElement, "Cursors", "Cursor");
            if (cursorList?.Count > 0)
            {
                foreach (XmlElement cursorElement in cursorList)
                {
                    var cursorName = MMXmlReader.ReadAttributeString(cursorElement, "Name", null, true);
                    var cursorDesign = this.ReadCursorDesign(cursorElement);
                    var cursorHotspot = this.ReadCursorHotspot(cursorElement);

                    var cursor = new MMCursorAsset(cursorName, cursorDesign, cursorHotspot);

                    if (!package.Texture.ContainsKey(cursor.Name))
                    {
                        package.Add(cursor);
                    }
                }
            }
        }

        private MMCursorDesign ReadCursorDesign(XmlElement cursorElement)
        {
            var designElement = cursorElement["Design"];
            var design = new MMCursorDesign
            {
                NormalSelect        = MMXmlReader.ReadAttributeString(designElement, "NormalSelect"       , "", true),
                HelpSelect          = MMXmlReader.ReadAttributeString(designElement, "HelpSelect"         , "", true),
                WorkingInBackground = MMXmlReader.ReadAttributeString(designElement, "WorkingInBackground", "", true),
                Busy                = MMXmlReader.ReadAttributeString(designElement, "Busy"               , "", true),
                PrecisionSelect     = MMXmlReader.ReadAttributeString(designElement, "PrecisionSelect"    , "", true),
                TextSelect          = MMXmlReader.ReadAttributeString(designElement, "TextSelect"         , "", true),
                Handwriting         = MMXmlReader.ReadAttributeString(designElement, "Handwriting"        , "", true),
                Unavailable         = MMXmlReader.ReadAttributeString(designElement, "Unavailable"        , "", true),
                VerticalResize      = MMXmlReader.ReadAttributeString(designElement, "VerticalResize"     , "", true),
                HorizontalResize    = MMXmlReader.ReadAttributeString(designElement, "HorizontalResize"   , "", true),
                DiagonalResize1     = MMXmlReader.ReadAttributeString(designElement, "DiagonalResize1"    , "", true),
                DiagonalResize2     = MMXmlReader.ReadAttributeString(designElement, "DiagonalResize2"    , "", true),
                Move                = MMXmlReader.ReadAttributeString(designElement, "Move"               , "", true),
                AlternativeSelect   = MMXmlReader.ReadAttributeString(designElement, "AlternativeSelect"  , "", true),
                LinkSelect          = MMXmlReader.ReadAttributeString(designElement, "LinkSelect"         , "", true)
            };

            return design;
        }

        private MMCursorHotspot ReadCursorHotspot(XmlElement cursorElement)
        {
            var hotspotElement = cursorElement["Hotspot"];
            var hotspot = new MMCursorHotspot
            {
                NormalSelect        = MMXmlReader.ReadAttributeVector2(hotspotElement, "NormalSelect"       , Vector2.Zero, true),
                HelpSelect          = MMXmlReader.ReadAttributeVector2(hotspotElement, "HelpSelect"         , Vector2.Zero, true),
                WorkingInBackground = MMXmlReader.ReadAttributeVector2(hotspotElement, "WorkingInBackground", Vector2.Zero, true),
                Busy                = MMXmlReader.ReadAttributeVector2(hotspotElement, "Busy"               , Vector2.Zero, true),
                PrecisionSelect     = MMXmlReader.ReadAttributeVector2(hotspotElement, "PrecisionSelect"    , Vector2.Zero, true),
                TextSelect          = MMXmlReader.ReadAttributeVector2(hotspotElement, "TextSelect"         , Vector2.Zero, true),
                Handwriting         = MMXmlReader.ReadAttributeVector2(hotspotElement, "Handwriting"        , Vector2.Zero, true),
                Unavailable         = MMXmlReader.ReadAttributeVector2(hotspotElement, "Unavailable"        , Vector2.Zero, true),
                VerticalResize      = MMXmlReader.ReadAttributeVector2(hotspotElement, "VerticalResize"     , Vector2.Zero, true),
                HorizontalResize    = MMXmlReader.ReadAttributeVector2(hotspotElement, "HorizontalResize"   , Vector2.Zero, true),
                DiagonalResize1     = MMXmlReader.ReadAttributeVector2(hotspotElement, "DiagonalResize1"    , Vector2.Zero, true),
                DiagonalResize2     = MMXmlReader.ReadAttributeVector2(hotspotElement, "DiagonalResize2"    , Vector2.Zero, true),
                Move                = MMXmlReader.ReadAttributeVector2(hotspotElement, "Move"               , Vector2.Zero, true),
                AlternativeSelect   = MMXmlReader.ReadAttributeVector2(hotspotElement, "AlternativeSelect"  , Vector2.Zero, true),
                LinkSelect          = MMXmlReader.ReadAttributeVector2(hotspotElement, "LinkSelect"         , Vector2.Zero, true)
            };

            return hotspot;
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