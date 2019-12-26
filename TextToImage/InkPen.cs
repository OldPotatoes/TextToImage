using System;
using System.Collections.Generic;
using System.Drawing;

namespace TextToImage
{
    public class InkPen
    {
        private const Boolean DEBUG = true;
        private const Int32 MAX_HEIGHT = 20000;

        private SizeF _absolutePagePosition;
        public SizeF AbsolutePagePosition { get { return _absolutePagePosition; } }

        public void CreateImage(ImageDetails details)
        {
            Int32 pageHeight = (Int32)WriteToImage(MAX_HEIGHT, details, false);
            WriteToImage(pageHeight, details, true);
        }

        public Single WriteToImage(Int32 imageHeight,
                                   ImageDetails details,
                                   Boolean save = false)
        {
            Image image = new Bitmap(details.Width, imageHeight);

            using (Graphics drawing = Graphics.FromImage(image))
            {
                drawing.Clear(details.BackgroundColor);
                drawing.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;

                _absolutePagePosition.Width = 0.0f;
                _absolutePagePosition.Height = 0.0f;
                using (Brush textBrush = new SolidBrush(details.TextColor))
                {
                    foreach (List<ImageText> lineOfPieces in details.TextPieces)
                        DrawLineOfText(drawing, textBrush, lineOfPieces, details.Width);
                }

                if (save)
                    drawing.Save();
            }

            if (save)
                image.Save(details.Path);

            return AbsolutePagePosition.Height;
        }

        public void DrawLineOfText(Graphics drawing,
                                    Brush textBrush,
                                    List<ImageText> lineOfTextPieces,
                                    Single width)
        {
            _absolutePagePosition.Width = 0;
            // This loop will write an entire line
            for (Int32 count = 0; count < lineOfTextPieces.Count; count++)
            {
                SizeF relativeCursorPosition;
                ImageText it = lineOfTextPieces[count];
                // This loop will write an entire line fragment
                do
                {
                    Single widthRemaining = width - _absolutePagePosition.Width;
                    (it.Text, relativeCursorPosition) = DrawLineFragment(drawing, textBrush, it, width, widthRemaining);
                    _absolutePagePosition.Height += relativeCursorPosition.Height;
                    if (relativeCursorPosition.Width == 0 || ! String.IsNullOrWhiteSpace(it.Text))
                        _absolutePagePosition.Width = 0;
                }
                while (it.Text.Length > 0);

                _absolutePagePosition.Width += relativeCursorPosition.Width;

                if (lineOfTextPieces.Count > 1 && count < lineOfTextPieces.Count - 1)
                {
                    _absolutePagePosition.Height -= relativeCursorPosition.Height;
                }
            }
        }

        public (String, SizeF) DrawLineFragment(Graphics drawing,
                                                 Brush textBrush,
                                                 ImageText textPiece,
                                                 Single pageWidth,
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
                                                                  pageWidth,
                                                                  pageWidthRemaining,
                                                                  textPiece.Font);
                return (textPiece.Text, relativeCursorPosition);
            }

            (textToFitWidth, textRemaining, relativeCursorPosition) = TapeMeasure.FindTextToFitWidth(textPiece.Text,
                                                                                                     pageWidth,
                                                                                                     pageWidthRemaining,
                                                                                                     textPiece.Font);

            if (! String.IsNullOrEmpty(textToFitWidth))
                drawing.DrawString(textToFitWidth, textPiece.Font, textBrush, AbsolutePagePosition.Width, AbsolutePagePosition.Height);

            if (DEBUG)
                Console.WriteLine($"InkPen: Text '{textToFitWidth}' written at {AbsolutePagePosition.Width},{AbsolutePagePosition.Height} pixels.");
            if (DEBUG)
                Console.WriteLine($"InkPen: Next line should be at '{relativeCursorPosition.Height}' pixels down page.");

            return (textRemaining, relativeCursorPosition);
        }
    }
}
