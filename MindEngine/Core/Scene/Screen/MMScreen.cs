namespace MindEngine.Core.Scene.Screen
{
    using Microsoft.Xna.Framework;
    using Node;

    /// <summary>
    /// Screen is the primary unit for processing different meta-game state. 
    /// The way you should use screen is to see it as a game state unit. 
    /// Normally, you would have Title Screen, Main Screen (Option Screen), 
    /// Game Screen. Most of the time, these screen are independent and serve 
    /// different functionality.
    /// </summary>
    public class MMScreen : MMNode
    {
        public MMScreen()
        {
        }

        public override string EntityClass => "Screen";

        protected override void DrawInternal(GameTime time)
        {
            this.Children.Draw(time);
        }
    }
}