namespace MindEngine.Core.Scene.Widget.Control
{
    using System;
    using Entity;

    public class MMControlManager : MMEntityNode
    {
        public MMControlManager(MMControlSkin skin, MMControlSettings settings)
        {
            if (skin == null)
            {
                throw new ArgumentNullException(nameof(skin));
            }

            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            this.Skin     = skin;
            this.Settings = settings;
        }

        #region Node

        protected override void OnAttachNodeEnd(MMEntityNode child)
        {
            base.OnAttachNodeEnd(child);
            var c = (MMControl)child;
            c.ControlManager = this;
        }

        protected override void OnDetachNodeEnd(MMEntityNode child)
        {
            base.OnDetachNodeEnd(child);
            var c = (MMControl)child;
            c.ControlManager = null;
        }

        #endregion

        #region Widget

        public MMControlSkin Skin { get; set; }

        public MMControlSettings Settings { get; set; }

        #endregion
    }
}