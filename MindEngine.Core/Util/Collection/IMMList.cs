namespace MindEngine.Core.Util.Collection
{
    using System.Collections.Generic;

    public interface IMMList<T> : ICollection<T>
    {
        T this[int index] { get; }

        int IndexOf(T item);

        void RemoveAt(int index);
    }
}