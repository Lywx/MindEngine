namespace MindEngine.Parser
{
    using System;
    using System.Xml;
    using Core;
    using Core.Utils;
    using Microsoft.Xna.Framework;

    public static class MMXmlReader
    {
        public static int ReadAttributeInt(XmlElement valueElement, string valueAttribute, int valueDefault, bool valueRequired)
        {
            return (int)ReadAttribute(valueElement, valueAttribute, typeof(int), valueDefault, valueRequired);
        }

        /// <remarks>
        ///     The primary difference from the overloaded version is that it allow
        ///     inherited from parent value when return value already has parent's value.
        /// </remarks>
        public static void ReadAttributeInt(XmlElement valueElement, string valueAttribute, ref int value, int valueDefault, bool valueRequired, bool valueInherited)
        {
            // Boxing the integer
            var valueObject = (object)value;

            // There is no need to unbox the value object
            ReadAttribute(valueElement, valueAttribute, ref valueObject, typeof(int), valueDefault, valueRequired, valueInherited);
        }

        public static bool ReadAttributeBool(XmlElement valueElement, string valueAttribute, bool valueDefault, bool valueRequired)
        {
            return (bool)ReadAttribute(valueElement, valueAttribute, typeof(bool), valueDefault, valueRequired);
        }

        /// <remarks>
        ///     The primary difference from the overloaded version is that it allow
        ///     inherited from parent value when return value already has parent's value.
        /// </remarks>
        public static void ReadAttributeBool(XmlElement valueElement, string valueAttribute, ref bool value, bool valueDefault, bool valueRequired, bool valueInherited)
        {
            var valueObject = (object)value;

            ReadAttribute(valueElement, valueAttribute, ref valueObject, typeof(bool), valueDefault, valueRequired, valueInherited);
        }

        public static byte ReadAttributeByte(
            XmlElement valueElement,
            string valueAttribute,
            byte valueDefault,
            bool valueRequired)
        {
            return (byte)ReadAttribute(valueElement, valueAttribute, typeof(byte), valueDefault, valueRequired);
        }

        /// <remarks>
        ///     The primary difference from the overloaded version is that it allow
        ///     inherited from parent value when return value already has parent's value.
        /// </remarks>
        public static void ReadAttributeByte(XmlElement valueElement, string valueAttribute, ref byte value, byte valueDefault, bool valueRequired, bool valueInherited)
        {
            var valueObject = (object)value;

            ReadAttribute(valueElement, valueAttribute, ref valueObject, typeof(byte), valueDefault, valueRequired, valueInherited);
        }

        /// <remarks>
        ///     The primary difference from the overloaded version is that it allow
        ///     inherited from parent value when return value already has parent's value.
        /// </remarks>
        public static void ReadAttributeVector2(XmlElement valueElement, string valueAttribute, ref Vector2 value, Vector2 valueDefault, bool valueRequired, bool valueInherited)
        {
            var valueString = value.ToString();

            ReadAttributeString(valueElement, valueAttribute, ref valueString, valueDefault.ToString(), valueRequired, valueInherited);

            try
            {
                value = MMVectorConverter.ParseVector2(valueString);
            }
            catch (Exception)
            {
                value = valueDefault;
            }
        }

        public static Vector2 ReadAttributeVector2(XmlElement valueElement, string valueAttribute, Vector2 valueDefault, bool valueRequired)
        {
            var valueString = ReadAttributeString(valueElement, valueAttribute, valueDefault.ToString(), valueRequired);

            try
            {
                return MMVectorConverter.ParseVector2(valueString);
            }
            catch (Exception)
            {
                return valueDefault;
            }
        }

        /// <remarks>
        ///     The primary difference from the overloaded version is that it allow
        ///     inherited from parent value when return value already has parent's value.
        /// </remarks>
        public static void ReadAttributeColor(XmlElement valueElement, string valueAttribute, ref Color value, Color valueDefault, bool valueRequired, bool valueInherited)
        {
            var valueString = MMColorConverter.ToArgbHexString(value);

            ReadAttributeString(valueElement, valueAttribute, ref valueString, MMColorConverter.ToArgbHexString(valueDefault), valueRequired, valueInherited);

            try
            {
                value = MMColorConverter.ParseArgbHex(valueString);
            }
            catch (Exception)
            {
                value = valueDefault;
            }
        }

        public static Color ReadAttributeColor(XmlElement valueElement, string valueAttribute, Color valueDefault, bool valueRequired)
        {
            var valueString = ReadAttributeString(valueElement, valueAttribute, MMColorConverter.ToArgbHexString(valueDefault), valueRequired);

            try
            {
                return MMColorConverter.ParseArgbHex(valueString);
            }
            catch (Exception)
            {
                return valueDefault;
            }
        }

        public static string ReadAttributeString(XmlElement valueElement, string valueAttribute, string valueDefault, bool valueRequired)
        {
            if (valueElement != null && valueElement.HasAttribute(valueAttribute))
            {
                return valueElement.Attributes[valueAttribute].Value;
            }
            else if (valueRequired)
            {
                throw new Exception(
                    $@"Missing required attribute ""{valueAttribute}"" in the file.");
            }
            else
            {
                return valueDefault;
            }
        }

        /// <param name="value">
        /// Return value. If you want to use value inheritance, you need to pass
        /// the inherited value in here. Ref parameter makes it possible for
        /// value to pass into argument.
        /// </param>
        /// <remarks>
        /// The primary difference from the overloaded version is that it allow
        /// inherited from parent value when return value already has parent's value.
        /// </remarks>
        public static void ReadAttributeString(XmlElement valueElement, string valueAttribute, ref string value, string valueDefault, bool valueRequired, bool valueInherited)
        {
            if (valueElement != null && valueElement.HasAttribute(valueAttribute))
            {
                value = valueElement.Attributes[valueAttribute].Value;
            }
            else if (valueInherited)
            {
                // Change nothing to existing return value
            }
            else if (valueRequired)
            {
                throw new ArgumentException(
                    $@"Missing required attribute ""{valueAttribute}"" in the file.");
            }
            else
            {
                // Set default value into ref
                value = valueDefault;
            }
        }

        /// <remarks>
        /// The primary difference from the overloaded version is that it allow
        /// inherited from parent value when return value already has parent's value.
        /// </remarks>
        private static void ReadAttribute(XmlElement valueElement, string valueAttribute, ref object value, Type valueType, object valueDefault, bool valueRequired, bool valueInherited)
        {
            // Necessary to make ref to work here
            var valueString = value.ToString();

            ReadAttributeString(valueElement, valueAttribute, ref valueString, valueDefault.ToString(), valueRequired, valueInherited);

            value = MMValueReader.ReadValue(valueString, valueType, valueDefault);
        }

        private static object ReadAttribute(XmlElement valueElement, string valueAttribute, Type valueType, object valueDefault, bool valueRequired)
        {
            var valueString = ReadAttributeString(valueElement, valueAttribute, valueDefault.ToString(), valueRequired);

            return MMValueReader.ReadValue(valueString, valueType, valueDefault);
        }
    }
}