using System.Collections.Generic;

namespace Macaron.BitmapFontImporter.Editor
{
    internal enum StringID
    {
        AllFileTypes,
        CanImportFontUsedSingleTextureOnly,
        ConvertedCharactersHasDifferentCount,
        ExceptionOccurredOnReadingFile,
        FailedToConvertCharacters,
        FailedToLoadTexture,
        FileEncoding,
        FntFile,
        FontFileExtensionMustBeFontsettings,
        FontFilePathMustBeIncludedAssetsDirectory,
        ForceMonospace,
        HasNoCharTags,
        HasNoPageTags,
        Import,
        ImportSuccess,
        InvalidCharacterID,
        InvalidFntFile,
        InvalidFontFileExtension,
        InvalidFontFilePath,
        NotSupportedCharset,
        OK,
        PackedFontShaderNotFound,
        SaveFont,
        SelectFile,
        SelectWithFilePanel,
        Settings,
        TextureFileNameIsEmpty,
    }

    internal class StringIDComparer : IEqualityComparer<StringID>
    {
        public static readonly StringIDComparer Instance = new StringIDComparer();

        private StringIDComparer()
        {
        }

        public bool Equals(StringID x, StringID y)
        {
            return x == y;
        }

        public int GetHashCode(StringID obj)
        {
            return (int)obj;
        }
    }
}
