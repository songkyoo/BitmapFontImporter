using System;
using UnityEditor;

namespace Macaron.BitmapFontImporter.Editor
{
    internal enum IndentLevelMode
    {
        None,
        Relative,
        Absolute,
    }

    internal struct IndentLevel : IDisposable
    {
        private int _level;
        private IndentLevelMode _mode;

        public IndentLevel(int level, IndentLevelMode mode = IndentLevelMode.Relative)
        {
            _mode = mode;

            switch (mode)
            {
                case IndentLevelMode.Relative:
                    _level = level;
                    EditorGUI.indentLevel += level;
                    break;

                case IndentLevelMode.Absolute:
                    _level = EditorGUI.indentLevel;
                    EditorGUI.indentLevel = level;
                    break;

                default:
                    _level = 0;
                    break;
            }
        }

        public void Dispose()
        {
            switch (_mode)
            {
                case IndentLevelMode.Relative:
                    EditorGUI.indentLevel -= _level;
                    break;

                case IndentLevelMode.Absolute:
                    EditorGUI.indentLevel = _level;
                    break;
            }

            _mode = IndentLevelMode.None;
        }
    }
}
