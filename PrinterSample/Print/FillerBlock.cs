using System.Collections.Generic;
using System.Drawing;

namespace PrinterSample.Print
{
    public class FillerBlock : Block
    {
        public string Filler { get; }
        public BlockFillPosition FillPosition { get; }

        public FillerBlock(string text, int width, BlockStyle style,
            string filler, BlockFillPosition fillPosition) : base(text, width, style)
        {
            Filler = filler;
            FillPosition = fillPosition;
        }

        public override string GetText(Graphics g)
        {
            var blockSize = new SizeF(GetWidth(), GetHeight(g));

            var resultText = Text;
            var textSize = g.MeasureString(Text, Font,
               blockSize, Format, out int charsFilled, out int linesFilled);

            var availableWidth = GetWidth() - textSize.Width;

            if (linesFilled == 1 && availableWidth > 0 && Filler.Length > 0)
            {
                var nextText = GetFilledText(resultText);
                var nextWidth = g.MeasureString(nextText, Font).ToSize().Width;

                while (nextWidth < GetWidth())
                {
                    resultText = nextText;
                    nextText = GetFilledText(resultText);
                    nextWidth = g.MeasureString(nextText, Font).ToSize().Width;
                }
            }
            return resultText;
        }

        private string GetFilledText(string text)
        {
            var filledTexts = new Dictionary<BlockFillPosition, string>
            {
                [BlockFillPosition.Left] = $"{Filler}{text}",
                [BlockFillPosition.Right] = $"{text}{Filler}",
                [BlockFillPosition.Both] = $"{Filler}{text}{Filler}",
            };

            return filledTexts[FillPosition];
        }
    }
}
