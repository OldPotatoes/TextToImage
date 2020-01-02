using NUnit.Framework;
using System;

namespace TextToImage.Tests
{
    [TestFixture]
    public class Test_TextToBeDrawn
    {
        [Test]
        public void TextToBeDrawn_Constructor_Empty()
        {
            // Arrange

            // Act
            var ttbd = new TextToBeDrawn();

            // Assert
            Assert.That(ttbd.Text, Is.EqualTo(String.Empty), "Empty constructor should contain no text");
            Assert.That(ttbd.Font, Is.Not.Null, "Empty constructor should have a default font");
            Assert.That(ttbd.LeftEdge, Is.EqualTo(0), "Empty constructor should have left edge at 0");
            Assert.That(ttbd.TopEdge, Is.EqualTo(0), "Empty constructor should have top edge at 0");
        }
    }
}
