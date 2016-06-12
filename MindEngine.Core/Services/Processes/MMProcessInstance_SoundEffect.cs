namespace MindEngine.Core.Services.Processes
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Audio;

    public class MMProcessInstance_SoundEffect : MMProcess
    {
        private readonly SoundEffectInstance soundInstance;

        public MMProcessInstance_SoundEffect(SoundEffect sound) 
            : base("Process (Sound Effect)", MMProcessCategory.System_Audio)
        {
            this.soundInstance = sound.CreateInstance();
        }

        public override void OnAbort()
        {
        }

        public override void OnFail()
        {
        }

        public override void OnInit()
        {
            base.OnInit();

            this.soundInstance.Play();
        }

        public override void OnSucceed()
        {

        }

        public override void Update(GameTime time)
        {
            base.Update(time);

            // When the sound instance is finished playing
            if (this.soundInstance.IsDisposed
                || this.soundInstance.State == SoundState.Stopped)
            {
                this.Succeed();
            }
        }
    }
}