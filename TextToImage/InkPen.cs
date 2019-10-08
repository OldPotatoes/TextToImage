using System;
using System.Collections.Generic;
using System.Drawing;

namespace TextToImage
{
    public class InkPen
    {
        private const Boolean DEBUG = true;
        private const Int32 MAX_HEIGHT = 20000;

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

                using (Brush textBrush = new SolidBrush(details.TextColor))
                {
                    foreach (List<ImageText> lineOfPieces in details.TextPieces)
                        distanceDownThePage = DrawLinesOfText(drawing,
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

            return distanceDownThePage;
        }

        public Single DrawLinesOfText(Graphics drawing,
                                    Brush textBrush,
                                    List<ImageText> lineOfTextPieces,
                                    Single width,
                                    Single distanceDownPage)
        {

            for (Int32 count = 0; count < lineOfTextPieces.Count; count++)
            {
                ImageText it = lineOfTextPieces[count];

                while (it.Text.Length > 0)
                {
                    (it.Text, distanceDownPage) = DrawLineFragment(drawing,
                                                                 textBrush,
                                                                 it,
                                                                 width,
                                                                 0.0f,
                                                                 distanceDownPage);
                }
            }

            return distanceDownPage;
        }

        //public (String, Single) DrawLine(Graphics drawing,
        //                                 Brush textBrush,
        //                                 List<ImageText> lineOfTextPieces,
        //                                 Single width,
        //                                 Single distanceDownPage)
        //{
        //    foreach (ImageText text in lineOfTextPieces)
        //    {
        //        if (text.Font == null)
        //            throw new Exception("Image text had a no font.");

        //    }


        //    TapeMeasure measure = new TapeMeasure();
        //    (String textToFitWidth,
        //     String remainingText,
        //     Single lineHeight) = measure.FindTextToFitWidth(textPiece.Text, (Int32)width, textPiece.Font);

        //    drawing.DrawString(textToFitWidth, textPiece.Font, textBrush, 0, distanceDownPage);

        //    if (DEBUG)
        //        Console.WriteLine($"InkPen: Text '{textToFitWidth}' written at {distanceDownPage} pixels down page.");

        //    distanceDownPage += lineHeight;

        //    if (DEBUG)
        //        Console.WriteLine($"InkPen: Next line should be at '{distanceDownPage}' pixels down page.");

        //    return (remainingText, distanceDownPage);
        //}

        public (String, Single) DrawLineFragment(Graphics drawing,
                                                 Brush textBrush,
                                                 ImageText textPiece,
                                                 Single pageWidthRemaining,
                                                 Single xOnPage,
                                                 Single yOnPage)
        {
            if (textPiece.Font == null)
                throw new Exception("Image text had a no font.");

            if (textPiece.Text.Length == 0)
                return (String.Empty, yOnPage);

            (String textToFitWidth, 
             String remainingText,
             Single lineHeight) = TapeMeasure.FindTextToFitWidth(textPiece.Text,
                                                                 pageWidthRemaining,
                                                                 textPiece.Font);

            drawing.DrawString(textToFitWidth, textPiece.Font, textBrush, xOnPage, yOnPage);

            if (DEBUG)
                Console.WriteLine($"InkPen: Text '{textToFitWidth}' written at {xOnPage},{yOnPage} pixels.");

            yOnPage += lineHeight;

            if (DEBUG)
                Console.WriteLine($"InkPen: Next line should be at '{yOnPage}' pixels down page.");

            return (remainingText, yOnPage);
        }
    }
}
