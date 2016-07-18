namespace MindEngine.Core.Scene.Widget
{
    using Content.Widget;

    public class MMClipControl : MMControl
    {
        protected MMClipControl()
        {
            this.ClientArea = new MMClipBox();

            base.Add(this.ClientArea);
        }

        #region Widget

        public virtual void Add(MMControl child, bool clientArea)
        {
            if (clientArea)
            {
                this.ClientArea.Add(child);
            }
            else
            {
                base.Add(child);
            }
        }

        public override void Remove(MMControl child)
        {
            base.Remove(child);
            this.ClientArea.Remove(child);
        }

        #endregion

        public override MMControlMargins ClientMargins
        {
            get
            {
                return base.ClientMargins;
            }
            set
            {
                base.ClientMargins = value;
                
                if (this.ClientArea != null)
                {
                    this.ClientArea.Margins = new MMControlMargins(
                        this.ClientMargins.Left,
                        this.ClientMargins.Top,
                        this.ClientMargins.Right,
                        this.ClientMargins.Bottom);
                }
            }
        }

        protected virtual void UpdateClientMargins()
        {
        }
    }
}