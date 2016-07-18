namespace MindEngine.Audio
{
    using Core.Component;

    public interface IMMAudioController : IMMGameComponent
    {
        /// <summary>
        /// Plays the music, clearing the music stack.
        /// </summary>
        void Start(MMAudio audio);

        void Stop();

        /// <summary>
        /// Plays the music.
        /// </summary>
        void Play(MMAudio audio);

        /// <summary>
        /// Plays the music, adding it to the music stack.
        /// </summary>
        void Push(MMAudio audio);

        void Pop();
    }
}