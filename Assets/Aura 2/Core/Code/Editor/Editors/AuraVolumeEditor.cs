
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

namespace Aura2API
{
    /// <summary>
    /// Custom Inspector for AuraVolume class
    /// </summary>
    [CustomEditor(typeof(AuraVolume))]
    //[CanEditMultipleObjects]
    public class AuraVolumeEditor : Editor
    {
        #region Private Members
        /// <summary>
        /// The current displayed tab index
        /// </summary>
        private int _tabIndex;
        /// <summary>
        /// The content of the density injection title
        /// </summary>
        private GUIContent _densityInjectionTitleContent;
        /// <summary>
        /// The content of the scattering injection title
        /// </summary>
        private GUIContent _scatteringInjectionTitleContent;
        /// <summary>
        /// The content of the light injection title
        /// </summary>
        private GUIContent _lightInjectionTitleContent;
        /// <summary>
        /// The content of the tint injection title
        /// </summary>
        private GUIContent _tintInjectionTitleContent;
        /// <summary>
        /// The content of the ambient lighting injection title
        /// </summary>
        private GUIContent _ambientInjectionTitleContent;
        /// <summary>
        /// The content of the boost injection title
        /// </summary>
        private GUIContent _boostInjectionTitleContent;
        /// <summary>
        /// The property for choosing the shape of the volume
        /// </summary>
        private SerializedProperty _volumeShapeProperty;
        /// <summary>
        /// The property for adjusting the borders' fadeout
        /// </summary>
        private SerializedProperty _falloffFadeProperty;
        /// <summary>
        /// The property for adjusting the borders' fadeout in X+ for a Cube volume
        /// </summary>
        private SerializedProperty _xPositiveCubeFadeProperty;
        /// <summary>
        /// The property for adjusting the borders' fadeout in X- for a Cube volume
        /// </summary>
        private SerializedProperty _xNegativeCubeFadeProperty;
        /// <summary>
        /// The property for adjusting the borders' fadeout in Y+ for a Cube volume
        /// </summary>
        private SerializedProperty _yPositiveCubeFadeProperty;
        /// <summary>
        /// The property for adjusting the borders' fadeout in Y- for a Cube volume
        /// </summary>
        private SerializedProperty _yNegativeCubeFadeProperty;
        /// <summary>
        /// The property for adjusting the borders' fadeout in Z+ for a Cube volume
        /// </summary>
        private SerializedProperty _zPositiveCubeFadeProperty;
        /// <summary>
        /// The property for adjusting the borders' fadeout in Z- for a Cube volume
        /// </summary>
        private SerializedProperty _zNegativeCubeFadeProperty;
        /// <summary>
        /// The property for adjusting the angular borders' fadeout for a Cone volume
        /// </summary>
        private SerializedProperty _angularConeFadeProperty;
        /// <summary>
        /// The property for adjusting the distance borders' fadeout for a Cone volume
        /// </summary>
        private SerializedProperty _distanceConeFadeProperty;
        /// <summary>
        /// The property for adjusting the barycentric distance borders' fadeout for a Cylinder volume
        /// </summary>
        private SerializedProperty _widthCylinderFadeProperty;
        /// <summary>
        /// The property for adjusting the borders' fadeout in Y- for a Cylinder volume
        /// </summary>
        private SerializedProperty _yNegativeCylinderFadeProperty;
        /// <summary>
        /// The property for adjusting the borders' fadeout in Y+ for a Cylinder volume
        /// </summary>
        private SerializedProperty _yPositiveCylinderFadeProperty;
        /// <summary>
        /// The property for adjusting the distance borders' fadeout for a Sphere volume
        /// </summary>
        private SerializedProperty _distanceSphereFadeProperty;
        /// <summary>
        /// The property for enabling the light probes lighting display
        /// </summary>
        private SerializedProperty _useAsLightProbesProxyVolumeProperty;
        /// <summary>
        /// The property for the light probes lighting multiplier
        /// </summary>
        private SerializedProperty _lightProbesMultiplierProperty;
        /// <summary>
        /// The property for enabling Texture2D mask
        /// </summary>
        private SerializedProperty _texture2DMaskBoolProperty;
        /// <summary>
        /// The property for the Texture2D mask Texture2D source
        /// </summary>
        private SerializedProperty _texture2DMaskTextureProperty;
        /// <summary>
        /// The property for the Texture2D mask transform
        /// </summary>
        private SerializedProperty _texture2DMaskTransformProperty;
        /// <summary>
        /// The property for enabling Texture3D mask
        /// </summary>
        private SerializedProperty _texture3DMaskBoolProperty;
        /// <summary>
        /// The property for the Texture3D mask Texture3D source
        /// </summary>
        private SerializedProperty _texture3DMaskTextureProperty;
        /// <summary>
        /// The property for the Texture3D mask transform
        /// </summary>
        private SerializedProperty _texture3DMaskTransformProperty;
        /// <summary>
        /// The property for enabling noise mask
        /// </summary>
        private SerializedProperty _noiseMaskBoolProperty;
        /// <summary>
        /// The property for the noise mask speed
        /// </summary>
        private SerializedProperty _noiseMaskSpeedProperty;
        /// <summary>
        /// The property for the nois mask transform
        /// </summary>
        private SerializedProperty _noiseMaskTransformProperty;
        /// <summary>
        /// The property for enabling density injection
        /// </summary>
        private SerializedProperty _densityInjectionBoolProperty;
        /// <summary>
        /// The property for the density injection parameters
        /// </summary>
        private SerializedProperty _densityInjectionParametersProperty;
        /// <summary>
        /// The property for enabling color injection
        /// </summary>
        private SerializedProperty _lightInjectionBoolProperty;
        /// <summary>
        /// The property for the color injection color
        /// </summary>
        private SerializedProperty _lightInjectionColorProperty;
        /// <summary>
        /// The property for the color injection parameters
        /// </summary>
        private SerializedProperty _lightInjectionParametersProperty;
        /// <summary>
        /// The property for enabling tint injection
        /// </summary>
        private SerializedProperty _tintInjectionBoolProperty;
        /// <summary>
        /// The property for the tint injection
        /// </summary>
        private SerializedProperty _tintInjectionColorProperty;
        /// <summary>
        /// The property for the tint injection parameters
        /// </summary>
        private SerializedProperty _tintInjectionParametersProperty;
        /// <summary>
        /// The property for enabling scattering injection
        /// </summary>
        private SerializedProperty _scatteringInjectionBoolProperty;
        /// <summary>
        /// The property for scattering injection parameters
        /// </summary>
        private SerializedProperty _scatteringInjectionParametersProperty;
        /// <summary>
        /// The property for enabling ambient lighting injection
        /// </summary>
        private SerializedProperty _ambientInjectionBoolProperty;
        /// <summary>
        /// The property for ambient lighting injection parameters
        /// </summary>
        private SerializedProperty _ambientInjectionParametersProperty;
        /// <summary>
        /// The property for enabling boost injection
        /// </summary>
        private SerializedProperty _boostInjectionBoolProperty;
        /// <summary>
        /// The property for boost injection parameters
        /// </summary>
        private SerializedProperty _boostInjectionParametersProperty;
        /// <summary>
        /// The current inspected Aura Volume
        /// </summary>
        private AuraVolume _currentVolume;
        #endregion

        #region Properties
        /// <summary>
        /// Is Alt key pressed?
        /// </summary>
        private bool AltKey
        {
            get
            {
                return Event.current.alt;
            }
        }
        #endregion

        #region Undocumented base class functions (https://docs.unity3d.com/ScriptReference/Editor.html)
        /// <summary>
        /// Tells the editor if the object has custom bounds to use when object is focussed in the scene view
        /// </summary>
        /// <returns>Yes it has bound or no</returns>
        private bool HasFrameBounds()
        {
            return true;
        }

        /// <summary>
        /// The custom bounds to use when object is focussed in the scene view
        /// </summary>
        /// <returns>The custom bounds</returns>
        private Bounds OnGetFrameBounds()
        {
            return _currentVolume.Bounds;
        }
        #endregion

        #region Overriden base class functions (https://docs.unity3d.com/ScriptReference/Editor.html)
        private void OnEnable()
        {
            _volumeShapeProperty = serializedObject.FindProperty("volumeShape.shape");
            _falloffFadeProperty = serializedObject.FindProperty("volumeShape.fading.falloffExponent");
            _xPositiveCubeFadeProperty = serializedObject.FindProperty("volumeShape.fading.xPositiveCubeFade");
            _xNegativeCubeFadeProperty = serializedObject.FindProperty("volumeShape.fading.xNegativeCubeFade");
            _yPositiveCubeFadeProperty = serializedObject.FindProperty("volumeShape.fading.yPositiveCubeFade");
            _yNegativeCubeFadeProperty = serializedObject.FindProperty("volumeShape.fading.yNegativeCubeFade");
            _zPositiveCubeFadeProperty = serializedObject.FindProperty("volumeShape.fading.zPositiveCubeFade");
            _zNegativeCubeFadeProperty = serializedObject.FindProperty("volumeShape.fading.zNegativeCubeFade");
            _angularConeFadeProperty = serializedObject.FindProperty("volumeShape.fading.angularConeFade");
            _distanceConeFadeProperty = serializedObject.FindProperty("volumeShape.fading.distanceConeFade");
            _widthCylinderFadeProperty = serializedObject.FindProperty("volumeShape.fading.widthCylinderFade");
            _yNegativeCylinderFadeProperty = serializedObject.FindProperty("volumeShape.fading.yNegativeCylinderFade");
            _yPositiveCylinderFadeProperty = serializedObject.FindProperty("volumeShape.fading.yPositiveCylinderFade");
            _distanceSphereFadeProperty = serializedObject.FindProperty("volumeShape.fading.distanceSphereFade");
            _useAsLightProbesProxyVolumeProperty = serializedObject.FindProperty("useAsLightProbesProxyVolume");
            _lightProbesMultiplierProperty = serializedObject.FindProperty("lightProbesMultiplier");
            _texture2DMaskBoolProperty = serializedObject.FindProperty("texture2DMask.enable");
            _texture2DMaskTextureProperty = serializedObject.FindProperty("texture2DMask.texture");
            _texture2DMaskTransformProperty = serializedObject.FindProperty("texture2DMask.transform");
            _texture3DMaskBoolProperty = serializedObject.FindProperty("texture3DMask.enable");
            _texture3DMaskTextureProperty = serializedObject.FindProperty("texture3DMask.texture");
            _texture3DMaskTransformProperty = serializedObject.FindProperty("texture3DMask.transform");
            _noiseMaskBoolProperty = serializedObject.FindProperty("noiseMask.enable");
            _noiseMaskSpeedProperty = serializedObject.FindProperty("noiseMask.speed");
            _noiseMaskTransformProperty = serializedObject.FindProperty("noiseMask.transform");

            _densityInjectionTitleContent = new GUIContent("Density", Aura.ResourcesCollection.densityIconTexture);
            _densityInjectionBoolProperty = serializedObject.FindProperty("densityInjection.enable");
            _densityInjectionParametersProperty = serializedObject.FindProperty("densityInjection");

            _scatteringInjectionTitleContent = new GUIContent("Scattering", Aura.ResourcesCollection.scatteringIconTexture);
            _scatteringInjectionBoolProperty = serializedObject.FindProperty("scatteringInjection.enable");
            _scatteringInjectionParametersProperty = serializedObject.FindProperty("scatteringInjection");

            _lightInjectionTitleContent = new GUIContent("Light", Aura.ResourcesCollection.illuminationColorIconTexture);
            _lightInjectionBoolProperty = serializedObject.FindProperty("lightInjection.injectionParameters.enable");
            _lightInjectionColorProperty = serializedObject.FindProperty("lightInjection.color");
            _lightInjectionParametersProperty = serializedObject.FindProperty("lightInjection.injectionParameters");

            _tintInjectionTitleContent = new GUIContent("Tint", Aura.ResourcesCollection.tintIconTexture);
            _tintInjectionBoolProperty = serializedObject.FindProperty("tintInjection.injectionParameters.enable");
            _tintInjectionColorProperty = serializedObject.FindProperty("tintInjection.color");
            _tintInjectionParametersProperty = serializedObject.FindProperty("tintInjection.injectionParameters");

            _ambientInjectionTitleContent = new GUIContent("Ambient Lighting", Aura.ResourcesCollection.illuminationIconTexture);
            _ambientInjectionBoolProperty = serializedObject.FindProperty("ambientInjection.enable");
            _ambientInjectionParametersProperty = serializedObject.FindProperty("ambientInjection");

            _boostInjectionTitleContent = new GUIContent("Boost", Aura.ResourcesCollection.boostIconTexture);
            _boostInjectionBoolProperty = serializedObject.FindProperty("boostInjection.enable");
            _boostInjectionParametersProperty = serializedObject.FindProperty("boostInjection");

            _currentVolume = (AuraVolume)target;
        }

        private void OnSceneGUI()
        {
            Color tmp = Handles.color;
            Handles.color = CustomGizmo.color;

            float handleSize = HandleUtility.GetHandleSize(_currentVolume.transform.position) * 0.05f;

            switch (_currentVolume.volumeShape.shape)
            {
                case VolumeType.Layer:
                    {
                        // Up
                        DisplayResizeHandle(handleSize, 1.0f, _currentVolume.transform.up, 1, true);
                    }
                    break;

                case VolumeType.Box:
                case VolumeType.Sphere:
                case VolumeType.Cylinder:
                    {
                        // Right
                        DisplayResizeHandle(handleSize, 0.5f, _currentVolume.transform.right, 0, false);
                        // Left
                        DisplayResizeHandle(handleSize, 0.5f, -_currentVolume.transform.right, 0, false);
                        // Up
                        DisplayResizeHandle(handleSize, 0.5f, _currentVolume.transform.up, 1, false);
                        // Down
                        DisplayResizeHandle(handleSize, 0.5f, -_currentVolume.transform.up, 1, false);
                        // Front
                        DisplayResizeHandle(handleSize, 0.5f, _currentVolume.transform.forward, 2, false);
                        // Back
                        DisplayResizeHandle(handleSize, 0.5f, -_currentVolume.transform.forward, 2, false);
                    }
                    break;

                case VolumeType.Cone: // TODO : Better sides' handle positions
                    {
                        // Right
                        DisplayResizeHandle(handleSize, 0.5f, _currentVolume.transform.right, 0, false);
                        // Left
                        DisplayResizeHandle(handleSize, 0.5f, -_currentVolume.transform.right, 0, false);
                        // Up
                        DisplayResizeHandle(handleSize, 0.5f, _currentVolume.transform.up, 1, false);
                        // Down
                        DisplayResizeHandle(handleSize, 0.5f, -_currentVolume.transform.up, 1, false);
                        // Forward
                        DisplayResizeHandle(handleSize, 1.0f, _currentVolume.transform.forward, 2, true);
                    }
                    break;
            }

            Handles.color = tmp;
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.Separator();
            EditorGUILayout.Separator();

            EditorGUILayout.BeginVertical(GuiStyles.ButtonNoHover);

            EditorGUILayout.BeginHorizontal(GuiStyles.BackgroundNoBorder);
            GUILayout.Space(24);
            EditorGUILayout.LabelField(new GUIContent(" Aura <b>Volume</b>", Aura.ResourcesCollection.volumeUiIconTexture), new GUIStyle(GuiStyles.LabelCenteredBig) { fontSize = 24 });
            if (GUILayout.Button(new GUIContent(Aura.ResourcesCollection.questionIconTexture, "Show Help"), GuiStyles.ButtonImageOnlyNoBorder, GUILayout.MaxWidth(24), GUILayout.MaxHeight(24)))
            {
                AuraEditorPrefs.DisplayVolumeIntroductionScreen = true;
                SceneView.RepaintAll();
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Separator();

            EditorGUILayout.BeginVertical(GuiStyles.Background);

            DisplaySettingsArea();

            EditorGUILayout.Separator();
            EditorGUILayout.Separator();
            EditorGUILayout.Separator();


            EditorGUILayout.BeginVertical(GuiStyles.ButtonNoHover);

            EditorGUILayout.Separator();
            GUILayout.Label(new GUIContent(" Data", Aura.ResourcesCollection.injectionIconTexture), new GUIStyle(GuiStyles.LabelBoldCenteredBig) { fontSize = 15 });
            EditorGUILayout.Separator();

            DisplayDensityInjectionArea();

            EditorGUILayout.Separator();
            EditorGUILayout.Separator();
            EditorGUILayout.Separator();

            DisplayTintInjectionArea();

            EditorGUILayout.Separator();
            EditorGUILayout.Separator();
            EditorGUILayout.Separator();

            DisplayScatteringInjectionArea();

            EditorGUILayout.Separator();
            EditorGUILayout.Separator();
            EditorGUILayout.Separator();

            DisplayBoostInjectionArea();

            EditorGUILayout.Separator();
            EditorGUILayout.Separator();
            EditorGUILayout.Separator();

            DisplayLightInjectionArea();

            EditorGUILayout.Separator();
            EditorGUILayout.Separator();
            EditorGUILayout.Separator();

            DisplayAmbientInjectionArea();

            EditorGUILayout.Separator();
            EditorGUILayout.Separator();
            EditorGUILayout.Separator();

            DisplayLightProbesLightingInjectionArea();

            EditorGUILayout.Separator();
            EditorGUILayout.Separator();

            EditorGUILayout.EndVertical();

            EditorGUILayout.EndVertical();

            EditorGUILayout.Separator();
            GuiHelpers.DisplayHelpToShowHelpBox();

            EditorGUILayout.EndVertical();

            EditorGUILayout.Separator();
            EditorGUILayout.Separator();

            serializedObject.ApplyModifiedProperties();
        }
        #endregion

        #region Functions
        /// <summary>
        /// Displays the resize handle manipulator for box enclosed and transform centered volume
        /// </summary>
        /// <param name="handleSize">The size of the handle</param>
        /// <param name="handleUnscaledDistanceFromPivot">The normalized distance to draw the handle</param>
        /// <param name="axis">The direction of resizing</param>
        /// <param name="componentMask">The axis of resizing (same as direction) x=0, y=1, z=2</param>
        private void DisplayResizeHandle(float handleSize, float handleUnscaledDistanceFromPivot, Vector3 axis, int componentMask, bool resizeAroundPivot)
        {
            float currentScale;
            if (componentMask == 0)
            {
                currentScale = _currentVolume.transform.localScale.x;
            }
            else if (componentMask == 1)
            {
                currentScale = _currentVolume.transform.localScale.y;
            }
            else
            {
                currentScale = _currentVolume.transform.localScale.z;
            }

            Vector3 initialHandlePosition = axis * currentScale * handleUnscaledDistanceFromPivot;
            EditorGUI.BeginChangeCheck();
            Vector3 modifiedHandlePosition = Handles.Slider(_currentVolume.transform.position + initialHandlePosition, axis, handleSize, Handles.DotHandleCap, 0.5f) - _currentVolume.transform.position;
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(_currentVolume, "Resize " + _currentVolume.name); // Unity is, as of 2017.2, unable to track transforms' changes, I'll leave it in case someday it does
                float scaleModifier = modifiedHandlePosition.magnitude / initialHandlePosition.magnitude * Vector3.Dot(modifiedHandlePosition.normalized, initialHandlePosition.normalized);
                if (!AltKey && !resizeAroundPivot)
                {
                    scaleModifier = (1.0f + scaleModifier) * 0.5f;
                }

                Vector3 localScale = _currentVolume.transform.localScale;
                if (componentMask == 0)
                {
                    localScale.x *= scaleModifier;
                }
                else if (componentMask == 1)
                {
                    localScale.y *= scaleModifier;
                }
                else
                {
                    localScale.z *= scaleModifier;
                }
                _currentVolume.transform.localScale = localScale;

                if (!AltKey && !resizeAroundPivot)
                {
                    Vector3 delta = modifiedHandlePosition - initialHandlePosition;
                    Vector3 position = _currentVolume.transform.position;
                    position += delta * 0.5f;
                    _currentVolume.transform.position = position;
                }
            }
        }

        /// <summary>
        /// Displays Settings Tab
        /// </summary>
        private void DisplaySettingsArea()
        {
            EditorGUILayout.BeginVertical(GuiStyles.ButtonNoHover);

            EditorGUILayout.Separator();
            GUILayout.Label(new GUIContent(" Masks", Aura.ResourcesCollection.shapeIconTexture), new GUIStyle(GuiStyles.LabelBoldCenteredBig) { fontSize = 15 });
            EditorGUILayout.Separator();

            DisplayShapeSettingsArea();

            EditorGUILayout.Separator();
            EditorGUILayout.Separator();
            EditorGUILayout.Separator();

            DisplayNoiseMaskArea();

            EditorGUILayout.Separator();
            EditorGUILayout.Separator();
            EditorGUILayout.Separator();

            DisplayTexture2DMaskArea();

            EditorGUILayout.Separator();
            EditorGUILayout.Separator();
            EditorGUILayout.Separator();

            DisplayTexture3DMaskArea();

            EditorGUILayout.Separator();
            EditorGUILayout.Separator();

            EditorGUILayout.EndVertical();
        }

        /// <summary>
        /// Displays Shape Settings Area
        /// </summary>
        private void DisplayShapeSettingsArea()
        {
            EditorGUILayout.BeginVertical();
            GuiHelpers.DrawContextualHelpBox("The \"Shape\" parameter allows you to define the volumetric shape of the volume used for injecting Density, Color or Scattering.\n\nYou will also be able to parameter the fading on the borders of the shape, allowing a smooth transition between the inside and the outside of the volume.");
            EditorGUILayout.Separator();
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Shape", GuiStyles.Label, GUILayout.MaxWidth(160));
            EditorGUILayout.PropertyField(_volumeShapeProperty, new GUIContent(""));
            EditorGUILayout.EndHorizontal();

            if ((VolumeType)_volumeShapeProperty.enumValueIndex != VolumeType.Global)
            {
                switch ((VolumeType)_volumeShapeProperty.enumValueIndex)
                {
                    case VolumeType.Box:
                        {
                            EditorGUILayout.Separator();
                            EditorGUILayout.BeginHorizontal();
                            GUILayout.Label("Width Fade", GuiStyles.Label, GUILayout.MaxWidth(160));
                            GuiHelpers.DrawMinMaxSlider(ref _xNegativeCubeFadeProperty, ref _xPositiveCubeFadeProperty, 0, 1, true);
                            EditorGUILayout.EndHorizontal();
                            EditorGUILayout.Separator();
                            EditorGUILayout.BeginHorizontal();
                            GUILayout.Label("Height Fade", GuiStyles.Label, GUILayout.MaxWidth(160));
                            GuiHelpers.DrawMinMaxSlider(ref _yNegativeCubeFadeProperty, ref _yPositiveCubeFadeProperty, 0, 1, true);
                            EditorGUILayout.EndHorizontal();
                            EditorGUILayout.Separator();
                            EditorGUILayout.BeginHorizontal();
                            GUILayout.Label("Depth Fade", GuiStyles.Label, GUILayout.MaxWidth(160));
                            GuiHelpers.DrawMinMaxSlider(ref _zNegativeCubeFadeProperty, ref _zPositiveCubeFadeProperty, 0, 1, true);
                            EditorGUILayout.EndHorizontal();
                        }
                        break;

                    case VolumeType.Cone:
                        {
                            EditorGUILayout.Separator();
                            EditorGUILayout.BeginHorizontal();
                            GUILayout.Label("Angular Fade", GuiStyles.Label, GUILayout.MaxWidth(160));
                            GuiHelpers.DrawSlider(ref _angularConeFadeProperty, 0, 1, true);
                            EditorGUILayout.EndHorizontal();
                            EditorGUILayout.Separator();
                            EditorGUILayout.BeginHorizontal();
                            GUILayout.Label("Distance Fade", GuiStyles.Label, GUILayout.MaxWidth(160));
                            GuiHelpers.DrawSlider(ref _distanceConeFadeProperty, 0, 1, true);
                            EditorGUILayout.EndHorizontal();
                        }
                        break;

                    case VolumeType.Cylinder:
                        {
                            EditorGUILayout.Separator();
                            EditorGUILayout.BeginHorizontal();
                            GUILayout.Label("Width Fade", GuiStyles.Label, GUILayout.MaxWidth(160));
                            GuiHelpers.DrawSlider(ref _widthCylinderFadeProperty, 0, 1, true);
                            EditorGUILayout.EndHorizontal();
                            EditorGUILayout.Separator();
                            EditorGUILayout.BeginHorizontal();
                            GUILayout.Label("Height Fade", GuiStyles.Label, GUILayout.MaxWidth(160));
                            GuiHelpers.DrawMinMaxSlider(ref _yNegativeCylinderFadeProperty, ref _yPositiveCylinderFadeProperty, 0, 1, true);
                            EditorGUILayout.EndHorizontal();
                        }
                        break;

                    case VolumeType.Sphere:
                        {
                            EditorGUILayout.Separator();
                            EditorGUILayout.BeginHorizontal();
                            GUILayout.Label("Distance Fade", GuiStyles.Label, GUILayout.MaxWidth(160));
                            GuiHelpers.DrawSlider(ref _distanceSphereFadeProperty, 0, 1, true);
                            EditorGUILayout.EndHorizontal();
                        }
                        break;
                }

                EditorGUILayout.Separator();
                
                GuiHelpers.DrawPositiveOnlyFloatField(ref _falloffFadeProperty, new GUIContent("Fade Falloff", "The curve of the fading"));
            }
            EditorGUILayout.EndVertical();
        }

        /// <summary>
        /// Displays Noise Mask Area
        /// </summary>
        private void DisplayNoiseMaskArea()
        {
            EditorGUILayout.BeginVertical(GuiStyles.Background);
            GuiHelpers.DrawContextualHelpBox("The \"Noise Mask\" parameter allows you to assign a dynamic morphing noise mask to the volume.\nThis noise will be used for masking the data injected.");
            GuiHelpers.DrawToggleChecker(ref _noiseMaskBoolProperty, new GUIContent("Noise Mask", Aura.ResourcesCollection.noiseIconTexture), true, true);
            if (_noiseMaskBoolProperty.boolValue)
            {
                //EditorGUI.BeginDisabledGroup(!_noiseMaskBoolProperty.boolValue);
                EditorGUILayout.BeginVertical(GuiStyles.EmptyMiddleAligned);
                GuiHelpers.DrawFloatField(ref _noiseMaskSpeedProperty, new GUIContent("Speed", "Speed of the morph"));
                EditorGUILayout.Separator();
                EditorGUILayout.Separator();
                GuiHelpers.DrawTransformField(ref _noiseMaskTransformProperty);
                EditorGUILayout.EndVertical();
                //EditorGUI.EndDisabledGroup();
            }
            EditorGUILayout.EndVertical();
        }

        /// <summary>
        /// Displays Texture2D Mask Area
        /// </summary>
        private void DisplayTexture2DMaskArea()
        {
            EditorGUILayout.BeginVertical(GuiStyles.Background);
            GuiHelpers.DrawToggleChecker(ref _texture2DMaskBoolProperty, new GUIContent("Texture2D Mask", Aura.ResourcesCollection.textureIconTexture, "The \"Texture Mask\" allows to assign a texture mask to the volume.\nRGB -> Will multiply the \"Strength\" parameter of the Color Injection.\nA -> Will multiply the \"Strength\" parameter of the Density and Scattering Injection"), true, true);
            if (_texture2DMaskBoolProperty.boolValue)
            {
                //EditorGUI.BeginDisabledGroup(!_texture2DMaskBoolProperty.boolValue);
                EditorGUILayout.BeginVertical(GuiStyles.EmptyMiddleAligned);
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Texture2D Mask", GuiStyles.Label, GUILayout.MaxWidth(100));
                EditorGUI.BeginChangeCheck();
                EditorGUILayout.PropertyField(_texture2DMaskTextureProperty, new GUIContent(""));
                EditorGUILayout.EndHorizontal();
                Texture2D textureMask = (Texture2D)_texture2DMaskTextureProperty.objectReferenceValue;
                if(textureMask != null)
                {
                    if (textureMask.width != VolumesCommonDataManager.texture2DMaskSize || textureMask.height != VolumesCommonDataManager.texture2DMaskSize)
                    {
                        GuiHelpers.DrawHelpBox("The Texture2D mask \"" + textureMask.name + "\" of \"" + _currentVolume.name + "\" volume is not of the required size, which is " + VolumesCommonDataManager.texture2DMaskSize + "²", HelpBoxType.Warning);
                    }
                }
                EditorGUILayout.Separator();
                EditorGUILayout.Separator();
                GuiHelpers.DrawTransformField(ref _texture2DMaskTransformProperty);
                EditorGUILayout.EndVertical();
                //EditorGUI.EndDisabledGroup();
            }
            EditorGUILayout.EndVertical();
        }

        /// <summary>
        /// Displays Texture3D Mask Area
        /// </summary>
        private void DisplayTexture3DMaskArea()
        {
            EditorGUILayout.BeginVertical(GuiStyles.Background);
            GuiHelpers.DrawToggleChecker(ref _texture3DMaskBoolProperty, new GUIContent("Texture3D Mask", Aura.ResourcesCollection.texture3DIconTexture, "The \"Texture Mask\" allows to assign a texture mask to the volume.\nRGB -> Will multiply the \"Strength\" parameter of the Color Injection.\nA -> Will multiply the \"Strength\" parameter of the Density and Scattering Injection"), true, true);
            if (_texture3DMaskBoolProperty.boolValue)
            {
                //EditorGUI.BeginDisabledGroup(!_texture3DMaskBoolProperty.boolValue);
                EditorGUILayout.BeginVertical(GuiStyles.EmptyMiddleAligned);
                GUILayout.Label(new GUIContent(" Experimental Feature (PREVIEW)", Aura.ResourcesCollection.experimentalIconTexture), GuiStyles.LabelBoldCentered);
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Texture3D Mask", GuiStyles.Label, GUILayout.MaxWidth(100));
                EditorGUILayout.PropertyField(_texture3DMaskTextureProperty, new GUIContent(""));
                EditorGUILayout.EndHorizontal();
                Texture3D textureMask = (Texture3D)_texture3DMaskTextureProperty.objectReferenceValue;
                if (textureMask != null)
                {
                    if (textureMask.width != VolumesCommonDataManager.texture3DMaskSize || textureMask.height != VolumesCommonDataManager.texture3DMaskSize || textureMask.depth != VolumesCommonDataManager.texture3DMaskSize)
                    {
                        GuiHelpers.DrawHelpBox("The Texture3D mask \"" + textureMask.name + "\" of \"" + _currentVolume.name + "\" volume is not of the required size, which is " + VolumesCommonDataManager.texture3DMaskSize + "³", HelpBoxType.Warning);
                    }
                }
                EditorGUILayout.Separator();
                EditorGUILayout.Separator();
                GuiHelpers.DrawTransformField(ref _texture3DMaskTransformProperty);
                EditorGUILayout.EndVertical();
                //EditorGUI.EndDisabledGroup();
            }
            EditorGUILayout.EndVertical();
        }

        /// <summary>
        /// Displays Density Injection area
        /// </summary>
        private void DisplayDensityInjectionArea()
        {
            EditorGUILayout.BeginVertical(GuiStyles.Background);
            GuiHelpers.DrawToggleChecker(ref _densityInjectionBoolProperty, _densityInjectionTitleContent, true, true);
            if (_densityInjectionBoolProperty.boolValue)
            {
                //EditorGUI.BeginDisabledGroup(!_densityInjectionBoolProperty.boolValue);
                EditorGUILayout.BeginVertical(GuiStyles.EmptyMiddleAligned);
                GuiHelpers.DrawInjectionField(ref _densityInjectionParametersProperty, _noiseMaskBoolProperty.boolValue, _currentVolume.UsesTexture2DMasking, _currentVolume.UsesTexture3DMasking);
                EditorGUILayout.Separator();
                EditorGUILayout.EndVertical();
                //EditorGUI.EndDisabledGroup();
            }
            EditorGUILayout.EndVertical();
        }

        /// <summary>
        /// Displays Light Injection area
        /// </summary>
        private void DisplayLightInjectionArea()
        {
            EditorGUILayout.BeginVertical(GuiStyles.Background);
            GuiHelpers.DrawContextualHelpBox("The \"Light Injection\" parameters allows you to add/remove color inside the system.\n\nIn other words, you will be able to add/remove light inside a defined area.\n TIP :The \"Strength\" parameter will accept negative values. Meaning that you will be able to remove Color.");
            GuiHelpers.DrawToggleChecker(ref _lightInjectionBoolProperty, _lightInjectionTitleContent, true, true);
            if (_lightInjectionBoolProperty.boolValue)
            {
                //EditorGUI.BeginDisabledGroup(!_lightInjectionBoolProperty.boolValue);
                EditorGUILayout.BeginVertical(GuiStyles.EmptyMiddleAligned);
                EditorGUILayout.PropertyField(_lightInjectionColorProperty);
                GuiHelpers.DrawInjectionField(ref _lightInjectionParametersProperty, _noiseMaskBoolProperty.boolValue, _currentVolume.UsesTexture2DMasking, _currentVolume.UsesTexture3DMasking);
                EditorGUILayout.Separator();
                EditorGUILayout.EndVertical();
                //EditorGUI.EndDisabledGroup();
            }
            EditorGUILayout.EndVertical();
        }

        /// <summary>
        /// Displays Tint Injection area
        /// </summary>
        private void DisplayTintInjectionArea()
        {
            EditorGUILayout.BeginVertical(GuiStyles.Background);
            GuiHelpers.DrawToggleChecker(ref _tintInjectionBoolProperty, _tintInjectionTitleContent, true, true);
            if (_tintInjectionBoolProperty.boolValue)
            {
                EditorGUILayout.BeginVertical(GuiStyles.EmptyMiddleAligned);
                EditorGUILayout.PropertyField(_tintInjectionColorProperty);
                GuiHelpers.DrawInjectionField(ref _tintInjectionParametersProperty, _noiseMaskBoolProperty.boolValue, _currentVolume.UsesTexture2DMasking, _currentVolume.UsesTexture3DMasking, true, 0, 1, true, "Brightness", "Brightness of the tinting color");
                EditorGUILayout.Separator();
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.EndVertical();
        }

        /// <summary>
        /// Displays Scattering Injection area
        /// </summary>
        private void DisplayScatteringInjectionArea()
        {
            EditorGUILayout.BeginVertical(GuiStyles.Background);
            GuiHelpers.DrawContextualHelpBox("The \"Scattering Injection\" parameters allows you to add/remove scattering inside the system.\n\nIn other words, you will be able to modify how light from light sources will bounce inside the micro particles and will be deviated by them. Typically, how \"wet\" the micro particles are.\n TIP :The \"Strength\" parameter will accept negative values. Meaning that you will be able to remove scattering as well.");
            GuiHelpers.DrawToggleChecker(ref _scatteringInjectionBoolProperty, _scatteringInjectionTitleContent, true, true);
            if (_scatteringInjectionBoolProperty.boolValue)
            {
                //EditorGUI.BeginDisabledGroup(!_scatteringInjectionBoolProperty.boolValue);
                EditorGUILayout.BeginVertical(GuiStyles.EmptyMiddleAligned);
                GuiHelpers.DrawInjectionField(ref _scatteringInjectionParametersProperty, _noiseMaskBoolProperty.boolValue, _currentVolume.UsesTexture2DMasking, _currentVolume.UsesTexture3DMasking);
                EditorGUILayout.Separator();
                EditorGUILayout.EndVertical();
                //EditorGUI.EndDisabledGroup();
            }
            EditorGUILayout.EndVertical();
        }

        /// <summary>
        /// Displays Boost Injection area
        /// </summary>
        private void DisplayBoostInjectionArea()
        {
            EditorGUILayout.BeginVertical(GuiStyles.Background);
            GuiHelpers.DrawToggleChecker(ref _boostInjectionBoolProperty, _boostInjectionTitleContent, true, true);
            if (_boostInjectionBoolProperty.boolValue)
            {
                EditorGUILayout.BeginVertical(GuiStyles.EmptyMiddleAligned);
                GuiHelpers.DrawInjectionField(ref _boostInjectionParametersProperty, _noiseMaskBoolProperty.boolValue, _currentVolume.UsesTexture2DMasking, _currentVolume.UsesTexture3DMasking);
                EditorGUILayout.Separator();
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.EndVertical();
        }

        /// <summary>
        /// Displays Ambient Lighting Injection area
        /// </summary>
        private void DisplayAmbientInjectionArea()
        {
            EditorGUILayout.BeginVertical(GuiStyles.Background);
            GuiHelpers.DrawToggleChecker(ref _ambientInjectionBoolProperty, _ambientInjectionTitleContent, true, true);
            if (_ambientInjectionBoolProperty.boolValue)
            {
                EditorGUILayout.BeginVertical(GuiStyles.EmptyMiddleAligned);
                GuiHelpers.DrawInjectionField(ref _ambientInjectionParametersProperty, _noiseMaskBoolProperty.boolValue, _currentVolume.UsesTexture2DMasking, _currentVolume.UsesTexture3DMasking);
                EditorGUILayout.Separator();
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.EndVertical();
        }

        /// <summary>
        /// Displays Light Probes settings area
        /// </summary>
        private void DisplayLightProbesLightingInjectionArea()
        {
            EditorGUILayout.BeginVertical(GuiStyles.Background);
            GuiHelpers.DrawContextualHelpBox("These parameters allow to enable the volume to inject the baked light probes illumination and tweak the strength of the injection.");
            GuiHelpers.DrawToggleChecker(ref _useAsLightProbesProxyVolumeProperty, new GUIContent("Light Probes Lighting", Aura.ResourcesCollection.lightProbesIconTexture), true, true);
            if (_useAsLightProbesProxyVolumeProperty.boolValue)
            {
                GUILayout.Label(new GUIContent(" Experimental Feature (PREVIEW)", Aura.ResourcesCollection.experimentalIconTexture), GuiStyles.LabelBoldCentered);
                //EditorGUI.BeginDisabledGroup(!_useAsLightProbesProxyVolumeProperty.boolValue);
                EditorGUILayout.BeginVertical(GuiStyles.EmptyMiddleAligned);
                GuiHelpers.DrawPositiveOnlyFloatField(ref _lightProbesMultiplierProperty, new GUIContent("Strength", "Multiplies the injected lighting"));
                EditorGUILayout.EndVertical();
                //EditorGUI.EndDisabledGroup();
            }
            EditorGUILayout.EndVertical();
        }
        #endregion
    }
}
