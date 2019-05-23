using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;

namespace Macaron.BitmapFontImporter.Editor
{
    partial class FntParser
    {
        private class XmlParser : IParser
        {
            private TextReader _reader;

            public XmlParser(TextReader reader)
            {
                _reader = reader;
            }

            public void Dispose()
            {
                if (_reader == null)
                {
                    return;
                }

                _reader.Dispose();
                _reader = null;
            }

            public FontInfo Parse()
            {
                if (_reader == null)
                {
                    throw new ObjectDisposedException(null);
                }

                var reader = XmlReader.Create(_reader);

                var info = default(InfoTag);
                var common = default(CommonTag);
                var pages = default(PageTag[]);
                var chars = default(CharTag[]);
                var kernings = default(KerningTag[]);

                while (reader.Read())
                {
                    if (reader.NodeType != XmlNodeType.Element)
                    {
                        continue;
                    }

                    switch (reader.Name)
                    {
                        case "info": info = ParseInfoTag(reader); break;
                        case "common": common = ParseCommonTag(reader); break;
                        case "pages": pages = ParsePagesTag(reader); break;
                        case "chars": chars = ParseCharsTag(reader); break;
                        case "kernings": kernings = ParseKerningsTag(reader); break;

                        // 지원하지 않는 태그는 무시한다.
                    }
                }

                return new FontInfo(info, common, pages, chars, kernings);
            }

            private static string GetOrDefault(XmlReader reader, string attributeName, string defaultValue)
            {
                string attribute = reader[attributeName];
                return string.IsNullOrEmpty(attribute) ? defaultValue : attribute;
            }

            private InfoTag ParseInfoTag(XmlReader reader)
            {
                string face = reader["face"];
                int size = int.Parse(reader["size"]);
                bool bold = int.Parse(GetOrDefault(reader, "bold", "0")) == 1;
                bool italic = int.Parse(GetOrDefault(reader, "italic", "0")) == 1;
                string charset = GetOrDefault(reader, "charset", "");
                bool unicode = int.Parse(GetOrDefault(reader, "unicode", "1")) == 1;
                int stretchH = int.Parse(GetOrDefault(reader, "stretchH", "100"));
                bool smooth = int.Parse(GetOrDefault(reader, "smooth", "1")) == 1;
                int aa = int.Parse(GetOrDefault(reader, "aa", "1"));
                int[] padding = GetOrDefault(reader, "padding", "0,0,0,0")
                    .Split(',')
                    .Select<string, int>(int.Parse)
                    .ToArray();
                int[] spacing = GetOrDefault(reader, "spacing", "0,0")
                    .Split(',')
                    .Select<string, int>(int.Parse)
                    .ToArray();
                int outline = int.Parse(GetOrDefault(reader, "outline", "0"));

                return new InfoTag(
                    face,
                    size,
                    bold,
                    italic,
                    charset,
                    unicode,
                    stretchH,
                    smooth,
                    aa,
                    padding[0],
                    padding[1],
                    padding[2],
                    padding[3],
                    spacing[0],
                    spacing[1],
                    outline);
            }

            private CommonTag ParseCommonTag(XmlReader reader)
            {
                int lineHeight = int.Parse(reader["lineHeight"]);
                int baseLine = int.Parse(reader["base"]);
                int scaleW = int.Parse(reader["scaleW"]);
                int scaleH = int.Parse(reader["scaleH"]);
                int pages = int.Parse(reader["pages"]);
                bool packed = int.Parse(GetOrDefault(reader, "packed", "0")) == 1;
                int alphaChnl = int.Parse(GetOrDefault(reader, "alphaChnl", "0"));
                int redChnl = int.Parse(GetOrDefault(reader, "redChnl", "0"));
                int greenChnl = int.Parse(GetOrDefault(reader, "greenChnl", "0"));
                int blueChnl = int.Parse(GetOrDefault(reader, "blueChnl", "0"));

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

            private PageTag[] ParsePagesTag(XmlReader reader)
            {
                var pages = new List<PageTag>();

                while (reader.Read() && reader.NodeType != XmlNodeType.EndElement)
                {
                    if (reader.NodeType == XmlNodeType.Element && reader.Name == "page")
                    {
                        pages.Add(ParsePageTag(reader));
                    }
                }

                return pages.ToArray();
            }

            private PageTag ParsePageTag(XmlReader reader)
            {
                int id = int.Parse(reader["id"]);
                string file = reader["file"];

                return new PageTag(id, file);
            }

            private CharTag[] ParseCharsTag(XmlReader reader)
            {
                var chars = new List<CharTag>();

                while (reader.Read() && reader.NodeType != XmlNodeType.EndElement)
                {
                    if (reader.NodeType == XmlNodeType.Element && reader.Name == "char")
                    {
                        chars.Add(ParseCharTag(reader));
                    }
                }

                return chars.ToArray();
            }

            private CharTag ParseCharTag(XmlReader reader)
            {
                int id = (int)uint.Parse(reader["id"]);
                int x = int.Parse(reader["x"]);
                int y = int.Parse(reader["y"]);
                int width = int.Parse(reader["width"]);
                int height = int.Parse(reader["height"]);
                int xOffset = int.Parse(reader["xoffset"]);
                int yOffset = int.Parse(reader["yoffset"]);
                int xAdvance = int.Parse(reader["xadvance"]);
                int page = int.Parse(reader["page"]);
                int channel = int.Parse(reader["chnl"]);

                return new CharTag(id, x, y, width, height, xOffset, yOffset, xAdvance, page, channel);
            }

            private KerningTag[] ParseKerningsTag(XmlReader reader)
            {
                var kernings = new List<KerningTag>();

                while (reader.Read() && reader.NodeType != XmlNodeType.EndElement)
                {
                    if (reader.NodeType == XmlNodeType.Element && reader.Name == "kerning")
                    {
                        kernings.Add(ParseKerningTag(reader));
                    }
                }

                return kernings.ToArray();
            }

            private KerningTag ParseKerningTag(XmlReader reader)
            {
                int first = int.Parse(reader["first"]);
                int second = int.Parse(reader["second"]);
                int amount = int.Parse(reader["amount"]);

                return new KerningTag(first, second, amount);
            }
        }
    }
}
