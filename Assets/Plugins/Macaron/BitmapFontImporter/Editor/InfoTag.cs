using UnityEngine;

namespace Macaron.BitmapFontImporter.Editor
{
    internal struct InfoTag
    {
        public readonly string Face;
        public readonly int Size;
        public readonly bool Bold;
        public readonly bool Italic;
        public readonly string Charset;
        public readonly bool Unicode;
        public readonly int StretchH;
        public readonly bool Smooth;
        public readonly int SupersamplingLevel;
        public readonly int PaddingUp;
        public readonly int PaddingRight;
        public readonly int PaddingDown;
        public readonly int PaddingLeft;
        public readonly int SpacingHoriz;
        public readonly int SpacingVert;
        public readonly int Outline;

        public InfoTag(
            string face,
            int size,
            bool bold,
            bool italic,
            string charset,
            bool unicode,
            int stretchH,
            bool smooth,
            int supersamplingLevel,
            int paddingUp,
            int paddingRight,
            int paddingDown,
            int paddingLeft,
            int spacingHoriz,
            int spacingVert,
            int outline)
        {
            Face = face;
            Size = size;
            Bold = bold;
            Italic = italic;
            Charset = charset;
            Unicode = unicode;
            StretchH = stretchH;
            Smooth = smooth;
            SupersamplingLevel = supersamplingLevel;
            PaddingUp = paddingUp;
            PaddingRight = paddingRight;
            PaddingDown = paddingDown;
            PaddingLeft = paddingLeft;
            SpacingHoriz = spacingHoriz;
            SpacingVert = spacingVert;
            Outline = outline;
        }

        public FontStyle Style
        {
            get
            {
                var style = FontStyle.Normal;

                if (Bold)
                {
                    style |= FontStyle.Bold;
                }

                if (Italic)
                {
                    style |= FontStyle.Italic;
                }

                return style;
            }
        }
    }
}
