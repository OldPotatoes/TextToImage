using System;
using System.Drawing;

namespace TextToImage
{
    public class TextToImage
    {
        const Int32 MAX_HEIGHT = 20000;
        Single PreviousLineHeight = 0;
        Single LineHeight = 0;
        Int32 LinesDrawn = 0;
        Font Font = new Font(FontFamily.GenericSansSerif, (Single)72.0, FontStyle.Regular);

        public void CreateImage(ImageDetails details)
        {
            //Int32 totalHeight = 0;
            Int32 height = 0;
            Image image = new Bitmap(details.Width, MAX_HEIGHT);

            // First just to measure the length of image
            using (Graphics drawing = Graphics.FromImage(image))
            {
                drawing.Clear(details.BackgroundColor);

                using (Brush textBrush = new SolidBrush(details.TextColor))
                {
                    foreach (ImageText piece in details.TextPieces)
                    {
                        if (piece.Font != null)
                            Font = piece.Font;

                        LinesDrawn = 0;

                        DrawTextPiece(drawing, textBrush, piece.Text, details.Width);
                        height += (Int32)LineHeight * LinesDrawn + 1;
                    }
                }
            }

            LinesDrawn = 0;

            // Then to actually save the file
            image = new Bitmap(details.Width, height);
            using (Graphics drawing = Graphics.FromImage(image))
            {
                drawing.Clear(details.BackgroundColor);

                using (Brush textBrush = new SolidBrush(details.TextColor))
                {
                    foreach (ImageText piece in details.TextPieces)
                    {
                        if (piece.Font != null)
                            Font = piece.Font;

                        DrawTextPiece(drawing, textBrush, piece.Text, details.Width);
                    }
                }
            }

            image.Save(details.Path);
        }

        private void DrawTextPiece(Graphics drawing, Brush textBrush, String text, Int32 width)
        {
            drawing.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;

            DrawLinesOfText(drawing, textBrush, text, width);
            drawing.Save();
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
            drawing.DrawString(textOfWidth, Font, textBrush, 0, PreviousLineHeight * LinesDrawn);

            return remainderText;
        }

        private (String, String) FindTextToFitWidth(String text, Int32 width)
        {
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

            PreviousLineHeight = LineHeight;
            LineHeight = textSize.Height;

            if (bestWidth == 0)
            {
                // A single word is longer than the maximum line

            }

            String textOfWidth = text.Substring(0, bestWidth);
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
