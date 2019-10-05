using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace TextToImage
{
    public class TapeMeasure
    {
        private const Boolean DEBUG = true;

        public SizeF MeasureText(String text, Font font)
        {
            SizeF textSize;
            using (Image dummyImage = new Bitmap(1, 1))
            {
                using (Graphics drawing = Graphics.FromImage(dummyImage))
                {
                    textSize = drawing.MeasureString(text, font);
                }
            }

            if (DEBUG)
                Console.WriteLine($"TapeMeasure: Text '{text}' is {textSize} pixels long.");

            return textSize;
        }

        public Int32 FindNextWordEnding(String text, Int32 currentWordEnding = 0)
        {
            Int32 count = currentWordEnding;
            while (count < text.Length)
            {
                if (Char.IsWhiteSpace(text, count))
                    break;

                count++;
            }

            if (DEBUG)
                Console.WriteLine($"TapeMeasure: In text '{text}' the next word ending after {currentWordEnding} is {count} characters in.");

            return count;
        }

        public (String, String, Single) FindTextToFitWidth(String text, Int32 pageWidth, Font font)
        {
            String remainderText = String.Empty;
            Int32 bestWidthInCharacters = 0;
            Int32 wordEndingIndex = 0;
            Boolean pastEndOfLine = false;

            SizeF textSize;
            while (!pastEndOfLine)
            {
                wordEndingIndex = FindNextWordEnding(text, wordEndingIndex);

                String lineOfText = text.Substring(0, wordEndingIndex);
                textSize = MeasureText(lineOfText, font);

                if (textSize.Width <= pageWidth)
                    bestWidthInCharacters = lineOfText.Length; // Added a word to the line to be drawn
                else
                    pastEndOfLine = true; // No more words in the line

                if (wordEndingIndex < text.Length - 1)
                    wordEndingIndex++; // Step past the white space
                else
                    pastEndOfLine = true; // At the end of the text
            }

            if (bestWidthInCharacters == 0)
            {
                // BUG
                // A single word is longer than the maximum line
            }

            String textInLine = text.Substring(0, bestWidthInCharacters);
            if (bestWidthInCharacters < text.Length)
                remainderText = text.Substring(bestWidthInCharacters + 1);

            if (DEBUG)
                Console.WriteLine($"TapeMeasure: In text '{text}' on a page {pageWidth} wide, the line is {textInLine} (its height is {textSize.Height}).");

            return (textInLine, remainderText, textSize.Height);
        }
    }
}
