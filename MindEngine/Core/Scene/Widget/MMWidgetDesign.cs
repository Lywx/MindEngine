namespace MindEngine.Core.Scene.Widget
{
    using System;

    public interface IMMControlDesign : ICloneable
    {
        string Name { get; set; }

        string Parent { get; set; }

        void Clone(MMWidgetDesign source);
    }

    /// <summary>
    /// Control design define a set of design property that could be shared or 
    /// copied among different control.
    /// </summary>
    public class MMWidgetDesign : IMMControlDesign
    {
        protected MMWidgetDesign(string name)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            this.Name = name;
        }

        protected MMWidgetDesign()
        {
            
        }

        public string Name { get; set; }

        public string Parent { get; set; }

        public virtual void Clone(MMWidgetDesign source)
        {
            this.CloneExtra(source);
        }

        private void CloneExtra(MMWidgetDesign source)
        {
            this.Name   = source.Name;
            this.Parent = source.Parent;
        }

        public virtual object Clone()
        {
            var clone = new MMWidgetDesign();
            clone.Clone(this);

            return clone;
        }
    }
}