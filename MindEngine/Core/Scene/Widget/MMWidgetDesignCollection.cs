namespace MindEngine.Core.Scene.Widget
{
    using System.Collections.Generic;

    /// <summary>
    /// Control element collections provide a simple property-inherited 
    /// mechanism in the .
    /// </summary>
    public class MMWidgetDesignCollection<T> : Dictionary<string, T> where T : MMWidgetDesign 
    {
        public void Add(T element) 
        {
            if (element.Parent != null && this.ContainsKey(element.Parent))
            {
                var elementInherited = (T)this[element.Parent].Clone();

                // Copy the override member in the element
                elementInherited.Clone(element);

                base.Add(element.Name, elementInherited);
            }
            else
            {
                base.Add(element.Name, element);
            }
        }
    }
}