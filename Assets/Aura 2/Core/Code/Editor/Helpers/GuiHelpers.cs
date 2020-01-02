
/***************************************************************************
*                                                                          *
*  Copyright (c) Raphaël Ernaelsten (@RaphErnaelsten)                      *
*  All Rights Reserved.                                                    *
*                                                                          *
*  NOTICE: Aura 2 is a commercial project.                                 * 
*  All information contained herein is, and remains the property of        *
*  Raphaël Ernaelsten.                                                     *
*  The intellectual and technical concepts contained herein are            *
*  proprietary to Raphaël Ernaelsten and are protected by copyright laws.  *
*  Dissemination of this information or reproduction of this material      *
*  is strictly forbidden.                                                  *
*                                                                          *
***************************************************************************/

using UnityEngine;
using UnityEditor;

namespace Aura2API
{
    /// <summary>
    /// Collection of helper functions for a Custom Inspector GUI
    /// </summary>
    public static class GuiHelpers
    {
        /// <summary>
        /// Draws a toggle button
        /// </summary>
        /// <param name="value">The boolean to toggle</param>
        /// <param name="guiContent">The label of the button</param>
        public static void DrawToggleButton(ref bool value, GUIContent guiContent)
        {
            value = GUILayout.Toggle(value, guiContent, value ? GuiStyles.ButtonPressed : GuiStyles.Button);
        }
        /// <summary>
        /// Draws a toggle button
        /// </summary>
        /// <param name="value">The boolean to toggle</param>
        /// <param name="label">The label of the button</param>
        public static void DrawToggleButton(ref bool value, string label)
        {
            DrawToggleButton(ref value, new GUIContent(label));
        }
        /// <summary>
        /// Draws a toggle button
        /// </summary>
        /// <param name="serializedProperty">The boolean property to toggle</param>
        /// <param name="guiContent">The label of the button</param>
        public static void DrawToggleButton(ref SerializedProperty serializedProperty, GUIContent guiContent)
        {
            if(serializedProperty.propertyType == SerializedPropertyType.Boolean)
            {
                bool tmp = serializedProperty.boolValue;
                DrawToggleButton(ref tmp, guiContent);
                serializedProperty.boolValue = tmp;
            }
            else
            {
                Debug.LogError("The property " + serializedProperty.name + " is not a boolean type");
            }
        }
        /// <summary>
        /// Draws a toggle button
        /// </summary>
        /// <param name="serializedProperty">The boolean property to toggle</param>
        /// <param name="label">The label of the button</param>
        public static void DrawToggleButton(ref SerializedProperty serializedProperty, string label)
        {
            DrawToggleButton(ref serializedProperty, new GUIContent(label));
        }

        public const int checkerSize = 19;
        /// <summary>
        /// Draws a toggle checker
        /// </summary>
        /// <param name="value">The boolean to toggle</param>
        /// <param name="guiContent">The label of the checker</param>
        public static bool DrawToggleChecker(bool value, GUIContent guiContent, bool bold = false, bool big = false)
        {
            int offset = guiContent.image != null ? Mathf.Max(checkerSize, guiContent.image.height) : checkerSize - Mathf.CeilToInt((float)GuiStyles.Label.padding.top * 0.5f);
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.BeginVertical(GUILayout.Width(checkerSize), GUILayout.Height(checkerSize));
            GUILayout.Space(Mathf.Ceil((float)(offset - checkerSize) * 0.5f) + Mathf.Ceil((float)GuiStyles.Label.padding.top * 0.5f));
            value = GUILayout.Toggle(value, value ? Aura.ResourcesCollection.checkerCheckedStylesTexture : null, GuiStyles.Checker, GUILayout.Width(checkerSize), GUILayout.Height(checkerSize));
            EditorGUILayout.EndVertical();
            GUILayout.Space(4);
            GUIStyle guiStyle = bold && big ? GuiStyles.LabelBoldBig : (bold ? GuiStyles.LabelBold : (big ? GuiStyles.LabelBig : GuiStyles.Label));
            value = GUILayout.Toggle(value, guiContent, guiStyle);
            EditorGUILayout.EndHorizontal();

            return value;
        }
        /// <summary>
        /// Draws a toggle checker
        /// </summary>
        /// <param name="value">The boolean to toggle</param>
        /// <param name="guiContent">The label of the checker</param>
        public static void DrawToggleChecker(ref bool value, GUIContent guiContent, bool bold = false, bool big = false)
        {
            value = DrawToggleChecker(value, guiContent, bold, big);
        }
        /// <summary>
        /// Draws a toggle checker
        /// </summary>
        /// <param name="value">The boolean to toggle</param>
        /// <param name="label">The label of the checker</param>
        public static void DrawToggleChecker(ref bool value, string label, bool bold = false, bool big = false)
        {
            DrawToggleChecker(ref value, new GUIContent(label), bold, big);
        }
        /// <summary>
        /// Draws a toggle checker
        /// </summary>
        /// <param name="value">The boolean to toggle</param>
        /// <param name="label">The label of the checker</param>
        public static bool DrawToggleChecker(bool value, string label, bool bold = false, bool big = false)
        {
            return DrawToggleChecker(value, new GUIContent(label), bold, big);
        }
        /// <summary>
        /// Draws a toggle checker
        /// </summary>
        /// <param name="serializedProperty">The boolean property to toggle</param>
        /// <param name="guiContent">The label of the checker</param>
        public static void DrawToggleChecker(ref SerializedProperty serializedProperty, GUIContent guiContent, bool bold = false, bool big = false)
        {
            if (serializedProperty.propertyType == SerializedPropertyType.Boolean)
            {
                bool tmp = serializedProperty.boolValue;
                DrawToggleChecker(ref tmp, guiContent, bold, big);
                serializedProperty.boolValue = tmp;
            }
            else
            {
                Debug.LogError("The property " + serializedProperty.name + " is not a boolean type");
            }
        }
        /// <summary>
        /// Draws a toggle checker
        /// </summary>
        /// <param name="serializedProperty">The boolean property to toggle</param>
        /// <param name="label">The label of the checker</param>
        public static void DrawToggleChecker(ref SerializedProperty serializedProperty, string label, bool bold = false, bool big = false)
        {
            DrawToggleChecker(ref serializedProperty, new GUIContent(label), bold, big);
        }

        /// <summary>
        /// Draws a colored separator
        /// </summary>
        /// <param name="color">The desired colore</param>
        /// <param name="rect">The position</param>
        /// <param name="thickness">The width</param>
        /// <param name="height">The height</param>
        /// <param name="widthOffset">The width margin</param>
        public static void DrawSeparator(Color color, Rect rect, int thickness, int height, int widthOffset)
        {
            rect.x -= widthOffset / 2;
            rect.width += widthOffset;
            rect.y += height / 2 - thickness / 2;
            rect.height = thickness;
            EditorGUI.DrawRect(rect, color);
            GUILayout.Space(height);
        }

        /// <summary>
        /// Draws a header with a logo texture
        /// </summary>
        /// <param name="logoTexture">The logo to draw</param>
        public static void DrawHeader(Texture2D logoTexture)
        {
            GuiHelpers.DrawTexture(logoTexture, logoTexture.width);
        }

        /// <summary>
        /// Draws a texture
        /// </summary>
        /// <param name="texture">The texture to draw</param>
        /// <param name="maxSize">The maximum pixel size</param>
        public static void DrawTexture(Texture2D texture, float maxSize)
        {
            Rect rect = EditorGUILayout.BeginHorizontal();
            Rect textureRect = GUILayoutUtility.GetAspectRect(texture.width / texture.height, GUILayout.MaxWidth(maxSize));
            textureRect.x += rect.width / 2 - textureRect.width / 2;
            GUI.DrawTexture(textureRect, texture);
            EditorGUILayout.EndHorizontal();
        }

        /// <summary>
        /// Draws the field required to draw a VolumeInjectionParameters object
        /// </summary>
        /// <param name="injectionProperty">The related serialized property</param>
        public static void DrawInjectionField(ref SerializedProperty injectionProperty, bool displayNoiseParameters, bool displayTexture2DMaskParameters, bool displayTexture3DMaskParameters, bool clampStrength = false, float clampStrengthMin = 0.0f, float clampStrengthMax = 1.0f, bool saturateLevelOuput = false, string strengthLabel = "Strength", string strengthTooltip = "Strength of the injected data. Can be set negative to remove data.")
        {
            SerializedProperty strengthProperty = injectionProperty.FindPropertyRelative("strength");
            GUIContent strengthContent = new GUIContent(strengthLabel, strengthTooltip);
            if(clampStrength)
            {
                DrawSlider(ref strengthProperty, clampStrengthMin, clampStrengthMax, strengthContent.text);
            }
            else
            {
                DrawFloatField(ref strengthProperty, strengthContent);
            }

            if (displayNoiseParameters)
            {
                //EditorGUI.BeginDisabledGroup(!displayNoiseParameters);
                EditorGUILayout.Separator();
                EditorGUILayout.Separator();
                SerializedProperty useNoiseMask = injectionProperty.FindPropertyRelative("useNoiseMask");
                GuiHelpers.DrawToggleChecker(ref useNoiseMask, "Use Noise Mask", true);
                if (useNoiseMask.boolValue)
                {
                    EditorGUILayout.Separator();
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.BeginVertical(GUILayout.Width(4));
                    GUILayout.Label("");
                    EditorGUILayout.EndVertical();
                    EditorGUILayout.BeginVertical();
                    SerializedProperty useNoiseMaskLevels = injectionProperty.FindPropertyRelative("useNoiseMaskLevels");
                    GuiHelpers.DrawToggleChecker(ref useNoiseMaskLevels, "Levels");
                    if (useNoiseMaskLevels.boolValue)
                    {
                        EditorGUILayout.Separator();
                        //EditorGUI.BeginDisabledGroup(!useNoiseMaskLevels.boolValue);
                        SerializedProperty noiseMaskLevelsProperty = injectionProperty.FindPropertyRelative("noiseMaskLevelParameters");
                        GuiHelpers.DrawLevelsField(ref noiseMaskLevelsProperty, saturateLevelOuput);
                        //EditorGUI.EndDisabledGroup();
                    }
                    EditorGUILayout.EndVertical();
                    EditorGUILayout.EndHorizontal();
                }
                //EditorGUI.EndDisabledGroup();
            }

            if (displayTexture2DMaskParameters)
            {
                //EditorGUI.BeginDisabledGroup(!displayTexture2DMaskParameters);
                EditorGUILayout.Separator();
                EditorGUILayout.Separator();
                SerializedProperty useTexture2DMask = injectionProperty.FindPropertyRelative("useTexture2DMask");
                GuiHelpers.DrawToggleChecker(ref useTexture2DMask, "Use Texture2D Mask", true);
                if (useTexture2DMask.boolValue)
                {
                    EditorGUILayout.Separator();
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.BeginVertical(GUILayout.Width(4));
                    GUILayout.Label("");
                    EditorGUILayout.EndVertical();
                    EditorGUILayout.BeginVertical();
                    SerializedProperty useTexture2DMaskLevels = injectionProperty.FindPropertyRelative("useTexture2DMaskLevels");
                    GuiHelpers.DrawToggleChecker(ref useTexture2DMaskLevels, "Levels");
                    if (useTexture2DMaskLevels.boolValue)
                    {
                        EditorGUILayout.Separator();
                        //EditorGUI.BeginDisabledGroup(!useTexture2DMaskLevels.boolValue);
                        SerializedProperty texture2DMaskLevelsProperty = injectionProperty.FindPropertyRelative("texture2DMaskLevelParameters");
                        GuiHelpers.DrawLevelsField(ref texture2DMaskLevelsProperty, saturateLevelOuput);
                        //EditorGUI.EndDisabledGroup();
                    }
                    EditorGUILayout.EndVertical();
                    EditorGUILayout.EndHorizontal();
                }
                //EditorGUI.EndDisabledGroup();
            }

            if (displayTexture3DMaskParameters)
            {
                //EditorGUI.BeginDisabledGroup(!displayTexture3DMaskParameters);
                EditorGUILayout.Separator();
                EditorGUILayout.Separator();
                SerializedProperty useTexture3DMask = injectionProperty.FindPropertyRelative("useTexture3DMask");
                GuiHelpers.DrawToggleChecker(ref useTexture3DMask, "Texture3D Mask Levels", true);
                if (useTexture3DMask.boolValue)
                {
                    EditorGUILayout.Separator();
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.BeginVertical(GUILayout.Width(4));
                    GUILayout.Label("");
                    EditorGUILayout.EndVertical();
                    EditorGUILayout.BeginVertical();
                    SerializedProperty useTexture3DMaskLevels = injectionProperty.FindPropertyRelative("useTexture3DMaskLevels");
                    GuiHelpers.DrawToggleChecker(ref useTexture3DMaskLevels, "Levels");
                    if (useTexture3DMaskLevels.boolValue)
                    {
                        EditorGUILayout.Separator();
                        //EditorGUI.BeginDisabledGroup(!useTexture3DMaskLevels.boolValue);
                        SerializedProperty texture3DMaskLevelsProperty = injectionProperty.FindPropertyRelative("texture3DMaskLevelParameters");
                        GuiHelpers.DrawLevelsField(ref texture3DMaskLevelsProperty, saturateLevelOuput);
                        //EditorGUI.EndDisabledGroup();
                    }
                    EditorGUILayout.EndVertical();
                    EditorGUILayout.EndHorizontal();
                }
                //EditorGUI.EndDisabledGroup();
            }
        }

        /// <summary>
        /// Draws the fields required for a LevelsParameters object
        /// </summary>
        /// <param name="levelsProperty">The related serialized property</param>
        public static void DrawLevelsField(ref SerializedProperty levelsProperty, bool saturateOuput = false)
        {
            GuiHelpers.DrawContextualHelpBox("The \"Levels\" parameter will filter the input value.\n\nKeeps the value between the \"Level Thresholds\" and remaps the range from 0 to 1.\"");
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Filter Range", GuiStyles.Label, GUILayout.MaxWidth(160));
            SerializedProperty levelLowThresholdProperty = levelsProperty.FindPropertyRelative("levelLowThreshold");
            SerializedProperty levelHiThresholdProperty = levelsProperty.FindPropertyRelative("levelHiThreshold");
            GuiHelpers.DrawMinMaxSlider(ref levelLowThresholdProperty, ref levelHiThresholdProperty, 0, 1);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Separator();

            GUIContent content = new GUIContent("Contrast", "Contrast the input mask");
            SerializedProperty contrastProperty = levelsProperty.FindPropertyRelative("contrast");
            DrawFloatField(ref contrastProperty, content);

            EditorGUILayout.Separator();

            content.text = "Output Min";
            content.tooltip = "Output minimum value";
            GuiHelpers.DrawContextualHelpBox("The \"Output\" parameters will rescale this new range\n\n0 will now equal the lower \"Output Value\" and 1 will now equal the higher.");
            SerializedProperty outputLowValueProperty = levelsProperty.FindPropertyRelative("outputLowValue");
            if(saturateOuput)
            {
                DrawSlider(ref outputLowValueProperty, 0.0f, 1.0f, content.text);
            }
            else
            {
                DrawFloatField(ref outputLowValueProperty, content);
            }
            content.text = "Output Max";
            content.tooltip = "Output maximum value";
            SerializedProperty outputHiValueProperty = levelsProperty.FindPropertyRelative("outputHiValue");
            if (saturateOuput)
            {
                DrawSlider(ref outputHiValueProperty, 0.0f, 1.0f, content.text);
            }
            else
            {
                DrawFloatField(ref outputHiValueProperty, content);
            }
        }

        /// <summary>
        /// Draws the fields required for a TransformParameters object
        /// </summary>
        /// <param name="transformProperty">The related serialized property</param>
        public static void DrawTransformField(ref SerializedProperty transformProperty)
        {
            EditorGUILayout.BeginHorizontal();
            SerializedProperty positionProperty = transformProperty.FindPropertyRelative("position");
            GUILayout.Label("Position", GuiStyles.Label, GUILayout.MaxWidth(160));
            GuiHelpers.DrawVector3Field(ref positionProperty);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Separator();

            EditorGUILayout.BeginHorizontal();
            SerializedProperty rotationProperty = transformProperty.FindPropertyRelative("rotation");
            GUILayout.Label("Rotation", GuiStyles.Label, GUILayout.MaxWidth(160));
            GuiHelpers.DrawVector3Field(ref rotationProperty);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Separator();

            EditorGUILayout.BeginHorizontal();
            SerializedProperty scaleProperty = transformProperty.FindPropertyRelative("scale");
            GUILayout.Label("Scale    ", GuiStyles.Label, GUILayout.MaxWidth(160));
            GuiHelpers.DrawVector3Field(ref scaleProperty);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Separator();

            SerializedProperty spaceProperty = transformProperty.FindPropertyRelative("space");
            GuiHelpers.DrawSpaceField(ref spaceProperty, "Space");
        }

        /// <summary>
        /// Draws a Texture2D object field
        /// </summary>
        /// <param name="texture2DProperty">The related serialized property</param>
        public static void DrawTexture2DField(ref SerializedProperty texture2DProperty)
        {
            EditorGUILayout.ObjectField(texture2DProperty.objectReferenceValue, typeof(Texture2D), false);
        }

        /// <summary>
        /// Draws a Texture3D object field
        /// </summary>
        /// <param name="texture3DProperty">The related serialized property</param>
        public static void DrawTexture3DField(ref SerializedProperty texture3DProperty)
        {
            EditorGUILayout.ObjectField(texture3DProperty.objectReferenceValue, typeof(Texture3D), false);
        }

        /// <summary>
        /// Draws a float field
        /// </summary>
        /// <param name="floatProperty">The related serialized property</param>
        public static void DrawFloatField(ref SerializedProperty floatProperty)
        {
            DrawFloatField(ref floatProperty, (GUIContent)null);
        }

        /// <summary>
        /// Draws a float field
        /// </summary>
        /// <param name="floatProperty">The related serialized property</param>
        /// <param name="label">The label to write</param>
        public static void DrawFloatField(ref SerializedProperty floatProperty, string label)
        {
            DrawFloatField(ref floatProperty, new GUIContent(label));
        }
        /// <summary>
        /// Draws a float field
        /// </summary>
        /// <param name="floatProperty">The related serialized property</param>
        /// <param name="label">The label to write</param>
        public static void DrawFloatField(ref SerializedProperty floatProperty, GUIContent label)
        {
            if (label != null)
            {
                floatProperty.floatValue = EditorGUILayout.FloatField(label, floatProperty.floatValue, GUILayout.MinWidth(15));
            }
            else
            {
                floatProperty.floatValue = EditorGUILayout.FloatField(floatProperty.floatValue, GUILayout.MinWidth(15));
            }
        }

        /// <summary>
        /// Draws a float field that cannot go under 0
        /// </summary>
        /// <param name="floatProperty">The related serialized property</param>
        public static void DrawPositiveOnlyFloatField(ref SerializedProperty floatProperty)
        {
            DrawPositiveOnlyFloatField(ref floatProperty, (GUIContent)null);
        }

        /// <summary>
        /// Draws a float field that cannot go under 0
        /// </summary>
        /// <param name="floatProperty">The related serialized property</param>
        /// <param name="label">The label to write</param>
        public static void DrawPositiveOnlyFloatField(ref SerializedProperty floatProperty, string label)
        {
            DrawPositiveOnlyFloatField(ref floatProperty, new GUIContent(label));
        }

        /// <summary>
        /// Draws a float field that cannot go under 0
        /// </summary>
        /// <param name="floatProperty">The related serialized property</param>
        /// <param name="label">The label to write</param>
        public static void DrawPositiveOnlyFloatField(ref SerializedProperty floatProperty, GUIContent label)
        {
            if(label !=null)
            {
                floatProperty.floatValue = EditorGUILayout.FloatField(label, floatProperty.floatValue, GUILayout.MinWidth(15));
            }
            else
            {
                floatProperty.floatValue = EditorGUILayout.FloatField(floatProperty.floatValue, GUILayout.MinWidth(15));
            }

            floatProperty.floatValue = Mathf.Max(floatProperty.floatValue, 0);
        }

        /// <summary>
        /// Draws a Vector3 field
        /// </summary>
        /// <param name="vector3Property">The related serialized property</param>
        public static void DrawVector3Field(ref SerializedProperty vector3Property)
        {
            GuiHelpers.DrawVector3Field(ref vector3Property, null);
        }

        /// <summary>
        /// Draws a Vector3 field
        /// </summary>
        /// <param name="vector3Property">The related serialized property</param>
        /// <param name="label">The label to write</param>
        public static void DrawVector3Field(ref SerializedProperty vector3Property, string label)
        {
            SerializedProperty xTmpProperty = vector3Property.FindPropertyRelative("x");
            SerializedProperty yTmpProperty = vector3Property.FindPropertyRelative("y");
            SerializedProperty zTmpProperty = vector3Property.FindPropertyRelative("z");

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("X", GuiStyles.Label, GUILayout.Width(14));
            GuiHelpers.DrawFloatField(ref xTmpProperty);

            GUILayout.Label("Y", GuiStyles.Label, GUILayout.Width(14));
            GuiHelpers.DrawFloatField(ref yTmpProperty);

            GUILayout.Label("Z", GuiStyles.Label, GUILayout.Width(14));
            GuiHelpers.DrawFloatField(ref zTmpProperty);
            EditorGUILayout.EndHorizontal();
        }

        /// <summary>
        /// Draws a Space field
        /// </summary>
        /// <param name="spaceProperty">The related serialized property</param>
        /// <param name="label">The label to write</param>
        public static void DrawSpaceField(ref SerializedProperty spaceProperty, string label)
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label(label, GuiStyles.Label, GUILayout.MaxWidth(160));
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("World", spaceProperty.enumValueIndex == 0 ? GuiStyles.ButtonPressed : GuiStyles.Button))
            {
                spaceProperty.enumValueIndex = 0;
            }
            if (GUILayout.Button("Local", spaceProperty.enumValueIndex == 1 ? GuiStyles.ButtonPressed : GuiStyles.Button))
            {
                spaceProperty.enumValueIndex = 1;
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndHorizontal();
        }

        /// <summary>
        /// Draws a float slider with a minimum and a maximum value
        /// </summary>
        /// <param name="min">The related serialized property</param>
        /// <param name="max">The related serialized property</param>
        /// <param name="minimumValue">The minimum possible value</param>
        /// <param name="maximumValue">The maximum possible value</param>
        /// <param name="invertMaxValue">One minus value</param>
        public static void DrawMinMaxSlider(ref SerializedProperty min, ref SerializedProperty max, float minimumValue, float maximumValue, bool invertMaxValue = false)
        {
            DrawMinMaxSlider(ref min, ref max, minimumValue, maximumValue, null, invertMaxValue);
        }

        /// <summary>
        /// Draws a float slider with a minimum and a maximum value
        /// </summary>
        /// <param name="min">The related serialized property</param>
        /// <param name="max">The related serialized property</param>
        /// <param name="minimumValue">The minimum possible value</param>
        /// <param name="maximumValue">The maximum possible value</param>
        /// <param name="label">The label to write</param>
        /// <param name="invertMaxValue">One minus value</param>
        public static void DrawMinMaxSlider(ref SerializedProperty min, ref SerializedProperty max, float minimumValue, float maximumValue, string label, bool invertMaxValue = false)
        {
            float minimumTmp = min.floatValue;
            float maximumTmp = max.floatValue;
            if(invertMaxValue)
            {
                maximumTmp = 1.0f - maximumTmp;
            }

            if (label != null)
            {
                EditorGUILayout.PrefixLabel(label);
            }

            EditorGUI.BeginChangeCheck();
            minimumTmp = Mathf.Clamp01(EditorGUILayout.DelayedFloatField(minimumTmp, GUILayout.MaxWidth(50), GUILayout.MinWidth(20)));
            EditorGUILayout.MinMaxSlider(ref minimumTmp, ref maximumTmp, minimumValue, maximumValue, GUILayout.MinWidth(5));
            maximumTmp = Mathf.Max(minimumTmp, Mathf.Clamp01(EditorGUILayout.DelayedFloatField(maximumTmp, GUILayout.MaxWidth(50), GUILayout.MinWidth(20))));

            if(EditorGUI.EndChangeCheck())
            {
                Event e = Event.current;
                if(e.control)
                {
                    minimumTmp = minimumTmp.Snap(0.125f);
                    maximumTmp = maximumTmp.Snap(0.125f);
                }

                if(invertMaxValue)
                {
                    maximumTmp = 1.0f - maximumTmp;
                }

                min.floatValue = minimumTmp;
                max.floatValue = maximumTmp;
            }
        }

        /// <summary>
        /// Draws a float slider
        /// </summary>
        /// <param name="value">The related serialized property</param>
        /// <param name="minimumValue">The minimum possible value</param>
        /// <param name="maximumValue">The maximum possible value</param>
        /// <param name="label">The label to write</param>
        /// <param name="invertOrientation">One minus value</param>
        public static void DrawSlider(ref SerializedProperty value, float minimumValue, float maximumValue, bool invertOrientation = false)
        {
            DrawSlider(ref value, minimumValue, maximumValue, null, invertOrientation);
        }

        /// <summary>
        /// Draws a float slider
        /// </summary>
        /// <param name="value">The related serialized property</param>
        /// <param name="minimumValue">The minimum possible value</param>
        /// <param name="maximumValue">The maximum possible value</param>
        /// <param name="label">The label to write</param>
        /// <param name="invertOrientation">One minus value</param>
        public static void DrawSlider(ref SerializedProperty value, float minimumValue, float maximumValue, string label, bool invertOrientation = false)
        {
            EditorGUILayout.BeginHorizontal();
            float tmp = value.floatValue;
            if (invertOrientation)
            {
                tmp = 1.0f - tmp; // TODO : ONLY WORKS WITH [0;1]
            }

            if (label != null)
            {
                EditorGUILayout.PrefixLabel(label);
            }

            EditorGUI.BeginChangeCheck();
            tmp = EditorGUILayout.Slider(tmp, minimumValue, maximumValue, GUILayout.MinWidth(5));
            if (EditorGUI.EndChangeCheck())
            {
                Event e = Event.current;
                if (e.control)
                {
                    tmp = tmp.Snap(0.125f);
                }

                if (invertOrientation)
                {
                    tmp = 1.0f - tmp; // TODO : ONLY WORKS WITH [0;1]
                }

                value.floatValue = tmp;
            }
            EditorGUILayout.EndHorizontal();
        }

        /// <summary>
        /// Draws an icon in an area
        /// </summary>
        /// <param name="rect">The screen rectangle</param>
        /// <param name="texture">The icon to draw</param>
        public static void DrawAreaIcon(Rect rect, Texture2D texture)
        {
            Rect volumeShapeTextureRect = rect;
            volumeShapeTextureRect.x += rect.width / 2 - texture.width / 2;
            volumeShapeTextureRect.y += 4;
            volumeShapeTextureRect.width = texture.width;
            volumeShapeTextureRect.height = texture.height;
            GUI.DrawTexture(volumeShapeTextureRect, texture, ScaleMode.ScaleToFit, true);
            GUILayout.Space(volumeShapeTextureRect.y + texture.height + 4);
        }

        /// <summary>
        /// Displays a helpbox
        /// </summary>
        /// <param name="text">The text shown in the box</param>
        /// <param name="type">The type of box (icon) shown</param>
        public static void DrawHelpBox(string text, HelpBoxType type)
        {
            EditorGUILayout.BeginHorizontal(GuiStyles.ButtonNoHoverNoBorder);
            GUIContent content = new GUIContent();
            switch (type)
            {
                case HelpBoxType.Warning:
                    {
                        content.image = Aura.ResourcesCollection.warningIconTexture;
                        content.text = "\n<b>WARNING : </b>\n\n" + text + "\n";
                    }
                    break;

                case HelpBoxType.Experimental:
                    {
                        content.image = Aura.ResourcesCollection.experimentalIconTexture;
                        content.text = "\n<b>PREVIEW : </b>\n\n" + text + "\n";
                    }
                    break;

                case HelpBoxType.Question:
                    {
                        content.image = Aura.ResourcesCollection.questionIconTexture;
                        content.text = "\n" + text + "\n";
                    }
                    break;
            }
            GUILayout.Label(content, GuiStyles.Label);
            EditorGUILayout.EndHorizontal();
        }

        /// <summary>
        /// Draws a helpbox when CTRL+ALT are held over the inspector
        /// </summary>
        /// <param name="message">The message to display in the helpbox</param>
        public static void DrawContextualHelpBox(string message)
        {
            if(Event.current.alt && Event.current.control)
            {
                DrawHelpBox(message, HelpBoxType.Question);
            }
        }

        /// <summary>
        /// Displays the tip to show the contextual help box
        /// </summary>
        public static void DisplayHelpToShowHelpBox()
        {
            GUILayout.Label("Hold CTRL+ALT to show help boxes.", GuiStyles.LabelCenteredSmall);
        }
    }
}
