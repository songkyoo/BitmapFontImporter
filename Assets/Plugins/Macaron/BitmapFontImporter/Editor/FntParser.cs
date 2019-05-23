using System;
using System.IO;
using System.Text;

namespace Macaron.BitmapFontImporter.Editor
{
    internal partial class FntParser
    {
        public static FontInfo Parse(Stream stream, Encoding encoding)
        {
            using (var parser = CreateParser(stream, encoding))
            {
                return parser.Parse();
            }
        }

        private static IParser CreateParser(Stream stream, Encoding encoding)
        {
            if (stream == null)
            {
                throw new ArgumentNullException("stream");
            }

            if (encoding == null)
            {
                throw new ArgumentNullException("encoding");
            }

            const int minLength = 4;
            var buffer = new byte[minLength];
            if (stream.Read(buffer, 0, buffer.Length) < minLength)
            {
                throw new FntParserException();
            }

            stream.Position = 0;

            // 바이너리.
            if (buffer[0] == 'B' && buffer[1] == 'M' && buffer[2] == 'F' && buffer[3] == 3)
            {
                return new BinaryParser(new BinaryReader(stream), encoding);
            }

            var reader = new StreamReader(stream, encoding);

            // XML
            if (reader.Peek() == '<')
            {
                return new XmlParser(reader);
            }
            // 일반 텍스트.
            else
            {
                return new TextParser(reader);
            }
        }
    }
}
