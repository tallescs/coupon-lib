using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace PrinterSample.Print
{
    public class Line
    {
        private IList<Block> _blocks { get; set; } = new List<Block>();
        public IList<Block> Blocks => _blocks;
        public int Width { get; }

        public Line(int width)
        {
            this.Width = width;
        }

        public int GetHeight(Graphics g)
        {
            if (!_blocks.Any())
                return 0;
            
            return _blocks.Max(p => p.GetTextHeight(g));
        }

        public void AddBlock(Block block)
        {
            var currentBlocksWidth = _blocks.Sum(p => p.Width);
            if (block.Width + currentBlocksWidth > Width)
                throw new ArgumentException("Adding this block would surpass this Line width");
            else if (block.Width == 0)
                throw new ArgumentException("Block width must be greater than 0.");

            _blocks.Add(block);
        }
    }
}