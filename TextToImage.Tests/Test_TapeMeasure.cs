using System;
using System.Drawing;
using NUnit.Framework;

namespace TextToImage.Tests
{
    class Test_TapeMeasure
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test_MeasureText_ZeroLength()
        {
            var ruler = new TapeMeasure();

            String text = "";
            var font = new Font(FontFamily.GenericSerif, (Single)100.0, FontStyle.Bold);
            SizeF textSize = ruler.MeasureText(text, font);

            Assert.AreEqual(0.0, textSize.Width, $"Text width is {textSize.Width}, not 0.0.");
        }

        [Test]
        public void Test_MeasureText_HasLength()
        {
            var ruler = new TapeMeasure();

            String text = "Hello I am a piece of text";
            var font = new Font(FontFamily.GenericSerif, (Single)100.0, FontStyle.Bold);
            SizeF textSize = ruler.MeasureText(text, font);

            Assert.Greater(textSize.Width, 1000.0, $"Text width is {textSize.Width}, which is not greater than 1000.0.");
        }

        [Test]
        public void Test_FindEndOfNextWord_FromStart()
        {
            var ruler = new TapeMeasure();

            String text = "Hello Elemental Purposeful Octopodes.";
            Int32 currentWordEnding = 0;
            Int32 numberOfCharacters = ruler.FindNextWordEnding(text, currentWordEnding);

            Assert.AreEqual(5, numberOfCharacters, $"There are {numberOfCharacters} characters in the word, not the 5 expected.");
        }

        [Test]
        public void Test_FindEndOfNextWord_FromMiddle()
        {
            var ruler = new TapeMeasure();

            String text = "Hello Elemental Purposeful Octopodes.";
            Int32 currentWordEnding = 16;
            Int32 numberOfCharacters = ruler.FindNextWordEnding(text, currentWordEnding);

            Assert.AreEqual(26, numberOfCharacters);
        }

        [Test]
        public void Test_FindTextToFitWidth_Short()
        {
            var ruler = new TapeMeasure();

            Int32 pageWidth = 600;
            String text = "Hello Elemental Purposeful Octopodes.";
            var font = new Font(FontFamily.GenericSerif, (Single)100.0, FontStyle.Bold);

            (String textOfWidth, 
             String remainderText, 
             Single lineHeight) = ruler.FindTextToFitWidth(text,
                                                           pageWidth,
                                                           font);

            Assert.Greater(lineHeight, 100);
            Assert.AreEqual("Hello", textOfWidth);
            Assert.AreEqual("Elemental Purposeful Octopodes.", remainderText);
        }

        [Test]
        public void Test_FindTextToFitWidth_Long()
        {
            var ruler = new TapeMeasure();

            Int32 pageWidth = 3000;
            String text = "Hello Elemental Purposeful Octopodes.";
            var font = new Font(FontFamily.GenericSerif, (Single)100.0, FontStyle.Bold);

            (String textOfWidth,
             String remainderText,
             Single lineHeight) = ruler.FindTextToFitWidth(text,
                                                           pageWidth,
                                                           font);

            Assert.Greater(lineHeight, 100);
            Assert.AreEqual(text, textOfWidth);
            Assert.AreEqual("", remainderText);
        }
    }
}
