namespace MindEngine.Core.Services.Events
{
    using Components;

    public interface IMMEventManager : IMMGameComponent
    {
        void AddListener(IMMEventListener listener);

        void RemoveListener(IMMEventListener listener);

        void QueueEvent(IMMEvent e);

        void QueueUniqueEvent(IMMEvent e);

        void RemoveQueuedEvent(IMMEvent e, bool allOccurances);

        void TriggerEvent(IMMEvent e);
    }
}