namespace MindEngine.Core.Scenes.Widget
{
    using Input;
    using Node;

    public abstract class MMWidget : MMNode
    {
        #region Constructors and Finalizer

        public MMWidget(string entityClass) : base(entityClass)
        {
        }

        #endregion

        public MMToolTip ToolTip { get; set; }

        public MMMouseHandlerRectClickable Region { get; set; }
    }
}
