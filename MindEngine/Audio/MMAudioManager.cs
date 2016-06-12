namespace MindEngine.Audio
{
    using System;
    using System.Collections.Generic;
    using Core;
    using Core.Components;

    /// <summary>
    ///     Static storage of SpriteFont objects and colors for use throughout the game.
    /// </summary>
    public class MMAudioManager : MMCompositeComponent, IMMAudioManager
    {
        #region Constructors

        public MMAudioManager(MMEngine engine, MMAudioSettings settings)
            : base(engine)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            this.Settings = settings;
        }

        #endregion

        private MMAudioSettings Settings { get; set; }

        #region Audio Data

        public MMAudio this[string index] => this.Audio[index];

        private Dictionary<string, MMAudio> Audio { get; } =
            new Dictionary<string, MMAudio>();

        #endregion

        #region Load and Unload

        protected override void LoadContent() {}

        /// <summary>
        ///     Release all references to the fonts.
        /// </summary>
        protected override void UnloadContent()
        {
            this.DisposeAudio();

            this.Audio.Clear();
        }

        #endregion Load and Unload

        #region Operations

        public void Add(MMAudioAsset audioAsset)
        {
            if (!this.Audio.ContainsKey(audioAsset.Name))
            {
                this.Audio.Add(audioAsset.Name, new MMAudio(audioAsset));
            }
        }

        public void Remove(MMAudioAsset audioAsset)
        {
            if (this.Audio.ContainsKey(audioAsset.Name))
            {
                this.Audio.Remove(audioAsset.Name);
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

        private void DisposeAudio()
        {
            // Dispose all the cues
        }

        #endregion
    }
}
