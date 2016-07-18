namespace MindEngine.Core.Util
{
    using Microsoft.Xna.Framework;

    public static class MMVectorConverter
    {
        public static Vector2 ParseVector2(string valueString)
        {
            var value = valueString.Split(',');

            float x = 0, y = 0;

            if (value.Length >= 1)
            {
                x = float.Parse(value[0]);
            }

            if (value.Length >= 2)
            {
                y = float.Parse(value[0]);
            }

            return new Vector2(x, y);
        }
    }
}
