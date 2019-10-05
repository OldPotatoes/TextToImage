using System;
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
                    foreach (ImageText piece in details.TextPieces)
                    {
                        if (piece.Font == null)
                            throw new Exception("Image text had a no font.");

                        distanceDownThePage = DrawLinesOfText(drawing,
                                                              textBrush,
                                                              piece.Font,
                                                              piece.Text,
                                                              details.Width,
                                                              distanceDownThePage);
                    }
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
                                    Font font,
                                    String textoDraw,
                                    Single width,
                                    Single distanceDownPage)
        {
            while (textoDraw.Length > 0)
            {
                (textoDraw, distanceDownPage) = DrawLineOfText(drawing,
                                                               textBrush,
                                                               font,
                                                               textoDraw,
                                                               width,
                                                               distanceDownPage);
            }

            return distanceDownPage;
        }

        public (String, Single) DrawLineOfText(Graphics drawing,
                                               Brush textBrush,
                                               Font font,
                                               String textToDrawFrom,
                                               Single width,
                                               Single distanceDownPage)
        {
            TapeMeasure measure = new TapeMeasure();
            (String textToFitWidth, 
             String remainingText,
             Single lineHeight) = measure.FindTextToFitWidth(textToDrawFrom, (Int32)width, font);

            drawing.DrawString(textToFitWidth, font, textBrush, 0, distanceDownPage);

            if (DEBUG)
                Console.WriteLine($"InkPen: Text '{textToFitWidth}' written at {distanceDownPage} pixels down page.");

            distanceDownPage += lineHeight;

            if (DEBUG)
                Console.WriteLine($"InkPen: Next line should be at '{distanceDownPage}' pixels down page.");

            return (remainingText, distanceDownPage);
        }
    }
}
