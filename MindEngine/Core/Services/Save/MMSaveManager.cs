namespace MindEngine.Core.Services.Save
{
    using System;
    using Microsoft.Xna.Framework;

    public abstract class MMSaveManager : GameComponent, IMMSaveManager
    {
        protected MMSaveManager(MMEngine engine)
            : base(engine)
        {
            if (engine == null)
            {
                throw new ArgumentNullException(nameof(engine));
            }
        }

        public virtual void Save() { }

        public virtual void Load() { }

        public override void Update(GameTime gameTime)
        {
            this.UpdateAutoSave();

            base.Update(gameTime);
        }

        #region Auto Save

        protected virtual DateTime AutoSavedTimestamp { get; set; }

        protected virtual bool AutoSaveTimeout => DateTime.Now - this.AutoSavedTimestamp > TimeSpan.FromSeconds(5);

        protected virtual bool AutoSavedRecently { get; set; }

        protected virtual bool AutoSaveCondition
        {
            get
            {
                var now = DateTime.Now;
                return now.Minute % 5 == 0 && now.Second == 55;
            }
        }

        protected virtual void AutoSave()
        {
            if (this.AutoSavedRecently)
            {
                return;
            }

            this.Save();

            this.AutoSavedTimestamp = DateTime.Now;
            this.AutoSavedRecently = true;
        }

        protected virtual void UpdateAutoSave()
        {
            if (this.AutoSaveCondition)
            {
                this.AutoSave();
            }

            if (this.AutoSaveTimeout)
            {
                this.AutoSavedRecently = false;
            }
        }

        #endregion
    }
}
