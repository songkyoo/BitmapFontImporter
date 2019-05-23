namespace Macaron.BitmapFontImporter.Editor
{
    internal struct KerningTag
    {
        public readonly int First;
        public readonly int Second;
        public readonly int Amount;

        public KerningTag(int first, int second, int amount)
        {
            First = first;
            Second = second;
            Amount = amount;
        }
    }
}
