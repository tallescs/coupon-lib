using System;
using System.Drawing;
using System.Drawing.Printing;

namespace PrinterSample.Print
{
    public abstract class CouponGenerator
    {
        protected readonly int _documentWidth;
        protected readonly Font _defaultFont;
        protected readonly Font _boldFont;
        protected readonly StringFormat _defaultFormat;
        protected readonly StringFormat _centerFormat;
        protected readonly StringFormat _rightFormat;
        protected readonly Margins _defaultMargins;

        //protected CouponGenerator(CouponStyle couponStyle)
        //{
        //    _documentWidth = couponStyle.CouponWidth;
        //    _defaultFont = couponStyle.DefaultFont;
        //    _boldFont = couponStyle.BoldFont;
        //    _defaultMargins = couponStyle.Margins;

        //    _defaultFormat = StringFormat.GenericTypographic;
        //    _centerFormat = new StringFormat(StringFormat.GenericTypographic)
        //    {
        //        Alignment = StringAlignment.Center
        //    };
        //    _rightFormat = new StringFormat(StringFormat.GenericTypographic)
        //    {
        //        Alignment = StringAlignment.Far
        //    };
        //}

        //public abstract Coupon CreateCoupon();

        //protected Line GenerateBlankLine() =>
        //    GenerateLine(" ");

        //protected Line GenerateLine(string text)
        //{
        //    var line = new Line(_documentWidth);
        //    var block = GenerateBlock(text, _defaultFont);
        //    line.AddBlock(block);
        //    return line;
        //}

        //protected Line GenerateLine(string text, Font font)
        //{
        //    var line = new Line(_documentWidth);
        //    var block = GenerateBlock(text, font);
        //    line.AddBlock(block);
        //    return line;
        //}

        //protected Line GenerateLine(params Block[] blocks)
        //{
        //    var line = new Line(_documentWidth);
        //    foreach (var b in blocks)
        //        line.AddBlock(b);
        //    return line;
        //}

        //protected Block GenerateBlock(string text, Font font, StringFormat format,
        //   Margins margins, int widthPercent = 100, BlockSpacing blockSpacing = BlockSpacing.Narrow)
        //{
        //    decimal adjust = (decimal)widthPercent / 100;
        //    int blockWidth = (int)Math.Floor(adjust * _documentWidth);

        //    return new Block(text, font, format, blockWidth, margins, blockSpacing)
        //    {
        //        Filler = "",
        //        FillPosition = BlockFillPosition.Right
        //    };
        //}

        //protected Block GenerateBlock(string text, int widthPercent = 100) =>
        //    GenerateBlock(text, _defaultFont, _defaultFormat, _defaultMargins, widthPercent);

        //protected Block GenerateBlock(string text, Font font, int widthPercent = 100, BlockSpacing blockSpacing = BlockSpacing.Narrow) =>
        //    GenerateBlock(text, font, _defaultFormat, _defaultMargins, widthPercent, blockSpacing);

        //protected Block GenerateBlock(string text, StringFormat format, int widthPercent = 100) =>
        //    GenerateBlock(text, _defaultFont, format, _defaultMargins, widthPercent);

        //protected Block GenerateBlock(string text, Font font, StringFormat format, int widthPercent = 100) =>
        //    GenerateBlock(text, font, format, _defaultMargins, widthPercent);

        //protected Block GenerateBlock(string text, Margins margins, int widthPercent = 100) =>
        //    GenerateBlock(text, _defaultFont, _defaultFormat, margins, widthPercent);

        //protected Block GenerateBlock(string text, StringFormat format, Margins margins, int widthPercent = 100) =>
        //    GenerateBlock(text, _defaultFont, format, margins, widthPercent);

        //protected Block GenerateFillerBlock(string text, string filler, int widthPercent = 100) =>
        //    GenerateFillerBlock(text, filler, _defaultFont, _defaultFormat, _defaultMargins, widthPercent);

        //protected Block GenerateFillerBlock(string text, string filler, StringFormat format,
        //    BlockFillPosition fillPosition = BlockFillPosition.Right) =>
        //    GenerateFillerBlock(text, filler, _defaultFont, format, _defaultMargins, fillPosition: fillPosition);

        //protected Block GenerateFillerBlock(string text, string filler,
        //    Font font, StringFormat format,
        //    Margins margins, int widthPercent = 100,
        //    BlockFillPosition fillPosition = BlockFillPosition.Right)
        //{
        //    decimal adjust = (decimal)widthPercent / 100;
        //    int blockWidth = (int)Math.Floor(adjust * _documentWidth);
        //    return new Block(text, font, format, blockWidth, margins)
        //    {
        //        Filler = filler,
        //        FillPosition = fillPosition
        //    };
        //}
    }
}
