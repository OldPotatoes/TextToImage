using System;
using System.Collections.Generic;
using System.Drawing;

namespace TextToImage
{
    public class InkPen
    {
        private const Boolean DEBUG = true;
        private const Int32 MAX_HEIGHT = 20000;

        private SizeF AbsolutePagePosition;

        public void CreateImage(ImageDetails details)
        {
            Int32 pageHeight = (Int32)WriteToImage(MAX_HEIGHT, details, false);
            WriteToImage(pageHeight, details, true);
        }

        public Single WriteToImage(Int32 imageHeight,
                                   ImageDetails details,
                                   Boolean save = false)
        {
            Single distanceDownThePage = 0.0f;
            Image image = new Bitmap(details.Width, imageHeight);

            using (Graphics drawing = Graphics.FromImage(image))
            {
                drawing.Clear(details.BackgroundColor);
                drawing.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;

                AbsolutePagePosition.Width = 0.0f;
                AbsolutePagePosition.Height = 0.0f;
                using (Brush textBrush = new SolidBrush(details.TextColor))
                {
                    foreach (List<ImageText> lineOfPieces in details.TextPieces)
                        distanceDownThePage = DrawLineOfText(drawing,
                                                              textBrush,
                                                              lineOfPieces,
                                                              details.Width,
                                                              distanceDownThePage);
                }

                if (save)
                    drawing.Save();
            }

            if (save)
                image.Save(details.Path);

            return AbsolutePagePosition.Height;
        }

        public Single DrawLineOfText(Graphics drawing,
                                    Brush textBrush,
                                    List<ImageText> lineOfTextPieces,
                                    Single width,
                                    Single distanceDownPage)
        {
            AbsolutePagePosition.Width = 0;
            for (Int32 count = 0; count < lineOfTextPieces.Count; count++)
            {
                SizeF relativeCursorPosition;
                ImageText it = lineOfTextPieces[count];
                do
                {
                    (it.Text, relativeCursorPosition) = DrawLineFragment(drawing, textBrush, it, width);
                    AbsolutePagePosition.Height += relativeCursorPosition.Height;
                }
                while (it.Text.Length > 0);

                if (lineOfTextPieces.Count > 1 && count == 0)
                {
                    AbsolutePagePosition.Height -= relativeCursorPosition.Height;
                    AbsolutePagePosition.Width += relativeCursorPosition.Width;
                }
            }

            return distanceDownPage;
        }

        public (String, SizeF) DrawLineFragment(Graphics drawing,
                                                 Brush textBrush,
                                                 ImageText textPiece,
                                                 Single pageWidthRemaining)
        {
            if (textPiece.Font == null)
                throw new Exception("Image text had no font.");

            SizeF relativeCursorPosition;
            String textToFitWidth;
            String textRemaining;
            if (textPiece.Text.Length == 0)
            {
                // Move down one line - (Use a hypothetical I)
                (_, _, relativeCursorPosition) = TapeMeasure.FindTextToFitWidth("I",
                                                                  pageWidthRemaining,
                                                                  pageWidthRemaining,
                                                                  textPiece.Font);
                return (textPiece.Text, relativeCursorPosition);
            }

            (textToFitWidth, textRemaining, relativeCursorPosition) = TapeMeasure.FindTextToFitWidth(textPiece.Text,
                                                                                                     pageWidthRemaining,
                                                                                                     pageWidthRemaining,
                                                                                                     textPiece.Font);

            drawing.DrawString(textToFitWidth, textPiece.Font, textBrush, AbsolutePagePosition.Width, AbsolutePagePosition.Height);

            if (DEBUG)
                Console.WriteLine($"InkPen: Text '{textToFitWidth}' written at {AbsolutePagePosition.Width},{AbsolutePagePosition.Height} pixels.");
            if (DEBUG)
                Console.WriteLine($"InkPen: Next line should be at '{AbsolutePagePosition.Height}' pixels down page.");

            return (textRemaining, relativeCursorPosition);
        }
    }
}
