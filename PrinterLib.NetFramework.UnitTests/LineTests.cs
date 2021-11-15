using NUnit.Framework;
using System;
using System.Drawing;
using System.Drawing.Printing;

namespace PrinterLib.NetFramework.UnitTests
{
    public class LineTests
    {
        private BlockStyle _style;

        [SetUp]
        public void Setup()
        {
            _style = new BlockStyle
            {
                Font = new Font(FontFamily.GenericMonospace, 10),
                Brush = Brushes.Black,
                Margins = new Margins(0, 0, 0, 0),
                StringFormat = StringFormat.GenericDefault,
            };
        }

        [Test]
        public void WhenSingleBlockWidthGreaterThanLine_AddBlock_ThrowsException()
        {
            var line = new Line(10);
            Assert.Throws<ArgumentException>(() => line.AddBlock(new Block("a", 11, _style)));
        }

        [Test]
        public void WhenMultipleBlockWidthGreaterThanLine_AddBlock_ThrowsException()
        {
            var line = new Line(10);
            line.AddBlock(new Block("a", 5, _style));
            Assert.Throws<ArgumentException>(() => line.AddBlock(new Block("a", 6, _style)));
        }

        [Test]
        public void AddBlockRespectAddedOrder()
        {
            var firstBlock = new Block("z", 10, _style);
            var secondBlock = new Block("a", 10, _style);

            var line = new Line(100);
            line.AddBlock(firstBlock);
            line.AddBlock(secondBlock);

            Assert.AreSame(line.Blocks[0], firstBlock);
            Assert.AreSame(line.Blocks[1], secondBlock);
        }
    }
}