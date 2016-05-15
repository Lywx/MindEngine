namespace MindEngine.Audio
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Audio;

    public interface IMMAudioDeviceController
    {
        AudioEngine AudioEngine { get; }

        SoundBank SoundBank { get; }

        WaveBank WaveBank { get; }

        Cue GetCue(string cueName);

        void PlayCue(string cueName);

        void Update(GameTime gameTime);
    }
}