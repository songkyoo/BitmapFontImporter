using System;

namespace Macaron.BitmapFontImporter.Editor
{
    partial class BitmapFontImporterWindow
    {
        private class ImportException : Exception
        {
            public ImportException() : base()
            {
            }

            public ImportException(string message) : base(message)
            {
            }

            public ImportException(string message, Exception inner) : base(message, inner)
            {
            }
        }
    }
}
