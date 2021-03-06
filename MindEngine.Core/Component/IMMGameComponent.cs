namespace MindEngine.Core.Component
{
    using System;
    using Microsoft.Xna.Framework;

    /// <summary>
    /// The engine implementation of game component.
    /// </summary>
    public interface IMMGameComponent : IGameComponent, IMMUpdateableOperations, IDisposable
    {
        
    }
}