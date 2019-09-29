using System;
using System.Drawing;

namespace TextToImage
{
    public class TextToImage
    {
        readonly Color TextColor = Color.AntiqueWhite;
        readonly Color BackgroundColor = Color.Black;
        Single LineHeight = 0;
        Int32 LinesDrawn = 0;
        Font Font = new Font(FontFamily.GenericSansSerif, (Single)72.0, FontStyle.Regular);

        public String CreateImageFile(String text, String path, Int32 width=1920, Font font=null)
        {
            Int32 maxHeight = 20000;

            if (font != null)
                Font = font;

            // First just to measure the length of image
            DrawText(text, width, maxHeight);

            Int32 height = (Int32)LineHeight * LinesDrawn + 1;
            LinesDrawn = 0;
            Image picture = DrawText(text, width, height);


            picture.Save(path);

            return path;
        }

        private Image DrawText(String text, Int32 width, Int32 maxHeight)
        {
            Image image = new Bitmap(width, maxHeight);
            using (Graphics drawing = Graphics.FromImage(image))
            {
                drawing.Clear(BackgroundColor);

                using (Brush textBrush = new SolidBrush(TextColor))
                {
                    drawing.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;

                    DrawLinesOfText(drawing, textBrush, text, width);
                    drawing.Save();
                }
            }

            return image;
        }

        private void DrawLinesOfText(Graphics drawing, Brush textBrush, String text, Int32 width)
        {
            String remainderText = text;

            while (remainderText.Length > 0)
            {
                remainderText = DrawLineOfText(drawing, textBrush, remainderText, width);
                LinesDrawn++;
            }
        }

        private String DrawLineOfText(Graphics drawing, Brush textBrush, String remainderText, Int32 width)
        {
            String textOfWidth;
            (textOfWidth, remainderText) = FindTextToFitWidth(remainderText, width);
            drawing.DrawString(textOfWidth, Font, textBrush, 0, LineHeight * LinesDrawn);

            return remainderText;
        }

        private (String, String) FindTextToFitWidth(String text, Int32 width)
        {
            String textOfWidth = String.Empty;
            String remainderText = String.Empty;
            Int32 bestWidth = 0;
            Int32 wordEndingIndex = 0;
            Boolean foundEndOfLine = false;

            SizeF textSize;
            while (!foundEndOfLine)
            {
                wordEndingIndex = FindNextWordEnding(text, wordEndingIndex);

                String line = text.Substring(0, wordEndingIndex);
                textSize = MeasureText(line);

                if (textSize.Width <= width)
                    bestWidth = line.Length; // Added a word to the line to be drawn
                else
                    foundEndOfLine = true; // No more words in the line

                if (wordEndingIndex < text.Length - 1)
                    wordEndingIndex++; // Step past the white space
                else
                    foundEndOfLine = true; // At the end of the text
            }

            LineHeight = textSize.Height;

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

        private SizeF MeasureText(String text)
        {
            SizeF textSize;
            using (Image dummyImage = new Bitmap(1, 1))
            {
                using (Graphics drawing = Graphics.FromImage(dummyImage))
                {
                    textSize = drawing.MeasureString(text, Font);
                }
            }

            return textSize;
        }
    }
}
