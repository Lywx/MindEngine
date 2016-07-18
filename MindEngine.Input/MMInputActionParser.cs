namespace MindEngine.Input
{
    using System;

    public static class MMInputActionParser
    {
        public static bool TryParse<T>(string value, out MMInputAction result)
            where T : MMInputActions
        {
            result = GetValue<T>(value);

            return !result.Equals(MMInputAction.None);
        }

        public static MMInputAction Parse<T>(string value)
            where T : MMInputActions
        {
            MMInputAction result;
            if (!TryParse<T>(value, out result))
            {
                throw new ArgumentException();
            }

            return result;
        }

        public static MMInputAction GetValue<T>(string name)
            where T : MMInputActions
        {
            var fields = typeof(T).GetFields();
            foreach (var field in fields)
            {
                if (field.IsPublic && field.IsStatic

                    // Ignore case
                    && string.Compare(name, field.Name,
                        StringComparison.OrdinalIgnoreCase) == 0)
                {
                    return (MMInputAction)field.GetValue(null);
                }
            }

            return new MMInputAction();
        }
    }
}