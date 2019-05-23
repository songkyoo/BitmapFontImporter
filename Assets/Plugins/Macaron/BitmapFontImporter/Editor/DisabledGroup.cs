using System;
using UnityEditor;

namespace Macaron.BitmapFontImporter.Editor
{
    internal struct DisabledGroup : IDisposable
    {
        private bool _disabled;

        public DisabledGroup(bool disabled)
        {
            _disabled = disabled;

            if (disabled)
            {
                EditorGUI.BeginDisabledGroup(disabled);
            }
        }

        public void Dispose()
        {
            if (_disabled)
            {
                EditorGUI.EndDisabledGroup();
            }
        }
    }
}
