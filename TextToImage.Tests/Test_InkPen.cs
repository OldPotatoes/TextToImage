using System;
using System.Collections.Generic;
using System.Drawing;
using NUnit.Framework;

namespace TextToImage.Tests
{
    class Test_InkPen
    {
        Boolean FloatWithinFivePercent(Single Expected, Single Actual)
        {
            return (Actual < Expected * 1.05 && Actual > Expected * 0.95) ? true : false;
        }

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test_DrawLineOfText_IsGood()
        {
            String startingText = "In that egg there was a bird, A rare bird and a rattlin' bird";
            var font = new Font(FontFamily.GenericSerif, (Single)100.0, FontStyle.Bold);
            var textPiece = new ImageText(startingText, font);
            Single startingDistanceDownPage = 100.0f;
            Single pageWidth = 500.0f;
            SizeF placeOnPage;

            Image image = new Bitmap(100, 100);
            using (Graphics drawing = Graphics.FromImage(image))
            {
                drawing.Clear(Color.Black);

                using (Brush textBrush = new SolidBrush(Color.White))
                {
                    var pen = new InkPen();
                    String endingText;
                    (endingText, placeOnPage) = pen.DrawLineFragment(drawing, textBrush, textPiece, pageWidth, 0.0f, startingDistanceDownPage);
                }
            }

            Single endingDistanceDownPage = placeOnPage.Height;

            Single lineHeight = 164f;
            Single expectedPage = startingDistanceDownPage + lineHeight * 1;

            Assert.IsTrue(FloatWithinFivePercent(expectedPage, endingDistanceDownPage), $"The distance down the page '{endingDistanceDownPage}' is not close to the expected distance '{expectedPage}'.");
            Assert.Greater(endingDistanceDownPage, startingDistanceDownPage, $"After adding a line, we've not moved down the page. We were at {startingDistanceDownPage}, we're now at {placeOnPage.Height}");
        }

        [Test]
        public void Test_DrawLineOfText_NoText()
        {
            String startingText = "";
            String endingText = "";
            Single startingDistanceDownPage = 100.0f;
            Single endingDistanceDownPage = 100.0f;
            Single pageWidth = 100.0f;
            var font = new Font(FontFamily.GenericSerif, (Single)100.0, FontStyle.Bold);
            var textPiece = new ImageText(startingText, font);
            SizeF placeOnPage;

            Image image = new Bitmap(100, 100);
            using (Graphics drawing = Graphics.FromImage(image))
            {
                drawing.Clear(Color.Black);

                using (Brush textBrush = new SolidBrush(Color.White))
                {
                    var pen = new InkPen();
                    (endingText, placeOnPage) = pen.DrawLineFragment(drawing, textBrush, textPiece, pageWidth, 0.0f, startingDistanceDownPage);
                }
            }

            Assert.AreEqual(endingText, startingText, $"The text should not change, but it was '{startingText}', and now it's '{endingText}'");
            Assert.Greater(placeOnPage.Height, startingDistanceDownPage, $"An empty line should move us down the page, but we were {startingDistanceDownPage} down the page, and now we're now {placeOnPage.Height} down the page.");
        }

        [Test]
        public void Test_DrawLinesOfText_IsGood()
        {
            String text = "In that egg there was a bird, A rare bird and a rattlin' bird";
            var font = new Font(FontFamily.GenericSerif, (Single)100.0, FontStyle.Bold);
            var textPiece = new ImageText(text, font);
            var listTextPieces = new List<ImageText>() { textPiece };
            Single startingDistanceDownPage = 100.0f;
            Single endingDistanceDownPage = 0.0f;
            Single pageWidth = 500.0f;

            Image image = new Bitmap(100, 100);
            using (Graphics drawing = Graphics.FromImage(image))
            {
                drawing.Clear(Color.Black);

                using (Brush textBrush = new SolidBrush(Color.White))
                {
                    var pen = new InkPen();
                    endingDistanceDownPage = pen.DrawLineOfText(drawing, textBrush, listTextPieces, pageWidth, startingDistanceDownPage);
                }
            }

            Single lineHeight = 164f;
            Single expectedPage = startingDistanceDownPage + lineHeight * 10;
            Assert.IsTrue(FloatWithinFivePercent(expectedPage, endingDistanceDownPage), $"After adding some lines, we've not moved down the page. We were at {startingDistanceDownPage}, we're now at {endingDistanceDownPage}");
        }

        [Test]
        public void Test_DrawLinesOfText_TwoEmpty()
        {
            var font = new Font(FontFamily.GenericSerif, (Single)100.0, FontStyle.Bold);
            var textPiece1 = new ImageText(String.Empty, font);
            var listTextPieces = new List<ImageText>() { textPiece1, textPiece1 };
            Single startingDistanceDownPage = 100.0f;
            Single endingDistanceDownPage = 0.0f;
            Single pageWidth = 500.0f;

            Image image = new Bitmap(100, 100);
            using (Graphics drawing = Graphics.FromImage(image))
            {
                drawing.Clear(Color.Black);

                using (Brush textBrush = new SolidBrush(Color.White))
                {
                    var pen = new InkPen();
                    endingDistanceDownPage = pen.DrawLineOfText(drawing, textBrush, listTextPieces, pageWidth, startingDistanceDownPage);
                }
            }

            Single lineHeight = 164f;
            Single expectedPage = startingDistanceDownPage + lineHeight * 2;
            Assert.IsTrue(FloatWithinFivePercent(expectedPage, endingDistanceDownPage), $"After adding some lines, we've not moved down the page. We were at {startingDistanceDownPage}, we're now at {endingDistanceDownPage}");
        }

        [Test]
        public void Test_WriteToImage_IsGood()
        {
            Int32 pageHeight = 1000;
            String path = "";
            var textPieces = new List<List<ImageText>>()
            {
                new List<ImageText>() { new ImageText("The first line of text", new Font(FontFamily.GenericSansSerif, (Single)72.0, FontStyle.Bold)) },
                new List<ImageText>() { new ImageText("The second line of text", new Font(FontFamily.GenericSansSerif, (Single)72.0, FontStyle.Regular)) },
                new List<ImageText>() { new ImageText("The third line of text", new Font(FontFamily.GenericSansSerif, (Single)60.0, FontStyle.Regular)) },
                new List<ImageText>() { new ImageText("The fourth line of text", new Font(FontFamily.GenericSansSerif, (Single)32.0, FontStyle.Regular)) }
            };

            ImageDetails details = new ImageDetails(path, Color.White, Color.Black, textPieces);

            var pen = new InkPen();
            Single distanceDownThePage = pen.WriteToImage(pageHeight, details);

            Assert.NotZero(distanceDownThePage, $"Moved no distance down the page.");
        }
    }
}
