namespace MindEngine.Parser
{
    using System;
    using System.Globalization;

    public static class MMValueReader
    {
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
