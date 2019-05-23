using System;

namespace Macaron.BitmapFontImporter.Editor
{
    internal partial class FntParser
    {
        private interface IParser : IDisposable
        {
            FontInfo Parse();
        }
    }
}
