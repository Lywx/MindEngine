namespace MindEngine.Core.Service
{
    using System;

    /// <summary>
    /// This class provides a separation layer between service accessors and 
    /// service providers. You could replace certain service at runtime, if you 
    /// intend to do so.
    /// </summary>
    public class MMEngineService : IMMEngineService
    {
        public MMEngineService(
            IMMEngineAudioService audio,
            IMMEngineDebugService debug,
            IMMEngineGraphicsService graphics,
            IMMEngineInputService input,
            IMMEngineInteropService interop,
            IMMEngineNumericalService numerical)
        {
            if (audio == null)
            {
                throw new ArgumentNullException(nameof(audio));
            }

            if (graphics == null)
            {
                throw new ArgumentNullException(nameof(graphics));
            }

            if (input == null)
            {
                throw new ArgumentNullException(nameof(input));
            }

            if (interop == null)
            {
                throw new ArgumentNullException(nameof(interop));
            }

            if (numerical == null)
            {
                throw new ArgumentNullException(nameof(numerical));
            }

            this.Audio     = audio;
            this.Debug     = debug;
            this.Graphics  = graphics;
            this.Input     = input;
            this.Interop   = interop;
            this.Numerical = numerical;
        }

        public IMMEngineAudioService Audio { get; set; }

        public IMMEngineDebugService Debug { get; set; }

        public IMMEngineGraphicsService Graphics { get; set; }

        public IMMEngineInputService Input { get; set; }

        public IMMEngineInteropService Interop { get; set; }

        public IMMEngineNumericalService Numerical { get; set; }
    }
}