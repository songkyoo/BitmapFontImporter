using System;
using System.Collections.Generic;
using UnityEngine;

namespace Macaron.BitmapFontImporter.Editor
{
    internal static class EditorResources
    {
        private const int _titleIconWidth = 14;
        private const int _titleIconHeight = 14;
        private const TextureFormat _titleIconFormat = TextureFormat.RGBA32;
        private const string _titleIconData = "goKCAIKCggCCgoIAOTk5ADk5OQA5OTkAOTk5ADk5OQA5OTkAOTk5ADk5OQCCgoIAgoKCAIKCggCCgoIAgoKCAIKCggA5OTkAOTk5ADk5OQA5OTkAOTk5ADk5OQA5OTkAOTk5AIKCggCCgoIAgoKCAIKCggCCgoIAgoKC/zk5Of85OTn/OTk5/zk5Of85OTn/OTk5/zk5Of85OTn/goKC/4KCggCCgoIAgoKCAIKCgv85OTn/OTk5/zk5Of85OTn/OTk5/zk5Of85OTn/OTk5/zk5Of85OTn/goKC/4KCggA5OTkAOTk5/zk5Of+CgoL/OTk5ADk5OQA5OTkAOTk5ADk5OQA5OTkAgoKC/zk5Of85OTn/OTk5ADk5OQA5OTn/OTk5/4KCggA5OTkAgoKCAIKCgv+CgoL/goKCADk5OQCCgoIAOTk5/zk5Of85OTkAOTk5ADk5Of85OTn/OTk5AIKCggCCgoL/OTk5/zk5Of+CgoL/goKCADk5OQA5OTn/OTk5/zk5OQA5OTkAOTk5ADk5OQCCgoIAgoKC/zk5Of85OTn/OTk5/zk5Of+CgoL/goKCADk5OQA5OTkAOTk5ADk5OQA5OTkAgoKCAIKCgv85OTn/OTk5/zk5Of85OTn/OTk5/zk5Of+CgoL/goKCADk5OQA5OTkAOTk5ADk5OQCCgoIAgoKCADk5OQCCgoL/OTk5/zk5Of+CgoL/OTk5AIKCggCCgoIAOTk5ADk5OQA5OTkAOTk5AIKCggCCgoIAgoKCAIKCgv85OTn/OTk5/4KCgv+CgoIAgoKCAIKCggA5OTkAOTk5AIKCggCCgoIAgoKCAIKCggCCgoIAgoKC/zk5Of85OTn/goKC/4KCggCCgoIAgoKCAIKCggCCgoIAgoKCAIKCggCCgoIAgoKCAIKCggCCgoIAOTk5ADk5OQCCgoIAgoKCAIKCggCCgoIAgoKCAIKCggCCgoIAgoKCAIKCggCCgoIAgoKCAIKCggA5OTkAOTk5AIKCggCCgoIAgoKCAIKCggCCgoIAgoKCAA==";
        private const int _confirmIconWidth = 14;
        private const int _confirmIconHeight = 14;
        private const TextureFormat _confirmIconFormat = TextureFormat.RGBA32;
        private const string _confirmIconData = "ACgAAAAoAAAAKAAAACgAAAAoAAAAKABCAJgA5wBFAHAARQAAAEUAAABFAAAARQAAAEUAAABFAAAAKAAAACgAAAAoAAAAKAAAACgAQgCtAPQAvwD/AKcA9gAUACIAFAAAABQAAAAUAAAAFAAAABQAAAAoAAAAKAAAACgAAAAoAEIArwDyAMUA/wDCAP8AvgD/AHAAsABwAAAAcAAAAHAAAABwAAAAcAAAACcAAAAnAAAAJwBAALcA9gDLAP8AyAD/AMUA/wDCAP8AtQD/ACcAQgAnAAAAJwAAACcAAAAnAAAAIwAAACMAOgC6APQA0gD/AM8A/wDLAP8AyAD/AMUA/wDCAP8AiQDQAAIAAwACAAAAAgAAAAIAAAAkADsAvADzANkA/wDVAP8A0gD/Ac4A/wbKA/8AyAD/AMUA/wC8AP8APwBoAD8AAAA/AAAAPwAADJUGyQXfAP8A3AD/ANkA/wDUAP8AdwCvDpUHzgHLAP8AyAD/AMUA/wCgAOsACgARAAoAAAAKAAADIwI5HsUS7wbgAf8C2gD/AHgArQADAAUCKQFDD8kH/wDLAP8AyAD/AMMA/wBYAI0AWAAAAFgAAAMjAgACHwE0G7sQ7gR5AK0AAwAEAAMAAAIpAQAMdweqBdAC/wDLAP8AyAD/ALEA+gAWACUAFgAAAyMCAAIfAQAABAAHAAEAAQADAAAAAwAAABMAAAATACASvgn2AM8A/wDLAP8AxwD/AHUAtQB1AAADIwIAAh8BAAAEAAAAAQAAAAMAAAADAAAAEwAAABMAAAdSBH0K0wX/AM8A/wDLAP8AvgD/AC0ASwMjAgACHwEAAAQAAAABAAAAAwAAAAYAAAAGAAAABgAAAAYAChGmCd4B0wD/AM8A/wDLAP8AigDMAyMCAAIfAQAABAAAAAEAAAADAAAABgAAAAYAAAAGAAAABgAAAzcCVhDSCP8B0wD/BcEA+wBSAHwDIwIAAh8BAAAEAAAAAQAAAAMAAAABAAAAAQAAAAEAAAABAAAAAQABDn0JswmHAr0AGgAsAFIAAA==";
        private const int _warningIconWidth = 14;
        private const int _warningIconHeight = 14;
        private const TextureFormat _warningIconFormat = TextureFormat.RGBA32;
        private const string _warningIconData = "EQcDEJhVGJ3EeRrMxXgZ0MZ1F9HFcxTRxXES0cVwEtHEbhLRxGwS0cNrEtDBaRLNm04SohQJBBOobhys/9ki///WHf//0Rj//8sU///HDP//wQb//7wB//+3AP//swD//a4A//2qAP/7pAD/qVwOsOizN+3/4Sj//9wj///WHf//0Rj//8wU/2NOGf9LOhP//78F//+3AP//swD//a8A//2qAP/miQztpXIyqf/mNf//4Sj//9wj///XHf//0Rj/h20f/3NcGv//xAn//70B//+3AP//swD//a8A/6deDq0mEQcn9s9K+f/lLP//4Sj//9wj///WHf++nB7/tZAa///IDf//wgb//70B//+3AP/4owf8LRQILiYRBwCRYCuV/+s8///lLP//4Sj//9wj/zssEf8gEwf//88Z///HDP//wgb//7wB/51ZEKEtFAgAGAsEABgLBBjxyEr1/+kw///lLP//4Sj/NSUP/xoNBP//1R///8wU///HDP/1qQ75IA4GICAOBgAYCwQAGAsEAIdZKIz/7kL//+kw///mL/8uHgz/FQcB///aJv//0Rj//8wU/5NWE5cgDgYAIA4GABQJBAAUCQQAFAkEFO/GTPT/7TX//+s2/yYVCP8RAwD//d0s///XHf/0shz4GwwFGxsMBQAbDAUAFAkEABQJBAAUCQQAe04kfv/wR///7z3/Hw0E/w8BAP/43C///9sj/4ZQFYobDAUAGwwFABsMBQANBgIADQYCAA0GAgANBgIN57xL6/7wQ/8bCgP/DgEA//XdM//ttSfyEggDEhIIAwASCAMAEggDAA0GAgANBgIADQYCAA0GAgBwRB9z/vJP/827Nv/JtTL//uYz/3tKFn8SCAMAEggDABIIAwASCAMACQQCAAkEAgAJBAIACQQCAAkEAgjXp0vc//RG///xPf/eqS7jDgYCDQ4GAgAOBgIADgYCAA4GAgAJBAIACQQCAAkEAgAJBAIACQQCACUQByWzfj64tYA3uyoTCCsOBgIADgYCAA4GAgAOBgIADgYCAA==";
        private const int _errorIconWidth = 14;
        private const int _errorIconHeight = 14;
        private const TextureFormat _errorIconFormat = TextureFormat.RGBA32;
        private const string _errorIconData = "FgUFABYFBR+rHh7dcBYWmQEAAAEBAAAAAQAAAAEAAABXEBAAVxAQAFcQEHmlFxfkHwcHLB8HBwAYBQUhrh8f290hIf/UHx//aBQUjWgUFAABAAAATg8PAE4PDwBODw9sxBQU/skPD/+oFBTlHwcHLKsoKNfjJib/4CMj/94hIf/UHx//aBQUjgEAAAEBAAAATQ8PbMcXF/7NEhL/yxER/8kPD/+nFxflSRQUY9k6OvrjJib/4CMj/94hIf/UHx//aBQUjVEQEG/MGhr/0RYW/88UFP/NEhL/xxYW/14SEoJJFBQAPxERVNg5OfnjJib/4CMj/94hIf/UHx//0R4e/9YaGv/TGBj/0RYW/8oZGf9UEBBzXhISAEkUFAA/EREAPxERVNo6OvvjJib/4CMj/94hIf/bHx//2Rwc/9YaGv/PHBz/VBERc1QQEABeEhIASRQUAD8REQA/EREAQRISV9k2NvvjJSX/4CMj/94hIf/bHx//0x8f/1oTE3pUEREAVBAQAF4SEgBQEREAUBERAFAREQBQERFr3Ckp/eUnJ//jJSX/4CMj/94hIf/UHx//ZxQUjQEAAAEBAAAAAQAAAFESEgBREhIAURISa+EsLP7qLCz/6Coq/+UnJ//jJSX/4CMj/94hIf/UHx//aBQUjmgUFAABAAAAUhMTAFITE23kMDD+7zAw/+0uLv/qLCz/3y0t/ts3N/zjJib/4CMj/94hIf/UHx//ZxQUjQEAAAFeFhZ76TMz/vM0NP/xMjL/7zAw/+UvL/9YFBR0QBERVto6OvvjJib/4CMj/94hIf/UHx//dBcXnaowMM33PT3/9TU1//M0NP/oMzP+WRQUdVgUFABAEREAPxERVdg5OfrjJib/4CMj/94iIv+xICDnEgQEGbE4ONL3PT3/7DY2/1gUFHNZFBQAWBQUAEAREQA/EREAPhERU9o6OvvjJyf/siIi3RgGBiISBAQAEgQEGbM3N9pqGhqJWBQUAFkUFABYFBQAQBERAD8REQA+EREASRQUYqotLdQaBgYlGAYGAA==";
        private const int _verticalDividerWidth = 2;
        private const int _verticalDividerHeight = 6;
        private const TextureFormat _verticalDividerFormat = TextureFormat.RGBA32;
        private const string _verticalDividerData = "urq6Orq6ujq6urqBurq6gbq6uv+6urr/urq6/7q6uv+6urqBurq6gbq6ujq6uro6";

        private static Dictionary<StringID, string> _stringResources;
        private static Texture2D _titleIcon;
        private static Texture2D _confirmIcon;
        private static Texture2D _warningIcon;
        private static Texture2D _errorIcon;
        private static Texture2D _verticalDivider;
        private static string[] _fntFileFilters;
        private static Vector2? _selectButtonSize;

        public static Texture2D TitleIcon
        {
            get
            {
                if (_titleIcon == null)
                {
                    _titleIcon = CreateTexture(_titleIconWidth, _titleIconHeight, _titleIconFormat, _titleIconData);
                }

                return _titleIcon;
            }
        }

        public static Texture2D ConfirmIcon
        {
            get
            {
                if (_confirmIcon == null)
                {
                    _confirmIcon = CreateTexture(
                        _confirmIconWidth,
                        _confirmIconHeight,
                        _confirmIconFormat,
                        _confirmIconData);
                }

                return _confirmIcon;
            }
        }

        public static Texture2D WarningIcon
        {
            get
            {
                if (_warningIcon == null)
                {
                    _warningIcon = CreateTexture(
                        _warningIconWidth,
                        _warningIconHeight,
                        _warningIconFormat,
                        _warningIconData);
                }

                return _warningIcon;
            }
        }

        public static Texture2D ErrorIcon
        {
            get
            {
                if (_errorIcon == null)
                {
                    _errorIcon = CreateTexture(
                        _errorIconWidth,
                        _errorIconHeight,
                        _errorIconFormat,
                        _errorIconData);
                }

                return _errorIcon;
            }
        }

        public static Texture2D VerticalDivider
        {
            get
            {
                if (_verticalDivider == null)
                {
                    _verticalDivider = CreateTexture(
                        _verticalDividerWidth,
                        _verticalDividerHeight,
                        _verticalDividerFormat,
                        _verticalDividerData);
                }

                return _verticalDivider;
            }
        }

        public static string[] FntFileFilters
        {
            get
            {
                if (_fntFileFilters == null)
                {
                    _fntFileFilters = new[]
                    {
                        "Fnt", "fnt,txt,xml",
                        GetString(StringID.AllFileTypes), "*"
                    };
                }

                return _fntFileFilters;
            }
        }

        public static Vector2 SelectButtonSize
        {
            get
            {
                if (_selectButtonSize == null)
                {
                    var style = new GUIStyle("button");
                    var content = new GUIContent(GetString(StringID.SelectWithFilePanel));
                    _selectButtonSize = style.CalcSize(content);
                }

                return _selectButtonSize.Value;
            }
        }

        public static string GetString(StringID id)
        {
            if (_stringResources == null)
            {
                _stringResources = new Dictionary<StringID, string>(StringIDComparer.Instance)
                {
                    { StringID.AllFileTypes, "모든 형식" },
                    { StringID.CanImportFontUsedSingleTextureOnly, "단일 텍스처를 사용하는 폰트만 사용할 수 있습니다." },
                    { StringID.ConvertedCharactersHasDifferentCount, "지정된 문자 집합에 대해 문자를 올바르게 변환할 수 없습니다." },
                    { StringID.ExceptionOccurredOnReadingFile, "파일을 읽는 중에 오류가 발생했습니다." },
                    { StringID.FailedToConvertCharacters, "문자 목록을 문자열로 변환할 수 없습니다." },
                    { StringID.FailedToLoadTexture, "텍스처 파일을 찾을 수 없습니다.\n경로: " },
                    { StringID.FileEncoding, "파일 인코딩" },
                    { StringID.FntFile, "Fnt 파일" },
                    { StringID.FontFileExtensionMustBeFontsettings, "폰트 파일의 확장자는 fontsettings여야 합니다." },
                    { StringID.FontFilePathMustBeIncludedAssetsDirectory, "생성될 폰트는 프로젝트의 Assets 폴더에 포함되어야 합니다." },
                    { StringID.ForceMonospace, "고정폭으로 변환" },
                    { StringID.HasNoCharTags, "문자 정보가 없습니다." },
                    { StringID.HasNoPageTags, "페이지 정보가 없습니다." },
                    { StringID.Import, "임포트" },
                    { StringID.ImportSuccess, "임포트 성공."},
                    { StringID.InvalidCharacterID, "문자 집합에 속하지 않는 문자 값을 가지고 있습니다." },
                    { StringID.InvalidFntFile, "유효한 fnt 파일이 아닙니다." },
                    { StringID.InvalidFontFileExtension, "유효하지 않은 파일 확장자" },
                    { StringID.InvalidFontFilePath, "유효하지 않은 파일 경로" },
                    { StringID.NotSupportedCharset, "지원하지 않는 charset입니다." },
                    { StringID.OK, "확인" },
                    { StringID.PackedFontShaderNotFound, "Packed Bitmap Font 쉐이더를 찾을 수 없습니다." },
                    { StringID.SaveFont, "폰트 저장" },
                    { StringID.SelectFile, "파일 선택" },
                    { StringID.SelectWithFilePanel, "선택…" },
                    { StringID.Settings, "설정"},
                    { StringID.TextureFileNameIsEmpty, "텍스처 파일명이 없습니다." },
                };
            }

            string result;
            return _stringResources.TryGetValue(id, out result) ? result : id.ToString();
        }

        private static Texture2D CreateTexture(int width, int height, TextureFormat format, string data)
        {
            var tex = new Texture2D(width, height, format, false, true)
            {
                hideFlags = HideFlags.DontSave,
                filterMode = FilterMode.Point
            };
            tex.LoadRawTextureData(Convert.FromBase64String(data));
            tex.Apply();

            return tex;
        }
    }
}
