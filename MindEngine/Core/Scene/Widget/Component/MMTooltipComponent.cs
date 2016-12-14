namespace MindEngine.Core.Scene.Widget.Component
{
    using Control;

    public class MMTooltipComponent : MMControlComponent
    {
        public new class MMElementRegistration : MMControlComponent.MMElementRegistration
        {
        }

        public MMTooltipComponent(MMControl controlParent, MMElementRegistration elementRegistration)
            : base(controlParent, elementRegistration)
        {
        }

        public MMTooltipComponent(MMControl controlParent)
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