namespace MindEngine.Core.Util.Collection
{
    using System;

    public class MMSortingList<T, TSortChangedEventArgs> : MMSortingFilteringList<T, EventArgs, TSortChangedEventArgs>
        where TSortChangedEventArgs : EventArgs
    {
        public MMSortingList(
            Comparison<T>                      sort, 
            Action<T, EventHandler<TSortChangedEventArgs>> sortChangedSubscriber, 
            Action<T, EventHandler<TSortChangedEventArgs>> sortChangedUnsubscriber) 
            : base(item => true, 
                  (item, handler) => {}, 
                  (item, handler) => {}, 
                  sort, 
                  sortChangedSubscriber, 
                  sortChangedUnsubscriber)
        {
        }
    }
}