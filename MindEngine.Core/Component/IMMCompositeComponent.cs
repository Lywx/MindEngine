namespace MindEngine.Core.Component
{
    /// <summary>
    /// A general component interface for most thing you would like to do in a component.
    /// </summary>
    public interface IMMCompositeComponent : 
        IMMInputtableComponent, 
        IMMDrawableComponent, 
        IMMGameComponent
    {
        
    }
}