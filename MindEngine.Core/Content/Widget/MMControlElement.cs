namespace MindEngine.Core.Content.Widget
{
    using System;

    public class MMControlElement : ICloneable
    {
        protected MMControlElement(string name)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            this.Name = name;
        }

        protected MMControlElement()
        {
            
        }

        public string Name { get; set; }

        public string Parent { get; set; }

        /// <summary>
        /// Copy all the data.
        /// </summary>
        public virtual void Copy(MMControlElement element)
        {
            this.Name   = string.Copy(element.Name);
            this.Parent = string.Copy(element.Parent);

            this.Learn(element);
        }
        
        /// <summary>
        /// Copy all the data except identification.
        /// </summary>
        public virtual void Learn(MMControlElement element)
        {
            
        }

        public virtual object Clone()
        {
            var clone = new MMControlElement();
            clone.Copy(this);

            return clone;
        }
    }
}