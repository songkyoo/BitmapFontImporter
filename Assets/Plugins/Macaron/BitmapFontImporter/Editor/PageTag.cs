namespace Macaron.BitmapFontImporter.Editor
{
    internal struct PageTag
    {
        public readonly int ID;
        public readonly string File;

        public PageTag(int id, string file)
        {
            ID = id;
            File = file;
        }
    }
}
