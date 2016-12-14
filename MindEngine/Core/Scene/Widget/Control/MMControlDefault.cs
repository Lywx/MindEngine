namespace MindEngine.Core.Scene.Widget.Control
{
    using Element;
    using Element.Style;
    using Geometry;
    using Microsoft.Xna.Framework;
    using Property;
    using Style;

    public class MMControlDefault
    {
        public static MMControlManager Manager;

        public static MMControlSkin Skin;

        public static MMControlSettings Settings;

        static MMControlDefault()
        {
            // Initialize default skin
            Skin = new MMControlSkin("Default");
            SkinSetupControlStyle();
            SkinSetupComponentStyle();
            SkinSetupElementStyle();

            Settings = new MMControlSettings();

            Manager = new MMControlManager(Skin, Settings);
        }

        private static void SkinSetupElementStyle()
        {
            Skin.ElementStyles.Add(new MMPrimitiveStyle("DefaultPrimitive")
            {
                X = 0, Y = 0, Z = 0, Width = 20, Height = 20, LineColor = Color.Red, LineThick = 1f
            });

            Skin.ElementStyles.Add(new MMTextStyle("DefaultText")
            {
                X = 2, Y = 2, Z = 2, Width = 20, Height = 20, TextProperty = new MMTextProperty
                {
                    FontBaseColor = Color.Red, FontName = "Lucida Console Regular", FontBaseSize = 1f, FontBaseLeading = 2, TextMonospaced = true
                }
            });
        }

        private static void SkinSetupComponentStyle()
        {
        }

        private static void SkinSetupControlStyle()
        {
            Skin.ControlStyles.Add(new MMControlStyle("Control")
            {
                Parent = null,
                LayoutProperty = new MMLayoutProperty
                {
                    MinimumSize    = new MMSize(0, 0),
                    DefaultSize    = new MMSize(1, 1),
                    ClientMargins  = new MMMargins(0, 0, 0, 0),
                    DefaultMargins = new MMMargins(0, 0, 0, 0),
                },
            });

            Skin.ControlStyles.Add(new MMControlStyle("Window")
            {
                Parent = "Control",
            });
        }
    }
}