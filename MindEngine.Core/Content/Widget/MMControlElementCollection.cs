namespace MindEngine.Core.Content.Widget
{
    using System.Collections.Generic;

    public class MMControlElementCollection<T> : Dictionary<string, T> where T : MMControlElement 
    {
        public void Add(T element) 
        {
            if (element.Parent != null && this.ContainsKey(element.Parent))
            {
                var elementInherited = (T)this[element.Parent].Clone();

                // Copy the override member in the element
                elementInherited.Copy(element);

                base.Add(element.Name, elementInherited);
            }
            else
            {
                base.Add(element.Name, element);
            }
        }
    }
}