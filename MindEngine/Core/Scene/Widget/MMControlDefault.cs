namespace MindEngine.Core.Scene.Widget
{
    using Content.Widget;
    using Microsoft.Xna.Framework;

    public class MMControlDefault
    {
        public static MMControlManager Manager;

        public static MMControlSkin Skin;

        static MMControlDefault()
        {
            // Initialize default skin
            Skin = new MMControlSkin("Default");
            SkinSetupDesign();
            SkinSetupLayer();

            // Initialize default control manager
            Manager = new MMControlManager(Skin);
        }

        private static void SkinSetupLayer()
        {
            Skin.Layers.Add(new MMControlLayer("DebugImageProperty")
            {
                X = 2, Y = 2, Width = 20, Height = 20, Text = new MMControlText
                {
                    FontColor = Color.Red, FontName = "Lucida Console Regular", FontBaseSize = 1f
                }
            });
        }

        private static void SkinSetupDesign()
        {
            Skin.Designs.Add(new MMControlDesign("Control")
            {
                Parent = null,
                Layout = new MMControlLayout
                {
                    MinimumSize    = new MMControlSize(0, 0),
                    DefaultSize    = new MMControlSize(1, 1),
                    ClientMargins  = new MMControlMargins(0, 0, 0, 0),
                    DefaultMargins = new MMControlMargins(0, 0, 0, 0),
                },
            });

            Skin.Designs.Add(new MMControlDesign("Window")
            {
                Parent = "Control",
            });
        }
    }
}