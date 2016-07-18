namespace MindEngine.Core.Content.Font
{
    using System.Collections.Generic;
    using Component;
    using Content.Font.Extensions;

    /// <summary>
    ///     Static storage of SpriteFont objects and colors for use throughout the game.
    /// </summary>
    public class MMFontManager : MMCompositeComponent, IMMFontManager
    {
        #region Constructors

        public MMFontManager(MMEngine engine)
            : base(engine)
        {
        }

        #endregion

        #region Initialization

        public override void Initialize()
        {
            // This method call LoadContent method to load necessary data
            base.Initialize();

            StringUtils.Initialize(this);
            StringExtension.Initialize(this);
        }

        #endregion

        #region Font 

        public MMFont this[string index] => this.Fonts[index];

        private Dictionary<string, MMFont> Fonts { get; } =
            new Dictionary<string, MMFont>();

        #endregion

        #region Load and Unload

        /// <remarks>
        /// This method is called by Initialized in base class.
        /// </remarks>
        protected override void LoadContent()
        {
        }

        /// <summary>
        ///     Release all references to the fonts.
        /// </summary>
        protected override void UnloadContent()
        {
            this.Fonts.Clear();
        }

        #endregion Load and Unload

        #region Operations

        public void Add(MMFontAsset fontAsset)
        {
            if (!this.Fonts.ContainsKey(fontAsset.Name))
            {
                this.Fonts.Add(fontAsset.Name, new MMFont(fontAsset));
            }
        }

        public void Remove(MMFontAsset fontAsset)
        {
            if (this.Fonts.ContainsKey(fontAsset.Name))
            {
                this.Fonts.Remove(fontAsset.Name);
            }
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
                        this.UnloadContent();
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
