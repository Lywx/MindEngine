namespace MindEngine.Core.Scene.Widget
{
    using System;
    using Content.Widget;
    using Microsoft.Xna.Framework;
    using Node;

    public class MMControlManager : MMNode
    {
        public MMControlManager(MMControlSkin skin)
        {
            if (skin == null)
            {
                throw new ArgumentNullException(nameof(skin));
            }

            this.Skin = skin;
        }

        #region Node

        protected override void EndAdd(MMNode child)
        {
            base.EndAdd(child);
            var c = (MMControl)child;
            c.Manager = this;
        }

        protected override void EndDetach(MMNode child)
        {
            base.EndDetach(child);
            var c = (MMControl)child;
            c.Manager = null;
        }

        #endregion

        #region Widget

        #region Skin

        public MMControlSkin Skin { get; set; }

        #endregion

        protected override void DrawInternal(GameTime time)
        {
            base.DrawInternal(time);
        }

        #endregion
    }
}