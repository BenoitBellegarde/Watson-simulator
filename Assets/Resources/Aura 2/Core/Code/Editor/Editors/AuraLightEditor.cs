
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
    /// Custom Inspector for AuraLight class
    /// </summary>
    [CustomEditor(typeof(AuraLight))]
    //[CanEditMultipleObjects]
    public class AuraLightEditor : Editor
    {
        #region Private Members
        /// <summary>
        /// The inspected aura light
        /// </summary>
        private AuraLight _component;
        /// <summary>
        /// The current displayed tab index
        /// </summary>
        private int _tabIndex;
        /// <summary>
        /// The content of the title of the common settings area
        /// </summary>
        private GUIContent _commonSettingsTitleContent;
        /// <summary>
        /// The content of the title of the directional light settings area
        /// </summary>
        private GUIContent _directionalLightSettingsTitleContent;
        /// <summary>
        /// The content of the title of the spot light settings area
        /// </summary>
        private GUIContent _spotLightSettingsTitleContent;
        /// <summary>
        /// The content of the title of the point light settings area
        /// </summary>
        private GUIContent _pointLightSettingsTitleContent;
        /// <summary>
        /// The property for strength
        /// </summary>
        private SerializedProperty _strengthProperty;
        /// <summary>
        /// The property for enabling light scattering use
        /// </summary>
        private SerializedProperty _useScatteringProperty;
        /// <summary>
        /// The property for enabling light scattering overriding
        /// </summary>
        private SerializedProperty _overrideScatteringProperty;
        /// <summary>
        /// The property for scattering to override with
        /// </summary>
        private SerializedProperty _overridingScatteringProperty;
        /// <summary>
        /// The property for using the color temperature tint
        /// </summary>
        private SerializedProperty _useColorTemperatureTintProperty;
        /// <summary>
        /// The property for enabling light color overriding
        /// </summary>
        private SerializedProperty _overrideColorProperty;
        /// <summary>
        /// The property for color to override with
        /// </summary>
        private SerializedProperty _overridingColorProperty;
        /// <summary>
        /// The property for enabling shadows
        /// </summary>
        private SerializedProperty _useShadowsProperty;
        /// <summary>
        /// The property for enabling cookies
        /// </summary>
        private SerializedProperty _useCookieProperty;
        /// <summary>
        /// The property for enabling out-of-phase color for directional lights
        /// </summary>
        private SerializedProperty _enableOutOfPhaseColorProperty;
        /// <summary>
        /// The property for color to use when out-of-phase
        /// </summary>
        private SerializedProperty _outOfPhaseColorProperty;
        /// <summary>
        /// The property for strength of the out-of-phase color
        /// </summary>
        private SerializedProperty _outOfPhaseColorStrengthProperty;
        /// <summary>
        /// The property for custom angular falloff start threshold for spot lights
        /// </summary>
        private SerializedProperty _customAngularFalloffThresholdProperty;
        /// <summary>
        /// The property for custom angular falloff exponent for spot lights
        /// </summary>
        private SerializedProperty _customAngularFalloffPowerProperty;
        /// <summary>
        /// The property for custom distance falloff start threshold for spot/point lights
        /// </summary>
        private SerializedProperty _customDistanceFalloffThresholdProperty;
        /// <summary>
        /// The property for custom distance falloff exponent for spot/point lights
        /// </summary>
        private SerializedProperty _customDistanceFalloffPowerProperty;
        /// <summary>
        /// The property for the start of the custom distance fade-in for spot/point lights' cookies
        /// </summary>
        private SerializedProperty _customCookieDistanceFalloffLowThresholdProperty;
        /// <summary>
        /// The property for the end of the custom distance fade-in for spot/point lights' cookies
        /// </summary>
        private SerializedProperty _customCookieDistanceFalloffHiThresholdProperty;
        /// <summary>
        /// The property for the exponent of the custom distance fade-in for spot/point lights' cookies
        /// </summary>
        private SerializedProperty _customCookieDistanceFalloffPowerProperty;
        #endregion

        #region Overriden base class functions (https://docs.unity3d.com/ScriptReference/Editor.html)
        private void OnEnable()
        {
            _component = (AuraLight)target;
            _commonSettingsTitleContent = new GUIContent("Common Parameters", Aura.ResourcesCollection.illuminationIconTexture);
            _directionalLightSettingsTitleContent = new GUIContent("Directional Parameters", Aura.ResourcesCollection.directionalLightIconTexture);
            _spotLightSettingsTitleContent = new GUIContent("Spot Parameters", Aura.ResourcesCollection.spotLightIconTexture);
            _pointLightSettingsTitleContent = new GUIContent("Point Parameters", Aura.ResourcesCollection.pointLightIconTexture);


            _strengthProperty = serializedObject.FindProperty("strength");
            _useScatteringProperty = serializedObject.FindProperty("useScattering");
            _overrideScatteringProperty = serializedObject.FindProperty("overrideScattering");
            _overridingScatteringProperty = serializedObject.FindProperty("overridingScattering");
            _useColorTemperatureTintProperty = serializedObject.FindProperty("useColorTemperatureTint");
            _overrideColorProperty = serializedObject.FindProperty("overrideColor");
            _overridingColorProperty = serializedObject.FindProperty("overridingColor");
            _useShadowsProperty = serializedObject.FindProperty("useShadow");
            _useCookieProperty = serializedObject.FindProperty("useCookie");

            _enableOutOfPhaseColorProperty = serializedObject.FindProperty("enableOutOfPhaseColor");
            _outOfPhaseColorProperty = serializedObject.FindProperty("outOfPhaseColor");
            _outOfPhaseColorStrengthProperty = serializedObject.FindProperty("outOfPhaseColorStrength");

            _customAngularFalloffThresholdProperty = serializedObject.FindProperty("customAngularFalloffThreshold");
            _customAngularFalloffPowerProperty = serializedObject.FindProperty("customAngularFalloffPower");
            _customDistanceFalloffThresholdProperty = serializedObject.FindProperty("customDistanceFalloffThreshold");
            _customDistanceFalloffPowerProperty = serializedObject.FindProperty("customDistanceFalloffPower");
            _customCookieDistanceFalloffLowThresholdProperty = serializedObject.FindProperty("customCookieDistanceFalloffStartThreshold");
            _customCookieDistanceFalloffHiThresholdProperty = serializedObject.FindProperty("customCookieDistanceFalloffEndThreshold");
            _customCookieDistanceFalloffPowerProperty = serializedObject.FindProperty("customCookieDistanceFalloffPower");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.Separator();
            EditorGUILayout.Separator();

            EditorGUILayout.BeginVertical(GuiStyles.ButtonNoHover);

            EditorGUILayout.BeginHorizontal(GuiStyles.BackgroundNoBorder);
            GUILayout.Space(24);
            EditorGUILayout.LabelField(new GUIContent(" Aura <b>Light</b>", Aura.ResourcesCollection.lightUiIconTexture), new GUIStyle(GuiStyles.LabelCenteredBig) { fontSize = 24 });
            if (GUILayout.Button(new GUIContent(Aura.ResourcesCollection.questionIconTexture, "Show Help"), GuiStyles.ButtonImageOnlyNoBorder, GUILayout.MaxWidth(24), GUILayout.MaxHeight(24)))
            {
                AuraEditorPrefs.DisplayLightIntroductionScreen = true;
                SceneView.RepaintAll();
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Separator();

            EditorGUILayout.BeginVertical(GuiStyles.Background);

            DisplayCommonSettingsArea();

            EditorGUILayout.Separator();
            EditorGUILayout.Separator();

            DisplayLightSettingsArea();

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
        /// Displays the common parameters tab
        /// </summary>
        private void DisplayCommonSettingsArea()
        {
            EditorGUILayout.BeginVertical(GuiStyles.ButtonNoHover);

            EditorGUILayout.Separator();
            GUILayout.Label(_commonSettingsTitleContent, new GUIStyle(GuiStyles.LabelBoldCenteredBig) { fontSize = 15 });
            EditorGUILayout.Separator();

            GuiHelpers.DrawFloatField(ref _strengthProperty, new GUIContent("Strength", "Multiplies the intensity of the light source in the system"));

            EditorGUILayout.Separator();
            EditorGUILayout.Separator();
            EditorGUILayout.Separator();

            EditorGUILayout.PropertyField(_useScatteringProperty);
            if((BooleanChoice)_useScatteringProperty.enumValueIndex == BooleanChoice.Default || (BooleanChoice)_useScatteringProperty.enumValueIndex == BooleanChoice.True)
            {
                EditorGUILayout.Separator();
                GuiHelpers.DrawToggleChecker(ref _overrideScatteringProperty, "Override Scattering");
                if (_overrideScatteringProperty.boolValue)
                {
                    EditorGUILayout.BeginVertical();
                    EditorGUILayout.PropertyField(_overridingScatteringProperty);
                    EditorGUILayout.EndVertical();
                }
            }

            EditorGUILayout.Separator();
            EditorGUILayout.Separator();
            EditorGUILayout.Separator();

            GuiHelpers.DrawContextualHelpBox("The \"Enable Shadows\" parameter allows you to compute the light's shadows (if enabled) in the system.");
            GuiHelpers.DrawToggleChecker(ref _useShadowsProperty, "Enable Shadows");

            EditorGUILayout.Separator();
            EditorGUILayout.Separator();

            GuiHelpers.DrawContextualHelpBox("The \"Enable Cookie\" parameter allows you to compute the light's cookie (if enabled) in the system.");
            //EditorGUI.BeginDisabledGroup(!_component.CastsCookie);
            GuiHelpers.DrawToggleChecker(ref _useCookieProperty, "Enable Cookie");
            //EditorGUI.EndDisabledGroup();

            EditorGUILayout.Separator();
            EditorGUILayout.Separator();
            EditorGUILayout.Separator();

            GuiHelpers.DrawContextualHelpBox("The \"Override Color\" parameter allows you to replace the light's color in the system.");
            GuiHelpers.DrawToggleChecker(ref _overrideColorProperty, "Override Color");
            if (_overrideColorProperty.boolValue)
            {
                //EditorGUI.BeginDisabledGroup(!_overrideColorProperty.boolValue);
                EditorGUILayout.BeginVertical();
                GuiHelpers.DrawContextualHelpBox("The \"Overriding Color\" is the color that will replace the light's color in the system.");
                EditorGUILayout.PropertyField(_overridingColorProperty);
                EditorGUILayout.EndVertical();
                //EditorGUI.EndDisabledGroup();
            }

            if(LightHelpers.IsColorTemperatureAvailable)
            {
                EditorGUILayout.Separator();
                EditorGUILayout.Separator();
                EditorGUILayout.Separator();

                GuiHelpers.DrawHelpBox("Unity currently gives no way to know if the color temperature mode is enabled on the light, therefore you need to manually enable it here under.", HelpBoxType.Warning);
                EditorGUILayout.Separator();
                GuiHelpers.DrawToggleChecker(ref _useColorTemperatureTintProperty, "Use Color Temperature Tint");
            }

            EditorGUILayout.Separator();

            EditorGUILayout.EndVertical();
        }

        /// <summary>
        /// Displays the additional parameters
        /// </summary>
        private void DisplayLightSettingsArea()
        {
            EditorGUILayout.BeginVertical(GuiStyles.ButtonNoHover);
            switch (((AuraLight)serializedObject.targetObject).Type)
            {
                case LightType.Directional:
                    {
                        DisplayDirectionalLightAdditionalSettingsArea();
                    }
                    break;

                case LightType.Spot:
                    {
                        DisplaySpotLightAdditionalSettingsArea();
                    }
                    break;

                case LightType.Point:
                    {
                        DisplayPointLightAdditionalSettingsArea();
                    }
                    break;
            }
            EditorGUILayout.EndVertical();
        }

        /// <summary>
        /// Displays the additional parameters for directional lights
        /// </summary>
        private void DisplayDirectionalLightAdditionalSettingsArea()
        {
            EditorGUILayout.BeginVertical();

            EditorGUILayout.Separator();
            GUILayout.Label(_directionalLightSettingsTitleContent, new GUIStyle(GuiStyles.LabelBoldCenteredBig) { fontSize = 15 });
            EditorGUILayout.Separator();

            GuiHelpers.DrawContextualHelpBox("The \"Enable Out-Of-Phase Color\" parameter allows you to use a color when the view angle is not towards the directional light (the decay is controlled by the scattering factor.");
            GuiHelpers.DrawToggleChecker(ref _enableOutOfPhaseColorProperty, "Enable Out-Of-Phase Color");
            if (_enableOutOfPhaseColorProperty.boolValue)
            {
                //EditorGUI.BeginDisabledGroup(!_enableOutOfPhaseColorProperty.boolValue);
                EditorGUILayout.BeginVertical();
                GuiHelpers.DrawContextualHelpBox("The color when the view direction is not towards the directional light.");
                EditorGUILayout.PropertyField(_outOfPhaseColorProperty);
                GuiHelpers.DrawFloatField(ref _outOfPhaseColorStrengthProperty, new GUIContent("Strength", "Multiplies the intensity of the color"));
                EditorGUILayout.EndVertical();
                //EditorGUI.EndDisabledGroup();
            }

            EditorGUILayout.Separator();

            EditorGUILayout.EndVertical();
        }

        /// <summary>
        /// Displays the additional parameters for spot lights
        /// </summary>
        private void DisplaySpotLightAdditionalSettingsArea()
        {
            EditorGUILayout.BeginVertical();

            EditorGUILayout.Separator();
            GUILayout.Label(_spotLightSettingsTitleContent, new GUIStyle(GuiStyles.LabelBoldCenteredBig) { fontSize = 15 });
            EditorGUILayout.Separator();

            DisplayLightAngularAttenuationParameters();

            EditorGUILayout.Separator();
            EditorGUILayout.Separator();
            EditorGUILayout.Separator();

            DisplayLightDistanceAttenuationParameters();

            EditorGUILayout.Separator();
            EditorGUILayout.Separator();
            EditorGUILayout.Separator();

            EditorGUI.BeginDisabledGroup(!_component.CastsCookie);
            DisplayCookieDistanceAttenuationParameters();
            EditorGUI.EndDisabledGroup();

            EditorGUILayout.Separator();

            EditorGUILayout.EndVertical();
        }

        /// <summary>
        /// Displays the additional parameters for point lights
        /// </summary>
        private void DisplayPointLightAdditionalSettingsArea()
        {
            EditorGUILayout.BeginVertical();

            EditorGUILayout.Separator();
            GUILayout.Label(_pointLightSettingsTitleContent, new GUIStyle(GuiStyles.LabelBoldCenteredBig) { fontSize = 15 });
            EditorGUILayout.Separator();

            DisplayLightDistanceAttenuationParameters();

            EditorGUILayout.Separator();
            EditorGUILayout.Separator();
            EditorGUILayout.Separator();

            EditorGUI.BeginDisabledGroup(!_component.CastsCookie);
            DisplayCookieDistanceAttenuationParameters();
            EditorGUI.EndDisabledGroup();

            EditorGUILayout.Separator();

            EditorGUILayout.EndVertical();
        }

        /// <summary>
        /// Displays angular fadeout parameters for spot lights
        /// </summary>
        private void DisplayLightAngularAttenuationParameters()
        {
            EditorGUILayout.BeginVertical();

            GUILayout.Label("Angular Attenuation", GuiStyles.LabelCenteredBig);
            EditorGUILayout.Separator();
            EditorGUILayout.Separator();

            GuiHelpers.DrawContextualHelpBox("The \"Threshold\" parameter is the normalized angle when the fade will start, until 1.");
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Threshold", GuiStyles.Label, GUILayout.MaxWidth(160));
            GuiHelpers.DrawSlider(ref _customAngularFalloffThresholdProperty, 0.0f, 1.0f);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Separator();
            
            GuiHelpers.DrawPositiveOnlyFloatField(ref _customAngularFalloffPowerProperty, new GUIContent("Exponent", "The curve of the fading"));

            EditorGUILayout.Separator();
            EditorGUILayout.Separator();

            GuiHelpers.DrawContextualHelpBox("Allows to reset to Unity's default values.");
            if(GUILayout.Button("Reset", GuiStyles.Button))
            {
                _customAngularFalloffThresholdProperty.floatValue = 0.8f;
                _customAngularFalloffPowerProperty.floatValue = 2.0f;
            }

            EditorGUILayout.EndVertical();
        }

        /// <summary>
        /// Displays distance fadeout parameters for spot/point lights
        /// </summary>
        private void DisplayLightDistanceAttenuationParameters()
        {
            EditorGUILayout.BeginVertical();

            GUILayout.Label("Distance Attenuation", GuiStyles.LabelCenteredBig);
            EditorGUILayout.Separator();
            EditorGUILayout.Separator();

            GuiHelpers.DrawContextualHelpBox("The \"Threshold\" parameter is the normalized distance when the fade will start, until 1.");
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Threshold", GuiStyles.Label, GUILayout.MaxWidth(160));
            GuiHelpers.DrawSlider(ref _customDistanceFalloffThresholdProperty, 0.0f, 1.0f);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Separator();
            
            GuiHelpers.DrawPositiveOnlyFloatField(ref _customDistanceFalloffPowerProperty, new GUIContent("Exponent", "The curve of the fading"));

            EditorGUILayout.Separator();
            EditorGUILayout.Separator();

            GuiHelpers.DrawContextualHelpBox("Allows to reset to Unity's default values.");
            if(GUILayout.Button("Reset", GuiStyles.Button))
            {
                _customDistanceFalloffThresholdProperty.floatValue = 0.5f;
                _customDistanceFalloffPowerProperty.floatValue = 2.0f;
            }

            EditorGUILayout.EndVertical();
        }

        /// <summary>
        /// Displays distance fadein parameters for spot/point lights' cookies
        /// </summary>
        private void DisplayCookieDistanceAttenuationParameters()
        {
            EditorGUILayout.BeginVertical();

            GUILayout.Label("Cookie Fade-In", GuiStyles.LabelCenteredBig);
            EditorGUILayout.Separator();
            EditorGUILayout.Separator();

            GuiHelpers.DrawContextualHelpBox("The \"Thresholds\" parameters are the normalized range where the cookie will fade in.");
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Fade-In Thresholds", GuiStyles.Label, GUILayout.MaxWidth(160));
            GuiHelpers.DrawMinMaxSlider(ref _customCookieDistanceFalloffLowThresholdProperty, ref _customCookieDistanceFalloffHiThresholdProperty, 0.0f, 1.0f);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Separator();
            
            GuiHelpers.DrawPositiveOnlyFloatField(ref _customCookieDistanceFalloffPowerProperty, new GUIContent("Exponent", "The curve of the fading"));

            EditorGUILayout.Separator();
            EditorGUILayout.Separator();

            GuiHelpers.DrawContextualHelpBox("Allows to reset to Unity's default values.");
            if(GUILayout.Button("Reset", GuiStyles.Button))
            {
                _customCookieDistanceFalloffLowThresholdProperty.floatValue = 0.1f;
                _customCookieDistanceFalloffHiThresholdProperty.floatValue = 0.25f;
                _customAngularFalloffPowerProperty.floatValue = 2.0f;
            }

            EditorGUILayout.EndVertical();
        }
        #endregion
    }
}
