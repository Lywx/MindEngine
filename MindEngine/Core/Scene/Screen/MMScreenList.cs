namespace MindEngine.Core.Scene.Screen
{
    using Entity;
    using Microsoft.Xna.Framework;

    public class MMScreenList : MMEntityDrawableList<MMScreen>
    {
        public override void Draw(GameTime time)
        {
            // Only draw the screen on the top so the screen as a state works 
            // as a pushdown automata
            this.DrawItems.Last((drawableParam, timeParam) => drawableParam.Draw(timeParam), time);
        }

        public override void Update(GameTime time)
        {
            // Only update the screen on the top so the screen as a state works 
            // as a pushdown automata
            this.UpdateItems.Last((updateableParam, timeParam) => updateableParam.Update(timeParam), time);
        }
    }
}
