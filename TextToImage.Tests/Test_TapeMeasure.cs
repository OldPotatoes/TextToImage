using System;
using System.Drawing;
using Moq;
using NUnit.Framework;

namespace TextToImage.Tests
{
    [TestFixture]
    class Test_TapeMeasure
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TapeMeasure_Constructor_NoParams()
        {
            // Arrange

            // Act
            var tm = new TapeMeasure();

            // Assert
            Assert.That(tm, Is.Not.Null, "Empty constructor should still construct");
        }

        [Test]
        public void TapeMeasure_Constructor_PassGraphics()
        {
            // Arrange
            var graphicsWrapper = GraphicsWrapper.Instance;

            // Act
            var tm = new TapeMeasure(graphicsWrapper);

            // Assert
            Assert.That(tm, Is.Not.Null, "Empty constructor should still construct");
        }

        [Test]
        public void TapeMeasure_FindEndOfNextWord_FromStart()
        {
            // Arrange
            String text = "Hello Elemental Purposeful Octopodes.";
            Int32 currentWordEnding = 0;

            // Act
            Int32 numberOfCharacters = TapeMeasure.FindNextWordEnding(text, currentWordEnding);

            // Assert
            Assert.AreEqual(5, numberOfCharacters, $"There are {numberOfCharacters} characters in the word, not the 5 expected.");
        }

        [Test]
        public void TapeMeasure_FindEndOfNextWord_FromMiddle()
        {
            // Arrange
            String text = "Hello Elemental Purposeful Octopodes.";
            Int32 currentWordEnding = 16;

            // Act
            Int32 numberOfCharacters = TapeMeasure.FindNextWordEnding(text, currentWordEnding);

            // Assert
            Assert.AreEqual(26, numberOfCharacters, $"There are {numberOfCharacters} characters in the word, not the 26 expected.");
        }

        [Test]
        public void TapeMeasure_MeasureText_ZeroLength()
        {
            // Arrange
            String text = "";
            Single textLength = 10 * text.Length;
            var font = new Font(FontFamily.GenericSerif, 100.0f, FontStyle.Bold);
            var mockGraphics = new Mock<IGraphics>();
            var measure = new TapeMeasure(mockGraphics.Object);
            mockGraphics.Setup(mock => mock.MeasureString(It.IsAny<String>(), It.IsAny<Font>())).Returns(new SizeF(textLength, 12));

            // Act
            SizeF textSize = measure.MeasureText(text, font);

            // Assert
            Assert.That(textSize.Width, Is.EqualTo(textLength), $"Text width is {textSize.Width}, not 10.0.");
            mockGraphics.Verify(mock => mock.MeasureString(It.IsAny<String>(), It.IsAny<Font>()), Times.Exactly(1));
        }

        [Test]
        public void TapeMeasure_MeasureText_HasLength()
        {
            // Arrange
            String text = "Hello I am a piece of text";
            Single textLength = 10 * text.Length;
            var font = new Font(FontFamily.GenericSerif, 100.0f, FontStyle.Bold);
            var mockGraphics = new Mock<IGraphics>();
            var measure = new TapeMeasure(mockGraphics.Object);
            mockGraphics.Setup(mock => mock.MeasureString(It.IsAny<String>(), It.IsAny<Font>())).Returns(new SizeF(textLength, 12));

            // Act
            SizeF textSize = measure.MeasureText(text, font);

            // Assert
            Assert.That(textSize.Width, Is.EqualTo(textLength), $"Text width is {textSize.Width}, not 10.0.");
            mockGraphics.Verify(mock => mock.MeasureString(It.IsAny<String>(), It.IsAny<Font>()), Times.Exactly(1));
        }

        //[Test]
        //public void TapeMeasure_FindWordFragmentToFitOnPage_LongWord()
        //{
        //    // Arrange
        //    String text = "Antidisestablishmentarianism.";
        //    Single textLength = 10 * text.Length;
        //    var font = new Font(FontFamily.GenericSerif, 100.0f, FontStyle.Bold);
        //    var mockGraphics = new Mock<IGraphics>();
        //    var measure = new TapeMeasure(mockGraphics.Object);
        //    mockGraphics.SetupSequence(mock => mock.MeasureString(It.IsAny<String>(), It.IsAny<Font>()))
        //        .Returns(new SizeF(textLength - 0, 12))
        //        .Returns(new SizeF(textLength - 10, 12))
        //        .Returns(new SizeF(textLength - 20, 12))
        //        .Returns(new SizeF(textLength - 30, 12))
        //        .Returns(new SizeF(textLength - 40, 12))
        //        .Returns(new SizeF(textLength - 50, 12))
        //        .Returns(new SizeF(textLength - 60, 12));
        //    Single pageWidth = 240.0f;

        //    // Act
        //    (String fragment, String remainder) = measure.FindWordFragmentToFitOnPage(text,
        //                                                                              pageWidth,
        //                                                                              font);

        //    // Assert
        //    Assert.That(fragment.Length, Is.EqualTo(pageWidth / 10), $"Text width is {fragment.Length}, not {pageWidth / 10}.");
        //    Assert.AreEqual(fragment.Substring(fragment.Length - 1), "-", $"Final character of {fragment} should be '-'.");
        //    Assert.AreEqual(fragment.Length + remainder.Length, text.Length + 1, $"Word fragment '{fragment}' and remainder '{remainder}' should be longer than {text}.");
        //}

        [Test]
        public void TapeMeasure_DefineTextToBeDrawn_EmptyText()
        {
            // We want an empty string to be effectively ignored

            // Arrange
            var pageSize = new SizeF(500, 2000);
            var startingLocation = new SizeF(0, 0);
            var text = "";
            var font = new Font(FontFamily.GenericSerif, 12, FontStyle.Bold);

            var mockGraphics = new Mock<IGraphics>();
            var measure = new TapeMeasure(mockGraphics.Object);

            // Act
            SectionToBeDrawn section = measure.DefineTextToBeDrawn(pageSize, startingLocation, text, font);

            // Assert
            Assert.That(section.SectionParts.Count, Is.EqualTo(0), "No text blocks.");
        }

        [Test]
        public void TapeMeasure_DefineTextToBeDrawn_Pilcrow()
        {
            // We want a Pilcrow to represent a new-line/carriage-return
            // by taking up the remainder of the line (and not printing the pilcrow)

            // Arrange
            var pageSize = new SizeF(500, 2000);
            var startingLocation = new SizeF(0, 0);
            var text = "¶";
            var font = new Font(FontFamily.GenericSerif, 12, FontStyle.Bold);

            var mockGraphics = new Mock<IGraphics>();
            var measure = new TapeMeasure(mockGraphics.Object);

            mockGraphics.Setup(mock => mock.MeasureString(It.IsAny<String>(), It.IsAny<Font>())).Returns(new SizeF(10, 12));

            // Act
            SectionToBeDrawn section = measure.DefineTextToBeDrawn(pageSize, startingLocation, text, font);

            // Assert
            Assert.That(section.SectionParts.Count, Is.EqualTo(1), "Only one text block.");
            Assert.That(section.SectionParts[0].Text, Is.EqualTo(""), "Text should be empty.");
            Assert.That(section.SectionParts[0].Font, Is.EqualTo(font), "Font must be the same.");
            Assert.That(section.SectionParts[0].LeftEdge, Is.EqualTo(0), "Block should start at the left side of the page.");
            Assert.That(section.SectionParts[0].TopEdge, Is.EqualTo(0), "Block should start at the top of the page.");
            Assert.That(section.SectionParts[0].Width, Is.EqualTo(500), "Block should reach the end of page.");
            Assert.That(section.SectionParts[0].Height, Is.EqualTo(12), "Block should be one line high.");
        }

        [Test]
        public void TapeMeasure_DefineTextToBeDrawn_ShortText()
        {
            // Arrange
            var pageSize = new SizeF(500, 2000);
            var startingLocation = new SizeF(0, 0);
            var text = "Short Text";
            var font = new Font(FontFamily.GenericSerif, 12, FontStyle.Bold);

            var mockGraphics = new Mock<IGraphics>();
            var measure = new TapeMeasure(mockGraphics.Object);

            mockGraphics.SetupSequence(mock => mock.MeasureString(It.IsAny<String>(), It.IsAny<Font>()))
                .Returns(new SizeF(50, 12))
                .Returns(new SizeF(100, 12));

            // Act
            SectionToBeDrawn section = measure.DefineTextToBeDrawn(pageSize, startingLocation, text, font);

            // Assert
            Assert.That(section.SectionParts.Count, Is.EqualTo(1), "Only one text block.");
            Assert.That(section.SectionParts[0].Text, Is.EqualTo(text), "Text should be all the short text.");
            Assert.That(section.SectionParts[0].Font, Is.EqualTo(font), "Font must be the same.");
            Assert.That(section.SectionParts[0].LeftEdge, Is.EqualTo(0), "Block should start at the left side of the page.");
            Assert.That(section.SectionParts[0].TopEdge, Is.EqualTo(0), "Block should start at the top of the page.");
            Assert.That(section.SectionParts[0].Width, Is.EqualTo(100), "Block should be length of string.");
            Assert.That(section.SectionParts[0].Height, Is.EqualTo(12), "Block should be one line high.");
        }

        [Test]
        public void TapeMeasure_DefineTextToBeDrawn_LongText()
        {
            // Arrange
            var pageSize = new SizeF(500, 2000);
            var startingLocation = new SizeF(0, 0);
            var text = "Hello Elemental Purposeful Octopodes. Your activity is graceless but your results are without equal.";
            var font = new Font(FontFamily.GenericSerif, 12, FontStyle.Bold);

            var mockGraphics = new Mock<IGraphics>();
            var measure = new TapeMeasure(mockGraphics.Object);

            mockGraphics.SetupSequence(mock => mock.MeasureString(It.IsAny<String>(), It.IsAny<Font>()))
                .Returns(new SizeF(50, 12))
                .Returns(new SizeF(160, 12))
                .Returns(new SizeF(260, 12))
                .Returns(new SizeF(370, 12))
                .Returns(new SizeF(420, 12))
                .Returns(new SizeF(510, 12))
                .Returns(new SizeF(420, 12))
                .Returns(new SizeF(80, 12))
                .Returns(new SizeF(110, 12))
                .Returns(new SizeF(210, 12))
                .Returns(new SizeF(250, 12))
                .Returns(new SizeF(300, 12))
                .Returns(new SizeF(380, 12))
                .Returns(new SizeF(420, 12))
                .Returns(new SizeF(500, 12))
                .Returns(new SizeF(570, 12))
                .Returns(new SizeF(500, 12))
                .Returns(new SizeF(60, 12));

            // Act
            SectionToBeDrawn section = measure.DefineTextToBeDrawn(pageSize, startingLocation, text, font);

            // Assert
            Assert.That(section.SectionParts.Count, Is.EqualTo(3), "There should be three text blocks.");
            Assert.That(section.SectionParts[0].Text, Is.EqualTo("Hello Elemental Purposeful Octopodes. Your"), "Text should be 'Hello Elemental Purposeful Octopodes. Your'.");
            Assert.That(section.SectionParts[0].Font, Is.EqualTo(font), "Font must be the same.");
            Assert.That(section.SectionParts[0].LeftEdge, Is.EqualTo(0), "Block should start at the left side of the page.");
            Assert.That(section.SectionParts[0].TopEdge, Is.EqualTo(0), "Block should start at the top of the page.");
            Assert.That(section.SectionParts[0].Width, Is.EqualTo(420), "Block should be width 420.");
            Assert.That(section.SectionParts[0].Height, Is.EqualTo(12), "Block should be one line high.");
            Assert.That(section.SectionParts[1].Text, Is.EqualTo("activity is graceless but your results are without"), "Text should be 'activity is graceless but your results are without'.");
            Assert.That(section.SectionParts[1].Font, Is.EqualTo(font), "Font must be the same.");
            Assert.That(section.SectionParts[1].LeftEdge, Is.EqualTo(0), "Block should start at the left side of the page.");
            Assert.That(section.SectionParts[1].TopEdge, Is.EqualTo(12), "Block should start one line down .");
            Assert.That(section.SectionParts[1].Width, Is.EqualTo(500), "Block should be 500 wide.");
            Assert.That(section.SectionParts[1].Height, Is.EqualTo(12), "Block should be one line high.");
            Assert.That(section.SectionParts[2].Text, Is.EqualTo("equal."), "Text should be 'equal'.");
            Assert.That(section.SectionParts[2].Font, Is.EqualTo(font), "Font must be the same.");
            Assert.That(section.SectionParts[2].LeftEdge, Is.EqualTo(0), "Block should start at the left side of the page.");
            Assert.That(section.SectionParts[2].TopEdge, Is.EqualTo(24), "Block should start two lines down.");
            Assert.That(section.SectionParts[2].Width, Is.EqualTo(60), "Block should be width 60.");
            Assert.That(section.SectionParts[2].Height, Is.EqualTo(12), "Block should be one line high.");
        }

        [Test]
        public void TapeMeasure_DefineTextToBeDrawn_ShortTextWithPilcrow()
        {
            // Arrange
            var pageSize = new SizeF(500, 2000);
            var startingLocation = new SizeF(0, 0);
            var shortText = "Short Text";
            var pilcrow = "¶";
            var text = shortText + pilcrow;
            var font = new Font(FontFamily.GenericSerif, 12, FontStyle.Bold);

            var mockGraphics = new Mock<IGraphics>();
            var measure = new TapeMeasure(mockGraphics.Object);

            mockGraphics.Setup(mock => mock.MeasureString(It.IsAny<String>(), It.IsAny<Font>())).Returns(new SizeF(100, 12));

            // Act
            SectionToBeDrawn section = measure.DefineTextToBeDrawn(pageSize, startingLocation, text, font);

            // Assert
            Assert.That(section.SectionParts.Count, Is.EqualTo(1), "Only one text block.");
            Assert.That(section.SectionParts[0].Text, Is.EqualTo(shortText), "Text should be short text.");
            Assert.That(section.SectionParts[0].Font, Is.EqualTo(font), "Font must be the same.");
            Assert.That(section.SectionParts[0].LeftEdge, Is.EqualTo(0), "Block should start at the left side of the page.");
            Assert.That(section.SectionParts[0].TopEdge, Is.EqualTo(0), "Block should start at the top of the page.");
            Assert.That(section.SectionParts[0].Width, Is.EqualTo(500), "Block should reach the end of page.");
            Assert.That(section.SectionParts[0].Height, Is.EqualTo(12), "Block should be one line high.");
        }

        //[Test]
        //public void TapeMeasure_FindTextToFitOnPage_Long()
        //{
        //    Single pageWidth = 3000.0f;
        //    Single pageWidthRemaining = 3000.0f;
        //    String text = "Hello Elemental Purposeful Octopodes.";
        //    var font = new Font(FontFamily.GenericSerif, (Single)100.0, FontStyle.Bold);
        //    var measure = new TapeMeasure();

        //    (String textOfWidth,
        //     String remainderText,
        //     SizeF lineSize) = measure.FindTextToFitOnPage(text,
        //                                                   pageWidth,
        //                                                   pageWidthRemaining,
        //                                                   font);

        //    Assert.Greater(lineSize.Height, 100);
        //    Assert.AreEqual(text, textOfWidth);
        //    Assert.AreEqual("", remainderText);
        //}

        //[Test]
        //public void TapeMeasure_FindTextToFitOnPage_SingleWord()
        //{
        //    Single pageWidth = 1000.0f;
        //    Single pageWidthRemaining = 1000.0f;
        //    String text = "Antidisestablishmentarianism.";
        //    var font = new Font(FontFamily.GenericSerif, (Single)100.0, FontStyle.Bold);
        //    var measure = new TapeMeasure();

        //    (String textOfWidth,
        //     String remainderText,
        //     SizeF lineSize) = measure.FindTextToFitOnPage(text,
        //                                                   pageWidth,
        //                                                   pageWidthRemaining,
        //                                                   font);

        //    Assert.Greater(lineSize.Height, 100);
        //    Assert.AreEqual(text, textOfWidth.Substring(0, textOfWidth.Length-1) + remainderText, $"Word fragment '{textOfWidth}' and remainder '{remainderText}' should equal original text: '{text}'.");
        //}

        //[Test]
        //public void TapeMeasure_FindTextToFitOnPage_SmallLastWord()
        //{
        //    Single pageWidth = 1000.0f;
        //    Single pageWidthRemaining = 1000.0f;
        //    String text = "Antidisestablishmentarianism X";
        //    var font = new Font(FontFamily.GenericSerif, (Single)100.0, FontStyle.Bold);
        //    var measure = new TapeMeasure();

        //    (String textOfWidth,
        //     String remainderText,
        //     SizeF lineSize) = measure.FindTextToFitOnPage(text,
        //                                                   pageWidth,
        //                                                   pageWidthRemaining,
        //                                                   font);

        //    Assert.Greater(lineSize.Height, 100);
        //    Assert.AreEqual(text, textOfWidth.Substring(0, textOfWidth.Length - 1) + remainderText, $"Word fragment '{textOfWidth}' and remainder '{remainderText}' should equal original text: '{text}'.");
        //}

        //[Test]
        //public void TapeMeasure_FindTextToFitOnPage_TwoFragments_OnOneLine()
        //{
        //    Single pageWidth = 3000.0f;
        //    String textTitle = "Title: ";
        //    String text = "Hello Elemental Purposeful Octopodes.";
        //    var fontTitle = new Font(FontFamily.GenericSerif, (Single)100.0, FontStyle.Bold);
        //    var font = new Font(FontFamily.GenericSerif, (Single)100.0, FontStyle.Regular);
        //    var measure = new TapeMeasure();

        //    (String textOfWidth,
        //     String remainderText,
        //     SizeF lineSize) = measure.FindTextToFitOnPage(textTitle,
        //                                                   pageWidth,
        //                                                   pageWidth,
        //                                                   fontTitle);
        //    Assert.Greater(lineSize.Height, 164);
        //    Assert.Less(lineSize.Height, 165);
        //    Assert.Greater(lineSize.Width, 364);
        //    Assert.Less(lineSize.Width, 365);
        //    Assert.AreEqual(textTitle, textOfWidth);
        //    Assert.AreEqual(String.Empty, remainderText);

        //    Single pageWidthRemaining = pageWidth - lineSize.Width;
        //    (textOfWidth,
        //     remainderText,
        //     lineSize) = measure.FindTextToFitOnPage(text,
        //                                                   pageWidth,
        //                                                   pageWidthRemaining,
        //                                                   font);

        //    Assert.Greater(lineSize.Height, 164);
        //    Assert.Less(lineSize.Height, 165);
        //    Assert.Greater(lineSize.Width, 2225);
        //    Assert.Less(lineSize.Width, 2226);
        //    Assert.AreEqual(text, textOfWidth);
        //    Assert.AreEqual(String.Empty, remainderText);
        //}

        //[Test]
        //public void TapeMeasure_FindTextToFitOnPage_TwoFragments_OnTwoLines()
        //{
        //    Single pageWidth = 600.0f;
        //    String textTitle = "Title: ";
        //    String text = "Hello Elemental Purposeful Octopodes.";
        //    var fontTitle = new Font(FontFamily.GenericSerif, (Single)100.0, FontStyle.Bold);
        //    var font = new Font(FontFamily.GenericSerif, (Single)100.0, FontStyle.Regular);
        //    var measure = new TapeMeasure();

        //    (String textOfWidth,
        //     String remainderText,
        //     SizeF lineSize) = measure.FindTextToFitOnPage(textTitle,
        //                                                   pageWidth,
        //                                                   pageWidth,
        //                                                   fontTitle);
        //    Assert.Greater(lineSize.Height, 164);
        //    Assert.Less(lineSize.Height, 165);
        //    Assert.Greater(lineSize.Width, 364);
        //    Assert.Less(lineSize.Width, 365);
        //    Assert.AreEqual(textTitle, textOfWidth);
        //    Assert.AreEqual(String.Empty, remainderText);

        //    Single pageWidthRemaining = pageWidth - lineSize.Width;
        //    (textOfWidth,
        //     remainderText,
        //     lineSize) = measure.FindTextToFitOnPage(text,
        //                                                   pageWidth,
        //                                                   pageWidthRemaining,
        //                                                   font);

        //    Assert.AreEqual(lineSize.Height, 0);
        //    Assert.AreEqual(lineSize.Width, 0);
        //    Assert.AreEqual(String.Empty, textOfWidth);
        //    Assert.AreEqual(text, remainderText);
        //}

        //[Test]
        //public void TapeMeasure_FindTextToFitOnPage_Paragraphs()
        //{
        //    Single pageWidth = 3000.0f;
        //    Single pageWidthRemaining = 3000.0f;
        //    String text = "Hello Elemental Purposeful¶Octopodes.";
        //    var font = new Font(FontFamily.GenericSerif, (Single)100.0, FontStyle.Bold);
        //    var measure = new TapeMeasure();

        //    (String textOfWidth,
        //     String remainderText,
        //     SizeF lineSize) = measure.FindTextToFitOnPage(text,
        //                                                   pageWidth,
        //                                                   pageWidthRemaining,
        //                                                   font);

        //    Assert.Greater(lineSize.Height, 100);
        //    Assert.AreEqual(text, textOfWidth);
        //    Assert.AreEqual("", remainderText);
        //}
    }
}
