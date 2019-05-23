using System;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEditor;

namespace Macaron.BitmapFontImporter.Editor
{
    public partial class BitmapFontImporterWindow : EditorWindow
    {
        #region Fields
        private int[] _encodingCodePages;
        private string[] _encodingNames;

        private Vector2 _scrollPosition;
        private bool _foldSettings;
        private string _lastFntDirectory;
        private string _lastImportDirectory;
        private Message[] _messages;

        [SerializeField] private string _fntPath;
        [SerializeField] private bool _forceMonospace;
        [SerializeField] private int _fileEncodingIndex;

        private SerializedObject _serializedObject;
        private SerializedProperty _fntPathProperty;
        private SerializedProperty _forceMonoSpaceProperty;
        private SerializedProperty _fileEncodingIndexProperty;
        #endregion

        #region EditorWindow Messages
        private void Awake()
        {
            // 인코딩 관련 정보 생성.
            var encodingInfos = Encoding.GetEncodings();
            var encodingCodePages = new int[encodingInfos.Length];
            var encodingNames = new string[encodingInfos.Length];

            for (int i = 0; i < encodingInfos.Length; ++i)
            {
                var encodingInfo = encodingInfos[i];

                string encodingName;
                encodingNames[i] = _encodingDisplayNames.TryGetValue(encodingInfo.CodePage, out encodingName) ?
                    encodingName :
                    encodingInfo.DisplayName;
                encodingCodePages[i] = encodingInfo.CodePage;
            }

            _encodingCodePages = encodingCodePages;
            _encodingNames = encodingNames;

            _messages = new Message[0];

            // 에디터 시작 시 창이 열려있는 경우 이전 값이 복구되기 때문에 여기서 초기화한다.
            _fntPath = string.Empty;
        }

        private void OnEnable()
        {
            titleContent = new GUIContent("Fnt Importer", EditorResources.TitleIcon);

            Undo.undoRedoPerformed += Repaint;

            _serializedObject = new SerializedObject(this);
            _fntPathProperty = _serializedObject.FindProperty("_fntPath");
            _forceMonoSpaceProperty = _serializedObject.FindProperty("_forceMonospace");
            _fileEncodingIndexProperty = _serializedObject.FindProperty("_fileEncodingIndex");

            // 저장된 값 읽기.
            _foldSettings = EditorPrefs.GetBool(EditorPrefsKey.FoldSettings);
            _lastFntDirectory = EditorPrefs.GetString(EditorPrefsKey.LastFntDirectory);
            _lastImportDirectory = EditorPrefs.GetString(EditorPrefsKey.LastImportDirectory);
            _forceMonospace = EditorPrefs.GetBool(EditorPrefsKey.ForceMonospace);

            var fileEncodingCodePage = EditorPrefs.GetInt(EditorPrefsKey.FileEncodingCodePage, -1);
            var fileEncodingIndex = Array.IndexOf(_encodingCodePages, fileEncodingCodePage);

            _fileEncodingIndex = fileEncodingIndex != -1 ?
                fileEncodingIndex :
                Array.FindIndex(_encodingCodePages, cp => cp == 65001);
        }

        private void OnGUI()
        {
            EditorGUIUtility.hierarchyMode = true;

            using (var scrollView = new EditorGUILayout.ScrollViewScope(_scrollPosition))
            {
                _scrollPosition = scrollView.scrollPosition;

                using (new EditorGUILayout.VerticalScope(new GUIStyle { padding = new RectOffset(14, 0, 6, 6) }))
                using (new ModifySerializedObject(_serializedObject))
                {
                    // Fnt 경로.
                    var fntPath = DrawFntFileField(_fntPathProperty.stringValue);

                    if (fntPath != _fntPathProperty.stringValue)
                    {
                        _fntPathProperty.stringValue = fntPath;
                        ClearMessages();
                    }

                    // 설정.
                    _foldSettings = EditorGUILayout.Foldout(_foldSettings, GetString(StringID.Settings));
                    if (_foldSettings)
                    {
                        using (new IndentLevel(1))
                        {
                            _forceMonoSpaceProperty.boolValue = EditorGUILayout.Toggle(
                                GetString(StringID.ForceMonospace),
                                _forceMonoSpaceProperty.boolValue);
                            _fileEncodingIndexProperty.intValue = EditorGUILayout.Popup(
                                GetString(StringID.FileEncoding),
                                _fileEncodingIndexProperty.intValue,
                                _encodingNames);
                        }
                    }
                }

                using (new DisabledGroup(string.IsNullOrEmpty(_fntPath)))
                {
                    if (GUILayout.Button(GetString(StringID.Import)))
                    {
                        GUI.FocusControl(string.Empty);

                        try
                        {
                            Import();
                        }
                        catch (ImportException e)
                        {
                            if (e.InnerException != null)
                            {
                                Debug.LogException(e.InnerException);
                            }

                            AddMessage(MessageType.Error, e.Message);
                        }
                    }
                }

                for (int i = 0; i < _messages.Length; ++i)
                {
                    var message = _messages[i];
                    DrawMessageBox(message.Type, message.Value);
                }
            }

            // 여백 클릭 시 포커스 해제.
            if (GUI.Button(new Rect(0.0f, 0.0f, position.width, position.height), string.Empty, GUIStyle.none))
            {
                GUI.FocusControl(string.Empty);
            }
        }

        private void OnDisable()
        {
            Undo.undoRedoPerformed -= Repaint;

            EditorPrefs.SetBool(EditorPrefsKey.FoldSettings, _foldSettings);
            EditorPrefs.SetString(EditorPrefsKey.LastFntDirectory, _lastFntDirectory);
            EditorPrefs.SetString(EditorPrefsKey.LastImportDirectory, _lastImportDirectory);
            EditorPrefs.SetBool(EditorPrefsKey.ForceMonospace, _forceMonospace);
            EditorPrefs.SetInt( EditorPrefsKey.FileEncodingCodePage, _encodingCodePages[_fileEncodingIndex]);

            _serializedObject.Dispose();
        }
        #endregion

        #region Methods
        private void AddMessage(MessageType type, string value)
        {
            Array.Resize(ref _messages, _messages.Length + 1);
            _messages[_messages.Length - 1] = new Message { Type = type, Value = value };
        }

        private void ClearMessages()
        {
            Array.Resize(ref _messages, 0);
        }

        private CharTag[] ConvertCodePage(string charset, CharTag[] charTags)
        {
            var encoding = default(Encoding);

            switch (charset.ToUpperInvariant())
            {
                case "ANSI": encoding = Encoding.GetEncoding(1252); break;
                case "DEFAULT": break;
                case "SYMBOL": break;
                case "MAC": encoding = Encoding.GetEncoding(10000); break;
                case "SHIFTJIS": encoding = Encoding.GetEncoding(932); break;
                case "HANGUL": encoding = Encoding.GetEncoding(949); break;
                case "JOHAB": encoding = Encoding.GetEncoding(1361); break;
                case "GB2312": encoding = Encoding.GetEncoding(936); break;
                case "CHINESEBIG5": encoding = Encoding.GetEncoding(950); break;
                case "GREEK": encoding = Encoding.GetEncoding(1253); break;
                case "TURKISH": encoding = Encoding.GetEncoding(1254); break;
                case "VIETNAMESE": encoding = Encoding.GetEncoding(1258); break;
                case "HEBREW": encoding = Encoding.GetEncoding(1255); break;
                case "ARABIC": encoding = Encoding.GetEncoding(1256); break;
                case "BALTIC": encoding = Encoding.GetEncoding(1257); break;
                case "RUSSIAN": encoding = Encoding.GetEncoding(1251); break;
                case "THAI": encoding = Encoding.GetEncoding(874); break;
                case "EASTEUROPE": encoding = Encoding.GetEncoding(1250); break;
                case "OEM": break;

                default:
                    break;
            }

            if (encoding != null)
            {
                byte[] bytes;

                using (var ms = new MemoryStream())
                using (var writer = new BinaryWriter(ms))
                {
                    foreach (var id in charTags.Select(tag => (uint)tag.ID))
                    {
                        if (id <= byte.MaxValue)
                        {
                            writer.Write((byte)id);
                        }
                        else if (id <= ushort.MaxValue)
                        {
                            writer.Write((ushort)id);
                        }
                        else
                        {
                            // multibyte character set은 최대 2바이트를 사용한다.
                            throw new ImportException(GetString(StringID.InvalidCharacterID));
                        }
                    }

                    writer.Flush();
                    bytes = ms.ToArray();
                }

                string chars;

                try
                {
                    chars = encoding.GetString(bytes);
                }
                catch (Exception)
                {
                    throw new ImportException(GetString(StringID.FailedToConvertCharacters));
                }

                if (chars.Length != charTags.Length)
                {
                    throw new ImportException(GetString(StringID.ConvertedCharactersHasDifferentCount));
                }

                var convertedCharTags = new CharTag[charTags.Length];

                for (int i = 0; i < charTags.Length; ++i)
                {
                    var ch = charTags[i];
                    convertedCharTags[i] = new CharTag(
                        chars[i],
                        ch.X,
                        ch.Y,
                        ch.Width,
                        ch.Height,
                        ch.XOffset,
                        ch.YOffset,
                        ch.XAdvance,
                        ch.Page,
                        ch.Channel);
                }

                return convertedCharTags;
            }
            else
            {
                throw new ImportException(GetString(StringID.NotSupportedCharset));
            }
        }

        private string DrawFntFileField(string fntPath)
        {
            var result = fntPath;

            // 라벨.
            EditorGUILayout.LabelField(GetString(StringID.FntFile));

            // 경로 표시.
            var selectButtonSize = EditorResources.SelectButtonSize;
            var pathRect = EditorGUILayout.GetControlRect();
            pathRect.xMax -= selectButtonSize.x + 4.0f;

            EditorGUI.SelectableLabel(pathRect, fntPath, EditorStyles.textField);

            // 선택 버튼.
            var selectButtonRect = new Rect(pathRect.xMax + 4.0f, pathRect.y, selectButtonSize.x, pathRect.height);

            if (GUI.Button(selectButtonRect, GetString(StringID.SelectWithFilePanel)))
            {
                GUI.FocusControl(string.Empty);

                var dataPath = Application.dataPath;
                var directory = Directory.Exists(_lastFntDirectory) ? _lastFntDirectory : dataPath;
                var path = EditorUtility.OpenFilePanelWithFilters(
                    GetString(StringID.SelectFile),
                    directory,
                    EditorResources.FntFileFilters);

                if (!string.IsNullOrEmpty(path))
                {
                    _lastFntDirectory = Path.GetDirectoryName(path);

                    // 프로젝트 내부의 파일일 경우 Assets로 시작하도록 수정.
                    // path는 정규화되어서 반환되기 때문에 대소문자 구분을 고려하지 않는다.
                    result = path.StartsWith(dataPath) ? path.Substring(dataPath.Length - 6) : path;
                }

                return result;
            }

            // 파일 경로 필드에 대한 파일 드래그 앤 드롭 처리.
            Event evt = Event.current;
            EventType evtType = evt.type;

            if (evtType != EventType.DragUpdated && evtType != EventType.DragPerform)
            {
                return result;
            }

            if (pathRect.Contains(evt.mousePosition))
            {
                var objs = DragAndDrop.objectReferences;
                var paths = DragAndDrop.paths;

                var path = string.Empty;
                var useEvt = false;

                if (objs.Length == 1)
                {
                    var obj = objs[0];
                    if (EditorUtility.IsPersistent(obj) && obj is TextAsset)
                    {
                        if (evtType == EventType.DragPerform)
                        {
                            path = AssetDatabase.GetAssetPath(obj);
                        }

                        useEvt = true;
                    }
                }
                else if (paths.Length == 1)
                {
                    if (evtType == EventType.DragPerform)
                    {
                        path = paths[0];
                    }

                    useEvt = true;
                }

                if (path.Length > 0)
                {
                    _lastFntDirectory = Path.GetDirectoryName(path);
                    result = path;

                    DragAndDrop.AcceptDrag();
                    GUI.FocusControl(string.Empty);
                }

                if (useEvt)
                {
                    DragAndDrop.visualMode = DragAndDropVisualMode.Generic;
                    evt.Use();
                }

            }

            return result;
        }

        private void Import()
        {
            var fontPath = string.Empty;

            if (!TryGetImportPath(out fontPath))
            {
                return;
            }

            ClearMessages();

            // fnt 파일 파싱.
            var dataPath = Application.dataPath;
            var fntPath = Path.GetFullPath(_fntPath).Replace('\\', '/');

            // 프로젝트의 에셋인 경우.
            if (fntPath.StartsWith(dataPath, StringComparison.OrdinalIgnoreCase))
            {
                fntPath = fntPath.Substring(dataPath.Length - 6);
            }

            // Fnt 파일 파싱.
            var fontInfo = ParseFntFile(fntPath);

            // 기존 폰트가 있다면 기존 폰트의 정보를 갱신하고 없다면 새로 생성한다.
            var font = AssetDatabase.LoadAssetAtPath<Font>(fontPath);
            var material = font != null ? font.material : null;
            var needCreateFont = font == null;
            var needCreateMaterial = font == null || font.material == null;

            // 텍스처 파일은 항상 fnt 파일과 같은 디렉토리에 있고 파일명은 경로를 포함하지 않는다고 가정한다.
            var textureFileName = fontInfo.Pages[0].File;

            if (needCreateMaterial && string.IsNullOrEmpty(textureFileName))
            {
                throw new ImportException(GetString(StringID.TextureFileNameIsEmpty));
            }

            // 재질 생성.
            if (needCreateMaterial)
            {
                var shader = Shader.Find(fontInfo.Common.Packed ? "Macaron/UI/Packed Bitmap Font" : "UI/Default Font");

                if (fontInfo.Common.Packed && shader == null)
                {
                    AddMessage(MessageType.Warning, GetString(StringID.PackedFontShaderNotFound));
                    shader = Shader.Find("UI/Default Font");
                }

                material = new Material(shader);
            }

            // 텍스처 읽기.
            Func<string> importTexture = () =>
            {
                return ImportTexture(
                    Path.GetDirectoryName(fntPath),
                    Path.GetDirectoryName(fontPath),
                    textureFileName,
                    false);
            };

            var texturePath = string.Empty;
            var needCreateTexture = material.mainTexture == null;
            var isAsset = fntPath.StartsWith("Assets", StringComparison.OrdinalIgnoreCase);

            if (needCreateTexture)
            {
                texturePath = isAsset ?
                    Path.Combine(Path.GetDirectoryName(fntPath), textureFileName).Replace('\\', '/') :
                    importTexture();
            }
            else if (!isAsset)
            {
                // 재질에 할당된 텍스처가 에셋으로 존재하지 않는 경우 없는 것으로 취급한다.
                var textureExists = false;
                var existingTexturePath = AssetDatabase.GetAssetPath(material.mainTexture);

                if (!string.IsNullOrEmpty(existingTexturePath))
                {
                    var existingTextureFileName = Path.GetFileName(existingTexturePath);
                    textureExists = existingTextureFileName.Equals(textureFileName, StringComparison.OrdinalIgnoreCase);
                }

                // 텍스처가 존재하고 이름이 같다면 덮어쓰고 임포트 설정은 유지한다.
                if (textureExists)
                {
                    ImportTexture(
                        Path.GetDirectoryName(fntPath),
                        Path.GetDirectoryName(existingTexturePath),
                        textureFileName,
                        true);
                }
                else
                {
                    texturePath = importTexture();
                }
            }

            // texturePath 값이 존재하면 새로 텍스처를 설정하는 경우.
            if (!string.IsNullOrEmpty(texturePath))
            {
                var importer = (TextureImporter)AssetImporter.GetAtPath(texturePath);

                importer.mipmapEnabled = false;
#if UNITY_5_5_OR_NEWER
                importer.textureCompression = TextureImporterCompression.Uncompressed;
                importer.alphaSource = TextureImporterAlphaSource.FromInput;
#else
                importer.textureFormat = TextureImporterFormat.AutomaticTruecolor;
                importer.grayscaleToAlpha = false;
#endif

                if (material.shader.name == "Macaron/UI/Packed Bitmap Font")
                {
                    importer.alphaIsTransparency = false;
#if UNITY_5_5_OR_NEWER
                    importer.sRGBTexture = false;
#else
                    importer.linearTexture = true;
#endif
                }

                importer.SaveAndReimport();

                var texture = AssetDatabase.LoadAssetAtPath<Texture2D>(texturePath);

                if (texture == null)
                {
                    AddMessage(MessageType.Error, GetString(StringID.FailedToLoadTexture) + texturePath);
                    return;
                }

                material.mainTexture = texture;
            }

            if (needCreateFont)
            {
                font = new Font(Path.GetFileNameWithoutExtension(_fntPath)) { material = material };
            }

            // 폰트 정보 변경.
            SetFont(font, fontInfo);

            // 에셋 생성.
            if (needCreateMaterial)
            {
                var materialPath = GetUniqueFileName(Path.ChangeExtension(fontPath, ".mat"));
                AssetDatabase.CreateAsset(material, materialPath);

                font.material = material;
            }
            else
            {
                EditorUtility.SetDirty(material);
            }

            if (needCreateFont)
            {
                AssetDatabase.CreateAsset(font, fontPath);
            }
            else
            {
                EditorUtility.SetDirty(font);
            }

            AssetDatabase.SaveAssets();

            // 유니티 5.x는 생성, 변경된 폰트가 에디터 재시작 전에 적용되지 않는 문제가 있다.
#if !UNITY_2017_1_OR_NEWER
            var tempPath = Application.temporaryCachePath + "/" + Path.GetRandomFileName();
            AssetDatabase.ExportPackage(fontPath, tempPath);
            AssetDatabase.DeleteAsset(fontPath);
            AssetDatabase.ImportPackage(tempPath, false);
            File.Delete(tempPath);
#endif

            AddMessage(MessageType.Info, GetString(StringID.ImportSuccess));
        }

        private string ImportTexture(string srcDir, string destDir, string textureFileName, bool overwrite)
        {
            var srcPath = Path.Combine(srcDir, textureFileName).Replace('\\', '/');

            if (!File.Exists(srcPath))
            {
                throw new ImportException("텍스처 파일을 찾을 수 없습니다.\n경로: " + srcPath);
            }

            var destPath = Path.Combine(destDir, textureFileName).Replace('\\', '/');

            if (!overwrite)
            {
                destPath = GetUniqueFileName(destPath);
            }

            try
            {
                File.Copy(srcPath, destPath, overwrite);
            }
            catch (Exception e)
            {
                throw new ImportException("텍스처 파일을 복사할 수 없습니다.", e);
            }

            AssetDatabase.ImportAsset(destPath);

            return destPath;
        }

        private CharacterInfo[] GetCharacterInfos(FontInfo fontInfo)
        {
            var commonTag = fontInfo.Common;
            var charTags = fontInfo.Chars;
            var characterInfos = new CharacterInfo[charTags.Length];
            var maxAdvance = 0;

            for (int i = 0; i < characterInfos.Length; ++i)
            {
                var charTag = charTags[i];

                if (charTag.XAdvance > maxAdvance)
                {
                    maxAdvance = charTag.XAdvance;
                }

                characterInfos[i] = GetCharacterInfo(commonTag, charTag);
            }

            if (_forceMonospace)
            {
                for (int i = 0; i < characterInfos.Length; ++i)
                {
                    var characterInfo = characterInfos[i];
                    int advance = characterInfo.advance;
                    int offset = (maxAdvance - advance) / 2;

                    characterInfo.minX += offset;
                    characterInfo.maxX += offset;
                    characterInfo.advance = maxAdvance;

                    characterInfos[i] = characterInfo;
                }
            }

            return characterInfos;
        }

        private FontInfo ParseFntFile(string path)
        {
            var stream = default(Stream);

            // 프로젝트의 에셋인 경우.
            if (path.StartsWith("Assets", StringComparison.OrdinalIgnoreCase))
            {
                var fntAsset = AssetDatabase.LoadAssetAtPath<TextAsset>(path);
                if (fntAsset == null)
                {
                    throw new ImportException("에셋이 없거나 TextAsset 형식이 아닙니다.\n경로: " + path);
                }

                stream = new MemoryStream(fntAsset.bytes);
            }
            else
            {
                try
                {
                    stream = File.Open(path, FileMode.Open, FileAccess.Read);
                }
                catch (Exception e)
                {
                    throw new ImportException("파일을 열 수 없습니다.\n경로: " + path, e);
                }
            }

            var codePage = _encodingCodePages[_fileEncodingIndex];
            var encoding = Encoding.GetEncoding(codePage);

            FontInfo fontInfo;

            try
            {
                fontInfo = FntParser.Parse(stream, encoding);
            }
            catch (FntParserException)
            {
                throw new ImportException(GetString(StringID.InvalidFntFile));
            }
            catch (Exception e)
            {
                throw new ImportException(GetString(StringID.ExceptionOccurredOnReadingFile), e);
            }

            // 값 검증.
            if (fontInfo.Pages == null)
            {
                throw new ImportException(GetString(StringID.HasNoPageTags));
            }

            if (fontInfo.Pages.Length != 1)
            {
                throw new ImportException(GetString(StringID.CanImportFontUsedSingleTextureOnly));
            }

            if (fontInfo.Chars == null)
            {
                throw new ImportException(GetString(StringID.HasNoCharTags));
            }

            // charset 변환.
            if (!fontInfo.Info.Unicode)
            {
                var convertedCharTags = ConvertCodePage(fontInfo.Info.Charset, fontInfo.Chars);

                if (convertedCharTags == null)
                {
                    return null;
                }

                var infoTag = fontInfo.Info;
                infoTag = new InfoTag(
                    infoTag.Face,
                    infoTag.Size,
                    infoTag.Bold,
                    infoTag.Italic,
                    string.Empty,
                    true,
                    infoTag.StretchH,
                    infoTag.Smooth,
                    infoTag.SupersamplingLevel,
                    infoTag.PaddingUp,
                    infoTag.PaddingRight,
                    infoTag.PaddingDown,
                    infoTag.PaddingLeft,
                    infoTag.SpacingHoriz,
                    infoTag.SpacingVert,
                    infoTag.Outline);

                fontInfo = new FontInfo(infoTag, fontInfo.Common, fontInfo.Pages, convertedCharTags, fontInfo.Kernings);
            }

            return fontInfo;
        }

        private void SetFont(Font font, FontInfo fontInfo)
        {
            font.characterInfo = GetCharacterInfos(fontInfo);

            var serializedFont = new SerializedObject(font);

            serializedFont.FindProperty("m_FontSize").floatValue = Math.Abs(fontInfo.Info.Size);
            serializedFont.FindProperty("m_LineSpacing").floatValue = fontInfo.Common.LineHeight;
            serializedFont.FindProperty("m_Ascent").floatValue = fontInfo.Common.BaseLine;
            serializedFont.FindProperty("m_Descent").floatValue = fontInfo.Common.BaseLine - fontInfo.Common.LineHeight;

            var kerningTags = fontInfo.Kernings;
            var kerningValues = serializedFont.FindProperty("m_KerningValues");

            if (_forceMonospace || kerningTags == null || kerningTags.Length == 0)
            {
                kerningValues.ClearArray();
            }
            else
            {
                for (int i = kerningValues.arraySize - 1; i >= kerningTags.Length; --i)
                {
                    kerningValues.DeleteArrayElementAtIndex(i);
                }

                for (int i = kerningValues.arraySize; i < kerningTags.Length; ++i)
                {
                    kerningValues.InsertArrayElementAtIndex(i);
                }

                for (int i = 0; i < kerningTags.Length; ++i)
                {
                    var kerning = kerningValues.GetArrayElementAtIndex(i);
                    kerning.FindPropertyRelative("first.first").intValue = kerningTags[i].First;
                    kerning.FindPropertyRelative("first.second").intValue = kerningTags[i].Second;
                    kerning.FindPropertyRelative("second").floatValue = kerningTags[i].Amount;
                }
            }

            serializedFont.ApplyModifiedPropertiesWithoutUndo();
            serializedFont.Dispose();
        }

        private bool TryGetImportPath(out string path)
        {
            var directoryName = Directory.Exists(_lastImportDirectory) ? _lastImportDirectory : Application.dataPath;
            var fileName = Path.GetFileNameWithoutExtension(_fntPath);

            // 생성할 폰트 경로 입력받기.
            for (; ; )
            {
                path = EditorUtility.SaveFilePanel(
                    EditorResources.GetString(StringID.SaveFont),
                    directoryName,
                    fileName,
                    "fontsettings");

                if (string.IsNullOrEmpty(path))
                {
                    // 취소.
                    return false;
                }
                else if (!path.StartsWith(Application.dataPath))
                {
                    EditorUtility.DisplayDialog(
                        GetString(StringID.InvalidFontFilePath),
                        GetString(StringID.FontFilePathMustBeIncludedAssetsDirectory),
                        GetString(StringID.OK));

                    fileName = Path.GetFileName(path);
                    continue;
                }
                else if (Path.GetExtension(path) != ".fontsettings")
                {
                    EditorUtility.DisplayDialog(
                        GetString(StringID.InvalidFontFileExtension),
                        GetString(StringID.FontFileExtensionMustBeFontsettings),
                        GetString(StringID.OK));

                    directoryName = Path.GetDirectoryName(path);
                    fileName = Path.GetFileName(path);
                    continue;
                }
                else
                {
                    _lastImportDirectory = Path.GetDirectoryName(path);
                    path = path.Substring(Application.dataPath.Length - 6);

                    return true;
                }
            }
        }
        #endregion
    }
}
