namespace MindEngine.Core.Scenes.Input
{
    using System;
    using Microsoft.Xna.Framework;
    using Mouse;

    public class MMMouseHanlderRectDraggable : MMMouseHandlerRectClickable, IMMMouseDraggable
    {
        private readonly int mouseHoldDistance = 6;

        private Vector2 mousePressedPosition;

        /// <summary>
        ///     Mouse position relative to rectangle
        /// </summary>
        private Vector2 mouseRelativePosition;

        #region Constructors and Finalizer

        public MMMouseHanlderRectDraggable()
        {
            this.SetupStateMachine();
        }

        ~MMMouseHanlderRectDraggable()
        {
            this.Dispose(true);
        }

        #endregion

        #region Element States

        //public bool AllowDrag { get; set; }

        //public bool AllowDrop { get; set; }

        private bool allowMove;

        public bool AllowMove
        {
            get { return this.allowMove; }
            set
            {
                var changed = value != this.allowMove;
                if (changed)
                {
                    if (value)
                    {
                        this.AddInputHandlers();
                    }
                    else
                    {
                        this.DisposeEventHandlers();
                    }
                }

                this.allowMove = value;
            }
        }

        #endregion

        #region State Machine

        protected enum RectangleMachineState
        {
            Pressing,

            Holding,

            Dragging,

            Released
        }

        protected enum RectangleMachineTrigger
        {
            Pressed,

            DraggedWithinRange,

            DraggedOutOfRange,

            Released
        }

        protected StateMachine<RectangleMachineState, RectangleMachineTrigger> RectangleStateMachine { get; private set; }

        #endregion

        #region Events

        public event EventHandler<EventArgs> MouseDrag;

        public event EventHandler<EventArgs> MouseMove;

        public event EventHandler<EventArgs> MouseDrop;

        #endregion

        #region Event Registration

        private void AddInputHandlers()
        {
            this.MousePress += this.EventMousePress;

            // Perform the same for inside or outside frame mouse up.
            this.MouseUp += this.EventMouseUp;
            this.MouseUpOut += this.EventMouseUp;
        }

        #endregion

        #region Event Handlers

        private void EventMousePress(object sender, EventArgs e)
        {
            var mousePosition = this.EngineInput.State.Mouse.Position;

            // Origin for deciding whether is dragging
            this.mousePressedPosition = mousePosition;

            // Save relative position of mouse compared to rectangle
            // mouse y-axis value is fixed at y-axis center of the rectangle
            this.mouseRelativePosition = mousePosition - this.Bound.Location.ToVector2();

            this.RectangleStateMachine.Fire(RectangleMachineTrigger.Pressed);
        }

        private void EventMouseUp(object sender, EventArgs e)
        {
            this.RectangleStateMachine.Fire(RectangleMachineTrigger.Released);
        }

        #endregion

        #region Event On

        private void OnMouseDropped()
        {
            this.MouseDrop?.Invoke(this, new EventArgs());
        }

        private void OnMouseDragged()
        {
            this.MouseDrag?.Invoke(this, new EventArgs());
        }

        #endregion

        #region Initialization

        private void SetupStateMachine()
        {
            this.RectangleStateMachine = new StateMachine<RectangleMachineState, RectangleMachineTrigger>(RectangleMachineState.Released);

            // Possible cross interference 
            this.RectangleStateMachine.Configure(RectangleMachineState.Released).PermitReentry(RectangleMachineTrigger.Released);
            this.RectangleStateMachine.Configure(RectangleMachineState.Released).Permit(RectangleMachineTrigger.Pressed, RectangleMachineState.Pressing);
            this.RectangleStateMachine.Configure(RectangleMachineState.Released).Ignore(RectangleMachineTrigger.DraggedWithinRange);
            this.RectangleStateMachine.Configure(RectangleMachineState.Released).Ignore(RectangleMachineTrigger.DraggedOutOfRange);

            // Possible cross interference
            this.RectangleStateMachine.Configure(RectangleMachineState.Pressing).PermitReentry(RectangleMachineTrigger.Pressed);
            this.RectangleStateMachine.Configure(RectangleMachineState.Pressing).Permit(RectangleMachineTrigger.Released, RectangleMachineState.Released);
            this.RectangleStateMachine.Configure(RectangleMachineState.Pressing).Permit(RectangleMachineTrigger.DraggedWithinRange, RectangleMachineState.Holding);
            this.RectangleStateMachine.Configure(RectangleMachineState.Pressing).Permit(RectangleMachineTrigger.DraggedOutOfRange, RectangleMachineState.Dragging);

            this.RectangleStateMachine.Configure(RectangleMachineState.Holding).PermitReentry(RectangleMachineTrigger.DraggedWithinRange);
            this.RectangleStateMachine.Configure(RectangleMachineState.Holding).Permit(RectangleMachineTrigger.DraggedOutOfRange, RectangleMachineState.Dragging);

            // Possible cross interference
            this.RectangleStateMachine.Configure(RectangleMachineState.Holding).Permit(RectangleMachineTrigger.Pressed, RectangleMachineState.Pressing);
            this.RectangleStateMachine.Configure(RectangleMachineState.Holding).Permit(RectangleMachineTrigger.Released, RectangleMachineState.Released);

            // Possible cross interference
            this.RectangleStateMachine.Configure(RectangleMachineState.Dragging).Permit(RectangleMachineTrigger.Pressed, RectangleMachineState.Released);
            this.RectangleStateMachine.Configure(RectangleMachineState.Dragging).Permit(RectangleMachineTrigger.Released, RectangleMachineState.Released);
            this.RectangleStateMachine.Configure(RectangleMachineState.Dragging).Ignore(RectangleMachineTrigger.DraggedOutOfRange);
            this.RectangleStateMachine.Configure(RectangleMachineState.Dragging).Ignore(RectangleMachineTrigger.DraggedWithinRange);

            this.RectangleStateMachine.Configure(RectangleMachineState.Dragging).OnEntry(() =>
            {
                this.MouseCacher.CacheInput(this.OnMouseDragged);
            });

            this.RectangleStateMachine.Configure(RectangleMachineState.Dragging).OnExit(() =>
            {
                this.MouseCacher.CacheInput(this.OnMouseDropped);
            });
        }

        #endregion

        #region Update

        public override void Update(GameTime time)
        {
            base.Update(time);

            if (this.AllowMove)
            {
                var mousePosition = this.EngineInput.State.Mouse.Position;

                this.UpdateBoundPosition(mousePosition);
                this.UpdateFrameStates(mousePosition);
            }
        }

        private void UpdateBoundPosition(Vector2 mousePosition)
        {
            if (this.RectangleStateMachine.IsInState(RectangleMachineState.Dragging))
            {
                // Keep rectangle relative position to the mouse position from 
                // changing 
                this.Position = (mousePosition - this.mouseRelativePosition).ToPoint();
            }
        }

        private void UpdateFrameStates(Vector2 mousePosition)
        {
            var isOutOfHoldLen =
                (mousePosition - this.mousePressedPosition).
                              Length() > this.mouseHoldDistance;

            this.RectangleStateMachine.Fire(
                isOutOfHoldLen ? RectangleMachineTrigger.DraggedOutOfRange : RectangleMachineTrigger.DraggedWithinRange);
        }

        #endregion Update

        #region IDisposable

        private bool IsDisposed { get; set; }

        protected override void Dispose(bool disposing)
        {
            try
            {
                if (disposing)
                {
                    if (!this.IsDisposed)
                    {
                        this.DisposeEvents();
                        this.DisposeEventHandlers();
                    }

                    this.IsDisposed = true;
                }
            }
            catch
            {
                // Ignored
            }
            finally
            {
                base.Dispose(disposing);
            }
        }

        private void DisposeEvents()
        {
            this.MouseDrag = null;
            this.MouseDrop = null;
        }

        private void DisposeEventHandlers()
        {
            this.MousePress -= this.EventMousePress;

            this.MouseUp -= this.EventMouseUp;
            this.MouseUpOut -= this.EventMouseUp;
        }

        #endregion
    }
}
