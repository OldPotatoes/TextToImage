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
                    //using (Brush yellowBrush = new SolidBrush(Color.Bisque))
                    //{
                        //Int32 lineCount = 0;
                        // Each lineOfPieces is one line on the page in the design
                        // (though it can spill into multiple lines if the page width requires it
                        foreach (List<ImageText> lineOfPieces in details.TextPieces)
                        {
                            //    Brush brushToUse = yellowBrush;
                            //    if (++lineCount == 7)
                            //        brushToUse = textBrush;

                            DrawListOfImageText(drawing, textBrush, lineOfPieces);
                        }
                    //}
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

                SectionToBeDrawn section = measure.DefineTextToBeDrawn(new SizeF(_pageWidth, _pageHeight),
                                                                       new SizeF(_lineLeft, _lineTop),
                                                                       it.Text,
                                                                       it.Font);
                foreach (TextToBeDrawn textBlock in section.SectionParts)
                {
                    if (_lineLeft > _pageWidth)
                    {
                        _lineLeft = 0;
                        _lineTop += bottomEdge;
                    }
                    drawing.DrawString(textBlock.Text, textBlock.Font, textBrush, textBlock.LeftEdge, textBlock.TopEdge);
                    lineHeight = (lineHeight < textBlock.Height) ? textBlock.Height : lineHeight;
                    bottomEdge = (bottomEdge < textBlock.TopEdge + lineHeight) ? textBlock.TopEdge + lineHeight : bottomEdge;
                    _lineLeft = textBlock.LeftEdge + textBlock.Width;
                    _lineTop = textBlock.TopEdge;
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
    }
}
