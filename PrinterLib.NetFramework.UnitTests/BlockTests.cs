using NUnit.Framework;
using System;
using System.Drawing;
using System.Drawing.Printing;

namespace PrinterLib.NetFramework.UnitTests
{
    public class BlockTests
    {
        private BlockStyle style;

        [SetUp]
        public void Setup()
        {
            style = new BlockStyle
            {
                Font = new Font(FontFamily.GenericMonospace, 10),
                Brush = Brushes.Black,
                Margins = new Margins(0, 0, 0, 0),
                StringFormat = StringFormat.GenericDefault,
            };
        }

        [Test]
        public void WhenWidthLowerThanZero_ThrowsException()
        {
            Assert.Throws<ArgumentException>(() => new Block("a", 0, style));
            Assert.Throws<ArgumentException>(() => new Block("a", -1, style));
        }

        [Test]
        public void WhenTextNullOrEmpty_ThrowsException()
        {
            Assert.Throws<ArgumentException>(() => new Block("", 1, style));
            Assert.Throws<ArgumentException>(() => new Block(null, 1, style));
        }
    }
}