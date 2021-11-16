using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace PrinterLib
{
    public class Line
    {
        protected BlockStyle BlockStyle { get; set; }
        protected List<Block> _blocks { get; set; } = new List<Block>();
        public IList<Block> Blocks => _blocks.AsReadOnly();
        public int Width { get; }

        public Line(int width)
        {
            this.Width = width;
        }

        public Line(int width, BlockStyle blockStyle)
        {
            Width = width;
            BlockStyle = blockStyle;
        }

        public Block AddBlock(string text)
        {
            ValidateBlockWidth(Width);

            var block = new Block(text, Width, BlockStyle);
            _blocks.Add(block);
            return block;
        }
        
        public Block AddBlock(string text, int percentWidth, BlockStyle style)
        {
            decimal adjust = (decimal)percentWidth / 100;
            int blockWidth = (int)Math.Floor(adjust * Width);

            ValidateBlockWidth(blockWidth);

            var block = new Block(text, blockWidth, style);
            _blocks.Add(block);
            return block;
        }

        public Block AddBlock(Block block)
        {
            ValidateBlockWidth(block.Width);
            _blocks.Add(block);
            return block;
        }

        public int GetHeight(Graphics g)
        {
            if (!_blocks.Any())
                return 0;
            
            return _blocks.Max(p => p.GetHeight(g));
        }

        private void ValidateBlockWidth(int blockWidth)
        {
            var currentBlocksWidth = _blocks.Sum(p => p.Width);
            if (blockWidth + currentBlocksWidth > Width)
                throw new ArgumentException("Adding this block would surpass this Line width");
        }
    }
}