
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
    /// Custom Inspector for Aura class
    /// </summary>
    [CustomEditor(typeof(AuraQualitySettings))]
    public class AuraQualitySettingsEditor : Editor
    {
        #region Private Members
        /// <summary>
        /// The property for showing the volumetric lighting only
        /// </summary>
        private static SerializedProperty _debugVolumetricLightingProperty;
        /// <summary>
        /// The property for the frustum grid resolution
        /// </summary>
        private static SerializedProperty _frustumGridResolutionProperty;
        /// <summary>
        /// The property for enabling automatic resizing in stereo mode
        /// </summary>
        private static SerializedProperty _enableAutomaticStereoResizingProperty;
        /// <summary>
        /// The property for grid far plane
        /// </summary>
        private static SerializedProperty _gridSettingsFarPlaneProperty;
        /// <summary>
        /// The property for the slices' depth bias
        /// </summary>
        private static SerializedProperty _depthBiasCoefficientProperty;
        /// <summary>
        /// The property for enabling volumes injection
        /// </summary>
        private static SerializedProperty _enableVolumesProperty;
        /// <summary>
        /// The property for enabling Texture2D masks
        /// </summary>
        private static SerializedProperty _enableVolumesTexture2DMaskProperty;
        /// <summary>
        /// The property for enabling Texture3D masks
        /// </summary>
        private static SerializedProperty _enableVolumesTexture3DMaskProperty;
        /// <summary>
        /// The property for noise masks
        /// </summary>
        private static SerializedProperty _enableVolumesNoiseMaskProperty;
        /// <summary>
        /// The property for enabling ambient lighting
        /// </summary>
        private static SerializedProperty _enableAmbientLightingProperty;
        /// <summary>
        /// The property for enabling light probes
        /// </summary>
        private static SerializedProperty _enableLightProbesProperty;
        /// <summary>
        /// The property for the light probes proxy volume resolution
        /// </summary>
        //private static SerializedProperty _lightProbesProxyGridResolutionProperty;
        /// <summary>
        /// The property for enabling Directional Lights
        /// </summary>
        private static SerializedProperty _enableDirectionalLightsProperty;
        /// <summary>
        /// The property for enabling Directional Lights shadows
        /// </summary>
        private static SerializedProperty _enableDirectionalLightsShadowsProperty;
        /// <summary>
        /// The property for enabling Spot Lights
        /// </summary>
        private static SerializedProperty _enableSpotLightsProperty;
        /// <summary>
        /// The property for enabling Spot Lights shadows
        /// </summary>
        private static SerializedProperty _enableSpotLightsShadowsProperty;
        /// <summary>
        /// The property for enabling Point Lights
        /// </summary>
        private static SerializedProperty _enablePointLightsProperty;
        /// <summary>
        /// The property for enabling Point Lights shadows
        /// </summary>
        private static SerializedProperty _enablePointLightsShadowsProperty;
        /// <summary>
        /// The property for enabling Lights cookies
        /// </summary>
        private static SerializedProperty _enableLightsCookiesProperty;
        /// <summary>
        /// The property for enabling per cell Occlusion Culling
        /// </summary>
        private static SerializedProperty _enableOcclusionCullingProperty;
        /// <summary>
        /// The property for enabling occlusion culling debug
        /// </summary>
        private static SerializedProperty _debugOcclusionCullingProperty;
        /// <summary>
        /// The property for adjusting per cell Occlusion Culling search accuracy
        /// </summary>
        private static SerializedProperty _occlusionCullingAccuracyProperty;
        /// <summary>
        /// The property for enabling blue noise dithering
        /// </summary>
        private static SerializedProperty _enableDitheringProperty;
        /// <summary>
        /// The property for Texture3D filtering
        /// </summary>
        private static SerializedProperty _texture3DFilteringProperty;
        /// <summary>
        /// The property for enabling denoising filter on the data texture3D
        /// </summary>
        private static SerializedProperty _enableDenoisingFilterProperty;
        /// <summary>
        /// The property for the range of the denoising filter on the data texture3D
        /// </summary>
        private static SerializedProperty _denoisingFilterRangeProperty;
        /// <summary>
        /// The property for enabling blur filter on the data texture3D
        /// </summary>
        private static SerializedProperty _enableBlurFilterProperty;
        /// <summary>
        /// The property for the range of the blur filter on the data texture3D
        /// </summary>
        private static SerializedProperty _blurFilterRangeProperty;
        /// <summary>
        /// The property for the type of the blur filter on the data texture3D
        /// </summary>
        private static SerializedProperty _blurFilterTypeProperty;
        /// <summary>
        /// The property for the type of the blur filter on the data texture3D
        /// </summary>
        private static SerializedProperty _blurFilterGaussianDeviationProperty;
        /// <summary>
        /// The property for enabling Temporal Reprojection
        /// </summary>
        private static SerializedProperty _enableTemporalReprojectionProperty;
        /// <summary>
        /// The property for adjusting Temporal Reprojection strength
        /// </summary>
        private static SerializedProperty _temporalReprojectionFactorProperty;
        /// <summary>
        /// The current inspected quality settings
        /// </summary>
        private static AuraQualitySettings _current;
        #endregion

        #region Properties
        /// <summary>
        /// Tells if the current quality settings is the one used for the edition visualization
        /// </summary>
        private static bool IsEditionQualitySettings
        {
            get
            {
                return _current == Aura.ResourcesCollection.editionCameraQualitySettings;
            }
        }
        #endregion

        #region Overriden base class functions (https://docs.unity3d.com/ScriptReference/Editor.html)
        private void OnEnable()
        {
            Initialize(serializedObject);
        }

        public override void OnInspectorGUI()
        {
            DrawCustomEditor(serializedObject, null, true, true);
        }
        #endregion

        #region Functions
        private static void Initialize(SerializedObject serializedObject)
        {
            _debugVolumetricLightingProperty = serializedObject.FindProperty("displayVolumetricLightingBuffer");
            _frustumGridResolutionProperty = serializedObject.FindProperty("frustumGridResolution");
            _enableAutomaticStereoResizingProperty = serializedObject.FindProperty("enableAutomaticStereoResizing");
            _gridSettingsFarPlaneProperty = serializedObject.FindProperty("farClipPlaneDistance");
            _depthBiasCoefficientProperty = serializedObject.FindProperty("depthBiasCoefficient");
            _enableVolumesProperty = serializedObject.FindProperty("enableVolumes");
            _enableVolumesTexture2DMaskProperty = serializedObject.FindProperty("enableVolumesTexture2DMask");
            _enableVolumesTexture3DMaskProperty = serializedObject.FindProperty("enableVolumesTexture3DMask");
            _enableVolumesNoiseMaskProperty = serializedObject.FindProperty("enableVolumesNoiseMask");
            _enableAmbientLightingProperty = serializedObject.FindProperty("enableAmbientLighting");
            _enableDirectionalLightsProperty = serializedObject.FindProperty("enableDirectionalLights");
            _enableDirectionalLightsShadowsProperty = serializedObject.FindProperty("enableDirectionalLightsShadows");
            _enableSpotLightsProperty = serializedObject.FindProperty("enableSpotLights");
            _enableSpotLightsShadowsProperty = serializedObject.FindProperty("enableSpotLightsShadows");
            _enablePointLightsProperty = serializedObject.FindProperty("enablePointLights");
            _enablePointLightsShadowsProperty = serializedObject.FindProperty("enablePointLightsShadows");
            _enableLightsCookiesProperty = serializedObject.FindProperty("enableLightsCookies");
            _enableDitheringProperty = serializedObject.FindProperty("enableDithering");
            _texture3DFilteringProperty = serializedObject.FindProperty("texture3DFiltering");
            _enableDenoisingFilterProperty = serializedObject.FindProperty("EXPERIMENTAL_enableDenoisingFilter");
            _denoisingFilterRangeProperty = serializedObject.FindProperty("EXPERIMENTAL_denoisingFilterRange");
            _enableBlurFilterProperty = serializedObject.FindProperty("EXPERIMENTAL_enableBlurFilter");
            _blurFilterRangeProperty = serializedObject.FindProperty("EXPERIMENTAL_blurFilterRange");
            _blurFilterTypeProperty = serializedObject.FindProperty("EXPERIMENTAL_blurFilterType");
            _blurFilterGaussianDeviationProperty = serializedObject.FindProperty("EXPERIMENTAL_blurFilterGaussianDeviation");
            _enableTemporalReprojectionProperty = serializedObject.FindProperty("enableTemporalReprojection");
            _temporalReprojectionFactorProperty = serializedObject.FindProperty("temporalReprojectionFactor");
            _enableOcclusionCullingProperty = serializedObject.FindProperty("enableOcclusionCulling");
            _debugOcclusionCullingProperty = serializedObject.FindProperty("debugOcclusionCulling");
            _occlusionCullingAccuracyProperty = serializedObject.FindProperty("occlusionCullingAccuracy");
            _enableLightProbesProperty = serializedObject.FindProperty("enableLightProbes");
            //_lightProbesProxyGridResolutionProperty = serializedObject.FindProperty("lightProbesProxyGridResolution");
            _current = (AuraQualitySettings)serializedObject.targetObject;
        }

        /// <summary>
        /// Draws the inspector
        /// </summary>
        public static void DrawCustomEditor(SerializedObject serializedObject, AuraCamera auraComponent, bool displayHeaderAndHelpBox, bool forceDisplayEditionSettings)
        {
            serializedObject.Update();

            if (displayHeaderAndHelpBox)
            {
                EditorGUILayout.BeginVertical(GuiStyles.ButtonNoHover);
                
                EditorGUILayout.BeginHorizontal(GuiStyles.BackgroundNoBorder);
                EditorGUILayout.LabelField(new GUIContent(" Aura <b>Quality Settings</b>", Aura.ResourcesCollection.qualitySettingsPresetIconTexture), new GUIStyle(GuiStyles.LabelCenteredBigBackground) { fontSize = 24 });
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.Separator();
            }

            Initialize(serializedObject);

            if(IsEditionQualitySettings && !forceDisplayEditionSettings)
            {
                EditorGUILayout.Separator();
                EditorGUILayout.BeginHorizontal(GuiStyles.Background);
                EditorGUILayout.Separator();
                GUILayout.Label(new GUIContent(" DO NOT DELETE THIS FILE!", Aura.ResourcesCollection.settingsIconTexture), GuiStyles.LabelBoldCenteredBig);
                EditorGUILayout.Separator();
                EditorGUILayout.EndHorizontal();
            }
            else
            {
                DisplaySettingsArea(serializedObject, auraComponent);

                if(displayHeaderAndHelpBox)
                {
                    EditorGUILayout.Separator();
                    GuiHelpers.DisplayHelpToShowHelpBox();
                    EditorGUILayout.EndVertical();

                    EditorGUILayout.Separator();
                    EditorGUILayout.Separator();
                }
            }

            serializedObject.ApplyModifiedProperties();
        }

        /// <summary>
        /// Displays the content of the settings tab
        /// </summary>
        private static void DisplaySettingsArea(SerializedObject serializedObject, AuraCamera auraComponent)
        {
            EditorGUILayout.BeginVertical(GuiStyles.Background);

            EditorGUILayout.BeginVertical(GuiStyles.ButtonNoHover);

            EditorGUILayout.Separator();
            GUILayout.Label(new GUIContent(" Debug", Aura.ResourcesCollection.debugIconTexture), GuiStyles.LabelBoldCenteredBig);
            EditorGUILayout.Separator();

            GuiHelpers.DrawToggleChecker(ref _debugVolumetricLightingProperty, "Display Volumetric Lighting Only");

            EditorGUILayout.Separator();

            GuiHelpers.DrawToggleChecker(ref _debugOcclusionCullingProperty, "Display Occlusion Miss");

            EditorGUILayout.Separator();
            EditorGUILayout.Separator();

            EditorGUILayout.EndVertical();

            EditorGUILayout.Separator();
            EditorGUILayout.Separator();
            EditorGUILayout.Separator();

            EditorGUILayout.BeginVertical(GuiStyles.ButtonNoHover);

            EditorGUILayout.Separator();
            GUILayout.Label(new GUIContent(" Grid", Aura.ResourcesCollection.gridIconTexture), GuiStyles.LabelBoldCenteredBig);
            EditorGUILayout.Separator();

            GuiHelpers.DrawContextualHelpBox("The \"Grid\" parameters allow you to determine the density of cells used to compute the volumetric lighting.\n\nThis cubic grid will be remapped on the frustum (the volume visible to the camera) and will range from the camera's near clip distance to the \"Range\" distance parameter (for performance saving and because behind a certain distance, changes are barely noticeable).");
            
            EditorGUILayout.Separator();
            EditorGUILayout.Separator();

            EditorGUILayout.BeginVertical();
            if (!IsEditionQualitySettings)
            {
                _frustumGridResolutionProperty.FindPropertyRelative("x").intValue = Mathf.Max(8, EditorGUILayout.IntField("Horizontal", _frustumGridResolutionProperty.FindPropertyRelative("x").intValue));
                _frustumGridResolutionProperty.FindPropertyRelative("y").intValue = Mathf.Max(8, EditorGUILayout.IntField("Vertical", _frustumGridResolutionProperty.FindPropertyRelative("y").intValue));
                _frustumGridResolutionProperty.FindPropertyRelative("z").intValue = Mathf.Max(8, EditorGUILayout.IntField("Depth", _frustumGridResolutionProperty.FindPropertyRelative("z").intValue));
                EditorGUILayout.Separator();
                if (GUILayout.Button("Set Resolution", GuiStyles.ButtonBold))
                {
                    //if (IsEditionQualitySettings)
                    //{
                    //    SceneViewVisualization.SetSceneViewVisualizationFrustumGridResolution(_frustumGridResolutionProperty.vector3IntValue);
                    //}
                    //else
                    {
                        _current.SetFrustumGridResolution(_frustumGridResolutionProperty.vector3IntValue);
                    }
                }

                EditorGUILayout.Separator();
                EditorGUILayout.Separator();

                GuiHelpers.DrawToggleChecker(ref _enableAutomaticStereoResizingProperty, new GUIContent("Resize Automatically In Stereo Mode", "Enables the automatic horizontal resizing of the frustum grid resolution when the camera is running in stereo mode\n(halved size for MultiPass, doubled size for SinglePass)"));

                EditorGUILayout.Separator();
                EditorGUILayout.Separator();
            }
            else
            {
                GuiHelpers.DrawHelpBox("Custom grid resolution in SceneView is not currently available", HelpBoxType.Experimental);
                EditorGUILayout.Separator();
                EditorGUILayout.Separator();
            }

            GuiHelpers.DrawPositiveOnlyFloatField(ref _gridSettingsFarPlaneProperty, "Range");
            if (auraComponent != null && _gridSettingsFarPlaneProperty.floatValue < auraComponent.CameraComponent.nearClipPlane)
            {
                GuiHelpers.DrawHelpBox("Range must be bigger than 0", HelpBoxType.Warning);
            }

            EditorGUILayout.Separator();

            GuiHelpers.DrawSlider(ref _depthBiasCoefficientProperty, 0, 1, "Depth Bias", true);

            EditorGUILayout.Separator();
            EditorGUILayout.Separator();    

            GuiHelpers.DrawContextualHelpBox("The \"Occlusion Culling\" allows to compute the maximum visible depth of the frustum grid.\n\nThis leads to avoid computing cells that are invisible to the camera because hidden behind objects.");
            GuiHelpers.DrawToggleChecker(ref _enableOcclusionCullingProperty, "Enable Occlusion Culling");
            EditorGUI.BeginDisabledGroup(!_enableOcclusionCullingProperty.boolValue);
            EditorGUILayout.BeginVertical(GuiStyles.EmptyMiddleAligned);
            EditorGUILayout.PropertyField(_occlusionCullingAccuracyProperty, new GUIContent("Accuracy"));
            EditorGUILayout.EndVertical();
            EditorGUI.EndDisabledGroup();

            EditorGUILayout.Separator();
            EditorGUILayout.Separator();

            EditorGUILayout.EndVertical();

            EditorGUILayout.EndVertical();

            EditorGUILayout.Separator();
            EditorGUILayout.Separator();
            EditorGUILayout.Separator();

            EditorGUILayout.BeginVertical(GuiStyles.ButtonNoHover);

            EditorGUILayout.Separator();
            GUILayout.Label(new GUIContent(" Filtering", Aura.ResourcesCollection.cameraIconTexture), GuiStyles.LabelBoldCenteredBig);
            EditorGUILayout.Separator();

            GuiHelpers.DrawContextualHelpBox("These parameters allow to configure the filtering of the volumetric data.");

            EditorGUILayout.Separator();
            EditorGUILayout.Separator();

            EditorGUILayout.BeginVertical();
            GuiHelpers.DrawToggleChecker(ref _enableDitheringProperty, "Enable Color Dithering");

            EditorGUILayout.Separator();
            EditorGUILayout.Separator();
            EditorGUILayout.Separator();

            EditorGUILayout.PropertyField(_texture3DFilteringProperty, new GUIContent("Texture3D Filtering"));

            EditorGUILayout.Separator();
            EditorGUILayout.Separator();
            EditorGUILayout.Separator();

            GuiHelpers.DrawContextualHelpBox("The \"Reprojection\" allows to blend the current (jittered) computed frame with the previous one.\n\nThis leads to a smoother volumetric lighting, especially with a low resolution grid.");
            GuiHelpers.DrawToggleChecker(ref _enableTemporalReprojectionProperty, "Enable Reprojection");
            EditorGUI.BeginDisabledGroup(!_enableTemporalReprojectionProperty.boolValue);
            EditorGUILayout.BeginVertical(GuiStyles.EmptyMiddleAligned);
            GuiHelpers.DrawSlider(ref _temporalReprojectionFactorProperty, 0, 1, "Reprojector factor");
            EditorGUILayout.EndVertical();
            EditorGUI.EndDisabledGroup();

            EditorGUILayout.Separator();

            EditorGUILayout.EndVertical();
            EditorGUILayout.EndVertical();

            EditorGUILayout.Separator();
            EditorGUILayout.Separator();
            EditorGUILayout.Separator();

            EditorGUILayout.BeginVertical(GuiStyles.ButtonNoHover);

            EditorGUILayout.Separator();
            GUILayout.Label(new GUIContent(" Options", Aura.ResourcesCollection.optionsIconTexture), GuiStyles.LabelBoldCenteredBig);
            EditorGUILayout.Separator();

            GuiHelpers.DrawContextualHelpBox("These parameters allow you to enable/disable some options.\n\nNote that the existence of the different contributions are handled by the system at runtime.");

            EditorGUILayout.Separator();
            EditorGUILayout.Separator();

            EditorGUILayout.BeginVertical();

            GuiHelpers.DrawToggleChecker(ref _enableAmbientLightingProperty, "Enable Ambient Lighting");

            EditorGUILayout.Separator();
            EditorGUILayout.Separator();

            GuiHelpers.DrawToggleChecker(ref _enableVolumesProperty, "Enable Volumes");
            EditorGUI.BeginDisabledGroup(!_enableVolumesProperty.boolValue);
            EditorGUILayout.BeginVertical(GuiStyles.EmptyMiddleAligned);
            GuiHelpers.DrawToggleChecker(ref _enableVolumesNoiseMaskProperty, "Enable Noise Masks");
            EditorGUILayout.Separator();
            GuiHelpers.DrawToggleChecker(ref _enableVolumesTexture2DMaskProperty, "Enable Texture2D Masks");
            EditorGUILayout.Separator();
            GuiHelpers.DrawToggleChecker(ref _enableVolumesTexture3DMaskProperty, "Enable Texture3D Masks");
            EditorGUILayout.EndVertical();
            EditorGUI.EndDisabledGroup();
            
            EditorGUILayout.Separator();
            EditorGUILayout.Separator();

            GuiHelpers.DrawToggleChecker(ref _enableDirectionalLightsProperty, "Enable Directional Lights");
            EditorGUI.BeginDisabledGroup(!_enableDirectionalLightsProperty.boolValue);
            EditorGUILayout.BeginVertical(GuiStyles.EmptyMiddleAligned);
            GuiHelpers.DrawToggleChecker(ref _enableDirectionalLightsShadowsProperty, "Enable Shadows");
            EditorGUILayout.EndVertical();
            EditorGUI.EndDisabledGroup();
            
            EditorGUILayout.Separator();
            EditorGUILayout.Separator();

            GuiHelpers.DrawToggleChecker(ref _enableSpotLightsProperty, "Enable Spot Lights");
            EditorGUI.BeginDisabledGroup(!_enableSpotLightsProperty.boolValue);
            EditorGUILayout.BeginVertical(GuiStyles.EmptyMiddleAligned);
            GuiHelpers.DrawToggleChecker(ref _enableSpotLightsShadowsProperty, "Enable Shadows");
            EditorGUILayout.EndVertical();
            EditorGUI.EndDisabledGroup();
            
            EditorGUILayout.Separator();
            EditorGUILayout.Separator();

            GuiHelpers.DrawToggleChecker(ref _enablePointLightsProperty, "Enable Point Lights");
            EditorGUI.BeginDisabledGroup(!_enablePointLightsProperty.boolValue);
            EditorGUILayout.BeginVertical(GuiStyles.EmptyMiddleAligned);
            GuiHelpers.DrawToggleChecker(ref _enablePointLightsShadowsProperty, "Enable Shadows");
            EditorGUILayout.EndVertical();
            EditorGUI.EndDisabledGroup();

            EditorGUILayout.Separator();
            EditorGUILayout.Separator();

            EditorGUI.BeginDisabledGroup(!_enableSpotLightsProperty.boolValue && !_enableSpotLightsProperty.boolValue && !_enableDirectionalLightsProperty.boolValue);
            GuiHelpers.DrawToggleChecker(ref _enableLightsCookiesProperty, "Enable Cookies");
            EditorGUI.EndDisabledGroup();

            EditorGUILayout.Separator();

            EditorGUILayout.EndVertical();

            EditorGUILayout.EndVertical();

            EditorGUILayout.Separator();
            EditorGUILayout.Separator();
            EditorGUILayout.Separator();

            EditorGUILayout.BeginVertical(GuiStyles.ButtonNoHover);

            EditorGUILayout.Separator();
            GUILayout.Label(new GUIContent(" Experimental Features", Aura.ResourcesCollection.experimentalIconTexture), GuiStyles.LabelBoldCenteredBig);
            EditorGUILayout.Separator();
            
            GuiHelpers.DrawHelpBox("These features are still under active development.\nThey might currently lead to visual/performance issues.", HelpBoxType.Experimental);

            EditorGUILayout.Separator();
            EditorGUILayout.Separator();

            GuiHelpers.DrawToggleChecker(ref _enableDenoisingFilterProperty, "Enable Denoising Filter");
            EditorGUI.BeginDisabledGroup(!_enableDenoisingFilterProperty.boolValue);
            EditorGUILayout.BeginVertical(GuiStyles.EmptyMiddleAligned);
            EditorGUILayout.PropertyField(_denoisingFilterRangeProperty, new GUIContent("Range"));
            EditorGUILayout.EndVertical();
            EditorGUI.EndDisabledGroup();

            EditorGUILayout.Separator();
            EditorGUILayout.Separator();

            GuiHelpers.DrawToggleChecker(ref _enableBlurFilterProperty, "Enable Blur Filter");
            EditorGUI.BeginDisabledGroup(!_enableBlurFilterProperty.boolValue);
            EditorGUILayout.BeginVertical(GuiStyles.EmptyMiddleAligned);
            EditorGUILayout.PropertyField(_blurFilterRangeProperty, new GUIContent("Range"));
            EditorGUILayout.PropertyField(_blurFilterTypeProperty, new GUIContent("Type"));
            if ((BlurFilterType)_blurFilterTypeProperty.enumValueIndex == BlurFilterType.Gaussian)
            {
                GuiHelpers.DrawSlider(ref _blurFilterGaussianDeviationProperty, 0.0f, 0.01f, "Lobe Bulge");
            }
            EditorGUILayout.EndVertical();
            EditorGUI.EndDisabledGroup();

            EditorGUILayout.Separator();
            EditorGUILayout.Separator();

            EditorGUILayout.BeginVertical();

            GuiHelpers.DrawContextualHelpBox("The \"Light Probes\" allows to compute the world space light probes' data and inject it as contributing light in the volumetric lighting system.");
            GuiHelpers.DrawToggleChecker(ref _enableLightProbesProperty, "Enable Light Probes");
            //if(!IsEditionQualitySettings)
            //{
            //    EditorGUI.BeginDisabledGroup(!_enableLightProbesProperty.boolValue);
            //    EditorGUILayout.BeginVertical(GuiStyles.EmptyMiddleAligned);
            //    _lightProbesProxyGridResolutionProperty.FindPropertyRelative("x").intValue = Mathf.Max(2, EditorGUILayout.IntField("Horizontal", _lightProbesProxyGridResolutionProperty.FindPropertyRelative("x").intValue));
            //    _lightProbesProxyGridResolutionProperty.FindPropertyRelative("y").intValue = Mathf.Max(2, EditorGUILayout.IntField("Vertical", _lightProbesProxyGridResolutionProperty.FindPropertyRelative("y").intValue));
            //    _lightProbesProxyGridResolutionProperty.FindPropertyRelative("z").intValue = Mathf.Max(2, EditorGUILayout.IntField("Depth", _lightProbesProxyGridResolutionProperty.FindPropertyRelative("z").intValue));
            //    EditorGUILayout.Separator();
            //    if (GUILayout.Button("Set Resolution", GuiStyles.ButtonNoHover))
            //    {
            //        _current.SetLightProbesProxyGridResolution(_lightProbesProxyGridResolutionProperty.vector3IntValue);
            //    }
            //    EditorGUILayout.EndVertical();
            //    EditorGUI.EndDisabledGroup();
            //}
            EditorGUILayout.Separator();
            EditorGUILayout.EndVertical();

            EditorGUILayout.EndVertical();

            EditorGUILayout.EndVertical();
        }
        #endregion
    }
}
