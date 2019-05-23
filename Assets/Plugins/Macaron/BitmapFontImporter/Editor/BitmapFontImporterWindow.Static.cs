using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;

namespace Macaron.BitmapFontImporter.Editor
{
    partial class BitmapFontImporterWindow
    {
        private static class EditorPrefsKey
        {
            private const string Prefix = "Macaron.BitmapFontImporter.Editor.BitmapFontImporterWindow.";
            public const string FoldSettings = Prefix + "FoldSettings";
            public const string LastFntDirectory = Prefix + "LastFntDirectory";
            public const string LastImportDirectory = Prefix + "LastImportDirectory";
            public const string ForceMonospace = Prefix + "ForceMonospace";
            public const string FileEncodingCodePage = Prefix + "FileEncodningCodePage";
        }

        private static readonly Dictionary<int, string> _encodingDisplayNames;

        static BitmapFontImporterWindow()
        {
            _encodingDisplayNames = new Dictionary<int, string>
            {
                { 37, "IBM EBCDIC (US-Canada)" },
                { 437, "OEM United States" },
                { 500, "IBM EBCDIC (International)" },
                { 708, "Arabic (ASMO 708)" },
                { 720, "Arabic (DOS)" },
                { 737, "Greek (DOS)" },
                { 775, "Baltic (DOS)" },
                { 850, "Western European (DOS)" },
                { 852, "Central European (DOS)" },
                { 855, "OEM Cyrillic" },
                { 857, "Turkish (DOS)" },
                { 858, "OEM Multilingual Latin I" },
                { 860, "Portuguese (DOS)" },
                { 861, "Icelandic (DOS)" },
                { 862, "Hebrew (DOS)" },
                { 863, "French Canadian (DOS)" },
                { 864, "Arabic (864)" },
                { 865, "Nordic (DOS)" },
                { 866, "Cyrillic (DOS)" },
                { 869, "Greek, Modern (DOS)" },
                { 870, "IBM EBCDIC (Multilingual Latin-2)" },
                { 874, "Thai (Windows)" },
                { 875, "IBM EBCDIC (Greek Modern)" },
                { 932, "Japanese (Shift-JIS)" },
                { 936, "Chinese Simplified (GB2312)" },
                { 949, "Korean" },
                { 950, "Chinese Traditional (Big5)" },
                { 1026, "IBM EBCDIC (Turkish Latin-5)" },
                { 1047, "IBM Latin-1" },
                { 1140, "IBM EBCDIC (US-Canada-Euro)" },
                { 1141, "IBM EBCDIC (Germany-Euro)" },
                { 1142, "IBM EBCDIC (Denmark-Norway-Euro)" },
                { 1143, "IBM EBCDIC (Finland-Sweden-Euro)" },
                { 1144, "IBM EBCDIC (Italy-Euro)" },
                { 1145, "IBM EBCDIC (Spain-Euro)" },
                { 1146, "IBM EBCDIC (UK-Euro)" },
                { 1147, "IBM EBCDIC (France-Euro)" },
                { 1148, "IBM EBCDIC (International-Euro)" },
                { 1149, "IBM EBCDIC (Icelandic-Euro)" },
                { 1200, "Unicode" },
                { 1201, "Unicode (Big-Endian)" },
                { 1250, "Central European (Windows)" },
                { 1251, "Cyrillic (Windows)" },
                { 1252, "Western European (Windows)" },
                { 1253, "Greek (Windows)" },
                { 1254, "Turkish (Windows)" },
                { 1255, "Hebrew (Windows)" },
                { 1256, "Arabic (Windows)" },
                { 1257, "Baltic (Windows)" },
                { 1258, "Vietnamese (Windows)" },
                { 1361, "Korean (Johab)" },
                { 10000, "Western European (Mac)" },
                { 10001, "Japanese (Mac)" },
                { 10002, "Chinese Traditional (Mac)" },
                { 10003, "Korean (Mac)" },
                { 10004, "Arabic (Mac)" },
                { 10005, "Hebrew (Mac)" },
                { 10006, "Greek (Mac)" },
                { 10007, "Cyrillic (Mac)" },
                { 10008, "Chinese Simplified (Mac)" },
                { 10010, "Romanian (Mac)" },
                { 10017, "Ukrainian (Mac)" },
                { 10021, "Thai (Mac)" },
                { 10029, "Central European (Mac)" },
                { 10079, "Icelandic (Mac)" },
                { 10081, "Turkish (Mac)" },
                { 10082, "Croatian (Mac)" },
                { 12000, "Unicode (UTF-32)" },
                { 12001, "Unicode (UTF-32 Big-Endian)" },
                { 20000, "Chinese Traditional (CNS)" },
                { 20001, "TCA Taiwan" },
                { 20002, "Chinese Traditional (Eten)" },
                { 20003, "IBM5550 Taiwan" },
                { 20004, "TeleText Taiwan" },
                { 20005, "Wang Taiwan" },
                { 20105, "Western European (IA5)" },
                { 20106, "German (IA5)" },
                { 20107, "Swedish (IA5)" },
                { 20108, "Norwegian (IA5)" },
                { 20127, "US-ASCII" },
                { 20261, "T.61" },
                { 20269, "ISO-6937" },
                { 20273, "IBM EBCDIC (Germany)" },
                { 20277, "IBM EBCDIC (Denmark-Norway)" },
                { 20278, "IBM EBCDIC (Finland-Sweden)" },
                { 20280, "IBM EBCDIC (Italy)" },
                { 20284, "IBM EBCDIC (Spain)" },
                { 20285, "IBM EBCDIC (UK)" },
                { 20290, "IBM EBCDIC (Japanese katakana)" },
                { 20297, "IBM EBCDIC (France)" },
                { 20420, "IBM EBCDIC (Arabic)" },
                { 20423, "IBM EBCDIC (Greek)" },
                { 20424, "IBM EBCDIC (Hebrew)" },
                { 20833, "IBM EBCDIC (Korean Extended)" },
                { 20838, "IBM EBCDIC (Thai)" },
                { 20866, "Cyrillic (KOI8-R)" },
                { 20871, "IBM EBCDIC (Icelandic)" },
                { 20880, "IBM EBCDIC (Cyrillic Russian)" },
                { 20905, "IBM EBCDIC (Turkish)" },
                { 20924, "IBM Latin-1" },
                { 20932, "Japanese (JIS 0208-1990 and 0212-1990)" },
                { 20936, "Chinese Simplified (GB2312-80)" },
                { 20949, "Korean Wansung" },
                { 21025, "IBM EBCDIC (Cyrillic Serbian-Bulgarian)" },
                { 21866, "Cyrillic (KOI8-U)" },
                { 28591, "Western European (ISO)" },
                { 28592, "Central European (ISO)" },
                { 28593, "Latin 3 (ISO)" },
                { 28594, "Baltic (ISO)" },
                { 28595, "Cyrillic (ISO)" },
                { 28596, "Arabic (ISO)" },
                { 28597, "Greek (ISO)" },
                { 28598, "Hebrew (ISO-Visual)" },
                { 28599, "Turkish (ISO)" },
                { 28603, "Estonian (ISO)" },
                { 28605, "Latin 9 (ISO)" },
                { 29001, "Europa" },
                { 38598, "Hebrew (ISO-Logical)" },
                { 50220, "Japanese (JIS)" },
                { 50221, "Japanese (JIS-Allow 1 byte Kana)" },
                { 50222, "Japanese (JIS-Allow 1 byte Kana - SO\u2215SI)" },
                { 50225, "Korean (ISO)" },
                { 50227, "Chinese Simplified (ISO-2022)" },
                { 51932, "Japanese (EUC)" },
                { 51936, "Chinese Simplified (EUC)" },
                { 51949, "Korean (EUC)" },
                { 52936, "Chinese Simplified (HZ)" },
                { 54936, "Chinese Simplified (GB18030)" },
                { 57002, "ISCII Devanagari" },
                { 57003, "ISCII Bengali" },
                { 57004, "ISCII Tamil" },
                { 57005, "ISCII Telugu" },
                { 57006, "ISCII Assamese" },
                { 57007, "ISCII Oriya" },
                { 57008, "ISCII Kannada" },
                { 57009, "ISCII Malayalam" },
                { 57010, "ISCII Gujarati" },
                { 57011, "ISCII Punjabi" },
                { 65000, "Unicode (UTF-7)" },
                { 65001, "Unicode (UTF-8)" }
            };
        }

        [MenuItem("Window/Bitmap Font Importer", false, 2050)]
        public static void ShowWindow()
        {
            var window = EditorWindow.GetWindow(typeof(BitmapFontImporterWindow));
            window.minSize = new Vector2(200.0f, 200.0f);
        }

        #region Methods
        private static void DrawTexture(Rect position, Texture2D tex)
        {
            if (tex == null)
            {
                return;
            }

            GUI.DrawTexture(position, tex);
        }

        private static void DrawMessageBox(MessageType type, string msg)
        {
            Texture2D iconTex = null;

            switch (type)
            {
                case MessageType.Info: iconTex = EditorResources.ConfirmIcon; break;
                case MessageType.Warning: iconTex = EditorResources.WarningIcon; break;
                case MessageType.Error: iconTex = EditorResources.ErrorIcon; break;
            }

            Texture2D dividerTex = EditorResources.VerticalDivider;

            EditorGUILayout.BeginHorizontal(EditorStyles.helpBox);

            Rect iconRect = EditorGUILayout.GetControlRect(GUILayout.Width(14), GUILayout.Height(14));
            Rect dividerRect = EditorGUILayout.GetControlRect(GUILayout.Width(1), GUILayout.Height(14));

            var labelStyle = new GUIStyle("label")
            {
                wordWrap = true
            };
            Rect labelRect = GUILayoutUtility.GetRect(new GUIContent(msg), labelStyle);
            EditorGUI.SelectableLabel(labelRect, msg, labelStyle);

            float iconOffset = Mathf.Max(0.0f, Mathf.Ceil((labelRect.height - 14.0f) * 0.5f));
            iconRect.yMin += iconOffset;
            iconRect.yMax += iconOffset;

            DrawTexture(iconRect, iconTex);

            if (dividerRect.height < labelRect.height)
            {
                dividerRect.yMax += labelRect.height - dividerRect.height;
            }

            GUI.Box(
                dividerRect,
                string.Empty,
                new GUIStyle()
                {
                    border = new RectOffset(1, 1, 3, 3),
                    normal = new GUIStyleState { background = dividerTex }
                });

            EditorGUILayout.EndHorizontal();
        }

        private static CharacterInfo GetCharacterInfo(CommonTag commonTag, CharTag charTag)
        {
            float textureWidth = (float)commonTag.ScaleW;
            float textureHeight = (float)commonTag.ScaleH;

            var uv = new Rect(
                charTag.X / textureWidth,
                charTag.Y / textureHeight,
                charTag.Width / textureWidth,
                charTag.Height / textureHeight);
            uv.y = 1.0f - (uv.y + uv.height);

            // 0은 부호를 판정할 수 없기 때문에 hlsl의 sign 함수가 1을 반환하는 가장 작은 값으로 치환한다.
            const float epsilon = 1.1754944E-38f;

            if (uv.xMin == 0.0f)
            {
                uv.xMin = epsilon;
            }

            if (uv.yMin == 0.0f)
            {
                uv.yMin = epsilon;
            }

            if (uv.xMax == 0.0f)
            {
                uv.xMax = epsilon;
            }

            if (uv.yMax == 0.0f)
            {
                uv.yMax = epsilon;
            }

            var uvBottomLeft = new Vector2(uv.xMin, uv.yMin);
            var uvBottomRight = new Vector2(uv.xMax, uv.yMin);
            var uvTopLeft = new Vector2(uv.xMin, uv.yMax);
            var uvTopRight = new Vector2(uv.xMax, uv.yMax);

            if (commonTag.Packed)
            {
                Vector2 sign;

                switch (charTag.Channel)
                {
                    case 1: sign = new Vector2(-1.0f, 1.0f); break; // blue
                    case 2: sign = new Vector2(1.0f, -1.0f); break; // green
                    case 4: sign = new Vector2(-1.0f, -1.0f); break; // red
                    case 8: sign = new Vector2(1.0f, 1.0f); break; // alpha

                    default:
                        sign = Vector2.zero;
                        break;
                }

                uvBottomLeft.Scale(sign);
                uvBottomRight.Scale(sign);
                uvTopLeft.Scale(sign);
                uvTopRight.Scale(sign);
            }

            return new CharacterInfo
            {
                index = charTag.ID,
                advance = charTag.XAdvance,
                uvBottomLeft = uvBottomLeft,
                uvBottomRight = uvBottomRight,
                uvTopLeft = uvTopLeft,
                uvTopRight = uvTopRight,
                minX = charTag.XOffset,
                maxX = charTag.XOffset + charTag.Width,
                minY = commonTag.BaseLine - (charTag.YOffset + charTag.Height),
                maxY = commonTag.BaseLine - charTag.YOffset
            };
        }

        private static string GetString(StringID id)
        {
            return EditorResources.GetString(id);
        }

        private static string GetUniqueFileName(string path)
        {
            var fileName = Path.Combine(
                Path.GetDirectoryName(path),
                Path.GetFileNameWithoutExtension(path)).Replace('\\', '/') + ' ';
            var extension = Path.GetExtension(path);

            var index = 0;
            var uniquePath = path;

            while (File.Exists(uniquePath))
            {
                index += 1;
                uniquePath = fileName + index + extension;
            }

            return uniquePath;
        }
        #endregion
    }
}
