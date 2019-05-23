using System;
using UnityEditor;

namespace Macaron.BitmapFontImporter.Editor
{
    partial class BitmapFontImporterWindow
    {
        [Serializable]
        private struct Message
        {
            public MessageType Type;
            public string Value;
        }
    }
}
