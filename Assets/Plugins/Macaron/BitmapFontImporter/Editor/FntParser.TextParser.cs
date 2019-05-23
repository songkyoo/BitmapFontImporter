using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Macaron.BitmapFontImporter.Editor
{
    partial class FntParser
    {
        private class TextParser : IParser
        {
            private class LineInfo
            {
                public readonly string Tag;
                public readonly Dictionary<string, string> Values;

                public LineInfo(string tag)
                {
                    Tag = tag;
                    Values = new Dictionary<string, string>();
                }
            }

            private static bool MoveNextWhitespace(string str, ref int index)
            {
                while (index < str.Length && !char.IsWhiteSpace(str, index))
                {
                    index += 1;
                }

                return index < str.Length;
            }

            private static bool MoveNextNonWhitespace(string str, ref int index)
            {
                while (index < str.Length && char.IsWhiteSpace(str, index))
                {
                    index += 1;
                }

                return index < str.Length;
            }

            private TextReader _reader;

            public TextParser(TextReader reader)
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

                var info = default(InfoTag);
                var common = default(CommonTag);
                var pages = new List<PageTag>();
                var chars = new List<CharTag>();
                var kernings = new List<KerningTag>();

                LineInfo lineInfo;
                while ((lineInfo = ReadLine()) != null)
                {
                    switch (lineInfo.Tag)
                    {
                        case "info": info = ParseInfoTag(lineInfo.Values); break;
                        case "common": common = ParseCommonTag(lineInfo.Values); break;
                        case "page": pages.Add(ParsePageTag(lineInfo.Values)); break;
                        case "char": chars.Add(ParseCharTag(lineInfo.Values)); break;
                        case "kerning": kernings.Add(ParseKerningTag(lineInfo.Values)); break;

                        // chars, kernings의 count 값은 사용하지 않는다.
                        case "chars":
                        case "kernings":
                            break;

                        // 지원하지 않는 태그는 무시한다.
                    }
                }

                return new FontInfo(info, common, pages.ToArray(), chars.ToArray(), kernings.ToArray());
            }

            private static string GetOrDefault(Dictionary<string, string> dict, string key, string defaultValue)
            {
                string value;
                return dict.TryGetValue(key, out value) ? value : defaultValue;
            }

            private LineInfo ReadLine()
            {
                string line = _reader.ReadLine();

                if (line == null)
                {
                    return null;
                }

                var index = 0;

                // 공백만 있는 라인은 건너뛴다.
                if (!MoveNextNonWhitespace(line, ref index))
                {
                    return ReadLine();
                }

                if (!MoveNextWhitespace(line, ref index))
                {
                    throw new FntParserException();
                }

                var lineInfo = new LineInfo(line.Substring(0, index));

                while (MoveNextNonWhitespace(line, ref index))
                {
                    int startIndex = index;

                    index = line.IndexOf('=', index);

                    if (index == -1)
                    {
                        throw new FntParserException();
                    }

                    string key = line.Substring(startIndex, index - startIndex).Trim();
                    string value = string.Empty;

                    index += 1;

                    if (!MoveNextNonWhitespace(line, ref index))
                    {
                        throw new FntParserException();
                    }

                    startIndex = index;

                    // 문자열 안에 이스케이프 문자가 없다고 가정한다.
                    if (line[index] == '"')
                    {
                        index = line.IndexOf('"', index + 1);

                        if (index == -1)
                        {
                            throw new FntParserException();
                        }

                        value = line.Substring(startIndex + 1, index - startIndex - 1);
                        index += 1;
                    }
                    else
                    {
                        MoveNextWhitespace(line, ref index);

                        value = line.Substring(startIndex, index - startIndex);
                    }

                    lineInfo.Values.Add(key, value);
                }

                return lineInfo;
            }

            private InfoTag ParseInfoTag(Dictionary<string, string> values)
            {
                string face = values["face"];
                int size = int.Parse(values["size"]);
                bool bold = int.Parse(GetOrDefault(values, "bold", "0")) == 1;
                bool italic = int.Parse(GetOrDefault(values, "italic", "0")) == 1;
                string charset = GetOrDefault(values, "charset", "");
                bool unicode = int.Parse(GetOrDefault(values, "unicode", "1")) == 1;
                int stretchH = int.Parse(GetOrDefault(values, "stretchH", "100"));
                bool smooth = int.Parse(GetOrDefault(values, "smooth", "1")) == 1;
                int aa = int.Parse(GetOrDefault(values, "aa", "1"));
                int[] padding = GetOrDefault(values, "padding", "0,0,0,0")
                    .Split(',')
                    .Select<string, int>(int.Parse)
                    .ToArray();
                int[] spacing = GetOrDefault(values, "spacing", "0,0")
                    .Split(',')
                    .Select<string, int>(int.Parse)
                    .ToArray();
                int outline = int.Parse(GetOrDefault(values, "outline", "0"));

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

            private CommonTag ParseCommonTag(Dictionary<string, string> values)
            {
                int lineHeight = int.Parse(values["lineHeight"]);
                int baseLine = int.Parse(values["base"]);
                int scaleW = int.Parse(values["scaleW"]);
                int scaleH = int.Parse(values["scaleH"]);
                int pages = int.Parse(values["pages"]);
                bool packed = int.Parse(GetOrDefault(values, "packed", "0")) == 1;
                int alphaChnl = int.Parse(GetOrDefault(values, "alphaChnl", "0"));
                int redChnl = int.Parse(GetOrDefault(values, "redChnl", "0"));
                int greenChnl = int.Parse(GetOrDefault(values, "greenChnl", "0"));
                int blueChnl = int.Parse(GetOrDefault(values, "blueChnl", "0"));

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

            private PageTag ParsePageTag(Dictionary<string, string> values)
            {
                int id = int.Parse(values["id"]);
                string file = values["file"];

                return new PageTag(id, file);
            }

            private CharTag ParseCharTag(Dictionary<string, string> values)
            {
                int id = (int)uint.Parse(values["id"]);
                int x = int.Parse(values["x"]);
                int y = int.Parse(values["y"]);
                int width = int.Parse(values["width"]);
                int height = int.Parse(values["height"]);
                int xOffset = int.Parse(values["xoffset"]);
                int yOffset = int.Parse(values["yoffset"]);
                int xAdvance = int.Parse(values["xadvance"]);
                int page = int.Parse(values["page"]);
                int channel = int.Parse(values["chnl"]);

                return new CharTag(id, x, y, width, height, xOffset, yOffset, xAdvance, page, channel);
            }

            private KerningTag ParseKerningTag(Dictionary<string, string> values)
            {
                int first = int.Parse(values["first"]);
                int second = int.Parse(values["second"]);
                int amount = int.Parse(values["amount"]);

                return new KerningTag(first, second, amount);
            }
        }
    }
}
