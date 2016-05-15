namespace MindEngine.Core.Contents.Fonts.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Contents.Fonts;

    public static class StringUtils
    {
        #region String Cropping

        /// <remarks>
        /// I chose this font because it is a Chinese font that contains English 
        /// characters. It is the most large font for my current use.
        /// </remarks>
        private static MMFont NSimSunRegularFont { get; set; }
        
        public static string CropMonospacedString(string str, float scale, int maxLength)
        {
            return CropString(NSimSunRegularFont, str, scale, maxLength, true);
        }

        public static string CropMonospacedStringByAsciiCount(string str, int count)
        {
            // Don't need to consider size when crop by character count
            return CropMonospacedString(str, 1.0f, (int)(count * NSimSunRegularFont.MonoData.AsciiSize(1.0f).X));
        }

        public static string CropString(MMFont font, string str, float scale, int maxLength, bool monospaced = false)
        {
            if (maxLength < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(maxLength));
            }

            var stringCropped = font.AvailableString(str);
            var stringSize    = font.MeasureString(stringCropped, scale, monospaced);

            var isCropped = false;
            var isOutOfRange = stringSize.X > maxLength;

            while (isOutOfRange)
            {
                isCropped = true;
                
                stringCropped = stringCropped.Substring(0, stringCropped.Length - 1);
                stringSize    = font.MeasureString(stringCropped, scale, monospaced);

                isOutOfRange = stringSize.X > maxLength;
            }

            if (isCropped)
            {
                return CropStringTail(stringCropped);
            }

            return stringCropped;
        }

        private static string CropStringTail(string str)
        {
            if (str.Length > 2)
            {
                var head = str.Substring(0, str.Length - 3);
                var tail = str.Substring(str.Length - 1 - 3, 3);

                return head + (string.IsNullOrWhiteSpace(tail) ? "   " : "...");
            }

            return str;
        }

        #endregion 

        #region Breaking

        public static IEnumerable<string> BreakStringByCharacterToEnumerable(string command, int characterNum)
        {
            var lines = new List<string>();

            while (command.Length > characterNum)
            {
                var splitCommand = command.Substring(0, characterNum);
                lines.Add(splitCommand);

                command = command.Substring(characterNum, command.Length - characterNum);
            }

            lines.Add(command);
            return lines;
        }

        /// <summary>
        /// Break string using word by word method.
        /// </summary>
        public static string BreakStringByWord(MMFont font, string str, float scale, float maxLineWidth, bool monospaced)
        {
            var spaceWidth = font.MeasureString(" ", scale, monospaced).X;

            var result = new StringBuilder();

            var lines = str.Split('\n');
            foreach (var line in lines)
            {
                var words = line.Split(' ');
                var lineWidth = 0f;
                foreach (var word in words)
                {
                    var size = font.MeasureString(word, scale, monospaced);
                    if (lineWidth + size.X < maxLineWidth)
                    {
                        result.Append(word + " ");
                        lineWidth += size.X + spaceWidth;
                    }
                    else
                    {
                        result.Append("\n" + word + " ");
                        lineWidth = size.X + spaceWidth;
                    }
                }

                result.Append("\n");
            }

            return result.ToString();
        }

        public static IEnumerable<string> BreakStringByWordToEnumerable(MMFont font, string str, float scale, float maxLineWidth, bool monospaced)
        {
            return BreakStringByWord(font, str, scale, maxLineWidth, monospaced).Split('\n');
        }

        #endregion

        #region Initialization

        public static void Initialize(IMMFontManager fonts)
        {
            if (fonts == null)
            {
                throw new ArgumentNullException(nameof(fonts));
            }

            NSimSunRegularFont = fonts["NSimSun Regular"];
        }

        #endregion
    }
}