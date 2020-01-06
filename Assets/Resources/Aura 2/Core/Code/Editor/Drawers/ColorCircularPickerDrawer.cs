
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

using UnityEditor;
using UnityEngine;
using System.Reflection;

namespace Aura2API
{
    /// <summary>
    /// Circular picker drawer attribute for Color properties
    /// </summary>
    [CustomPropertyDrawer(typeof(ColorCircularPickerAttribute))]
    internal sealed class ColorCircularPickerDrawer : PropertyDrawer
    {
        #region Private Members
        /// <summary>
        /// Maximum pixel size for the drawer
        /// </summary>
        private const int _maxSize = 256;
        /// <summary>
        /// The material used for drawing the picker
        /// </summary>
        private static Material _material;
        /// <summary>
        /// The drawn area
        /// </summary>
        private Rect _drawArea;
        /// <summary>
        /// Reflected method allowing to store a copied color in the clipboard with the correct format
        /// </summary>
        private MethodInfo _setColorToClipboardMethod;
        /// <summary>
        /// Reflected method allowing to know if the clipboard has a copied color
        /// </summary>
        private MethodInfo _hasColorInClipboardMethod;
        /// <summary>
        /// Reflected method allowing to retrieve a copied color from the clipboard
        /// </summary>
        private MethodInfo _getColorFromClipboardMethod;
        /// <summary>
        /// Temporary color to stock when color is pasted
        /// </summary>
        private Color _pastedColor;
        /// <summary>
        /// Temporary bool to tell if there was a color pasted
        /// </summary>
        private bool _wasColorPasted;
        #endregion

        #region Properties
        private MethodInfo SetColorToClipboardMethod
        {
            get
            {
                if(_setColorToClipboardMethod == null)
                {
                    _setColorToClipboardMethod = typeof(EditorGUIUtility).GetMethod("SetPasteboardColor", BindingFlags.InvokeMethod | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.IgnoreReturn);
                }

                return _setColorToClipboardMethod;
            }
        }

        private MethodInfo HasColorInClipboardMethod
        {
            get
            {
                if(_hasColorInClipboardMethod == null)
                {
                    _hasColorInClipboardMethod = typeof(EditorGUIUtility).GetMethod("HasPasteboardColor", BindingFlags.InvokeMethod | BindingFlags.NonPublic | BindingFlags.Static);
                }

                return _hasColorInClipboardMethod;
            }
        }

        private MethodInfo GetColorFromClipboardMethod
        {
            get
            {
                if(_getColorFromClipboardMethod == null)
                {
                    _getColorFromClipboardMethod = typeof(EditorGUIUtility).GetMethod("GetPasteboardColor", BindingFlags.InvokeMethod | BindingFlags.NonPublic | BindingFlags.Static);
                }

                return _getColorFromClipboardMethod;
            }
        }
        #endregion

        #region Overriden base class functions (https://docs.unity3d.com/ScriptReference/PropertyDrawer.html)
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if(property.propertyType == SerializedPropertyType.Color)
            {
                if(Event.current.type == EventType.Layout)
                {
                    return;
                }

                if(ColorCircularPickerDrawer._material == null)
                {
                    ColorCircularPickerDrawer._material = new Material(Shader.Find("Hidden/Aura2/GUI/DrawCircularPickerDisk"))
                                                         {
                                                             hideFlags = HideFlags.HideAndDontSave
                                                         };
                }

                if(((ColorCircularPickerAttribute)attribute).showLabel)
                {
                    EditorGUI.LabelField(position, label, GuiStyles.LabelCenteredSmall);
                    position.y += 8;
                }

                int size = GetSize((int)position.x * 2);

                _drawArea = position;
                _drawArea.width = size;
                _drawArea.height = _drawArea.width;
                _drawArea.x += position.width * 0.5f - _drawArea.width * 0.5f;

                const float colorDiskSize = 0.45f;
                ColorCircularPickerDrawer._material.SetFloat("colorDiskSize", colorDiskSize);

                Vector3 hsv;
                Color.RGBToHSV(property.colorValue, out hsv.x, out hsv.y, out hsv.z);
                hsv.x = Mathf.Clamp(hsv.x, 0.000001f, 0.999999f);
                hsv.y = Mathf.Clamp(hsv.y, 0.000001f, 0.999999f);
                hsv.z = Mathf.Clamp(hsv.z, 0.000001f, 0.999999f);

                hsv = GetInput(_drawArea, hsv, colorDiskSize);
                if(_wasColorPasted)
                {
                    property.colorValue = _pastedColor;
                    Color.RGBToHSV(property.colorValue, out hsv.x, out hsv.y, out hsv.z);
                    _wasColorPasted = false;
                }

                Rect slidersArea = _drawArea;
                slidersArea.y += slidersArea.height;
                slidersArea.y += EditorGUIUtility.singleLineHeight;
                slidersArea.height = EditorGUIUtility.singleLineHeight;
                GUI.Label(slidersArea, "Hue", EditorStyles.centeredGreyMiniLabel);
                slidersArea.y += EditorGUIUtility.singleLineHeight;
                hsv.x = GUI.HorizontalSlider(slidersArea, hsv.x, 0, 0.999999f);
                slidersArea.y += EditorGUIUtility.singleLineHeight * 2;
                GUI.Label(slidersArea, "Saturation", EditorStyles.centeredGreyMiniLabel);
                slidersArea.y += EditorGUIUtility.singleLineHeight;
                hsv.y = GUI.HorizontalSlider(slidersArea, hsv.y, 0.000001f, 1);

                property.colorValue = Color.HSVToRGB(hsv.x, hsv.y, hsv.z);

                if(Event.current.type == EventType.Repaint)
                {
                    Vector2 thumbPos = Vector2.zero;
                    float theta = hsv.x * (Mathf.PI * 2f);
                    float len = hsv.y * colorDiskSize;
                    thumbPos.x = Mathf.Cos(theta + Mathf.PI / 2f);
                    thumbPos.y = -Mathf.Sin(theta - Mathf.PI / 2f);
                    thumbPos *= len;
                    thumbPos += Vector2.one * 0.5f;
                    ColorCircularPickerDrawer._material.SetVector("pickerCoordinates", thumbPos);

                    ColorCircularPickerDrawer._material.SetFloat("alpha", GUI.enabled ? 1f : 0.333f);

                    RenderTexture tmpRenderTarget = RenderTexture.active;
                    RenderTexture renderTarget = RenderTexture.GetTemporary((int)(size * EditorGUIUtility.pixelsPerPoint), (int)(size * EditorGUIUtility.pixelsPerPoint), 0, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Linear);
                    Graphics.Blit(null, renderTarget, ColorCircularPickerDrawer._material, EditorGUIUtility.isProSkin ? 0 : 1);
                    RenderTexture.active = tmpRenderTarget;
                    GUI.DrawTexture(_drawArea, renderTarget);
                    RenderTexture.ReleaseTemporary(renderTarget);
                }
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return (((ColorCircularPickerAttribute)attribute).showLabel ? EditorGUIUtility.singleLineHeight : 0) + _drawArea.height + EditorGUIUtility.singleLineHeight * 7;
        }
        #endregion

        #region Functions
        /// <summary>
        /// Computes the width based on a given margin
        /// </summary>
        /// <param name="rightMargin">The margin on the right</param>
        /// <returns></returns>
        private int GetSize(int rightMargin)
        {
            int size = Mathf.FloorToInt(EditorGUIUtility.currentViewWidth) - rightMargin;
            size = Mathf.Min(size, ColorCircularPickerDrawer._maxSize);

            return size;
        }

        /// <summary>
        /// Computes the Hue and Saturation values based on a position on a disk
        /// </summary>
        /// <param name="x">The horizontal position</param>
        /// <param name="y">The vertical position</param>
        /// <param name="radius">The radius of the disk</param>
        /// <param name="hue">The output hue (based on the angle difference from the point-center vector and the horizontal)</param>
        /// <param name="saturation">The saturation (based on the normalized distance of the point from the center)</param>
        private void GetHueSaturation(float x, float y, float radius, out float hue, out float saturation)
        {
            hue = Mathf.Atan2(x, -y);
            hue = 1f - (hue > 0 ? hue : Mathf.PI * 2f + hue) / (Mathf.PI * 2f);
            Vector2 scaledPos = new Vector2(x, y);
            saturation = Mathf.Clamp01(scaledPos.magnitude / radius);
        }

        /// <summary>
        /// Watches for mouse interaction with the drawer
        /// </summary>
        /// <param name="bounds">The rectangle on screen to watch</param>
        /// <param name="hsv">The current HSV color</param>
        /// <param name="radius">The raduis of the picker's disk</param>
        /// <returns></returns>
        private Vector3 GetInput(Rect bounds, Vector3 hsv, float radius)
        {
            Event e = Event.current;
            int id = GUIUtility.GetControlID("ColorCircularPickerDrawer".GetHashCode(), FocusType.Passive, bounds);

            Vector2 mousePos = e.mousePosition;
            Vector2 relativePos = mousePos - new Vector2(bounds.center.x, bounds.center.y);
            relativePos.x /= bounds.width;
            relativePos.y /= bounds.height;

            if(e.type == EventType.MouseDown && GUIUtility.hotControl == 0 && bounds.Contains(mousePos))
            {
                switch(e.button)
                {
                    case 0 :
                        if(relativePos.magnitude <= radius)
                        {
                            e.Use();
                            GetHueSaturation(relativePos.x, relativePos.y, radius, out hsv.x, out hsv.y);
                            GUIUtility.hotControl = id;
                            GUI.changed = true;
                        }
                        break;
                    case 1 :
                        GenericMenu copyPasteMenu = new GenericMenu();
                        copyPasteMenu.AddItem(new GUIContent("Copy"), false, CopyColorValue, hsv);
                        if(HasColorInClipboard())
                        {
                            //copyPasteMenu.AddSeparator("");
                            copyPasteMenu.AddItem(new GUIContent("Paste"), false, PasteColorValue);
                        }
                        copyPasteMenu.ShowAsContext();

                        GUI.changed = true;
                        e.Use();
                        break;
                }
            }
            else if(e.type == EventType.MouseDrag && e.button == 0 && GUIUtility.hotControl == id)
            {
                e.Use();
                GUI.changed = true;
                GetHueSaturation(relativePos.x, relativePos.y, radius, out hsv.x, out hsv.y);
            }
            else if(e.rawType == EventType.MouseUp && e.button == 0 && GUIUtility.hotControl == id)
            {
                e.Use();
                GUIUtility.hotControl = 0;
            }

            return hsv;
        }

        /// <summary>
        /// Stores a color into the clipboard
        /// </summary>
        private void SetColorToClipboard(Color color)
        {
            SetColorToClipboardMethod.Invoke(null, new object[1]{color});
        }

        /// <summary>
        /// Stores the HSV color in the clipboard in the #hex format
        /// </summary>
        /// <param name="hsv">The HSV color (Vector3)</param>
        private void CopyColorValue(object hsv)
        {
            Vector3 hsvValue = (Vector3)hsv;
            Color colorFromHsv = Color.HSVToRGB(hsvValue.x, hsvValue.y, hsvValue.z);
            SetColorToClipboard(colorFromHsv);
        }

        /// <summary>
        /// Tells if a color is in the clipboard
        /// </summary>
        private bool HasColorInClipboard()
        {
            return (bool)HasColorInClipboardMethod.Invoke(null, null);
        }

        /// <summary>
        /// Stores a color into the clipboard
        /// </summary>
        private Color GetColorFromClipboard()
        {
            return (Color)GetColorFromClipboardMethod.Invoke(null, null);
        }

        /// <summary>
        /// Retrieves the color from the clipboard and sets the toggle so the current HSV will be replaced on next update
        /// </summary>
        private void PasteColorValue()
        {
            _pastedColor = GetColorFromClipboard();
            _wasColorPasted = true;
        }
        #endregion
    }
}
