using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Macaron.BitmapFontImporter.Editor
{
    partial class FntParser
    {
        private class BinaryParser : IParser
        {
            private BinaryReader _reader;
            private Encoding _encoding;

            public BinaryParser(BinaryReader reader, Encoding encoding)
            {
                _reader = reader;
                _encoding = encoding;
            }

            public void Dispose()
            {
                if (_reader == null)
                {
                    return;
                }

                (_reader as IDisposable).Dispose();
                _reader = null;
            }

            public FontInfo Parse()
            {
                if (_reader == null)
                {
                    throw new ObjectDisposedException(null);
                }

                var info = default(InfoTag);
                var common = default(CommonTag);
                var pages = default(PageTag[]);
                var chars = default(CharTag[]);
                var kernings = default(KerningTag[]);

                _reader.ReadBytes(4); // signature

                while (_reader.PeekChar() != -1)
                {
                    var type = _reader.ReadByte();
                    switch (type)
                    {
                        case 1: info = ParseInfoTag(_reader); break;
                        case 2: common = ParseCommonTag(_reader); break;
                        case 3: pages = ParsePagesTag(_reader); break;
                        case 4: chars = ParseCharsTag(_reader); break;
                        case 5: kernings = ParseKerningsTag(_reader); break;

                        default:
                            // 지원하지 않는 형식은 건너뛴다.
                            int blockSize = Convert.ToInt32(_reader.ReadUInt32());
                            if (_reader.ReadBytes(blockSize).Length != blockSize)
                            {
                                throw new FntParserException();
                            }
                            break;
                    }
                }

                return new FontInfo(info, common, pages, chars, kernings);
            }

            private InfoTag ParseInfoTag(BinaryReader reader)
            {
                uint blockSize = reader.ReadUInt32();

                int fontSize = reader.ReadInt16();
                byte bitField = reader.ReadByte();
                bool smooth = (bitField & 0x80) != 0;
                bool unicode = (bitField & 0x40) != 0;
                bool italic = (bitField & 0x20) != 0;
                bool bold = (bitField & 0x10) != 0;
                int charset = reader.ReadByte();
                int stretchH = reader.ReadUInt16();
                int aa = reader.ReadByte();
                int paddingUp = reader.ReadByte();
                int paddingRight = reader.ReadByte();
                int paddingDown = reader.ReadByte();
                int paddingLeft = reader.ReadByte();
                int spacingHoriz = reader.ReadByte();
                int spacingVert = reader.ReadByte();
                int outline = reader.ReadByte();

                byte[] faceBytes = reader.ReadBytes(Convert.ToInt32(blockSize) - 14);
                string face = GetNullTerminatedString(faceBytes);

                string charsetStr = string.Empty;

                if (!unicode)
                {
                    switch (charset)
                    {
                        case 0: charsetStr = "ANSI"; break;
                        case 1: charsetStr = "DEFAULT"; break;
                        case 2: charsetStr = "SYMBOL"; break;
                        case 77: charsetStr = "MAC"; break;
                        case 128: charsetStr = "SHIFTJIS"; break;
                        case 129: charsetStr = "HANGUL"; break;
                        case 130: charsetStr = "JOHAB"; break;
                        case 134: charsetStr = "GB2312"; break;
                        case 136: charsetStr = "CHINESEBIG5"; break;
                        case 161: charsetStr = "GREEK"; break;
                        case 162: charsetStr = "TURKISH"; break;
                        case 163: charsetStr = "VIETNAMESE"; break;
                        case 177: charsetStr = "HEBREW"; break;
                        case 178: charsetStr = "ARABIC"; break;
                        case 186: charsetStr = "BALTIC"; break;
                        case 204: charsetStr = "RUSSIAN"; break;
                        case 222: charsetStr = "THAI"; break;
                        case 238: charsetStr = "EASTEUROPE"; break;
                        case 255: charsetStr = "OEM"; break;
                    }
                }

                return new InfoTag(
                    face,
                    fontSize,
                    bold,
                    italic,
                    charsetStr,
                    unicode,
                    stretchH,
                    smooth,
                    aa,
                    paddingUp,
                    paddingRight,
                    paddingDown,
                    paddingLeft,
                    spacingHoriz,
                    spacingVert,
                    outline);
            }

            private CommonTag ParseCommonTag(BinaryReader reader)
            {
                reader.ReadUInt32(); // blockSize

                int lineHeight = reader.ReadUInt16();
                int baseLine = reader.ReadUInt16();
                int scaleW = reader.ReadUInt16();
                int scaleH = reader.ReadUInt16();
                int pages = reader.ReadUInt16();
                byte bitField = reader.ReadByte();
                bool packed = (bitField & 0x01) != 0;
                int alphaChnl = reader.ReadByte();
                int redChnl = reader.ReadByte();
                int greenChnl = reader.ReadByte();
                int blueChnl = reader.ReadByte();

                return new CommonTag(
                    lineHeight,
                    baseLine,
                    scaleW,
                    scaleH,
                    pages,
                    packed,
                    alphaChnl,
                    redChnl,
                    greenChnl,
                    blueChnl);
            }

            private PageTag[] ParsePagesTag(BinaryReader reader)
            {
                uint blockSize = reader.ReadUInt32();

                byte[] bytes = reader.ReadBytes(Convert.ToInt32(blockSize));
                var pages = new List<PageTag>();

                for (int id = 0, startIndex = 0; startIndex < bytes.Length; ++id)
                {
                    string file = GetNullTerminatedString(bytes, ref startIndex);
                    pages.Add(new PageTag(id, file));
                }

                return pages.ToArray();
            }

            private CharTag[] ParseCharsTag(BinaryReader reader)
            {
                uint blockSize = reader.ReadUInt32();

                int count = Convert.ToInt32(blockSize) / 20;
                var chars = new CharTag[count];

                for (int i = 0; i < count; ++i)
                {
                    int id = (int)reader.ReadUInt32();
                    int x = reader.ReadUInt16();
                    int y = reader.ReadUInt16();
                    int width = reader.ReadUInt16();
                    int height = reader.ReadUInt16();
                    int xOffset = reader.ReadInt16();
                    int yOffset = reader.ReadInt16();
                    int xAdvance = reader.ReadInt16();
                    int page = reader.ReadByte();
                    int channel = reader.ReadByte();

                    chars[i] = new CharTag(id, x, y, width, height, xOffset, yOffset, xAdvance, page, channel);
                }

                return chars;
            }

            private KerningTag[] ParseKerningsTag(BinaryReader reader)
            {
                uint blockSize = reader.ReadUInt32();

                int count = Convert.ToInt32(blockSize) / 10;
                var kerningTags = new KerningTag[count];

                for (int i = 0; i < count; ++i)
                {
                    int first = Convert.ToInt32(reader.ReadUInt32());
                    int second = Convert.ToInt32(reader.ReadUInt32());
                    int amount = reader.ReadInt16();

                    kerningTags[i] = new KerningTag(first, second, amount);
                }

                return kerningTags;
            }

            private string GetNullTerminatedString(byte[] bytes)
            {
                int startIndex = 0;
                return GetNullTerminatedString(bytes, ref startIndex);
            }

            private string GetNullTerminatedString(byte[] bytes, ref int startIndex)
            {
                int endIndex = Array.IndexOf(bytes, (byte)0, startIndex);

                if (endIndex == -1)
                {
                    throw new FntParserException();
                }

                string result = _encoding.GetString(bytes, startIndex, endIndex - startIndex);
                startIndex = endIndex + 1;

                return result;
            }
        }
    }
}
