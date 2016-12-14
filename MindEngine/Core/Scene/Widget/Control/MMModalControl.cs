namespace MindEngine.Core.Scene.Widget.Control
{
    public class MMModalControl : MMContainerControl
    {
        public new class MMComponentRegistration : MMContainerControl.MMComponentRegistration
        {

        }

        public new class MMControlRegistration : MMContainerControl.MMControlRegistration
        {

        }

        protected MMModalControl(MMControlRegistration controlRegistration, MMComponentRegistration componentRegistration)
            : base(controlRegistration, componentRegistration)
        {
            
        }

        public new MMComponentRegistration ComponentRegistration { get; set; }

        public new MMControlRegistration ControlRegistration { get; set; }
    }
}