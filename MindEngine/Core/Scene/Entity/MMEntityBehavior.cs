namespace MindEngine.Core.Scene.Entity
{
    using System;
    using Microsoft.Xna.Framework;

    public class MMEntityUpdateBehavior : MMEntityBehavior
    {
        public MMEntityUpdateBehavior(int updateIndex, Action<GameTime> updateAction)
        {
            this.UpdateOrder = updateIndex;
            this.UpdateAction = updateAction;
        }

        /// <summary>
        /// </summary>
        /// <remarks>
        ///     Used by MMEntityUpdateComponent.Name.
        /// </remarks>
        public static string Name { get; } = "Update";

        private Action<GameTime> UpdateAction { get; }

        protected override void UpdateInternal(GameTime time)
        {
            this.UpdateAction.Invoke(time);
        }
    }

    public class MMEntityBehavior : MMEntityUpdatable
    {
        /// <summary>
        /// </summary>
        /// <param name="updateIndex">Index has to be 0-based positive index.</param>
        protected MMEntityBehavior()
        {
        }

        public sealed override int UpdateOrder
        {
            get { return base.UpdateOrder; }
            set { base.UpdateOrder = value; }
        }
    }
}
