using System;

namespace Macaron.BitmapFontImporter.Editor
{
    internal struct CharTag
    {
        public readonly int ID;
        public readonly int X;
        public readonly int Y;
        public readonly int Width;
        public readonly int Height;
        public readonly int XOffset;
        public readonly int YOffset;
        public readonly int XAdvance;
        public readonly int Page;
        public readonly int Channel;

        public CharTag(
            int id,
            int x,
            int y,
            int width,
            int height,
            int xOffset,
            int yOffset,
            int xAdvance,
            int page,
            int channel)
        {
            ID = id;
            X = x;
            Y = y;
            Width = width;
            Height = height;
            XOffset = xOffset;
            YOffset = yOffset;
            XAdvance = xAdvance;
            Page = page;
            Channel = channel;
        }
    }
}
