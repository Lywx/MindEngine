namespace MindEngine.Parser
{
    using System;
    using System.Globalization;

    public static class MMValueReader
    {
        public static T ReadData<T>(string valueString, Func<string, object, T> valueReader, T valueDefault)
        {
            if (valueReader == null)
            {
                throw new ArgumentNullException(nameof(valueReader));
            }

            try
            {
                return valueReader.Invoke(valueString, valueDefault);
            }
            catch (Exception)
            {
                return valueDefault; 
            }
        }

        /// <summary>
        /// Read data value in string format.
        /// </summary>
        public static object ReadValue(string valueString, Type valueType, object valueDefault)
        {
            try
            {
                return Convert.ChangeType(valueString, valueType, CultureInfo.InvariantCulture);
            }
            catch (Exception)
            {
                return valueDefault; 
            }
        }
    }
}
