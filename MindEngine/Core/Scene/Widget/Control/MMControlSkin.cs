namespace MindEngine.Core.Scene.Widget.Control
{
    using Component.Style;
    using Control.Style;
    using Element;
    using Element.Style;

    public class MMControlSkin
    {
        public MMControlSkin(string name)
        {
            this.Name = name;
        }

        public string Name { get; }

        /// <summary>
        /// Control style defines a set of control specific property that could  
        /// be reused.
        /// </summary>
        public MMWidgetDesignCollection<MMControlStyle> ControlStyles { get; set; } = new MMWidgetDesignCollection<MMControlStyle>();

        /// <summary>
        /// Component style defines a set of control specific property that could  
        /// be reused.
        /// </summary>
        public MMWidgetDesignCollection<MMComponentStyle> ComponentStyles { get; set; } = new MMWidgetDesignCollection<MMComponentStyle>();

        /// <summary>
        /// Control element defines a set of non-control elements that could be 
        /// reused in widget design. 
        /// </summary>
        public MMWidgetDesignCollection<MMElementStyle> ElementStyles { get; set; } = new MMWidgetDesignCollection<MMElementStyle>();
    }
}