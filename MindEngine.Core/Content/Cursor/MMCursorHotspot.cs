namespace MindEngine.Core.Content.Cursor
{
    using Microsoft.Xna.Framework;

    public class MMCursorHotspot
    {
        public Vector2 NormalSelect { get; set; }

        public Vector2 HelpSelect { get; set; }

        public Vector2 WorkingInBackground { get; set; }

        public Vector2 Busy { get; set; }

        public Vector2 PrecisionSelect { get; set; }

        public Vector2 TextSelect { get; set; }

        public Vector2 Handwriting { get; set; }

        public Vector2 Unavailable { get; set; }

        public Vector2 VerticalResize { get; set; }

        public Vector2 HorizontalResize { get; set; }

        public Vector2 DiagonalResize1 { get; set; }

        public Vector2 DiagonalResize2 { get; set; }

        public Vector2 Move { get; set; }

        public Vector2 AlternativeSelect { get; set; }

        public Vector2 LinkSelect { get; set; }
    }
}