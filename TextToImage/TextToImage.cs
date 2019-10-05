using System;
using System.Drawing;

namespace TextToImage
{
    public class TextToImage
    {
        const Int32 MAX_HEIGHT = 20000;
        //Single PreviousLineHeight = 0;
        Single LineHeight = 0;
        Int32 LinesDrawn = 0;
        Font Font = new Font(FontFamily.GenericSansSerif, (Single)72.0, FontStyle.Regular);

        public void CreateImage(ImageDetails details)
        {
            //Int32 totalHeight = 0;
            Int32 height = 0;
            var pen = new InkPen();
            Image image = new Bitmap(details.Width, MAX_HEIGHT);

            // First just to measure the length of image
            using (Graphics drawing = Graphics.FromImage(image))
            {
                drawing.Clear(details.BackgroundColor);

                Single distanceDownPage = 0.0f;
                using (Brush textBrush = new SolidBrush(details.TextColor))
                {
                    foreach (ImageText piece in details.TextPieces)
                    {
                        if (piece.Font != null)
                            Font = piece.Font;

                        LinesDrawn = 0;

                        drawing.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
                        //DrawTextPiece(drawing, textBrush, piece.Text, details.Width, distanceDownPage);
                        Single distanceDownThePage = pen.DrawLinesOfText(drawing, textBrush, Font, piece.Text, details.Width, distanceDownPage);
                        //drawing.Save();
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

                Single distanceDownPage = 0.0f;
                using (Brush textBrush = new SolidBrush(details.TextColor))
                {
                    foreach (ImageText piece in details.TextPieces)
                    {
                        if (piece.Font != null)
                            Font = piece.Font;

                        drawing.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
                        //DrawTextPiece(drawing, textBrush, piece.Text, details.Width, distanceDownPage);
                        Single distanceDownThePage = pen.DrawLinesOfText(drawing, textBrush, Font, piece.Text, details.Width, distanceDownPage);
                        drawing.Save();
                    }
                }
            }

            image.Save(details.Path);
        }

        //public void DrawTextPiece(Graphics drawing, Brush textBrush, String text, Int32 width, Single distanceDownPage)
        //{
        //    var pen = new InkPen();
        //    drawing.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;

        //    Single distanceDownThePage = pen.DrawLinesOfText(drawing, textBrush, Font, text, width, distanceDownPage);
        //    drawing.Save();
        //}
    }
}
