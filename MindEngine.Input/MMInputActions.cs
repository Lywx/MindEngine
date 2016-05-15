namespace MindEngine.Input
{
    using System;

    /// <remarks>
    /// You should inherit this class to define customized actions. 
    /// </remarks>
    public class MMInputActions
    {
        #region Direction

        public static MMInputAction Up = Guid.NewGuid();

        public static MMInputAction Down = Guid.NewGuid();

        public static MMInputAction Left = Guid.NewGuid();

        public static MMInputAction Right = Guid.NewGuid(); 

        #endregion

        public static MMInputAction Copy = Guid.NewGuid();

        public static MMInputAction Paste = Guid.NewGuid();

        public static MMInputAction Escape = Guid.NewGuid();

        //public static MMInputAction ComboUp = 0x719D5A45;

        //public static MMInputAction ComboDown = 0x51DE59FE;

        //public static MMInputAction ComboLeft = 0x2AC0A01F;

        //public static MMInputAction ComboRight = 0x0280CAF0;

        #region Operations

        public static bool TryParse(string value, out MMInputAction result)
        {
            return MMInputActionParser.TryParse<MMInputActions>(value, out result);
        }

        public static MMInputAction Parse(string value)
        {
            return MMInputActionParser.Parse<MMInputActions>(value);
        }

        #endregion
    }
}