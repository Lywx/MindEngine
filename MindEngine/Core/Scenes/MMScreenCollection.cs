namespace MindEngine.Core.Scenes
{
    using System.Linq;
    using Entity;
    using Microsoft.Xna.Framework;

    public class MMScreenCollection : MMDrawHandlerCollection<MMScreen>
    {
        public void UpdateInput(GameTime time)
        {
            Items.Last().UpdateInput(time, null);
        }
    }
}
