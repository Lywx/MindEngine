namespace MindEngine.Audio
{
    using Core;
    using Core.Component;
    using Microsoft.Xna.Framework.Audio;

    public interface IMMAudioDeviceController : IMMGameComponent, IMMUpdateableOperations
    {
        AudioEngine AudioEngine { get; }

        SoundBank SoundBank { get; }

        WaveBank WaveBank { get; }

        bool IsInitialized { get; }

        Cue GetCue(string cueName);

        void PlayCue(string cueName);
    }
}