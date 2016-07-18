namespace MindEngine.Core.Content.Widget
{
    public class MMControlSkin
    {
        public MMControlSkin(string name)
        {
            this.Name = name;
        }

        public string Name { get; }

        /// <summary>
        /// Control design defines a set of control specific property that must 
        /// exist in any widget design.
        /// </summary>
        public MMControlElementCollection<MMControlDesign> Designs { get; set; } = new MMControlElementCollection<MMControlDesign>();

        /// <summary>
        /// Control layer defines a set of non-control elements that could be 
        /// reused in widget design. 
        /// </summary>
        public MMControlElementCollection<MMControlLayer> Layers { get; set; } = new MMControlElementCollection<MMControlLayer>();

        public MMControlSettings Settings { get; set; } = new MMControlSettings();
    }
}
