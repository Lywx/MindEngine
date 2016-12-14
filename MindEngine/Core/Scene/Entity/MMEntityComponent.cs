namespace MindEngine.Core.Scene.Entity
{
    using System;
    using Microsoft.Xna.Framework;

    public abstract class MMEntityComponent : MMEntityBehavior
    {
        protected MMEntityComponent()
        {
        }

        public abstract string ComponentName { get; }
    }

    /// <summary>
    /// This class is a container class for all the MMEntityUpdateBehavior for a 
    /// given MMEntity. All the update behavior would live inside this class
    /// during a entity's life time.
    /// </summary>
    public class MMEntityUpdateComponent : MMEntityComponent
    {
        public MMEntityUpdateComponent()
        {
        }

        public override string ComponentName => MMEntityUpdateBehavior.Name;

        private MMEntityUpdatableList UpdateList { get; set; } = new MMEntityUpdatableList();

        public void AttachBehavior(MMEntityUpdateBehavior updateBehavior)
        {
            var updateIndex = updateBehavior.UpdateOrder;

            // Adding update behavior based on duplicated update index is prohibited
            if (this.UpdateList.FindIndex(updatable => updatable.UpdateOrder == updateIndex) != -1)
            {
                throw new InvalidOperationException("The update index has been taken.");
            }

            this.UpdateList.Add(updateBehavior);
        }

        public void DetachBehavior(int updateIndex)
        {
            var internalIndex = this.UpdateList.FindIndex(updatable => updatable.UpdateOrder == updateIndex);
            this.UpdateList.RemoveAt(internalIndex);
        }

        public void ClearBehaviors()
        {
            this.UpdateList.Clear();
        }

        protected override void UpdateInternal(GameTime time)
        {
            this.UpdateList.Update(time);
        }
    }
}
