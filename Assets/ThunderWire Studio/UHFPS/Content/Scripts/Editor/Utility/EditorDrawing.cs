using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using Object = UnityEngine.Object;

namespace ThunderWire.Editors
{
    public class MultiToolbarItem
    {
        public GUIContent content;
        public SerializedProperty property;

        public MultiToolbarItem(GUIContent content, SerializedProperty toggleProperty)
        {
            this.content = content;
            property = toggleProperty;
        }
    }

    public static class EditorDrawing
    {
        public static class Styles
        {
            public static GUIStyle borderBoxHeaderStyle
            {
                get
                {
                    GUIStyle style = new GUIStyle();
                    style.margin = new RectOffset(3, 3, 2, 2);
                    style.padding = new RectOffset(5, 5, 2, 5);
                    return style;
                }
            }

            public static GUIStyle borderBoxStyle
            {
                get
                {
                    GUIStyle style = new GUIStyle();
                    style.margin = new RectOffset(3, 3, 2, 2);
                    style.padding = new RectOffset(5, 5, 5, 5);
                    return style;
                }
            }

            public static GUIStyle miniBoldLabelCenter
            {
                get
                {
                    GUIStyle style = new GUIStyle(EditorStyles.miniBoldLabel);
                    style.alignment = TextAnchor.MiddleLeft;
                    return style;
                }
            }

            public static GUIStyle RichLabel
            {
                get => new GUIStyle(EditorStyles.label)
                {
                    richText = true
                };
            }

            public static Texture2D TransparentCheckerTexture
            {
                get
                {
                    if (EditorGUIUtility.isProSkin)
                    {
                        return EditorGUIUtility.LoadRequired("Previews/Textures/textureCheckerDark.png") as Texture2D;
                    }

                    return EditorGUIUtility.LoadRequired("Previews/Textures/textureChecker.png") as Texture2D;
                }
            }
        }

        public static GUIStyle CenterStyle(GUIStyle style)
        {
            return new GUIStyle(style)
            {
                alignment = TextAnchor.MiddleCenter
            };
        }

        public static GUIStyle CenterStyleLeft(GUIStyle style)
        {
            return new GUIStyle(style)
            {
                alignment = TextAnchor.MiddleLeft
            };
        }

        public class BorderBoxScope : GUI.Scope
        {
            public BorderBoxScope(bool roundedBox = true)
            {
                BeginBorderLayout(roundedBox);
            }

            public BorderBoxScope(GUIContent title, float headerHeight = 18f, bool roundedBox = true)
            {
                BeginHeaderBorderLayout(title, headerHeight, roundedBox);
            }

            protected override void CloseScope()
            {
                EndBorderHeaderLayout();
            }
        }

        public class ToggleBorderBoxScope : GUI.Scope
        {
            public ToggleBorderBoxScope(GUIContent title, SerializedProperty toggle, float headerHeight = 18f, bool roundedBox = true)
            {
                toggle.boolValue = BeginToggleBorderLayout(title, toggle.boolValue, headerHeight, roundedBox);
            }

            protected override void CloseScope()
            {
                EndBorderHeaderLayout();
            }
        }

        public class IconSizeScope : GUI.Scope
        {
            private Vector2 prevIconSize;

            public IconSizeScope(Vector2 iconSize)
            {
                prevIconSize = EditorGUIUtility.GetIconSize();
                EditorGUIUtility.SetIconSize(iconSize);
            }

            public IconSizeScope(float iconSize)
            {
                prevIconSize = EditorGUIUtility.GetIconSize();
                EditorGUIUtility.SetIconSize(new Vector2(iconSize, iconSize));
            }

            public IconSizeScope(float x, float y)
            {
                prevIconSize = EditorGUIUtility.GetIconSize();
                EditorGUIUtility.SetIconSize(new Vector2(x, y));
            }

            protected override void CloseScope()
            {
                EditorGUIUtility.SetIconSize(prevIconSize);
            }
        }

        /// <summary>
        /// Draw custom border box.
        /// </summary>
        public static void DrawBorderBox(Rect rect, RectOffset border, Color color)
        {
            if (Event.current.type != EventType.Repaint)
                return;

            Color orgColor = GUI.color;

            GUI.color *= color;
            GUI.DrawTexture(new Rect(rect.x, rect.y, rect.width, border.top), EditorGUIUtility.whiteTexture); //top
            GUI.DrawTexture(new Rect(rect.x, rect.yMax - border.bottom, rect.width, border.bottom), EditorGUIUtility.whiteTexture); //bottom
            GUI.DrawTexture(new Rect(rect.x, rect.y + border.left, border.left, rect.height - 2 * border.left), EditorGUIUtility.whiteTexture); //left
            GUI.DrawTexture(new Rect(rect.xMax - border.right, rect.y + border.right, border.right, rect.height - 2 * border.right), EditorGUIUtility.whiteTexture); //right

            GUI.color = orgColor;
        }

        /// <summary>
        /// Draw the heading at the top of the inspector.
        /// </summary>
        public static void DrawInspectorHeader(GUIContent title, Object script = null)
        {
            GUIStyle headerStyle = new GUIStyle(EditorStyles.boldLabel)
            {
                fontSize = 15,
                alignment = TextAnchor.MiddleCenter
            };

            headerStyle.normal.textColor = Color.white;
            title.text = title.text.ToUpper();

            using (new IconSizeScope(18))
            {
                Rect rect = GUILayoutUtility.GetRect(1, 30);
                ColorUtility.TryParseHtmlString("#151515", out Color color);
                EditorGUI.DrawRect(rect, color);

                if (script != null)
                {
                    MonoScript monoScript = MonoScript.FromMonoBehaviour((MonoBehaviour)script);
                    Event e = Event.current;

                    if (rect.Contains(e.mousePosition))
                    {
                        if (e.type == EventType.MouseDown && e.button == 0)
                        {
                            EditorGUIUtility.PingObject(monoScript);
                        }
                    }

                    using (new IconSizeScope(13))
                    {
                        // draw saveable icon
                        bool flag1 = typeof(UHFPS.Runtime.ISaveable).IsAssignableFrom(script.GetType());
                        bool flag2 = typeof(UHFPS.Runtime.IRuntimeSaveable).IsAssignableFrom(script.GetType());
                        bool flag3 = typeof(UHFPS.Runtime.ISaveableCustom).IsAssignableFrom(script.GetType());

                        if (flag1 || flag2 || flag3)
                        {
                            Rect iconRect = rect;
                            iconRect.xMin = iconRect.xMax - EditorGUIUtility.singleLineHeight - 3f;

                            GUIContent icon = EditorGUIUtility.TrIconContent("CacheServerConnected", "Script is Saveable");
                            EditorGUI.LabelField(iconRect, icon);
                        }
                    }
                }

                EditorGUI.LabelField(rect, title, headerStyle);
            }
        }

        /// <summary>
        /// Draw classic prefix label.
        /// </summary>
        public static void DrawPrefixLabel(string title, string text, GUIStyle style)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel(title);
            EditorGUILayout.LabelField(text, style);
            EditorGUILayout.EndHorizontal();
        }

        /// <summary>
        /// Draw a property field with a label after the field.
        /// </summary>
        public static void DrawPropertyAndLabel(SerializedProperty property, GUIContent content)
        {
            DrawPropertyAndLabel(property, content, EditorStyles.boldLabel);
        }

        /// <summary>
        /// Draw a property field with a label after the field.
        /// </summary>
        public static void DrawPropertyAndLabel(SerializedProperty property, GUIContent content, GUIStyle labelStyle)
        {
            Rect rect = EditorGUILayout.GetControlRect();

            float labelWidth = labelStyle.CalcSize(content).x;
            rect.xMax -= labelWidth + 2f;

            EditorGUI.PropertyField(rect, property);

            rect.xMin = rect.xMax + 2f;
            rect.xMax += labelWidth + 2f;
            EditorGUI.LabelField(rect, content, labelStyle);
        }

        /// <summary>
        /// Draw horizontal separator.
        /// </summary>
        public static void Separator(int height = 1)
        {
            Rect rect = EditorGUILayout.GetControlRect(false, height);
            rect.height = height;
            EditorGUI.DrawRect(rect, new Color(0.5f, 0.5f, 0.5f, 1));
        }

        /// <summary>
        /// Draw a large sprite selection similar to the EditorGUI.ObjectField sprite selection.
        /// <br>But with the correct, not stretched image.</br>
        /// </summary>
        public static Object DrawLargeSpriteSelector(Rect rect, Object obj)
        {
            Rect borderRect = rect;
            borderRect.height -= 1;
            borderRect.width -= 1;
            GUI.Box(borderRect, GUIContent.none, new GUIStyle("HelpBox"));

            Rect iconRect = rect;
            iconRect.height -= 10;
            iconRect.width -= 10;
            iconRect.x += 5f;
            iconRect.y += 5f;

            DrawTransparentTextureWithChecker(iconRect, obj);

            Rect mageSelectRect = rect;
            mageSelectRect.yMax -= 15f;

            Event e = Event.current;
            if (mageSelectRect.Contains(e.mousePosition))
            {
                if (e.type == EventType.MouseDown && e.button == 0 && obj != null)
                {
                    EditorGUIUtility.PingObject(obj);
                    e.Use();
                }
            }

            Rect buttonRect = rect;
            buttonRect.height = 15f;
            buttonRect.xMin = rect.width - rect.width * 0.7f;
            buttonRect.y = rect.height - 15f;
            buttonRect.x -= 1f;

            if (GUI.Button(buttonRect, "Select", EditorStyles.objectFieldThumb))
            {
                EditorGUIUtility.ShowObjectPicker<Sprite>(obj, false, "", GUIUtility.GetControlID(FocusType.Passive));
            }
            else if (Event.current.commandName == "ObjectSelectorUpdated")
            {
                return EditorGUIUtility.GetObjectPickerObject();
            }

            return obj;
        }

        /// <summary>
        /// Draw classic object field with picker.
        /// </summary>
        public static bool ObjectField(Rect rect, GUIContent text, GUIContent tooltip = null)
        {
            using (new IconSizeScope(12f))
            {
                GUI.Box(rect, text, EditorStyles.objectField);

                GUIStyle buttonStyle = new GUIStyle("ObjectFieldButton") { richText = true };
                Rect buttonRect = buttonStyle.margin.Remove(new Rect(rect.xMax - 19, rect.y, 19, rect.height));

                return GUI.Button(buttonRect, tooltip ?? new GUIContent(), buttonStyle);
            }
        }

        /// <summary>
        /// Draw a multi selection toolbar.
        /// </summary>
        public static void DrawMultiToolbar(Rect rect, MultiToolbarItem[] contents, float buttonWidth = 100) 
        {
            GUIStyle style = new GUIStyle(GUI.skin.button);
            string name = style.name;

            GUIStyle midStyle = GUI.skin.FindStyle(name + "mid") ?? style;
            GUIStyle firstStyle = GUI.skin.FindStyle(name + "left") ?? midStyle;
            GUIStyle lastStyle = GUI.skin.FindStyle(name + "right") ?? midStyle;

            Rect toolbarRect = rect;
            toolbarRect.width = buttonWidth;

            for (int i = 0; i < contents.Length; i++)
            {
                if(i == 0)
                {
                    // first
                    contents[i].property.boolValue = GUI.Toggle(toolbarRect, contents[i].property.boolValue, contents[i].content, firstStyle);
                    toolbarRect.x += buttonWidth;
                }
                else if(i == contents.Length - 1)
                {
                    // last
                    contents[i].property.boolValue = GUI.Toggle(toolbarRect, contents[i].property.boolValue, contents[i].content, lastStyle);
                    toolbarRect.x += buttonWidth;
                }
                else
                {
                    // mid
                    contents[i].property.boolValue = GUI.Toggle(toolbarRect, contents[i].property.boolValue, contents[i].content, midStyle);
                    toolbarRect.x += buttonWidth;
                }
            }
        }

        /// <summary>
        /// Draw multi selection popup menu.
        /// </summary>
        public static void DrawMultiSelectPopup(Rect rect, string[] content, string[] selected, Action<string[]> onItemSelect, int maxDisplay = 3)
        {
            GenericMenu menu = new GenericMenu();
            List<string> selectedArr = new List<string>(selected);

            for (int i = 0; i < content.Length; i++)
            {
                string name = content[i];
                bool on = selected.Any(x => x.Equals(name));

                menu.AddItem(new GUIContent(name), on, data =>
                {
                    if (selectedArr.Contains(name))
                        selectedArr.Remove(name);
                    else selectedArr.Add(name);
                    onItemSelect?.Invoke(selectedArr.ToArray());
                },
                name);
            }

            string popupTitle = "Nothing";
            if (selected.Length > 0)
            {
                popupTitle = string.Join(", ", selected.Take(maxDisplay));

                if (selected.Length > maxDisplay)
                    popupTitle += " ...";
            }

            if (GUI.Button(rect, popupTitle, EditorStyles.popup))
            {
                menu.DropDown(rect);
            }
        }

        /// <summary>
        /// Draw multi selection popup menu.
        /// </summary>
        public static void DrawMultiSelectPopup(Rect rect, GUIContent title, string[] content, string[] selected, Action<string[]> onItemSelect, int maxDisplay = 3)
        {
            GenericMenu menu = new GenericMenu();
            GUIContent guiTitle = new GUIContent(title);
            List<string> selectedArr = new List<string>(selected);

            for (int i = 0; i < content.Length; i++)
            {
                string name = content[i];
                bool on = selected.Any(x => x.Equals(name));

                menu.AddItem(new GUIContent(name), on, data =>
                {
                    if (selectedArr.Contains(name))
                        selectedArr.Remove(name);
                    else selectedArr.Add(name);
                    onItemSelect?.Invoke(selectedArr.ToArray());
                },
                name);
            }

            if (selected.Length > 0)
            {
                guiTitle.text = string.Join(", ", selected.Take(maxDisplay));

                if (selected.Length > maxDisplay)
                    guiTitle.text += " ...";
            }

            if (GUI.Button(rect, guiTitle, EditorStyles.popup))
            {
                menu.DropDown(rect);
            }
        }

        /// <summary>
        /// Draw string selection popup menu.
        /// </summary>
        public static void DrawStringSelectPopup(Rect rect, string[] content, string selected, Action<string> onItemSelect)
        {
            GenericMenu menu = new GenericMenu();

            for (int i = 0; i < content.Length; i++)
            {
                string name = content[i];
                bool on = name.Equals(selected);

                menu.AddItem(new GUIContent(name), on, data =>
                {
                    string selection = (string)data;
                    string result = selected;

                    if (selected != selection)
                        result = selection;

                    onItemSelect?.Invoke(result);
                },
                name);
            }

            string popupTitle = string.IsNullOrEmpty(selected) ? "Nothing" : selected;
            if (GUI.Button(rect, popupTitle, EditorStyles.popup))
            {
                menu.DropDown(rect);
            }
        }

        /// <summary>
        /// Draw string selection popup menu.
        /// </summary>
        public static void DrawStringSelectPopup(Rect rect, GUIContent title, string[] content, string selected, Action<string> onItemSelect)
        {
            GenericMenu menu = new GenericMenu();
            GUIContent guiTitle = new GUIContent(title);

            for (int i = 0; i < content.Length; i++)
            {
                string name = content[i];
                bool on = name.Equals(selected);

                menu.AddItem(new GUIContent(name), on, data =>
                {
                    string selection = (string)data;
                    string result = selected;

                    if (selected != selection)
                        result = selection;

                    onItemSelect?.Invoke(result);
                },
                name);
            }

            if (!string.IsNullOrEmpty(selected))
                guiTitle.text = selected;

            if (GUI.Button(rect, guiTitle, EditorStyles.popup))
            {
                menu.DropDown(rect);
            }
        }

        /// <summary>
        /// Draw custom class property list.
        /// </summary>
        public static void DrawList(SerializedProperty listProperty, GUIContent title, float headerHeight = 18f, bool roundedBox = true)
        {
            if(BeginFoldoutBorderLayout(listProperty, title, headerHeight, roundedBox))
            {
                for (int i = 0; i < listProperty.arraySize; i++)
                {
                    SerializedProperty listItem = listProperty.GetArrayElementAtIndex(i);
                    var properties = GetAllProperties(listItem);
                    string elementName = "Element " + i;

                    var firstProperty = properties.First();
                    if(firstProperty.Value.propertyType == SerializedPropertyType.String)
                    {
                        string stringValue = firstProperty.Value.stringValue;
                        if (!string.IsNullOrEmpty(stringValue)) elementName = stringValue;
                    }

                    if (BeginFoldoutBorderLayout(listItem, new GUIContent(elementName), out Rect itemHeaderRect))
                    {
                        properties.DrawAll(true);
                        EndBorderHeaderLayout();
                    }

                    GUIContent minus = EditorGUIUtility.TrIconContent("Toolbar Minus");
                    Rect minusRect = itemHeaderRect;
                    minusRect.xMin = minusRect.xMax - EditorGUIUtility.singleLineHeight;
                    minusRect.x -= 3f;
                    minusRect.y += 3f;

                    if (GUI.Button(minusRect, minus, EditorStyles.iconButton))
                    {
                        listProperty.DeleteArrayElementAtIndex(i);
                    }
                }

                if (listProperty.arraySize > 0)
                    EditorGUILayout.Space(2f);

                if (GUILayout.Button("Add"))
                {
                    listProperty.arraySize++;
                }

                EndBorderHeaderLayout();
            }
        }

        /// <summary>
        /// Draw header by specified rect.
        /// </summary>
        public static void DrawHeader(Rect headerRect, GUIContent title, GUIStyle labelStyle = null)
        {
            // header color
            EditorGUI.DrawRect(headerRect, new Color(0.1f, 0.1f, 0.1f, 0.4f));

            // title
            Rect labelRect = headerRect;
            labelRect.xMin += 4f;
            labelRect.y -= 1f;
            EditorGUI.LabelField(labelRect, title, labelStyle == null ? Styles.miniBoldLabelCenter : labelStyle);
        }

        /// <summary>
        /// Draw toggle header by specified rect.
        /// </summary>
        public static bool DrawToggleHeader(Rect headerRect, GUIContent title, bool toggle)
        {
            // header color
            EditorGUI.DrawRect(headerRect, new Color(0.1f, 0.1f, 0.1f, 0.4f));

            // toggle
            Rect toggleRect = headerRect;
            toggleRect.width = EditorGUIUtility.singleLineHeight;
            toggleRect.x += 4f;
            toggle = GUI.Toggle(toggleRect, toggle, new GUIContent("", "Enabled"), EditorStyles.toggle);

            // title
            Rect labelRect = headerRect;
            labelRect.xMin += toggleRect.x;
            labelRect.y -= 1f;
            EditorGUI.LabelField(labelRect, title, Styles.miniBoldLabelCenter);

            return toggle;
        }

        /// <summary>
        /// Draw foldout header by specified rect.
        /// </summary>
        public static bool DrawFoldoutHeader(Rect headerRect, GUIContent title, bool expanded)
        {
            // header color
            EditorGUI.DrawRect(headerRect, new Color(0.1f, 0.1f, 0.1f, 0.4f));

            // foldout
            Rect foldoutRect = headerRect;
            foldoutRect.width = EditorGUIUtility.singleLineHeight;
            foldoutRect.x += 4f;
            GUI.Toggle(foldoutRect, expanded, GUIContent.none, EditorStyles.foldout);

            // title
            Rect labelRect = headerRect;
            labelRect.xMin = foldoutRect.xMax - 4f;
            labelRect.y -= 1f;
            EditorGUI.LabelField(labelRect, title, Styles.miniBoldLabelCenter);

            // events
            headerRect.xMax -= EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            Event e = Event.current;
            if (headerRect.Contains(e.mousePosition))
            {
                if (e.type == EventType.MouseDown && e.button == 0)
                {
                    expanded = !expanded;
                    e.Use();
                }
            }

            return expanded;
        }

        /// <summary>
        /// Draw foldout header by specified rect.
        /// </summary>
        public static bool DrawFoldoutHeader(Rect headerRect, GUIContent title, float minusWidth, bool expanded)
        {
            // header color
            EditorGUI.DrawRect(headerRect, new Color(0.1f, 0.1f, 0.1f, 0.4f));

            // foldout
            Rect foldoutRect = headerRect;
            foldoutRect.width = EditorGUIUtility.singleLineHeight;
            foldoutRect.x += 4f;
            GUI.Toggle(foldoutRect, expanded, GUIContent.none, EditorStyles.foldout);

            // title
            Rect labelRect = headerRect;
            labelRect.xMin = foldoutRect.xMax - 4f;
            labelRect.y -= 1f;
            EditorGUI.LabelField(labelRect, title, Styles.miniBoldLabelCenter);

            // events
            headerRect.xMax -= EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing + minusWidth;
            Event e = Event.current;
            if (headerRect.Contains(e.mousePosition))
            {
                if (e.type == EventType.MouseDown && e.button == 0)
                {
                    expanded = !expanded;
                    e.Use();
                }
            }

            return expanded;
        }

        /// <summary>
        /// Draw foldout toggle header by specified rect.
        /// </summary>
        public static void DrawFoldoutToggleHeader(Rect headerRect, GUIContent title, ref bool expanded, ref bool toggle, bool canExpand = true)
        {
            // header color
            EditorGUI.DrawRect(headerRect, new Color(0.1f, 0.1f, 0.1f, 0.4f));

            // foldout
            Rect foldoutRect = headerRect;
            if (canExpand)
            {
                foldoutRect.width = EditorGUIUtility.singleLineHeight;
                foldoutRect.x += 4f;
                GUI.Toggle(foldoutRect, expanded, GUIContent.none, EditorStyles.foldout);
            }

            // toggle
            Rect toggleRect = headerRect;
            toggleRect.width = EditorGUIUtility.singleLineHeight;
            toggleRect.x += canExpand ? foldoutRect.x : 4f;
            toggle = GUI.Toggle(toggleRect, toggle, new GUIContent("", "Enabled"), EditorStyles.toggle);

            // title
            Rect labelRect = headerRect;
            labelRect.xMin += toggleRect.x;
            labelRect.y -= 1f;
            EditorGUI.LabelField(labelRect, title, Styles.miniBoldLabelCenter);

            // events
            headerRect.xMax += toggleRect.x;
            Event e = Event.current;
            if (headerRect.Contains(e.mousePosition) && canExpand)
            {
                if (e.type == EventType.MouseDown && e.button == 0)
                {
                    expanded = !expanded;
                    e.Use();
                }
            }
        }

        /// <summary>
        /// Draw header with border.
        /// </summary>
        public static Rect DrawHeaderWithBorder(ref Rect rect, GUIContent title, float headerHeight = 18f, bool roundedBox = true)
        {
            GUI.Box(rect, GUIContent.none, new GUIStyle(roundedBox ? "HelpBox" : "Tooltip"));
            rect.x += 1;
            rect.y += 1;
            rect.height -= 1;
            rect.width -= 2;

            Rect headerRect = rect;
            headerRect.height = headerHeight + EditorGUIUtility.standardVerticalSpacing;

            rect.y += headerRect.height;
            rect.height -= headerRect.height;

            Rect titleRect = headerRect;
            titleRect.x += 2f;

            using (new IconSizeScope(14))
            {
                EditorGUI.DrawRect(headerRect, new Color(0.1f, 0.1f, 0.1f, 0.4f));
                EditorGUI.LabelField(titleRect, title, EditorStyles.miniBoldLabel);
            }

            return headerRect;
        }

        /// <summary>
        /// Begin a bordered vertical group;
        /// </summary>
        public static void BeginBorderLayout(bool roundedBox = true)
        {
            Rect drawingRect = EditorGUILayout.BeginVertical(Styles.borderBoxStyle);
            GUI.Box(drawingRect, GUIContent.none, new GUIStyle(roundedBox ? "HelpBox" : "Tooltip"));
        }

        /// <summary>
        /// Begin a bordered vertical header group;
        /// </summary>
        public static Rect BeginHeaderBorderLayout(GUIContent title, float headerHeight = 18f, bool roundedBox = true)
        {
            Rect headerRect = EditorGUILayout.GetControlRect(false, headerHeight + 4f);
            Rect drawingRect = EditorGUILayout.BeginVertical(Styles.borderBoxHeaderStyle);
            drawingRect.yMin -= headerHeight + 6f;

            GUI.Box(drawingRect, GUIContent.none, new GUIStyle(roundedBox ? "HelpBox" : "Tooltip"));
            DrawHeader(headerRect, title);
            return headerRect;
        }

        /// <summary>
        /// Begin a bordered vertical foldout group.
        /// </summary>
        public static bool BeginFoldoutBorderLayout(GUIContent title, ref bool expanded, float headerHeight = 18f, bool roundedBox = true)
        {
            Rect headerRect = EditorGUILayout.GetControlRect(false, headerHeight + 4f);
            Rect boxRect = headerRect;
            bool foldoutResult = expanded;

            if (expanded)
            {
                Rect drawingRect = EditorGUILayout.BeginVertical(Styles.borderBoxHeaderStyle);
                boxRect.yMax = drawingRect.yMax;
            }

            GUI.Box(boxRect, GUIContent.none, new GUIStyle(roundedBox ? "HelpBox" : "Tooltip"));
            expanded = DrawFoldoutHeader(headerRect, title, expanded);
            return foldoutResult;
        }

        /// <summary>
        /// Begin a bordered vertical foldout group.
        /// </summary>
        public static bool BeginFoldoutBorderLayout(GUIContent title, bool expanded, float headerHeight = 18f, bool roundedBox = true)
        {
            Rect headerRect = EditorGUILayout.GetControlRect(false, headerHeight + 4f);
            Rect boxRect = headerRect;

            if (expanded)
            {
                Rect drawingRect = EditorGUILayout.BeginVertical(Styles.borderBoxHeaderStyle);
                boxRect.yMax = drawingRect.yMax;
            }

            GUI.Box(boxRect, GUIContent.none, new GUIStyle(roundedBox ? "HelpBox" : "Tooltip"));
            return DrawFoldoutHeader(headerRect, title, expanded);
        }

        /// <summary>
        /// Begin a bordered vertical foldout group.
        /// </summary>
        public static bool BeginFoldoutBorderLayout(GUIContent title, ref bool expanded, out Rect headerRect, float headerHeight = 18f, bool roundedBox = true)
        {
            headerRect = EditorGUILayout.GetControlRect(false, headerHeight + 4f);
            Rect boxRect = headerRect;
            bool foldoutResult = expanded;

            if (expanded)
            {
                Rect drawingRect = EditorGUILayout.BeginVertical(Styles.borderBoxHeaderStyle);
                boxRect.yMax = drawingRect.yMax;
            }

            GUI.Box(boxRect, GUIContent.none, new GUIStyle(roundedBox ? "HelpBox" : "Tooltip"));
            expanded = DrawFoldoutHeader(headerRect, title, expanded);
            return foldoutResult;
        }

        /// <summary>
        /// Begin a bordered vertical foldout group.
        /// </summary>
        public static bool BeginFoldoutBorderLayout(SerializedProperty foldoutProperty, GUIContent title, float headerHeight = 18f, bool roundedBox = true)
        {
            Rect headerRect = EditorGUILayout.GetControlRect(false, headerHeight + 4f);
            Rect boxRect = headerRect;
            bool foldoutResult = foldoutProperty.isExpanded;

            if (foldoutResult)
            {
                Rect drawingRect = EditorGUILayout.BeginVertical(Styles.borderBoxHeaderStyle);
                boxRect.yMax = drawingRect.yMax;
            }

            GUI.Box(boxRect, GUIContent.none, new GUIStyle(roundedBox ? "HelpBox" : "Tooltip"));
            foldoutProperty.isExpanded = DrawFoldoutHeader(headerRect, title, foldoutProperty.isExpanded);
            return foldoutResult;
        }

        /// <summary>
        /// Begin a bordered vertical foldout group.
        /// </summary>
        public static bool BeginFoldoutBorderLayout(SerializedProperty foldoutProperty, GUIContent title, out Rect headerRect, float headerHeight = 18f, bool roundedBox = true)
        {
            headerRect = EditorGUILayout.GetControlRect(false, headerHeight + 4f);
            Rect boxRect = headerRect;
            bool foldoutResult = foldoutProperty.isExpanded;

            if (foldoutResult)
            {
                Rect drawingRect = EditorGUILayout.BeginVertical(Styles.borderBoxHeaderStyle);
                boxRect.yMax = drawingRect.yMax;
            }

            GUI.Box(boxRect, GUIContent.none, new GUIStyle(roundedBox ? "HelpBox" : "Tooltip"));
            foldoutProperty.isExpanded = DrawFoldoutHeader(headerRect, title, foldoutProperty.isExpanded);
            return foldoutResult;
        }

        /// <summary>
        /// Begin a bordered vertical foldout group.
        /// </summary>
        public static bool BeginFoldoutBorderLayout(SerializedProperty foldoutProperty, GUIContent title, out Rect headerRect, float minusWidth, float headerHeight = 18f, bool roundedBox = true)
        {
            headerRect = EditorGUILayout.GetControlRect(false, headerHeight + 4f);
            Rect boxRect = headerRect;
            bool foldoutResult = foldoutProperty.isExpanded;

            if (foldoutResult)
            {
                Rect drawingRect = EditorGUILayout.BeginVertical(Styles.borderBoxHeaderStyle);
                boxRect.yMax = drawingRect.yMax;
            }

            GUI.Box(boxRect, GUIContent.none, new GUIStyle(roundedBox ? "HelpBox" : "Tooltip"));
            foldoutProperty.isExpanded = DrawFoldoutHeader(headerRect, title, minusWidth, foldoutProperty.isExpanded);
            return foldoutResult;
        }

        /// <summary>
        /// Begin a bordered vertical toggle group.
        /// </summary>
        public static bool BeginToggleBorderLayout(GUIContent title, bool toggle, float headerHeight = 18f, bool roundedBox = true)
        {
            Rect headerRect = EditorGUILayout.GetControlRect(false, headerHeight + 4f);
            Rect drawingRect = EditorGUILayout.BeginVertical(Styles.borderBoxHeaderStyle);
            drawingRect.yMin -= headerHeight + 6f;

            GUI.Box(drawingRect, GUIContent.none, new GUIStyle(roundedBox ? "HelpBox" : "Tooltip"));
            return DrawToggleHeader(headerRect, title, toggle);
        }

        /// <summary>
        /// Begin a bordered vertical toggle group.
        /// </summary>
        public static bool BeginToggleBorderLayout(GUIContent title, bool toggle, out Rect headerRect, float headerHeight = 18f, bool roundedBox = true)
        {
            headerRect = EditorGUILayout.GetControlRect(false, headerHeight + 4f);
            Rect drawingRect = EditorGUILayout.BeginVertical(Styles.borderBoxHeaderStyle);
            drawingRect.yMin -= headerHeight + 6f;

            GUI.Box(drawingRect, GUIContent.none, new GUIStyle(roundedBox ? "HelpBox" : "Tooltip"));
            return DrawToggleHeader(headerRect, title, toggle);
        }

        /// <summary>
        /// Begin a bordered vertical foldout toggle group.
        /// </summary>
        public static bool BeginFoldoutToggleBorderLayout(GUIContent title, ref bool expanded, ref bool toggle, float headerHeight = 18f, bool roundedBox = true, bool canExpand = true)
        {
            Rect headerRect = EditorGUILayout.GetControlRect(false, headerHeight + 4f);
            Rect boxRect = headerRect;
            bool foldoutResult = expanded;

            if (foldoutResult)
            {
                Rect drawingRect = EditorGUILayout.BeginVertical(Styles.borderBoxHeaderStyle);
                boxRect.yMax = drawingRect.yMax;
            }

            GUI.Box(boxRect, GUIContent.none, new GUIStyle(roundedBox ? "HelpBox" : "Tooltip"));
            DrawFoldoutToggleHeader(headerRect, title, ref expanded, ref toggle, canExpand);
            return foldoutResult;
        }

        /// <summary>
        /// Begin a bordered vertical foldout toggle group.
        /// </summary>
        public static bool BeginFoldoutToggleBorderLayout(GUIContent title, ref bool expanded, ref bool toggle, out Rect headerRect, float headerHeight = 18f, bool roundedBox = true, bool canExpand = true)
        {
            headerRect = EditorGUILayout.GetControlRect(false, headerHeight + 4f);
            Rect boxRect = headerRect;
            bool foldoutResult = expanded;

            if (foldoutResult)
            {
                Rect drawingRect = EditorGUILayout.BeginVertical(Styles.borderBoxHeaderStyle);
                boxRect.yMax = drawingRect.yMax;
            }

            GUI.Box(boxRect, GUIContent.none, new GUIStyle(roundedBox ? "HelpBox" : "Tooltip"));
            DrawFoldoutToggleHeader(headerRect, title, ref expanded, ref toggle, canExpand);
            return foldoutResult;
        }

        /// <summary>
        /// Begin a bordered vertical foldout toggle group.
        /// </summary>
        public static bool BeginFoldoutToggleBorderLayout(SerializedProperty foldoutProperty, GUIContent title, ref bool toggle, float headerHeight = 18f, bool roundedBox = true, bool canExpand = true)
        {
            Rect headerRect = EditorGUILayout.GetControlRect(false, headerHeight + 4f);
            Rect boxRect = headerRect;

            bool expanded = foldoutProperty.isExpanded;
            bool foldoutResult = expanded;

            if (foldoutResult)
            {
                Rect drawingRect = EditorGUILayout.BeginVertical(Styles.borderBoxHeaderStyle);
                boxRect.yMax = drawingRect.yMax;
            }

            GUI.Box(boxRect, GUIContent.none, new GUIStyle(roundedBox ? "HelpBox" : "Tooltip"));
            DrawFoldoutToggleHeader(headerRect, title, ref expanded, ref toggle, canExpand);
            foldoutProperty.isExpanded = expanded;
            return foldoutResult;
        }

        /// <summary>
        /// Begin a bordered vertical foldout toggle group.
        /// </summary>
        public static bool BeginFoldoutToggleBorderLayout(GUIContent title, SerializedProperty toggleProperty, float headerHeight = 18f, bool roundedBox = true, bool canExpand = true)
        {
            Rect headerRect = EditorGUILayout.GetControlRect(false, headerHeight + 4f);
            Rect boxRect = headerRect;

            bool expanded = toggleProperty.isExpanded;
            bool toggle = toggleProperty.boolValue;
            bool foldoutResult = expanded;

            if (foldoutResult)
            {
                Rect drawingRect = EditorGUILayout.BeginVertical(Styles.borderBoxHeaderStyle);
                boxRect.yMax = drawingRect.yMax;
            }

            GUI.Box(boxRect, GUIContent.none, new GUIStyle(roundedBox ? "HelpBox" : "Tooltip"));
            DrawFoldoutToggleHeader(headerRect, title, ref expanded, ref toggle, canExpand);
            toggleProperty.boolValue = toggle;
            toggleProperty.isExpanded = expanded;
            return foldoutResult;
        }

        /// <summary>
        /// Begin a bordered vertical foldout toggle group.
        /// </summary>
        public static bool BeginFoldoutToggleBorderLayout(SerializedProperty foldoutProperty, GUIContent title, ref bool toggle, out Rect headerRect, float headerHeight = 18f, bool roundedBox = true, bool canExpand = true)
        {
            headerRect = EditorGUILayout.GetControlRect(false, headerHeight + 4f);
            Rect boxRect = headerRect;

            bool expanded = foldoutProperty.isExpanded;
            bool foldoutResult = expanded;

            if (foldoutResult)
            {
                Rect drawingRect = EditorGUILayout.BeginVertical(Styles.borderBoxHeaderStyle);
                boxRect.yMax = drawingRect.yMax;
            }

            GUI.Box(boxRect, GUIContent.none, new GUIStyle(roundedBox ? "HelpBox" : "Tooltip"));
            DrawFoldoutToggleHeader(headerRect, title, ref expanded, ref toggle, canExpand);
            foldoutProperty.isExpanded = expanded;
            return foldoutResult;
        }

        /// <summary>
        /// End a bordered vertical group.
        /// </summary>
        public static void EndBorderHeaderLayout()
        {
            EditorGUILayout.EndVertical();
        }

        /// <summary>
        /// Draw all class property childs inside bordered foldout.
        /// </summary>
        public static Rect DrawClassBorderFoldout(SerializedProperty classProperty, GUIContent title, float headerHeight = 18f, bool roundedBox = true)
        {
            var classChildrens = classProperty.GetVisibleChildrens();
            if (BeginFoldoutBorderLayout(classProperty, title, out Rect headerRect, headerHeight, roundedBox))
            {
                foreach (var child in classChildrens)
                {
                    EditorGUI.BeginChangeCheck();
                    {
                        bool isArray = IsArray(child);

                        if (isArray) EditorGUI.indentLevel++;
                        {
                            EditorGUILayout.PropertyField(child, true);
                        }
                        if (isArray) EditorGUI.indentLevel--;
                    }
                    if (EditorGUI.EndChangeCheck())
                    {
                        classProperty.serializedObject.ApplyModifiedProperties();
                    }
                }

                EndBorderHeaderLayout();
            }

            return headerRect;
        }

        /// <summary>
        /// Draw all class property childs inside bordered foldout.
        /// </summary>
        public static Rect DrawClassBorderFoldout(SerializedProperty classProperty, GUIContent title, ref bool expanded, float headerHeight = 18f, bool roundedBox = true)
        {
            var classChildrens = classProperty.GetVisibleChildrens();
            if (BeginFoldoutBorderLayout(title, ref expanded, out Rect headerRect, headerHeight, roundedBox))
            {
                foreach (var child in classChildrens)
                {
                    EditorGUI.BeginChangeCheck();
                    {
                        bool isArray = IsArray(child);

                        if (isArray) EditorGUI.indentLevel++;
                        {
                            EditorGUILayout.PropertyField(child, true);
                        }
                        if (isArray) EditorGUI.indentLevel--;
                    }
                    if (EditorGUI.EndChangeCheck())
                    {
                        classProperty.serializedObject.ApplyModifiedProperties();
                    }
                }

                EndBorderHeaderLayout();
            }

            return headerRect;
        }

        /// <summary>
        /// Draw scriptable property childs inside bordered foldout.
        /// </summary>
        public static Rect DrawScriptableBorderFoldout(SerializedProperty scriptableProperty, GUIContent title, float headerHeight = 18f, bool roundedBox = true)
        {
            SerializedObject serializedObject = scriptableProperty.propertyType == SerializedPropertyType.ObjectReference
                ? new SerializedObject(scriptableProperty.objectReferenceValue)
                : scriptableProperty.serializedObject;

            SerializedProperty iterator = serializedObject.GetIterator();
            HasIteratorChilds(iterator);

            if (BeginFoldoutBorderLayout(scriptableProperty, title, out Rect headerRect, headerHeight, roundedBox))
            {
                do
                {
                    EditorGUI.BeginChangeCheck();
                    {
                        SerializedProperty child = serializedObject.FindProperty(iterator.name);
                        bool isArray = IsArray(child);

                        if (isArray) EditorGUI.indentLevel++;
                        {
                            EditorGUILayout.PropertyField(child, true);
                        }
                        if (isArray) EditorGUI.indentLevel--;
                    }
                    if (EditorGUI.EndChangeCheck())
                    {
                        serializedObject.ApplyModifiedProperties();
                    }
                }
                while (iterator.NextVisible(false));

                EndBorderHeaderLayout();
            }

            return headerRect;
        }

        /// <summary>
        /// Draw scriptable property childs inside bordered foldout toggle.
        /// </summary>
        public static Rect DrawScriptableBorderFoldoutToggle(SerializedProperty scriptableProperty, GUIContent title, ref bool toggle, float headerHeight = 18f, bool roundedBox = true)
        {
            SerializedObject serializedObject = scriptableProperty.propertyType == SerializedPropertyType.ObjectReference
                ? new SerializedObject(scriptableProperty.objectReferenceValue)
                : scriptableProperty.serializedObject;

            SerializedProperty iterator = serializedObject.GetIterator();
            bool hasChilds = HasIteratorChilds(iterator);

            if (BeginFoldoutToggleBorderLayout(scriptableProperty, title, ref toggle, out Rect headerRect, headerHeight, roundedBox, canExpand: hasChilds))
            {
                do
                {
                    EditorGUI.BeginChangeCheck();
                    {
                        SerializedProperty child = serializedObject.FindProperty(iterator.name);
                        bool isArray = IsArray(child);

                        if (isArray) EditorGUI.indentLevel++;
                        {
                            EditorGUILayout.PropertyField(child, true);
                        }
                        if (isArray) EditorGUI.indentLevel--;
                    }
                    if (EditorGUI.EndChangeCheck())
                    {
                        serializedObject.ApplyModifiedProperties();
                    }
                }
                while (iterator.NextVisible(false));

                EndBorderHeaderLayout();
            }

            return headerRect;
        }

        /// <summary>
        /// Draw scriptable property childs inside bordered foldout toggle.
        /// </summary>
        public static Rect DrawScriptableBorderFoldoutToggle(SerializedProperty scriptableProperty, GUIContent title, ref bool expanded, ref bool toggle, float headerHeight = 18f, bool roundedBox = true)
        {
            SerializedObject serializedObject = scriptableProperty.propertyType == SerializedPropertyType.ObjectReference
                ? new SerializedObject(scriptableProperty.objectReferenceValue)
                : scriptableProperty.serializedObject;

            SerializedProperty iterator = serializedObject.GetIterator();
            bool hasChilds = HasIteratorChilds(iterator);

            if (BeginFoldoutToggleBorderLayout(title, ref expanded, ref toggle, out Rect headerRect, headerHeight, roundedBox, canExpand: hasChilds))
            {
                do
                {
                    EditorGUI.BeginChangeCheck();
                    {
                        SerializedProperty child = serializedObject.FindProperty(iterator.name);
                        bool isArray = IsArray(child);

                        if (isArray) EditorGUI.indentLevel++;
                        {
                            EditorGUILayout.PropertyField(child, true);
                        }
                        if (isArray) EditorGUI.indentLevel--;
                    }
                    if (EditorGUI.EndChangeCheck())
                    {
                        serializedObject.ApplyModifiedProperties();
                    }
                }
                while (iterator.NextVisible(false));

                EndBorderHeaderLayout();
            }

            return headerRect;
        }

        /// <summary>
        /// Create a new texture with the given dimensions and color.
        /// </summary>
        public static Texture2D MakeTexture(int width, int height, Color color)
        {
            Color[] pix = new Color[width * height];

            for (int i = 0; i < pix.Length; i++)
                pix[i] = color;

            Texture2D result = new Texture2D(width, height);
            result.SetPixels(pix);
            result.Apply();

            return result;
        }

        /// <summary>
        /// Draw texture with a transprent background.
        /// </summary>
        public static void DrawTransparentTexture(Rect rect, Texture image)
        {
            Color guiColor = GUI.color;
            GUI.color = Color.clear;
            EditorGUI.DrawTextureTransparent(rect, image, ScaleMode.ScaleToFit);
            GUI.color = guiColor;
        }

        /// <summary>
        /// Draw texture with a transprent background.
        /// </summary>
        public static void DrawTransparentTextureWithChecker(Rect rect, Object spriteObj)
        {
            GUI.DrawTexture(rect, Styles.TransparentCheckerTexture, ScaleMode.ScaleToFit, alphaBlend: false);
            if(spriteObj != null) GUI.DrawTexture(rect, ((Sprite)spriteObj).texture, ScaleMode.ScaleToFit, alphaBlend: true);
        }

        public static PropertyCollection GetAllProperties(SerializedProperty classProperty)
        {
            PropertyCollection properties = new();

            SerializedProperty currentProperty = classProperty.Copy();
            SerializedProperty nextSiblingProperty = classProperty.Copy();
            nextSiblingProperty.NextVisible(false);

            if (currentProperty.NextVisible(true))
            {
                do
                {
                    if (SerializedProperty.EqualContents(currentProperty, nextSiblingProperty))
                        break;

                    properties.Add(currentProperty.name, currentProperty.Copy());
                }
                while (currentProperty.NextVisible(false));
            }

            return properties;
        }

        public static PropertyCollection GetAllProperties(SerializedObject serializedObject)
        {
            PropertyCollection properties = new();

            SerializedProperty property = serializedObject.GetIterator();
            SerializedProperty currentProperty = property.Copy();
            SerializedProperty nextSiblingProperty = property.Copy();

            if (currentProperty.NextVisible(true))
            {
                do
                {
                    if (SerializedProperty.EqualContents(currentProperty, nextSiblingProperty))
                        break;

                    properties.Add(currentProperty.name, currentProperty.Copy());
                }
                while (currentProperty.NextVisible(false));
            }

            return properties;
        }

        public static bool HasIteratorChilds(SerializedProperty iterator)
        {
            return iterator != null && iterator.NextVisible(true) && iterator.NextVisible(false);
        }

        public static bool IsArray(SerializedProperty property)
        {
            return property.isArray && property.propertyType != SerializedPropertyType.String;
        }

        public static float GetPropertyChildHeight(SerializedProperty iterator)
        {
            SerializedProperty ite = iterator.Copy();
            float totalHeight = EditorGUI.GetPropertyHeight(ite, true);

            while (ite.NextVisible(false))
            {
                totalHeight += EditorGUIUtility.standardVerticalSpacing;
                totalHeight += EditorGUI.GetPropertyHeight(ite, true);
            }

            return totalHeight;
        }
    }

    public static class AdvancedDropdownExtensions
    {
        public static void Show(this AdvancedDropdown dropdown, Rect buttonRect, float maxHeight)
        {
            dropdown.Show(buttonRect);
            SetMaxHeightForOpenedPopup(buttonRect, maxHeight);
        }

        private static void SetMaxHeightForOpenedPopup(Rect buttonRect, float maxHeight)
        {
            var window = EditorWindow.focusedWindow;

            if (window == null)
            {
                Debug.LogWarning("EditorWindow.focusedWindow was null.");
                return;
            }

            if (!string.Equals(window.GetType().Namespace, typeof(AdvancedDropdown).Namespace))
            {
                Debug.LogWarning("EditorWindow.focusedWindow " + EditorWindow.focusedWindow.GetType().FullName + " was not in expected namespace.");
                return;
            }

            var position = window.position;
            if (position.height <= maxHeight)
            {
                return;
            }

            position.height = maxHeight;
            window.minSize = position.size;
            window.maxSize = position.size;
            window.position = position;
            window.ShowAsDropDown(GUIUtility.GUIToScreenRect(buttonRect), position.size);
        }
    }
}