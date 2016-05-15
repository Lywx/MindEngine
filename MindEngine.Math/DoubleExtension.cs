namespace MindEngine.Math
{
    using System;

    /// <reference>
    /// http://stackoverflow.com/questions/3874627/floating-point-comparison-functions-for-c-sharp
    /// </reference>
    public static class DoubleExtension
    {
        public static bool NearlyEqual(this double a, double b, double epsilon)
        {
            var absA    = Math.Abs(a);
            var absB    = Math.Abs(b);
            var absDiff = Math.Abs(a - b);

            if (a == b)
            { 
                // Shortcut, handles infinities
                return true;
            }
            else if (a == 0 || b == 0 || absDiff < double.MinValue)
            {
                // a or b is zero or both are extremely close to it
                // relative error is less meaningful here
                return absDiff < (epsilon * double.MinValue);
            }
            else
            { 
                // Use relative error
                return absDiff / (absA + absB) < epsilon;
            }
        }
    }
}
