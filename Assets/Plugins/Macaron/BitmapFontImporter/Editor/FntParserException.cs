using System;

namespace Macaron.BitmapFontImporter.Editor
{
    internal class FntParserException : Exception
    {
        public FntParserException() : base()
        {
        }

        public FntParserException(string message) : base(message)
        {
        }

        public FntParserException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}
