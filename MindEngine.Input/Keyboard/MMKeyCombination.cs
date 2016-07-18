namespace MindEngine.Input.Keyboard
{
    using Microsoft.Xna.Framework.Input;
    using System.Collections.Generic;

    /// <remarks>
    ///     Index key is normal key. Value key is modifier combination.
    /// </remarks>
    public class MMKeyCombination : Dictionary<Keys, List<Keys>> 
    {
    }
}