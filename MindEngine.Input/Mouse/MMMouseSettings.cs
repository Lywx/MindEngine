namespace MindEngine.Input.Mouse
{
    public static class MMMouseSettings
    {
        public const int ButtonPressDelay = 500;

        public const int ButtonRepeatDelay = 50;

        public const int WheelDelta =

#if WINDOWS

            // https://msdn.microsoft.com/en-us/library/windows/desktop/ms645617%28v=vs.85%29.aspx?f=255&MSPPError=-2147217396
            120;
#elif LINUX

    // http://linux.die.net/man/3/qwheelevent
            120;
#endif
    }
}
