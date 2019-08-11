using System;
using System.Drawing;

namespace TextToImage
{
    public class TextToImage
    {
        readonly Color TextColor = Color.AntiqueWhite;
        readonly Color BackgroundColor = Color.Black;
        const Int32 LineHeight = 25;

        public String CreateImageFile(String text, Int32 width, Int32 maxHeight, String path)
        {
            Font font = new Font(FontFamily.GenericSansSerif, (Single)12.0, FontStyle.Regular);

            Image picture = DrawText(text, font, width, maxHeight);
            picture.Save(path);

            return path;
        }

        private Image DrawText(String text, Font font, Int32 width, Int32 maxHeight)
        {
            Image image = new Bitmap(width, maxHeight);
            using (Graphics drawing = Graphics.FromImage(image))
            {
                drawing.Clear(BackgroundColor);

                using (Brush textBrush = new SolidBrush(TextColor))
                {
                    drawing.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;

                    DrawLineOfText(text, font, width, textBrush, drawing);
                    drawing.Save();
                }
            }

            return image;
        }

        private void DrawLineOfText(String text, Font font, Int32 width, Brush textBrush, Graphics drawing)
        {
            String textOfWidth = String.Empty;
            String remainderText = text;

            Int32 linesDrawn = 0;
            while (remainderText.Length > 0)
            {
                (textOfWidth, remainderText) = FindTextToFitWidth(remainderText, font, width);
                drawing.DrawString(textOfWidth, font, textBrush, 0, LineHeight * linesDrawn);
                linesDrawn++;
            }
        }

        private (String, String) FindTextToFitWidth(String text, Font font, Int32 width)
        {
            String textOfWidth = String.Empty;
            String remainderText = String.Empty;
            Int32 bestWidth = 0;
            Int32 wordEndingIndex = 0;
            Boolean foundEndOfLine = false;

            while (!foundEndOfLine)
            {
                wordEndingIndex = FindNextWordEnding(text, wordEndingIndex);

                String line = text.Substring(0, wordEndingIndex);
                SizeF textSize = MeasureText(line, font);

                if (textSize.Width <= width)
                    bestWidth = line.Length; // Added a word to the line to be drawn
                else
                    foundEndOfLine = true; // No more words in the line

                if (wordEndingIndex < text.Length - 1)
                    wordEndingIndex++; // Step past the white space
                else
                    foundEndOfLine = true; // At the end of the text
            }

            if (bestWidth == 0)
            {
                // A single word is longer than the maximum line

            }

            textOfWidth = text.Substring(0, bestWidth);
            if (bestWidth < text.Length)
                remainderText = text.Substring(bestWidth + 1);

            return (textOfWidth, remainderText);
        }

        private Int32 FindNextWordEnding(String text, Int32 currentWordEnding = 0)
        {
            Int32 count = currentWordEnding;
            while (count < text.Length)
            {
                if (Char.IsWhiteSpace(text, count))
                    break;

                count++;
            }

            return count;
        }

        private SizeF MeasureText(String text, Font font)
        {
            SizeF textSize;
            using (Image dummyImage = new Bitmap(1, 1))
            {
                using (Graphics drawing = Graphics.FromImage(dummyImage))
                {
                    textSize = drawing.MeasureString(text, font);
                }
            }

            return textSize;
        }
    }
}
