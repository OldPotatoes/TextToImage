using System;
using System.Collections.Generic;
using System.Drawing;
using NUnit.Framework;

namespace TextToImage.Tests
{
    class Test_InkPen
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test_DrawLineOfText_IsGood()
        {
            String startingText = "In that egg there was a bird, A rare bird and a rattlin' bird";
            String endingText = String.Empty;
            Single startingDistanceDownPage = 100.0f;
            Single endingDistanceDownPage = 0.0f;
            Single pageWidth = 500.0f;

            Image image = new Bitmap(100, 100);
            using (Graphics drawing = Graphics.FromImage(image))
            {
                drawing.Clear(Color.Black);

                using (Brush textBrush = new SolidBrush(Color.White))
                {
                    var font = new Font(FontFamily.GenericSerif, (Single)100.0, FontStyle.Bold);
                    var pen = new InkPen();
                    (endingText, endingDistanceDownPage) = pen.DrawLineOfText(drawing, textBrush, font, startingText, pageWidth, startingDistanceDownPage);
                }
            }

            Assert.Less(endingText.Length, startingText.Length, $"The ending text '{endingText}' is not shorter than the starting text '{startingText}'.");
            Assert.Greater(endingDistanceDownPage, startingDistanceDownPage, $"After adding a line, we've not moved down the page. We were at {startingDistanceDownPage}, we're now at {endingDistanceDownPage}");
        }

        [Test]
        public void Test_DrawLineOfText_NoText()
        {
            String startingText = "";
            String endingText = "";
            Single startingDistanceDownPage = 100.0f;
            Single endingDistanceDownPage = 100.0f;
            Single pageWidth = 100.0f;

            Image image = new Bitmap(100, 100);
            using (Graphics drawing = Graphics.FromImage(image))
            {
                drawing.Clear(Color.Black);

                using (Brush textBrush = new SolidBrush(Color.White))
                {
                    var font = new Font(FontFamily.GenericSerif, (Single)100.0, FontStyle.Bold);
                    var pen = new InkPen();
                    (endingText, endingDistanceDownPage) = pen.DrawLineOfText(drawing, textBrush, font, startingText, pageWidth, startingDistanceDownPage);
                }
            }

            Assert.AreEqual(endingText, startingText, $"The text should not change, but it was '{startingText}', and now it's '{endingText}'");
            Assert.AreEqual(endingDistanceDownPage, startingDistanceDownPage, $"The text should not change, but we were {startingDistanceDownPage} down the page, and now we're now {endingDistanceDownPage} down the page.");
        }

        [Test]
        public void Test_DrawLinesOfText_IsGood()
        {
            String text = "In that egg there was a bird, A rare bird and a rattlin' bird";
            Single startingDistanceDownPage = 100.0f;
            Single endingDistanceDownPage = 0.0f;
            Single pageWidth = 500.0f;

            Image image = new Bitmap(100, 100);
            using (Graphics drawing = Graphics.FromImage(image))
            {
                drawing.Clear(Color.Black);

                using (Brush textBrush = new SolidBrush(Color.White))
                {
                    var font = new Font(FontFamily.GenericSerif, (Single)100.0, FontStyle.Bold);
                    var pen = new InkPen();
                    endingDistanceDownPage = pen.DrawLinesOfText(drawing, textBrush, font, text, pageWidth, startingDistanceDownPage);
                }
            }

            Assert.Greater(endingDistanceDownPage, startingDistanceDownPage, $"After adding some lines, we've not moved down the page. We were at {startingDistanceDownPage}, we're now at {endingDistanceDownPage}");
        }

        [Test]
        public void Test_WriteToImage_IsGood()
        {
            Int32 pageWidth = 1000;
            Int32 pageHeight = 1000;
            String path = "";
            var textPieces = new List<ImageText>()
            {
                new ImageText("The first line of text", new Font(FontFamily.GenericSansSerif, (Single)72.0, FontStyle.Bold)),
                new ImageText("The second line of text", new Font(FontFamily.GenericSansSerif, (Single)72.0, FontStyle.Regular)),
                new ImageText("The third line of text", new Font(FontFamily.GenericSansSerif, (Single)60.0, FontStyle.Regular)),
                new ImageText("The fourth line of text", new Font(FontFamily.GenericSansSerif, (Single)32.0, FontStyle.Regular))
            };

            ImageDetails details = new ImageDetails(path, Color.White, Color.Black, textPieces);

            var pen = new InkPen();
            //Single distanceDownThePage = pen.WriteToImage(pageWidth, pageHeight, Color.Black, Color.White, textPieces);
            Single distanceDownThePage = pen.WriteToImage(pageHeight, details);

            Assert.NotZero(distanceDownThePage, $"Moved no distance down the page.");
        }
    }
}
