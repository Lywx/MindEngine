namespace MindEngine.Core.Scenes.Entity
{
    using System;
    using Microsoft.Xna.Framework;

    public class MMUpdateBehavior : MMEntityBehavior
    {
        public MMUpdateBehavior(Action<GameTime> updateAction)
        {
            this.UpdateAction = updateAction;
        }

        private Action<GameTime> UpdateAction { get; set; }

        public override void Update(GameTime time)
        {
            this.UpdateAction.Invoke(time);
        }
    }

    public abstract class MMEntityBehavior
    {
        public abstract void Update(GameTime time);
    }
}
