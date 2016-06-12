namespace MindEngine.Core.Services.Events
{
    using System.Collections.Generic;

    public interface IMMEventListener
    {
        List<int> RegisteredEvents { get; }

        bool HandleEvent(IMMEvent e);
    }
}