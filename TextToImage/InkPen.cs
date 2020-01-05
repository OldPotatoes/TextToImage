using System;
using System.Windows;
using System.Collections.Generic;
using System.Drawing;

namespace TextToImage
{
    public class InkPen
    {
        private const Boolean DEBUG = true;
        private const Int32 MAX_HEIGHT = 20000;

        private Int32 _lineNumber;
        private Int32 _pageWidth;
        private Int32 _pageHeight;
        //private SizeF _absolutePagePosition;
        private Single _lineLeft;
        private Single _lineTop;
        private Single _lineRight;
        private Single _lineBottom;

        public Int32 LineNumber { get { return _lineNumber; } }
        public Int32 PageWidth { get { return _pageWidth; } }
        public Int32 PageHeight { get { return _pageHeight; } }
        //public SizeF AbsolutePagePosition { get { return _absolutePagePosition; } }
        public Single LineLeft { get { return _lineLeft; } }
        public Single LineTop { get { return _lineTop; } }
        public Single LineRight { get { return _lineRight; } }
        public Single LineBottom { get { return _lineBottom; } }
        public Single PageWidthRemaining { get { return PageWidth - LineRight; } }
        public Single CursorX { get; set; }
        public Single CursorY { get; set; }

        public InkPen()
        {
        }

        public InkPen(Int32 pageWidth, Int32 pageHeight)
        {
            _pageWidth = pageWidth;
            _pageHeight = pageHeight;
        }

        public void CreateImage(ImageDetails details)
        {
            WriteToImage(MAX_HEIGHT, details, false);
            WriteToImage((Int32)_lineTop, details, true);
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
                _lineRight = 0;
                _lineTop = 0;
                _lineBottom = 0;

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
                    SectionToBeDrawn section = measure.DefineTextToBeDrawn(new SizeF(_pageWidth, _pageHeight),
                                                                           new SizeF(_lineLeft, _lineTop),
                                                                           textPiece.Item1,
                                                                           it.Font);
                    foreach (TextToBeDrawn textBlock in section.SectionParts)
                    {
                        if (_lineLeft > _pageWidth)
                        {
                            _lineLeft = 0;
                            _lineTop += bottomEdge;
                        }

                        Font fontToUse = textBlock.Font;
                        if (textPiece.Item2)
                            fontToUse = new Font(textBlock.Font.FontFamily, textBlock.Font.Size, FontStyle.Italic);

                        drawing.DrawString(textBlock.Text, fontToUse, textBrush, textBlock.LeftEdge, textBlock.TopEdge);
                        lineHeight = (lineHeight < textBlock.Height) ? textBlock.Height : lineHeight;
                        bottomEdge = (bottomEdge < textBlock.TopEdge + lineHeight) ? textBlock.TopEdge + lineHeight : bottomEdge;
                        _lineLeft = textBlock.LeftEdge + textBlock.Width;
                        _lineTop = textBlock.TopEdge;
                    }
                }
            }
            _lineLeft = 0;
            _lineTop = bottomEdge;
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
                Int32 endIndex = 0;
                Int32 textLength = 0;
                Boolean isItalics = false;
                foreach (Int32 asteriskIndex in asterisks)
                {
                    endIndex = asteriskIndex;
                    textLength = endIndex - startIndex;
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
