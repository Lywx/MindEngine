namespace MindEngine.Input
{
    using System;

    [Flags]
    public enum MMInputDevice
    {
        None = 0x00,

        Keyboard = 0x01,

        Mouse = 0x02,

        GamePad = 0x04,

        All = Keyboard | Mouse | 0x04
    }
}