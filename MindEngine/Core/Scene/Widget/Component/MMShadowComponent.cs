namespace MindEngine.Core.Scene.Widget.Component
{
    using Control;

    public class MMShadowComponent : MMControlComponent
    {
        public new class MMElementRegistration : MMControlComponent.MMElementRegistration
        {
            
        }

        public MMShadowComponent(MMControl controlParent, MMElementRegistration elementRegistration)
            : base(controlParent, elementRegistration)
        {
            
        }

        public MMShadowComponent(MMControl controlParent)
            : base(controlParent)
        {
            
        }

        #region Element

        private MMElementRegistration elementRegistration;

        public new MMElementRegistration ElementRegistration
        {
            get
            {
                if (this.elementRegistration == null)
                {
                    this.elementRegistration = new MMElementRegistration();
                }

                return this.elementRegistration;
            }
            set
            {
                this.elementRegistration = value;
                base.ElementRegistration = value;
            }
        }

        #endregion
    }
}