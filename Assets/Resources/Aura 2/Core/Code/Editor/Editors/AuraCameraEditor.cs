
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

using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Aura2API
{
    /// <summary>
    /// Custom Inspector for AuraCamera class
    /// </summary>
    [CustomEditor(typeof(AuraCamera))]
    public class AuraCameraEditor : Editor
    {
        #region Private Members
        /// <summary>
        /// The current drawn AuraCamera component
        /// </summary>
        private AuraCamera _currentAuraComponent;
        /// <summary>
        /// Should base settings area be displayed or quality settings
        /// </summary>
        private bool _showBaseSettings = true;
        /// <summary>
        /// The content of the base settings tab button
        /// </summary>
        private GUIContent _baseSettingsTabButtonContent;
        /// <summary>
        /// The content of the quality settings tab button
        /// </summary>
        private GUIContent _qualitySettingsTabButtonContent;
        /// <summary>
        /// The property for the frustum base settings
        /// </summary>
        private SerializedProperty _baseSettingsProperty;
        /// <summary>
        /// The property for the frustum base settings
        /// </summary>
        private SerializedProperty _qualitySettingsProperty;
        /// <summary>
        /// The reference base settings
        /// </summary>
        private AuraBaseSettings _previousBaseSettings;
        /// <summary>
        /// The reference quality settings
        /// </summary>
        private AuraQualitySettings _previousQualitySettings;
        /// <summary>
        /// Array of all the found base settings found in the assets
        /// </summary>
        private string[] _foundBaseSettingsPresets;
        /// <summary>
        /// The id of the selected base settings
        /// </summary>
        private int _selectedBaseSettingsPresetId;
        /// <summary>
        /// Array with the names of the found base settings
        /// </summary>
        private string[] _foundBaseSettingsPresetsName;
        /// <summary>
        /// List of all the found quality settings found in the assets
        /// </summary>
        private List<string> _foundQualitySettingsPresets;
        /// <summary>
        /// The id of the selected quality settings
        /// </summary>
        private int _selectedQualitySettingsPresetId;
        /// <summary>
        /// Array with the names of the found quality settings
        /// </summary>
        private string[] _foundQualitySettingsPresetsName;
        #endregion

        #region Overriden base class functions (https://docs.unity3d.com/ScriptReference/Editor.html)
        private void OnEnable()
        {
            _currentAuraComponent = (AuraCamera)serializedObject.targetObject;

            _baseSettingsTabButtonContent = new GUIContent("Base Settings", Aura.ResourcesCollection.injectionIconTexture, "Displays the BASE SETTINGS panel");
            _baseSettingsProperty = serializedObject.FindProperty("frustumSettings.baseSettings");

            _qualitySettingsTabButtonContent = new GUIContent("Quality Settings", Aura.ResourcesCollection.optionsIconTexture, "Displays the QUALITY SETTINGS panel");
            _qualitySettingsProperty = serializedObject.FindProperty("frustumSettings.qualitySettings");

            _previousQualitySettings = (AuraQualitySettings)_qualitySettingsProperty.objectReferenceValue;

            PopulateExistingBaseSettingsPresetsList();

            PopulateExistingQualitySettingsPresetsList();
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.Separator();
            EditorGUILayout.Separator();

            EditorGUILayout.BeginVertical(GuiStyles.ButtonNoHover);
            
            EditorGUILayout.BeginHorizontal(GuiStyles.BackgroundNoBorder);
            GUILayout.Space(24);
            EditorGUILayout.LabelField(new GUIContent(" Aura <b>Camera</b>", Aura.ResourcesCollection.cameraUiIconTexture), new GUIStyle(GuiStyles.LabelCenteredBig) { fontSize = 24});
            if(GUILayout.Button(new GUIContent(Aura.ResourcesCollection.questionIconTexture, "Show Help"), GuiStyles.ButtonImageOnlyNoBorder, GUILayout.MaxWidth(24), GUILayout.MaxHeight(24)))
            {
                AuraEditorPrefs.DisplayCameraIntroductionScreen = true;
                SceneView.RepaintAll();
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Separator();

            EditorGUILayout.BeginVertical(GuiStyles.Background);

            EditorGUILayout.Separator();

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button(_baseSettingsTabButtonContent, !_showBaseSettings ? GuiStyles.ButtonBigBold : GuiStyles.ButtonPressedBigBold))
            {
                _showBaseSettings = true;
            }

            GUILayout.Space(8);

            if (GUILayout.Button(_qualitySettingsTabButtonContent, _showBaseSettings ? GuiStyles.ButtonBigBold : GuiStyles.ButtonPressedBigBold))
            {
                _showBaseSettings = false;
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Separator();
            EditorGUILayout.Separator();

            if (_showBaseSettings)
            {
                DisplayBaseSettingsArea();
            }
            else
            {
                DisplayQualitySettingsArea();
            }

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
        /// Retrievse all the quality settings
        /// </summary>
        private void PopulateExistingQualitySettingsPresetsList()
        {
            _foundQualitySettingsPresets = new List<string>();
            string[] tmp = AssetDatabase.FindAssets("t:AuraQualitySettings");
            for (int i = 0; i < tmp.Length; ++i)
            {
                string path = AssetDatabase.GUIDToAssetPath(tmp[i]);
                if (!AssetDatabase.LoadAssetAtPath<AuraQualitySettings>(path).Equals(Aura.ResourcesCollection.editionCameraQualitySettings)) // rule out edition quality settings
                {
                    _foundQualitySettingsPresets.Add(path);
                }
            }

            _selectedQualitySettingsPresetId = -1;
            if (_foundQualitySettingsPresets.Count > 0)
            {
                _foundQualitySettingsPresetsName = new string[_foundQualitySettingsPresets.Count];
                for (int i = 0; i < _foundQualitySettingsPresets.Count; ++i)
                {
                    AuraQualitySettings preset = AssetDatabase.LoadAssetAtPath<AuraQualitySettings>(_foundQualitySettingsPresets[i]);
                    if (_currentAuraComponent.frustumSettings.qualitySettings == preset)
                    {
                        _foundQualitySettingsPresetsName[i] = "Current";
                        _selectedQualitySettingsPresetId = i;
                        continue;
                    }
                    _foundQualitySettingsPresetsName[i] = preset.name;
                    string presetPath = Directory.GetParent(AssetDatabase.GetAssetPath(preset)).FullName;
                    presetPath = ("Assets" + presetPath.Substring(Application.dataPath.Length) + "\\").Replace('/', '\\');
                    _foundQualitySettingsPresetsName[i] += " (in " + presetPath + ")";
                }
            }
        }

        /// <summary>
        /// Retrievse all the base settings
        /// </summary>
        private void PopulateExistingBaseSettingsPresetsList()
        {
            _foundBaseSettingsPresets = AssetDatabase.FindAssets("t:AuraBaseSettings");
            _selectedBaseSettingsPresetId = -1;
            if (_foundBaseSettingsPresets.Length > 0)
            {
                _foundBaseSettingsPresetsName = new string[_foundBaseSettingsPresets.Length];
                for (int i = 0; i < _foundBaseSettingsPresets.Length; ++i)
                {
                    _foundBaseSettingsPresets[i] = AssetDatabase.GUIDToAssetPath(_foundBaseSettingsPresets[i]);
                    AuraBaseSettings preset = AssetDatabase.LoadAssetAtPath<AuraBaseSettings>(_foundBaseSettingsPresets[i]);
                    if (_currentAuraComponent.frustumSettings.baseSettings == preset)
                    {
                        _foundBaseSettingsPresetsName[i] = "Current";
                        _selectedBaseSettingsPresetId = i;
                        continue;
                    }
                    _foundBaseSettingsPresetsName[i] = preset.name;
                    string presetPath = Directory.GetParent(AssetDatabase.GetAssetPath(preset)).FullName;
                    presetPath = ("Assets" + presetPath.Substring(Application.dataPath.Length) + "\\").Replace('/', '\\');
                    _foundBaseSettingsPresetsName[i] += " (in " + presetPath + ")";
                }
            }
        }

        /// <summary>
        /// Draws the base settings area
        /// </summary>
        private void DisplayBaseSettingsArea()
        {
            AuraBaseSettings currentBaseSettings = (AuraBaseSettings)_baseSettingsProperty.objectReferenceValue;
            if (!ReferenceEquals(_previousBaseSettings, currentBaseSettings))
            {
                PopulateExistingBaseSettingsPresetsList();
            }
            _previousBaseSettings = currentBaseSettings;

            EditorGUILayout.BeginVertical(GuiStyles.ButtonNoHover);

            EditorGUILayout.BeginVertical(GuiStyles.Background);

            EditorGUILayout.Separator();
            GUILayout.Label(new GUIContent("Aura <b>Base Settings</b>", Aura.ResourcesCollection.baseSettingsPresetIconTexture), new GUIStyle(GuiStyles.LabelCenteredBig) { fontSize = 15 });
            EditorGUILayout.Separator();

            GuiHelpers.DrawContextualHelpBox("The \"Base Settings Preset\" is the asset that holds the base settings for the data computation.");

            if(_foundBaseSettingsPresets.Length != AssetDatabase.FindAssets("t:AuraBaseSettings").Length)
            {
                PopulateExistingBaseSettingsPresetsList();
            }

            if (_foundBaseSettingsPresets.Length > 0)
            {
                EditorGUI.BeginChangeCheck();
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Select Existing", GuiStyles.Label, GUILayout.MaxWidth(96));
                _selectedBaseSettingsPresetId = EditorGUILayout.Popup(_selectedBaseSettingsPresetId, _foundBaseSettingsPresetsName);
                EditorGUILayout.EndHorizontal();
                if (EditorGUI.EndChangeCheck())
                {
                    _currentAuraComponent.frustumSettings.LoadBaseSettings(AssetDatabase.LoadAssetAtPath<AuraBaseSettings>(_foundBaseSettingsPresets[_selectedBaseSettingsPresetId]));
                    PopulateExistingBaseSettingsPresetsList();
                }

                EditorGUILayout.Separator();
                EditorGUILayout.Separator();
            }

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Preset File", GuiStyles.Label, GUILayout.MaxWidth(80));
            EditorGUILayout.PropertyField(_baseSettingsProperty, new GUIContent(""));
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Separator();
            EditorGUILayout.Separator();

            EditorGUILayout.BeginHorizontal();
            //if (GUILayout.Button(new GUIContent("Load Existing", Aura.AuraResourcesCollection.loadIconTexture, "Loads an existing preset file"), GuiStyles.ButtonBigNoBorder))
            //{
            //    _currentAuraComponent.frustumSettings.LoadBaseSettings();
            //}
            //GUILayout.Space(8);
            if (GUILayout.Button(new GUIContent("Save As New Preset", Aura.ResourcesCollection.saveIconTexture, "Saves the current settings in a new file"), GuiStyles.ButtonBigNoBorder))
            {
                _currentAuraComponent.frustumSettings.SaveBaseSettings();
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Separator();
            EditorGUILayout.Separator();

            EditorGUILayout.EndVertical();

            EditorGUILayout.Separator();
            EditorGUILayout.Separator();
            EditorGUILayout.Separator();

            SerializedObject baseSettingsObject = new SerializedObject(_currentAuraComponent.frustumSettings.BaseSettings);
            AuraBaseSettingsEditor.DrawCustomEditor(baseSettingsObject, _currentAuraComponent);

            EditorGUILayout.EndVertical();  
        }

        /// <summary>
        /// Draws the quality settings area
        /// </summary>
        private void DisplayQualitySettingsArea()
        {
            AuraQualitySettings currentQualitySettings = (AuraQualitySettings)_qualitySettingsProperty.objectReferenceValue;
            if (!ReferenceEquals(_previousQualitySettings, currentQualitySettings))
            {
                _currentAuraComponent.frustumSettings.RaiseOnQualityChanged();
                PopulateExistingQualitySettingsPresetsList();
            }
            _previousQualitySettings = currentQualitySettings;

            EditorGUILayout.BeginVertical(GuiStyles.ButtonNoHover);

            EditorGUILayout.BeginVertical(GuiStyles.Background);

            EditorGUILayout.Separator();
            GUILayout.Label(new GUIContent("Aura <b>Quality Settings</b>", Aura.ResourcesCollection.qualitySettingsPresetIconTexture), new GUIStyle(GuiStyles.LabelCenteredBig) { fontSize = 15 });
            EditorGUILayout.Separator();
            GuiHelpers.DrawContextualHelpBox("The \"Quality Settings Preset\" is the asset that holds the quality settings for the data computation.");

            if (_foundQualitySettingsPresets.Count != AssetDatabase.FindAssets("t:AuraQualitySettings").Length)
            {
                PopulateExistingQualitySettingsPresetsList();
            }

            if (_foundQualitySettingsPresets.Count > 0)
            {
                EditorGUI.BeginChangeCheck();
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Select Existing", GuiStyles.Label, GUILayout.MaxWidth(96));
                _selectedQualitySettingsPresetId = EditorGUILayout.Popup(_selectedQualitySettingsPresetId, _foundQualitySettingsPresetsName);
                EditorGUILayout.EndHorizontal();
                if (EditorGUI.EndChangeCheck())
                {
                    _currentAuraComponent.frustumSettings.LoadQualitySettings(AssetDatabase.LoadAssetAtPath<AuraQualitySettings>(_foundQualitySettingsPresets[_selectedQualitySettingsPresetId]));
                    PopulateExistingQualitySettingsPresetsList();
                }

                EditorGUILayout.Separator();
                EditorGUILayout.Separator();
            }

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Preset File", GuiStyles.Label, GUILayout.MaxWidth(80));
            EditorGUILayout.PropertyField(_qualitySettingsProperty, new GUIContent(""));
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Separator();
            EditorGUILayout.Separator();

            EditorGUILayout.BeginHorizontal();
            //if (GUILayout.Button(new GUIContent("Load Existing", Aura.AuraResourcesCollection.loadIconTexture, "Loads an existing preset file"), GuiStyles.ButtonBigNoBorder))
            //{
            //    _currentAuraComponent.frustumSettings.LoadQualitySettings();
            //}
            //GUILayout.Space(8);
            if (GUILayout.Button(new GUIContent("Save As New Preset", Aura.ResourcesCollection.saveIconTexture, "Saves the current settings in a new file"), GuiStyles.ButtonBigNoBorder))
            {
                _currentAuraComponent.frustumSettings.SaveQualitySettings();
            }
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.Separator();
            EditorGUILayout.Separator();

            EditorGUILayout.EndVertical();

            EditorGUILayout.Separator();
            EditorGUILayout.Separator();
            EditorGUILayout.Separator();

            SerializedObject qualitySettingsObject = new SerializedObject(_currentAuraComponent.frustumSettings.QualitySettings);
            AuraQualitySettingsEditor.DrawCustomEditor(qualitySettingsObject, _currentAuraComponent, false, true);

            EditorGUILayout.EndVertical();

            EditorGUILayout.Separator();
        }
        #endregion
    }
}
