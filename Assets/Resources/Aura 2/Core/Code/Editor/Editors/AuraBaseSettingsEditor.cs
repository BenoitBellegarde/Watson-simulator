
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
    [CustomEditor(typeof(AuraBaseSettings))]
    public class AuraBaseSettingsEditor : Editor
    {
        #region Private Members
        /// <summary>
        /// The property for base density usage
        /// </summary>
        private static SerializedProperty _useDensityProperty;
        /// <summary>
        /// The property for base density
        /// </summary>
        private static SerializedProperty _baseDensityProperty;
        /// <summary>
        /// The property for base scattering usage
        /// </summary>
        private static SerializedProperty _useScatteringProperty;
        /// <summary>
        /// The property for base scattering
        /// </summary>
        private static SerializedProperty _baseScatteringProperty;
        /// <summary>
        /// The property for base color usage
        /// </summary>
        private static SerializedProperty _useColorProperty;
        /// <summary>
        /// The property for base color
        /// </summary>
        private static SerializedProperty _baseColorProperty;
        /// <summary>
        /// The property for base color strength
        /// </summary>
        private static SerializedProperty _baseColorStrengthProperty;
        /// <summary>
        /// The property for base tint usage
        /// </summary>
        private static SerializedProperty _useTintProperty;
        /// <summary>
        /// The property for base tint
        /// </summary>
        private static SerializedProperty _baseTintProperty;
        /// <summary>
        /// The property for base tint strength
        /// </summary>
        private static SerializedProperty _baseTintStrengthProperty;
        /// <summary>
        /// The property for ambient lighting usage
        /// </summary>
        private static SerializedProperty _useAmbientLightingProperty;
        /// <summary>
        /// The property for ambient lighting strength
        /// </summary>
        private static SerializedProperty _ambientLightingStrengthProperty;
        /// <summary>
        /// The property for extinction usage
        /// </summary>
        private static SerializedProperty _useExtinctionProperty;
        /// <summary>
        /// The property for extinction
        /// </summary>
        private static SerializedProperty _extinctionProperty;
        #endregion

        #region Overriden base class functions (https://docs.unity3d.com/ScriptReference/Editor.html)
        private void OnEnable()
        {
            Initialize(serializedObject);
        }

        public override void OnInspectorGUI()
        {
            DrawCustomEditor(serializedObject, null);
        }
        #endregion

        #region Functions
        /// <summary>
        /// Initialized the needed resources
        /// </summary>
        /// <param name="serializedObject"></param>
        private static void Initialize(SerializedObject serializedObject)
        {
            _useDensityProperty = serializedObject.FindProperty("useDensity");
            _baseDensityProperty = serializedObject.FindProperty("density");
            _useScatteringProperty = serializedObject.FindProperty("useScattering");
            _baseScatteringProperty = serializedObject.FindProperty("scattering");
            _useColorProperty = serializedObject.FindProperty("useColor");
            _baseColorProperty = serializedObject.FindProperty("color");
            _baseColorStrengthProperty = serializedObject.FindProperty("colorStrength");
            _useTintProperty = serializedObject.FindProperty("useTint");
            _baseTintProperty = serializedObject.FindProperty("tint");
            _baseTintStrengthProperty = serializedObject.FindProperty("tintStrength");
            _useAmbientLightingProperty = serializedObject.FindProperty("useAmbientLighting");
            _ambientLightingStrengthProperty = serializedObject.FindProperty("ambientLightingStrength");
            _useExtinctionProperty = serializedObject.FindProperty("useExtinction");
            _extinctionProperty = serializedObject.FindProperty("extinction");
        }

        /// <summary>
        /// Draws the editor
        /// </summary>
        /// <param name="serializedObject"></param>
        /// <param name="auraComponent"></param>
        public static void DrawCustomEditor(SerializedObject serializedObject, AuraCamera auraComponent)
        {
            serializedObject.Update();

            if (auraComponent == null)
            {
                EditorGUILayout.BeginVertical(GuiStyles.ButtonNoHover);

                //GuiHelpers.DrawHeader(Aura.ResourcesCollection.logoTexture);
                EditorGUILayout.LabelField(new GUIContent(" Aura <b>Base Settings</b>", Aura.ResourcesCollection.baseSettingsPresetIconTexture), new GUIStyle(GuiStyles.LabelCenteredBigBackground) { fontSize = 24 });

                EditorGUILayout.Separator();
            }
            else
            {
                Initialize(serializedObject);
            }

            DisplayBaseSettingsArea();

            if (auraComponent == null)
            {
                EditorGUILayout.Separator();
                GuiHelpers.DisplayHelpToShowHelpBox();
                EditorGUILayout.EndVertical();

                EditorGUILayout.Separator();
                EditorGUILayout.Separator();
            }

            serializedObject.ApplyModifiedProperties();
        }

        /// <summary>
        /// Displays the content of the "World's Primary Injection" tab
        /// </summary>
        private static void DisplayBaseSettingsArea()
        {
            EditorGUILayout.BeginVertical(GuiStyles.Background);

            EditorGUILayout.BeginVertical(GuiStyles.ButtonNoHover);

            EditorGUILayout.Separator();
            GUILayout.Label(new GUIContent(" Global Injection", Aura.ResourcesCollection.injectionIconTexture), GuiStyles.LabelBoldCenteredBig);
            EditorGUILayout.Separator();

            GuiHelpers.DrawContextualHelpBox("These parameters are the ambient Density, Lighting and Scattering, set globally for the whole surrounding environment.");

            EditorGUILayout.Separator();
            EditorGUILayout.Separator();

            EditorGUILayout.BeginVertical();
            GuiHelpers.DrawToggleChecker(ref _useDensityProperty, new GUIContent(" Density", Aura.ResourcesCollection.densityIconTexture, "Injects an ambient density"), true, true);
            if(_useDensityProperty.boolValue)
            {
                //EditorGUI.BeginDisabledGroup(!_useDensityProperty.boolValue);
                EditorGUILayout.BeginVertical(GuiStyles.EmptyMiddleAligned);
                GuiHelpers.DrawPositiveOnlyFloatField(ref _baseDensityProperty, "Density");
                EditorGUILayout.Separator();
                EditorGUILayout.EndVertical();
                //EditorGUI.EndDisabledGroup();
            }
            
            EditorGUILayout.Separator();
            EditorGUILayout.Separator();

            GuiHelpers.DrawToggleChecker(ref _useScatteringProperty, new GUIContent(" Scattering", Aura.ResourcesCollection.scatteringIconTexture, "Injects an ambient scattering"), true, true);
            if (_useScatteringProperty.boolValue)
            {
                //EditorGUI.BeginDisabledGroup(!_useScatteringProperty.boolValue);
                EditorGUILayout.BeginVertical(GuiStyles.EmptyMiddleAligned);
                GuiHelpers.DrawSlider(ref _baseScatteringProperty, 0, 1);
                EditorGUILayout.Separator();
                EditorGUILayout.EndVertical();
                //EditorGUI.EndDisabledGroup();
            }
            
            EditorGUILayout.Separator();
            EditorGUILayout.Separator();

            GuiHelpers.DrawToggleChecker(ref _useAmbientLightingProperty, new GUIContent(" Ambient Lighting", Aura.ResourcesCollection.illuminationIconTexture, "Injects the \"Environment lighting\" set in the \"Lighting\" window"), true, true);
            if (_useAmbientLightingProperty.boolValue)
            {
                //EditorGUI.BeginDisabledGroup(!_useAmbientLightingProperty.boolValue);
                EditorGUILayout.BeginVertical(GuiStyles.EmptyMiddleAligned);
                GuiHelpers.DrawPositiveOnlyFloatField(ref _ambientLightingStrengthProperty, "Strength");
                EditorGUILayout.Separator();
                EditorGUILayout.EndVertical();
                //EditorGUI.EndDisabledGroup();
            }

            EditorGUILayout.Separator();
            EditorGUILayout.Separator();

            GuiHelpers.DrawToggleChecker(ref _useTintProperty, new GUIContent(" Tint", Aura.ResourcesCollection.tintIconTexture, "Injects an tinting tint"), true, true);
            if (_useTintProperty.boolValue)
            {
                //EditorGUI.BeginDisabledGroup(!_useTintProperty.boolValue);
                EditorGUILayout.BeginVertical(GuiStyles.EmptyMiddleAligned);
                EditorGUILayout.PropertyField(_baseTintProperty);
                GuiHelpers.DrawSlider(ref _baseTintStrengthProperty, 0.0f, 1.0f, "Brightness");
                EditorGUILayout.Separator();
                EditorGUILayout.EndVertical();
                //EditorGUI.EndDisabledGroup();
            }

            EditorGUILayout.Separator();
            EditorGUILayout.Separator();

            GuiHelpers.DrawToggleChecker(ref _useColorProperty, new GUIContent(" Light", Aura.ResourcesCollection.illuminationColorIconTexture, "Injects an ambient light"), true, true);
            if (_useColorProperty.boolValue)
            {
                //EditorGUI.BeginDisabledGroup(!_useColorProperty.boolValue);
                EditorGUILayout.BeginVertical(GuiStyles.EmptyMiddleAligned);
                EditorGUILayout.PropertyField(_baseColorProperty);
                GuiHelpers.DrawPositiveOnlyFloatField(ref _baseColorStrengthProperty, "Strength");
                EditorGUILayout.Separator();
                EditorGUILayout.EndVertical();
                //EditorGUI.EndDisabledGroup();
            }
            
            EditorGUILayout.Separator();
            EditorGUILayout.Separator();

            GuiHelpers.DrawToggleChecker(ref _useExtinctionProperty, new GUIContent(" Extinction", Aura.ResourcesCollection.extinctionIconTexture, "Simulates the depth related decay of light"), true, true);
            if (_useExtinctionProperty.boolValue)
            {
                //EditorGUI.BeginDisabledGroup(!_useExtinctionProperty.boolValue);
                EditorGUILayout.BeginVertical(GuiStyles.EmptyMiddleAligned);
                GuiHelpers.DrawSlider(ref _extinctionProperty, 0, 1, true);
                EditorGUILayout.Separator();
                EditorGUILayout.EndVertical();
                //EditorGUI.EndDisabledGroup();
            }

            EditorGUILayout.Separator();
            EditorGUILayout.Separator();

            EditorGUILayout.EndVertical();

            EditorGUILayout.EndVertical();

            EditorGUILayout.EndVertical();
        }
        #endregion
    }
}
