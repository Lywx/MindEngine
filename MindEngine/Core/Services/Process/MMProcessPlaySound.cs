namespace MindEngine.Core.Services.Process
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Audio;

    public class MMProcessPlaySound : MMProcess
    {
        private readonly SoundEffectInstance soundInstance;

        public MMProcessPlaySound(SoundEffect sound) 
            : base("Process (Play Sound)", MMProcessCategory.System_Audio)
        {
            this.soundInstance = sound.CreateInstance();
        }

        public override void OnEnter()
        {
            this.soundInstance.Play();
        }

        public override void OnUpdate(GameTime time)
        {
            // When the sound instance is finished playing
            if (this.soundInstance.IsDisposed
                || this.soundInstance.State == SoundState.Stopped)
            {
                this.Exit();
            }
        }
    }
}