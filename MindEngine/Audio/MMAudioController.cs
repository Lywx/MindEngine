namespace MindEngine.Audio
{
    using System;
    using System.Collections.Generic;
    using Core;
    using Core.Components;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Audio;

    /// <summary>
    ///     Component that manages audio playback for all cues.
    /// </summary>
    /// <remarks>
    ///     Similar to a class found in the Net Rumble starter kit on the
    ///     XNA Creators Club Online website (http://creators.xna.com).
    ///     It does not need a finalizer, for it does not directly own
    ///     unmanaged resource.
    /// </remarks>
    public class MMAudioController : MMCompositeComponent, IMMAudioController
    {
        #region Constructors and Finalizer

        public MMAudioController(MMEngine engine, IMMAudioDeviceController audioDeviceController)
            : base(engine)
        {
            if (audioDeviceController == null)
            {
                throw new ArgumentNullException(nameof(audioDeviceController));
            }

            this.AudioDeviceController = audioDeviceController;
        }

        #endregion

        #region Component Data

        private IMMAudioDeviceController AudioDeviceController { get; }

        #endregion

        #region Update 

        /// <summary>
        ///     Update the audio manager, particularly the engine.
        /// </summary>
        public override void Update(GameTime time)
        {
            if (this.IsPlaying
                && this.audioCueCurrent.IsStopped)
            {
                this.Pop();
            }

            base.Update(time);
        }

        #endregion

        #region State Data

        public bool IsInitialized => this.AudioDeviceController.IsInitialized;

        private bool IsPlaying => this.audioCueCurrent != null;

        private bool IsPlayingAudio(string audio)
        {
            return audio != null &&
                   audio.Equals(this.audioCueCurrent.Name);
        }

        private bool IsPlayingAudio(MMAudio audio)
        {
            return this.IsPlayingAudio(audio.Name);
        }

        #endregion

        #region Music Operations

        /// <summary>
        ///     Stack of music, for layered music playback.
        /// </summary>
        private readonly Stack<string> audioStack = new Stack<string>();

        /// <summary>
        ///     The audio cue for the music currently playing, if any.
        /// </summary>
        private Cue audioCueCurrent;

        public void Start(MMAudio audio)
        {
            this.Stop();
            this.Push(audio);
        }

        public void Play(MMAudio audio)
        {
            if (audio != null)
            {
                this.AudioDeviceController.PlayCue(audio.Name);
            }
        }

        public void Push(MMAudio audio)
        {
            if (this.IsInitialized)
            {
                this.audioStack.Push(audio.Name);

                if (!this.IsPlaying
                    || !this.IsPlayingAudio(audio))
                {
                    if (this.IsPlaying)
                    {
                        this.StopAudioCurrent();
                    }

                    this.audioCueCurrent = this.AudioDeviceController.GetCue(audio.Name);
                    this.audioCueCurrent.Play();
                }
            }
        }

        /// <summary>
        ///     Stops the current music and plays the previous music on the stack.
        /// </summary>
        public void Pop()
        {
            if (this.IsInitialized)
            {
                string audio = null;

                if (this.audioStack.Count > 0)
                {
                    this.audioStack.Pop();

                    // Get the previous audio in the stack when the stack is not empty
                    if (this.audioStack.Count > 0)
                    {
                        audio = this.audioStack.Peek();
                    }
                }

                var audioPreviousPlaying = audio != null && this.IsPlayingAudio(audio);

                if (!this.IsPlaying ||
                    audioPreviousPlaying)
                {
                    this.StopAudioCurrent();

                    if (audio != null)
                    {
                        this.audioCueCurrent = this.AudioDeviceController.GetCue(audio);
                        this.audioCueCurrent?.Play();
                    }
                }
            }
        }

        public void Stop()
        {
            this.audioStack.Clear();

            this.StopAudioCurrent();
        }

        private void StopAudioCurrent()
        {
            this.audioCueCurrent?.Stop(AudioStopOptions.AsAuthored);

            this.DisposeAudioCurrent();
        }

        #endregion Music

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
                        this.Stop();

                        this.AudioDeviceController.Dispose();
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

        private void DisposeAudioCurrent()
        {
            this.audioCueCurrent?.Dispose();
            this.audioCueCurrent = null;
        }

        #endregion
    }
}
