namespace Macaron.BitmapFontImporter.Editor
{
    internal struct CommonTag
    {
        public readonly int LineHeight;
        public readonly int BaseLine;
        public readonly int ScaleW;
        public readonly int ScaleH;
        public readonly int Pages;
        public readonly bool Packed;
        public readonly int AlphaChannel;
        public readonly int RedChannel;
        public readonly int GreenChannel;
        public readonly int BlueChannel;

        public CommonTag(
            int lineHeight,
            int baseLine,
            int scaleW,
            int scaleH,
            int pages,
            bool packed,
            int alphaChnl,
            int redChnl,
            int greenChnl,
            int blueChnl)
        {
            LineHeight = lineHeight;
            BaseLine = baseLine;
            ScaleW = scaleW;
            ScaleH = scaleH;
            Pages = pages;
            Packed = packed;
            AlphaChannel = alphaChnl;
            RedChannel = redChnl;
            GreenChannel = greenChnl;
            BlueChannel = blueChnl;
        }
    }
}
