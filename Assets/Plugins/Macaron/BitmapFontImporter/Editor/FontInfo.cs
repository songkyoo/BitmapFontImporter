namespace Macaron.BitmapFontImporter.Editor
{
    internal class FontInfo
    {
        public readonly InfoTag Info;
        public readonly CommonTag Common;
        public readonly PageTag[] Pages;
        public readonly CharTag[] Chars;
        public readonly KerningTag[] Kernings;

        public FontInfo(InfoTag info, CommonTag common, PageTag[] pages, CharTag[] chars, KerningTag[] kernings)
        {
            Info = info;
            Common = common;
            Pages = pages;
            Chars = chars;
            Kernings = kernings;
        }
    }
}
