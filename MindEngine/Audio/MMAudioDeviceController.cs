namespace MindEngine.Audio
{
    using System;
    using Core;
    using Core.Component;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Audio;

    public class MMAudioDeviceController : MMCompositeComponent, IMMAudioDeviceController
    {
        internal MMAudioDeviceController(
            MMEngine engine,
            AudioEngine audioEngine,
            WaveBank waveBank,
            SoundBank soundBank) : base(engine)
        {
            if (audioEngine == null)
            {
                throw new ArgumentNullException(nameof(audioEngine));
            }

            if (waveBank == null)
            {
                throw new ArgumentNullException(nameof(waveBank));
            }

            if (soundBank == null)
            {
                throw new ArgumentNullException(nameof(soundBank));
            }

            this.AudioEngine = audioEngine;
            this.WaveBank = waveBank;
            this.SoundBank = soundBank;
        }

        #region State Data

        public bool IsInitialized => this.AudioEngine != null &&
                                     this.SoundBank != null &&
                                     this.WaveBank != null;

        #endregion

        public override void Update(GameTime gameTime)
        {
            this.AudioEngine?.Update();
        }

        #region Component Data

        /// <summary>
        ///     The audio engine used to play all cues.
        /// </summary>
        public AudioEngine AudioEngine { get; set; }

        /// <summary>
        ///     The soundbank that contains all cues.
        /// </summary>
        public SoundBank SoundBank { get; set; }

        /// <summary>
        ///     The wavebank with all wave files for this game.
        /// </summary>
        public WaveBank WaveBank { get; set; }

        #endregion

        #region Cue Operations

        /// <summary>
        ///     Retrieve a cue by name.
        /// </summary>
        /// <param name="cueName">The name of the cue requested.</param>
        /// <returns>The cue corresponding to the name provided.</returns>
        public Cue GetCue(string cueName)
        {
            var isValidCueName = !string.IsNullOrEmpty(cueName);

            if (!this.IsInitialized
                ||
                !isValidCueName)
            {
                return null;
            }

            return this.SoundBank.GetCue(cueName);
        }

        /// <summary>
        ///     Plays a cue by name.
        /// </summary>
        /// <param name="cueName">The name of the cue to play.</param>
        public void PlayCue(string cueName)
        {
            if (!this.IsInitialized)
            {
                return;
            }

            this.SoundBank.PlayCue(cueName);
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
                        this.DisposeAudioComponents();
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

        private void DisposeAudioComponents()
        {
            this.AudioEngine?.Dispose();
            this.AudioEngine = null;

            this.SoundBank?.Dispose();
            this.SoundBank = null;

            this.WaveBank?.Dispose();
            this.WaveBank = null;
        }

        #endregion
    }
}
