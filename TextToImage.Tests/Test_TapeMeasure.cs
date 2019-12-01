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
            String text = "";
            var font = new Font(FontFamily.GenericSerif, (Single)100.0, FontStyle.Bold);
            SizeF textSize = TapeMeasure.MeasureText(text, font);

            Assert.AreEqual(0.0, textSize.Width, $"Text width is {textSize.Width}, not 0.0.");
        }

        [Test]
        public void Test_MeasureText_HasLength()
        {
            String text = "Hello I am a piece of text";
            var font = new Font(FontFamily.GenericSerif, (Single)100.0, FontStyle.Bold);
            SizeF textSize = TapeMeasure.MeasureText(text, font);

            Assert.Greater(textSize.Width, 1000.0, $"Text width is {textSize.Width}, which is not greater than 1000.0.");
        }

        [Test]
        public void Test_FindEndOfNextWord_FromStart()
        {
            String text = "Hello Elemental Purposeful Octopodes.";
            Int32 currentWordEnding = 0;
            Int32 numberOfCharacters = TapeMeasure.FindNextWordEnding(text, currentWordEnding);

            Assert.AreEqual(5, numberOfCharacters, $"There are {numberOfCharacters} characters in the word, not the 5 expected.");
        }

        [Test]
        public void Test_FindEndOfNextWord_FromMiddle()
        {
            String text = "Hello Elemental Purposeful Octopodes.";
            Int32 currentWordEnding = 16;
            Int32 numberOfCharacters = TapeMeasure.FindNextWordEnding(text, currentWordEnding);

            Assert.AreEqual(26, numberOfCharacters);
        }

        [Test]
        public void Test_FindTextToFitWidth_Short()
        {
            Single pageWidth = 600.0f;
            Single pageWidthRemaining = 600.0f;
            String text = "Hello Elemental Purposeful Octopodes.";
            var font = new Font(FontFamily.GenericSerif, (Single)100.0, FontStyle.Bold);

            (String textOfWidth, 
             String remainderText,
             SizeF lineSize) = TapeMeasure.FindTextToFitWidth(text,
                                                           pageWidth,
                                                           pageWidthRemaining,
                                                           font);

            Assert.Greater(lineSize.Height, 100);
            Assert.AreEqual("Hello", textOfWidth);
            Assert.AreEqual("Elemental Purposeful Octopodes.", remainderText);
        }

        [Test]
        public void Test_FindTextToFitWidth_Long()
        {
            Single pageWidth = 3000.0f;
            Single pageWidthRemaining = 3000.0f;
            String text = "Hello Elemental Purposeful Octopodes.";
            var font = new Font(FontFamily.GenericSerif, (Single)100.0, FontStyle.Bold);

            (String textOfWidth,
             String remainderText,
             SizeF lineSize) = TapeMeasure.FindTextToFitWidth(text,
                                                           pageWidth,
                                                           pageWidthRemaining,
                                                           font);

            Assert.Greater(lineSize.Height, 100);
            Assert.AreEqual(text, textOfWidth);
            Assert.AreEqual("", remainderText);
        }

        [Test]
        public void Test_FindTextToFitWidth_SingleWord()
        {
            Single pageWidth = 1000.0f;
            Single pageWidthRemaining = 1000.0f;
            String text = "Antidisestablishmentarianism.";
            var font = new Font(FontFamily.GenericSerif, (Single)100.0, FontStyle.Bold);

            (String textOfWidth,
             String remainderText,
             SizeF lineSize) = TapeMeasure.FindTextToFitWidth(text,
                                                           pageWidth,
                                                           pageWidthRemaining,
                                                           font);

            Assert.Greater(lineSize.Height, 100);
            Assert.AreEqual(text, textOfWidth.Substring(0, textOfWidth.Length-1) + remainderText, $"Word fragment '{textOfWidth}' and remainder '{remainderText}' should equal original text: '{text}'.");
        }

        [Test]
        public void Test_FindTextToFitWidth_SmallLastWord()
        {
            Single pageWidth = 1000.0f;
            Single pageWidthRemaining = 1000.0f;
            String text = "Antidisestablishmentarianism X";
            var font = new Font(FontFamily.GenericSerif, (Single)100.0, FontStyle.Bold);

            (String textOfWidth,
             String remainderText,
             SizeF lineSize) = TapeMeasure.FindTextToFitWidth(text,
                                                           pageWidth,
                                                           pageWidthRemaining,
                                                           font);

            Assert.Greater(lineSize.Height, 100);
            Assert.AreEqual(text, textOfWidth.Substring(0, textOfWidth.Length - 1) + remainderText, $"Word fragment '{textOfWidth}' and remainder '{remainderText}' should equal original text: '{text}'.");
        }

        [Test]
        public void Test_FindTextToFitWidth_TwoFragments_OnOneLine()
        {
            Single pageWidth = 3000.0f;
            String textTitle = "Title: ";
            String text = "Hello Elemental Purposeful Octopodes.";
            var fontTitle = new Font(FontFamily.GenericSerif, (Single)100.0, FontStyle.Bold);
            var font = new Font(FontFamily.GenericSerif, (Single)100.0, FontStyle.Regular);

            (String textOfWidth,
             String remainderText,
             SizeF lineSize) = TapeMeasure.FindTextToFitWidth(textTitle,
                                                           pageWidth,
                                                           pageWidth,
                                                           fontTitle);
            Assert.Greater(lineSize.Height, 164);
            Assert.Less(lineSize.Height, 165);
            Assert.Greater(lineSize.Width, 364);
            Assert.Less(lineSize.Width, 365);
            Assert.AreEqual(textTitle, textOfWidth);
            Assert.AreEqual(String.Empty, remainderText);

            Single pageWidthRemaining = pageWidth - lineSize.Width;
            (textOfWidth,
             remainderText,
             lineSize) = TapeMeasure.FindTextToFitWidth(text,
                                                           pageWidth,
                                                           pageWidthRemaining,
                                                           font);

            Assert.Greater(lineSize.Height, 164);
            Assert.Less(lineSize.Height, 165);
            Assert.Greater(lineSize.Width, 2225);
            Assert.Less(lineSize.Width, 2226);
            Assert.AreEqual(text, textOfWidth);
            Assert.AreEqual(String.Empty, remainderText);
        }

        [Test]
        public void Test_FindTextToFitWidth_TwoFragments_OnTwoLines()
        {
            Single pageWidth = 600.0f;
            String textTitle = "Title: ";
            String text = "Hello Elemental Purposeful Octopodes.";
            var fontTitle = new Font(FontFamily.GenericSerif, (Single)100.0, FontStyle.Bold);
            var font = new Font(FontFamily.GenericSerif, (Single)100.0, FontStyle.Regular);

            (String textOfWidth,
             String remainderText,
             SizeF lineSize) = TapeMeasure.FindTextToFitWidth(textTitle,
                                                           pageWidth,
                                                           pageWidth,
                                                           fontTitle);
            Assert.Greater(lineSize.Height, 164);
            Assert.Less(lineSize.Height, 165);
            Assert.Greater(lineSize.Width, 364);
            Assert.Less(lineSize.Width, 365);
            Assert.AreEqual(textTitle, textOfWidth);
            Assert.AreEqual(String.Empty, remainderText);

            Single pageWidthRemaining = pageWidth - lineSize.Width;
            (textOfWidth,
             remainderText,
             lineSize) = TapeMeasure.FindTextToFitWidth(text,
                                                           pageWidth,
                                                           pageWidthRemaining,
                                                           font);

            Assert.AreEqual(lineSize.Height, 0);
            Assert.AreEqual(lineSize.Width, 0);
            Assert.AreEqual(String.Empty, textOfWidth);
            Assert.AreEqual(text, remainderText);
        }

        [Test]
        public void Test_FindWordFragmentToFitWidth_SingleWord()
        {
            Single pageWidth = 1000.0f;
            String text = "Antidisestablishmentarianism.";
            var font = new Font(FontFamily.GenericSerif, (Single)100.0, FontStyle.Bold);

            (String fragment, String remainder) = TapeMeasure.FindWordFragmentToFitWidth(text,
                                                                                         pageWidth,
                                                                                         font);

            Assert.Greater(fragment.Length, 0, "Word fragment is empty.");
            Assert.AreEqual(fragment.Substring(fragment.Length-1), "-", $"Final character of {fragment} should be '-'.");
            Assert.AreEqual(fragment.Length + remainder.Length, text.Length+1, $"Word fragment '{fragment}' and remainder '{remainder}' should be longer than {text}.");
        }
    }
}
