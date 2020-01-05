using System;
using System.Collections.Generic;
using System.Drawing;

namespace TextToImage
{
    public class InkPen
    {
        private const Int32 MAX_HEIGHT = 20000;
        private readonly Int32 _pageHeight;
        private Single _lineLeft;

        public Int32 PageWidth { get; }
        public Single LineTop { get; private set; }
        public Single LineRight { get; private set; }
        public Single LineBottom { get; private set; }
        public Single CursorX { get; set; }
        public Single CursorY { get; set; }

        public InkPen()
        {
        }

        public InkPen(Int32 pageWidth, Int32 pageHeight)
        {
            PageWidth = pageWidth;
            _pageHeight = pageHeight;
        }

        public void CreateImage(ImageDetails details)
        {
            WriteToImage(MAX_HEIGHT, details, false);
            WriteToImage((Int32)LineTop, details, true);
        }

        public void WriteToImage(Int32 imageHeight, ImageDetails details, Boolean save = false)
        {
            // details is a whole page to be converted into an image
            Image image = new Bitmap(details.Width, imageHeight);

            using (Graphics drawing = Graphics.FromImage(image))
            {
                drawing.Clear(details.BackgroundColor);
                drawing.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;

                _lineLeft = 0;
                LineRight = 0;
                LineTop = 0;
                LineBottom = 0;

                CursorX = 0;
                CursorY = 0;
                using (Brush textBrush = new SolidBrush(details.TextColor))
                {
                    foreach (List<ImageText> lineOfPieces in details.TextPieces)
                    {
                        DrawListOfImageText(drawing, textBrush, lineOfPieces);
                    }
                }

                if (save)
                    drawing.Save();
            }

            if (save)
                image.Save(details.Path);
        }

        public void DrawListOfImageText(Graphics drawing,
                                        Brush textBrush,
                                        List<ImageText> lineOfTextPieces)
        {
            // Each lineOfTextPieces is one line on the page in the design
            // (though it can spill into multiple lines if the page width requires it
            Single lineHeight = 0f;
            Single bottomEdge = 0f;
            var measure = new TapeMeasure();
            for (Int32 count = 0; count < lineOfTextPieces.Count; count++)
            {
                // Each imageText is one chunk of one line, though depending on line length
                // it may be broken into multiple parts within a section
                ImageText it = lineOfTextPieces[count];

                // Split text where italics are found
                List<(String, Boolean)> textPieces = SplitTextWithItalics(it.Text);
                foreach ((String, Boolean) textPiece in textPieces)
                {
                    SectionToBeDrawn section = measure.DefineTextToBeDrawn(new SizeF(PageWidth, _pageHeight),
                                                                           new SizeF(_lineLeft, LineTop),
                                                                           textPiece.Item1,
                                                                           it.Font);
                    foreach (TextToBeDrawn textBlock in section.SectionParts)
                    {
                        if (_lineLeft > PageWidth)
                        {
                            _lineLeft = 0;
                            LineTop += bottomEdge;
                        }

                        Font fontToUse = textBlock.Font;
                        if (textPiece.Item2)
                            fontToUse = new Font(textBlock.Font.FontFamily, textBlock.Font.Size, FontStyle.Italic);

                        drawing.DrawString(textBlock.Text, fontToUse, textBrush, textBlock.LeftEdge, textBlock.TopEdge);
                        lineHeight = (lineHeight < textBlock.Height) ? textBlock.Height : lineHeight;
                        bottomEdge = (bottomEdge < textBlock.TopEdge + lineHeight) ? textBlock.TopEdge + lineHeight : bottomEdge;
                        _lineLeft = textBlock.LeftEdge + textBlock.Width;
                        LineTop = textBlock.TopEdge;
                    }
                }
            }
            _lineLeft = 0;
            LineTop = bottomEdge;
        }

        public void DrawSection(Graphics drawingSurface, Brush brush, SectionToBeDrawn section)
        {
            foreach (TextToBeDrawn part in section.SectionParts)
            {
                drawingSurface.DrawString(part.Text, part.Font, brush, LineRight, LineTop);
            }
        }

        public List<(String, Boolean)> SplitTextWithItalics(String text)
        {
            List<(String, Boolean)> textPieces = new List<(String, Boolean)>();

            var asterisks = new List<Int32>();
            if (text.Contains('*'))
            {
                Int32 locationIndex = 0;
                while (locationIndex >= 0)
                {
                    locationIndex = text.IndexOf('*', locationIndex + 1);
                    if (locationIndex >= 0)
                        asterisks.Add(locationIndex);
                }

                if (asterisks.Count % 2 == 1)
                    throw new Exception("Odd number of asterisks in a piece of text.");

                Int32 startIndex = 0;
                Boolean isItalics = false;
                foreach (Int32 asteriskIndex in asterisks)
                {
                    Int32 endIndex = asteriskIndex;
                    Int32 textLength = endIndex - startIndex;
                    startIndex = (startIndex == 0) ? startIndex : startIndex + 1;
                    textPieces.Add((text.Substring(startIndex, endIndex - startIndex), isItalics));
                    startIndex = asteriskIndex;
                    isItalics = !isItalics;
                }
                textPieces.Add((text.Substring(startIndex + 1), isItalics));
            }
            else
                textPieces.Add((text, false));

            return textPieces;
        }
    }
}
