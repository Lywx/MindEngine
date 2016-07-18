namespace MindEngine.Core.Util
{
    using System;

    public class MMSortingCollection<T> : MMSortingFilteringCollection<T, EventArgs, EventArgs>
    {
        public MMSortingCollection(
            Comparison<T>                      sort, 
            Action<T, EventHandler<EventArgs>> sortChangedSubscriber, 
            Action<T, EventHandler<EventArgs>> sortChangedUnsubscriber) 
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