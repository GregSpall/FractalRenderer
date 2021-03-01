using System;
using System.Collections.Generic;
using System.Linq;

namespace Fractals.Processing
{
    public class ColourPalette
    {
        private List<PaletteMarker> _palette;

        public ColourPalette()
        {
            _palette = new List<PaletteMarker>();
        }

        public void AddMarker(Colour colour, decimal percentPosition)
        {
            if (percentPosition < 0 || percentPosition > 100)
                throw new ArgumentException("Position is a percentage, values cannot be less than 0 or greater than 100.");

            if (_palette.Any(p => p.PercentPosition == percentPosition))
                _palette.First(p => p.PercentPosition == percentPosition).Colour = colour;
            else
                _palette.Add(new PaletteMarker { Colour = colour, PercentPosition = percentPosition });

            _palette = _palette.OrderBy(p => p.PercentPosition).ToList();
        }

        public Colour GetColour(decimal position)
        {
            PaletteMarker prev = null;
            PaletteMarker next = null;

            foreach (var marker in _palette)
            {
                if (position >= marker.PercentPosition)
                {
                    prev = marker;
                }
                if (position <= marker.PercentPosition)
                {
                    next = marker;
                    break;
                }
            }

            if (prev == null || next == null || prev.PercentPosition == next.PercentPosition)
                return prev?.Colour ?? next?.Colour ?? Colour.Black();

            var ratio = GetPositionRatio(prev.PercentPosition, next.PercentPosition, position);
            return GetColourFromRatio(prev.Colour, next.Colour, ratio);
        }

        private decimal GetPositionRatio(decimal prevPosition, decimal nextPosition, decimal targetPosition)
        {
            var prevNextDiff = nextPosition - prevPosition;
            var targetDiffPosition = targetPosition - prevPosition;
            return targetDiffPosition / prevNextDiff;
        }

        private Colour GetColourFromRatio(Colour prev, Colour next, decimal ratio)
        {
            return new Colour(
                GetColourByteFromRatio(prev.Red, next.Red, ratio),
                GetColourByteFromRatio(prev.Green, next.Green, ratio),
                GetColourByteFromRatio(prev.Blue, next.Blue, ratio),
                GetColourByteFromRatio(prev.Alpha, next.Alpha, ratio)
            );
        }

        private byte GetColourByteFromRatio(byte min, byte max, decimal minRatio)
        {
            var output = 0m;

            output += min * minRatio;
            output += max * (1 - minRatio);

            return (byte)Math.Round(output);
        }


        private class PaletteMarker
        {
            public Colour Colour { get; set; }
            public decimal PercentPosition { get; set; }
        }
    }

    public class Colour
    {
        public byte Red { get; set; }
        public byte Green { get; set; }
        public byte Blue { get; set; }
        public byte Alpha { get; set; }

        public Colour(byte red, byte green, byte blue, byte alpha)
        {
            Red = red;
            Green = green;
            Blue = blue;
            Alpha = alpha;
        }

        public byte[] ToBytes()
        {
            return new [] { Red, Green, Blue, Alpha };
        }

        public static Colour Black() => new Colour(0,0,0,255);
        public static Colour White() => new Colour(255,255,255,255);
    }
}
