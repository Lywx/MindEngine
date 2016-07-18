namespace MindEngine.Core.Content.Widget
{
    using System;

    public class MMControlDesign : MMControlElement, ICloneable
    {
        public MMControlDesign(string name) : base(name)
        {

        }

        private MMControlDesign()
        {
            
        }

        /// <summary>
        /// Control layout define the most basic spatial design in a control. 
        /// This property is used across all the widgets.
        /// </summary>
        public MMControlLayout Layout { get; set; }

        public override void Copy(MMControlElement element)
        {
            base.Copy(element);
        }

        public override void Learn(MMControlElement element)
        {
            base.Learn(element);

            var design = element as MMControlDesign;
            if (design != null)
            {
                this.Layout = (MMControlLayout)design.Layout.Clone();
            }
        }

        public override object Clone()
        {
            var clone = new MMControlDesign();
            clone.Copy(this);

            return clone;
        }
    }
}