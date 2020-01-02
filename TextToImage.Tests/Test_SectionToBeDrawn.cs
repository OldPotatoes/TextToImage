using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextToImage.Tests
{
    [TestFixture]
    public class Test_SectionToBeDrawn
    {
        [Test]
        public void SectionToBeDrawn_Constructor_Empty()
        {
            // Arrange

            // Act
            var stbd = new SectionToBeDrawn();

            // Assert
            Assert.That(stbd, Is.Not.Null, "Empty constructor should still construct");
            Assert.That(stbd.SectionParts.Count, Is.EqualTo(0), "Empty constructor should still construct");
        }
    }
}
