namespace MindEngine.Core.Scenes
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using Components;
    using Microsoft.Xna.Framework;

    public interface IMMScreenManager : IMMDrawableComponent
    {
        void AddScreen(MMScreen screen);

        void RemoveScreen(MMScreen screen);

        void RemoveScreenFrom(int index);

        void OnExit();
    }

    public class MMScreenManager : MMCompositeComponent, IMMScreenManager
    {
        #region Constructors

        /// <summary>
        /// Constructs a new screen manager component.
        /// </summary>
        public MMScreenManager(MMEngine engine)
            : base(engine)
        {
            this.Settings = new MMScreenSettings();
        }

        #endregion Constructors

        #region Draw

        public void BeginDraw(GameTime time)
        {
            
        }

        public void EndDraw(GameTime time)
        {
            
        }

        #endregion

        #region Screen Data

        public MMScreenSettings Settings { get; set; }

        /// <summary>
        /// Expose an array holding all the screens. We return a copy rather
        /// than the real master list, because screens should only ever be added
        /// or removed using the AddScreen and RemoveScreen methods.
        /// </summary>
        public MMScreen[] Screens => this.screens.ToArray();

        private List<MMScreen> ScreensToUpdate { get; set; } = new List<MMScreen>();

        private readonly MMScreenCollection screens = new MMScreenCollection();

        #endregion Screen Data

        #region Trace Data

        private bool traceEnabled;

        /// <summary>
        /// If true, the manager prints out a list of all the screens
        /// each time it is updated. This can be useful for making sure
        /// everything is being added and removed at the right times.
        /// </summary>
        public bool TraceEnabled
        {
            get
            {
                return this.traceEnabled;
            }

            set
            {
                this.traceEnabled = value;
            }
        }

        #endregion Trace Data

        #region Update and Draw

        /// <summary>
        /// Tells each screen to draw itself.
        /// </summary>
        public override void Draw(GameTime time)
        {
            if (
                // When the game window is inactive, don't draw to save resources
                this.Game.IsActive ||

                // Or 
                this.Settings.AlwaysDraw)
            {
                this.screens.Draw(time);
            }
        }

        /// <summary>
        /// Allows each screen to run logic.
        /// </summary>
        public override void Update(GameTime time)
        {
            // When the game window is active, update the screen because users 
            // actually use it
            if (this.Engine.IsActive ||

                // Or
                this.Settings.AlwaysActive)
            {
                this.UpdateScreens(time);
            }
        }

        private void UpdateScreens(GameTime time)
        {
            this.screens.Update(time);
            this.screens.UpdateInput(time);
        }

        #endregion

        #region Operations

        public void AddScreen(MMScreen screen)
        {
            this.screens.Add(screen);
            screen.OnEnter();
        }

        public void RemoveScreen(MMScreen screen)
        {
            this.screens.Remove(screen);
            this.ScreensToUpdate.Remove(screen);
        }

        public void RemoveScreenFrom(int index)
        {
            for (int i = this.screens.Count - 1; i > 0; --i)
            {
                if (i >= index)
                {
                    this.RemoveScreen(this.screens.Last());
                }
            }
        }

        public void ReplaceScreen(MMScreen screen)
        {
            if (this.screens.Count != 0)
            {
                this.RemoveScreen(this.screens.Last());
            }

            this.AddScreen(screen);
        }

        /// <summary>
        /// Prints a list of all the screens, for debugging.
        /// </summary>
        private void TraceScreens()
        {
            var names = new List<string>();

            foreach (var screen in this.screens.ToArray())
            {
                names.Add(screen.GetType().Name);
            }

            Debug.WriteLine(string.Join(", ", names.ToArray()));
        }

        #endregion Operations

        #region Events

        /// <summary>
        /// Called when the MMGame Engine is closed by Exit / Restart operations.
        /// </summary>
        public void OnExit()
        {
            this.UnloadContent();
        }

        #endregion

        #region IDisposable

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                foreach (var screen in this.screens)
                {
                    screen.Dispose();
                }

                this.screens.Clear();
            }

            base.Dispose(disposing);
        }

        #endregion

    }
}
