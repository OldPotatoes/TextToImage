using System;
using System.Drawing;

namespace TextToImage
{
    public class TapeMeasure
    {
        private const Boolean DEBUG = true;

        public static (String lineText, String remainderText, SizeF lineSize) FindTextToFitWidth(String text, Single pageWidth, Single pageWidthRemaining, Font font)
        {
            String remainderText = String.Empty;
            Int32 bestWidthInCharacters = 0;
            Int32 wordEndingIndex = 0;
            Boolean pastEndOfLine = false;
            Boolean textAllMeasured = false;

            SizeF textSize = new SizeF();
            while (!pastEndOfLine && !textAllMeasured)
            {
                wordEndingIndex = FindNextWordEnding(text, wordEndingIndex);

                String lineOfText = text.Substring(0, wordEndingIndex);
                textSize = MeasureText(lineOfText, font);

                if (textSize.Width > pageWidthRemaining)
                    pastEndOfLine = true; // No more room on the line
                else
                    bestWidthInCharacters = lineOfText.Length; // Added a word to the line to be drawn

                if (wordEndingIndex == text.Length)
                    textAllMeasured = true; // At the end of the text
                else
                    wordEndingIndex++; // Step past the white space
            }

            String textInLine = String.Empty;
            if (bestWidthInCharacters == 0)
            {
                if (pageWidth != pageWidthRemaining)
                {
                    // Try moving down a line and filling whole new line
                    remainderText = text;
                    return (textInLine, remainderText, new SizeF());
                }
                else
                {
                    // A single word is longer than the maximum line
                    (textInLine, remainderText) = FindWordFragmentToFitWidth(text, pageWidth, font);
                }
            }
            else
            {
                textInLine = text.Substring(0, bestWidthInCharacters);
                if (bestWidthInCharacters < text.Length)
                    remainderText = text.Substring(bestWidthInCharacters + 1);
            }

            if (DEBUG)
                Console.WriteLine($"TapeMeasure: In text '{text}' on a page with {pageWidthRemaining} width remaining, the line is {textInLine} (its height is {textSize.Height}).");

            return (textInLine, remainderText, textSize);
        }

        public static (String, String) FindWordFragmentToFitWidth(String word, Single pageWidth, Font font)
        {
            Boolean FragmentFits = false;
            String wordFragment = word;
            String fragmentWithHyphen = String.Empty;
            String remainderText = String.Empty;

            while (!FragmentFits)
            {
                wordFragment = wordFragment.Substring(0, wordFragment.Length-1);
                fragmentWithHyphen = wordFragment + "-";
                SizeF textSize = MeasureText(fragmentWithHyphen, font);

                if (textSize.Width < pageWidth)
                {
                    FragmentFits = true;
                    remainderText = word.Substring(wordFragment.Length);
                }
            }

            return (fragmentWithHyphen, remainderText);
        }

        public static SizeF MeasureText(String text, Font font)
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

        public static Int32 FindNextWordEnding(String text, Int32 currentWordEnding = 0)
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
    }
}
