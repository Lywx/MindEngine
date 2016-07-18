namespace MindEngine.Core.Service.State
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using Component;
    using Microsoft.Xna.Framework;
    using Scene.Screen;

    /// <summary>
    /// The game state manager.
    /// </summary>
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
            this.ScreenSettings = new MMScreenSettings();
        }

        #endregion Constructors

        #region Screen Data

        public MMScreenSettings ScreenSettings { get; set; }

        /// <summary>
        /// Expose an array holding all the screens. We return a copy rather
        /// than the real master list, because screens should only ever be added
        /// or removed using the AddScreen and RemoveScreen methods.
        /// </summary>
        public MMScreen[] Screens => this.screens.ToArray();

        private List<MMScreen> screensToUpdate = new List<MMScreen>();

        private readonly MMScreenCollection screens = new MMScreenCollection();

        #endregion Screen Data

        #region Trace Data

        /// <summary>
        /// If true, the manager prints out a list of all the screens
        /// each time it is updated. This can be useful for making sure
        /// everything is being added and removed at the right times.
        /// </summary>
        public bool TraceEnabled { get; set; }

        #endregion Trace Data

        #region Update and Draw

        public bool DrawEnabled =>

            // When the game window is inactive, don't draw to save resources
            this.Game.IsActive ||

            // Or 
            this.ScreenSettings.DrawAlwaysEnabled;

        /// <summary>
        /// Tells each screen to draw itself.
        /// </summary>
        public override void Draw(GameTime time)
        {
            if (this.DrawEnabled)
            {
                this.screens.Draw(time);
            }
        }

        public bool UpdateEnabled =>

            // When the game window is active, update the screen because users 
            // actually use it
            this.Engine.IsActive ||

            // Or
            this.ScreenSettings.UpdateAlwaysEnable;

        /// <summary>
        /// Allows each screen to run logic.
        /// </summary>
        public override void Update(GameTime time)
        {
            if (this.UpdateEnabled)
            {
                this.screens.Update(time);
            }
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
            this.screensToUpdate.Remove(screen);
            screen.OnExit();
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
