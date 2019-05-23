using System;
using UnityEditor;

namespace Macaron.BitmapFontImporter.Editor
{
    internal struct ModifySerializedObject : IDisposable
    {
        private SerializedObject _serializedObject;

        public ModifySerializedObject(SerializedObject serializedObject)
        {
            _serializedObject = serializedObject;
            _serializedObject.Update();
        }

        public void Dispose()
        {
            _serializedObject.ApplyModifiedProperties();
        }
    }
}
