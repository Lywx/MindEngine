namespace MindEngine.Core.Scene.Screen
{
    using Entity;
    using Microsoft.Xna.Framework;

    public class MMScreenCollection : MMDrawEntityCollection<MMScreen>
    {
        public override void Draw(GameTime time)
        {
            // Only draw the screen on the top so the screen as a state works 
            // as a pushdown automata
            this.ItemsDrawEntity.Last((drawableParam, timeParam) => drawableParam.Draw(timeParam), time);
        }

        public override void Update(GameTime time)
        {
            // Only update the screen on the top so the screen as a state works 
            // as a pushdown automata
            this.ItemsUpdateEntity.Last((updateableParam, timeParam) => updateableParam.Update(timeParam), time);
        }
    }
}
