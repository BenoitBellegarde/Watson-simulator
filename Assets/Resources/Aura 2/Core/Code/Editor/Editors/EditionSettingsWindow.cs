
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
    /// Window allowing to tweak the settings of the Aura component in the scene view
    /// </summary>
    public class EditionSettingsWindow : EditorWindow
    {
        #region Private Members
        /// <summary>
        /// Serialized object from EditionCameraQualitySettings
        /// </summary>
        private static SerializedObject _editionCameraQualitySettingsObject;
        /// <summary>
        /// Position of the scroll
        /// </summary>
        private Vector2 _scrollPosition;
        #endregion

        #region Properties
        /// <summary>
        /// Serialized object from EditionCameraQualitySettings
        /// </summary>
        private static SerializedObject EditionCameraQualitySettingsObject
        {
            get
            {
                if (_editionCameraQualitySettingsObject == null)
                {
                    _editionCameraQualitySettingsObject = new SerializedObject(Aura.ResourcesCollection.editionCameraQualitySettings);
                }

                return _editionCameraQualitySettingsObject;
            }
        }
        #endregion

        #region Overriden base class functions (https://docs.unity3d.com/ScriptReference/EditorWindow.html)
        [MenuItem("Window/Aura 2/Edition Settings", priority = 0)]
        private static void Init()
        {
            EditionSettingsWindow window = (EditionSettingsWindow)EditorWindow.GetWindow(typeof(EditionSettingsWindow));
            window.titleContent.text = "Aura 2 Edition Settings";
            window.Show();
        }

        private void OnGUI()
        {
            _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);

            EditorGUILayout.BeginVertical(GuiStyles.Background);
            EditorGUILayout.Separator();
            EditorGUILayout.BeginVertical(GuiStyles.ButtonNoHover);
            EditorGUILayout.Separator();

            EditorGUILayout.BeginHorizontal(GuiStyles.BackgroundNoBorder);
            EditorGUILayout.LabelField(new GUIContent(" Aura <b>Edition Settings</b>", Aura.ResourcesCollection.logoIconTexture), new GUIStyle(GuiStyles.LabelCenteredBig) { fontSize = 24 });
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Separator();
            EditorGUILayout.Separator();
            EditorGUILayout.Separator();

            if (!EditorApplication.isCompiling)
            {
                DisplayEditionSettingsArea();
            }
            else
            {
                EditorGUILayout.BeginVertical(GuiStyles.Background);
                GUILayout.Label(new GUIContent("PLEASE WAIT", Aura.ResourcesCollection.settingsIconTexture), GuiStyles.LabelBoldCenteredBig);
                EditorGUILayout.EndVertical();
            }

            EditorGUILayout.EndVertical();
            EditorGUILayout.EndVertical();

            EditorGUILayout.EndScrollView();
        }

        /// <summary>
        /// Draws the settings area
        /// </summary>
        private void DisplayEditionSettingsArea()
        {
            EditorGUILayout.BeginVertical(GuiStyles.Background);
            GUILayout.Label(new GUIContent(" Edition Settings", Aura.ResourcesCollection.settingsIconTexture), GuiStyles.LabelBoldCenteredBig);
            EditorGUILayout.Separator();
            EditorGUI.BeginChangeCheck();
            AuraEditorPrefs.DisplayCameraSlicesInEdition = GuiHelpers.DrawToggleChecker(AuraEditorPrefs.DisplayCameraSlicesInEdition, " Show slices on Cameras when selected");
            if (EditorGUI.EndChangeCheck())
            {
                SceneView.RepaintAll();
            }
            EditorGUILayout.Separator();
            //EditorGUI.BeginChangeCheck();
            EditorGUI.BeginDisabledGroup(true);
            GuiHelpers.DrawToggleChecker(AuraEditorPrefs.DisplayAuraGuiInParentComponents, " Display Aura buttons in Camera/Light components");
            EditorGUI.EndDisabledGroup();
            //if (EditorGUI.EndChangeCheck())
            //{
            //    AuraEditorPrefs.DisplayAuraGuiInParentComponents = !AuraEditorPrefs.DisplayAuraGuiInParentComponents;
            //}
            EditorGUILayout.Separator();
            EditorGUI.BeginChangeCheck();
            GuiHelpers.DrawToggleChecker(AuraEditorPrefs.DisplayGizmosOnCameras, " Display gizmos on Cameras");
            if (EditorGUI.EndChangeCheck())
            {
                AuraEditorPrefs.DisplayGizmosOnCameras = !AuraEditorPrefs.DisplayGizmosOnCameras;
            }
            EditorGUILayout.Separator();
            EditorGUI.BeginChangeCheck();
            GuiHelpers.DrawToggleChecker(AuraEditorPrefs.DisplayGizmosOnLights, " Display gizmos on Lights");
            if (EditorGUI.EndChangeCheck())
            {
                AuraEditorPrefs.DisplayGizmosOnLights = !AuraEditorPrefs.DisplayGizmosOnLights;
            }
            EditorGUILayout.Separator();
            EditorGUI.BeginChangeCheck();
            GuiHelpers.DrawToggleChecker(AuraEditorPrefs.DisplayGizmosOnVolumes, " Display gizmos on Volumes");
            if (EditorGUI.EndChangeCheck())
            {
                AuraEditorPrefs.DisplayGizmosOnVolumes = !AuraEditorPrefs.DisplayGizmosOnVolumes;
            }
            EditorGUILayout.Separator();
            EditorGUI.BeginChangeCheck();
            GuiHelpers.DrawToggleChecker(AuraEditorPrefs.DisplayGizmosWhenSelected, " Display gizmos when objects are selected");
            if (EditorGUI.EndChangeCheck())
            {
                AuraEditorPrefs.DisplayGizmosWhenSelected = !AuraEditorPrefs.DisplayGizmosWhenSelected;
            }
            EditorGUILayout.Separator();
            EditorGUI.BeginChangeCheck();
            GuiHelpers.DrawToggleChecker(AuraEditorPrefs.DisplayGizmosWhenUnselected, " Display gizmos when objects are unselected");
            if (EditorGUI.EndChangeCheck())
            {
                AuraEditorPrefs.DisplayGizmosWhenUnselected = !AuraEditorPrefs.DisplayGizmosWhenUnselected;
            }
            EditorGUILayout.EndVertical();

            EditorGUILayout.Separator();
            EditorGUILayout.Separator();
            EditorGUILayout.Separator();

            EditorGUILayout.BeginVertical(GuiStyles.Background);
            GUILayout.Label(new GUIContent(" Toolbox Settings", Aura.ResourcesCollection.debugIconTexture), GuiStyles.LabelBoldCenteredBig);
            EditorGUILayout.Separator();
            EditorGUI.BeginChangeCheck();
            EditorGUI.BeginChangeCheck();
            GuiHelpers.DrawToggleChecker(SceneViewToolbox.IsVisible, " Display Toolbox");
            if (EditorGUI.EndChangeCheck())
            {
                SceneViewToolbox.Display(!SceneViewToolbox.IsVisible);
            }
            EditorGUILayout.Separator();
            EditorGUI.BeginChangeCheck();
            GuiHelpers.DrawToggleChecker(SceneViewToolbox.IsExpanded, " Expand Toolbox");
            if (EditorGUI.EndChangeCheck())
            {
                SceneViewToolbox.Expand(!SceneViewToolbox.IsExpanded);
            }
            EditorGUILayout.Separator();
            EditorGUILayout.Separator();
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Display Toolbox on Left", AuraEditorPrefs.ToolboxPosition == 0 ? GuiStyles.ButtonPressed : GuiStyles.Button))
            {
                AuraEditorPrefs.ToolboxPosition = 0;
            }
            if (GUILayout.Button("Display Toolbox on Right", AuraEditorPrefs.ToolboxPosition == 1 ? GuiStyles.ButtonPressed : GuiStyles.Button))
            {
                AuraEditorPrefs.ToolboxPosition = 1;
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Separator();
            EditorGUILayout.Separator();
            AuraEditorPrefs.DisplayDebugPanelInToolbox = GuiHelpers.DrawToggleChecker(AuraEditorPrefs.DisplayDebugPanelInToolbox, " Show DEBUG panel");
            EditorGUILayout.Separator();
            EditorGUI.BeginChangeCheck();
            GuiHelpers.DrawToggleChecker(SceneViewToolbox.DisplayNotifications, " Display Notifications");
            if (EditorGUI.EndChangeCheck())
            {
                SceneViewToolbox.DisplayNotifications = !SceneViewToolbox.DisplayNotifications;
            }
            EditorGUILayout.Separator();
            EditorGUI.BeginChangeCheck();
            GuiHelpers.DrawToggleChecker(SceneViewToolbox.EnableAnimations, " Enable Animations");
            if (EditorGUI.EndChangeCheck())
            {
                SceneViewToolbox.EnableAnimations = !SceneViewToolbox.EnableAnimations;
            }
            EditorGUILayout.Separator();
            EditorGUILayout.Separator();
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.BeginVertical(GUILayout.MaxWidth(160));
            GUILayout.Space(6);
            GUILayout.Label("Presets Previews Layout", GuiStyles.Label);
            EditorGUILayout.EndVertical();
            if (GUILayout.Button(new GUIContent("List", Aura.ResourcesCollection.listIconTexture), SceneViewToolbox.PresetsPreviewsPerRow == 1 ? GuiStyles.ButtonPressedNoBorder : GuiStyles.ButtonNoBorder))
            {
                SceneViewToolbox.PresetsPreviewsPerRow = 1;
            }
            if (GUILayout.Button(new GUIContent("Grid", Aura.ResourcesCollection.gridIconTexture), SceneViewToolbox.PresetsPreviewsPerRow == 2 ? GuiStyles.ButtonPressedNoBorder : GuiStyles.ButtonNoBorder))
            {
                SceneViewToolbox.PresetsPreviewsPerRow = 2;
            }
            EditorGUILayout.EndHorizontal();
            if (EditorGUI.EndChangeCheck())
            {
                SceneView.RepaintAll();
            }
            EditorGUILayout.Separator();
            EditorGUILayout.EndVertical();

            EditorGUILayout.Separator();
            EditorGUILayout.Separator();
            EditorGUILayout.Separator();

            EditorGUILayout.BeginVertical(GuiStyles.Background);
            GUILayout.Label(new GUIContent(" SceneView Visualization", Aura.ResourcesCollection.cameraIconTexture), GuiStyles.LabelBoldCenteredBig);
            EditorGUILayout.Separator();
            EditorGUI.BeginChangeCheck();

            AuraEditorPrefs.EnableAuraInSceneView = GuiHelpers.DrawToggleChecker(AuraEditorPrefs.EnableAuraInSceneView, "Enable visualization in SceneView");

            EditorGUILayout.Separator();
            GuiHelpers.DrawHelpBox("Enabling Aura in the SceneView may break the SceneView rendering when loading a scene or building player.\n\nTo get the SceneView rendering back, <b>disable then re-enable back</b> post-effects rendering in the toggle dropdown located in the top border of the SceneView.", HelpBoxType.Experimental);
            EditorGUILayout.Separator();

            if (EditorGUI.EndChangeCheck())
            {
                if (AuraEditorPrefs.EnableAuraInSceneView)
                {
                    SceneViewVisualization.EnableAuraInSceneView();
                }
                else
                {
                    SceneViewVisualization.DisableAuraInSceneView();
                }
            }
            
            EditorGUI.BeginDisabledGroup(!AuraEditorPrefs.EnableAuraInSceneView);
            AuraQualitySettingsEditor.DrawCustomEditor(EditionCameraQualitySettingsObject, null, false, true);
            EditorGUI.EndDisabledGroup();

            EditorGUILayout.Separator();
            EditorGUILayout.EndVertical();

            EditorGUILayout.Separator();
            EditorGUILayout.Separator();
            EditorGUILayout.Separator();

            EditorGUILayout.BeginVertical(GuiStyles.Background);
            GUILayout.Label(new GUIContent(" Introduction Screens", Aura.ResourcesCollection.questionIconTexture), GuiStyles.LabelBoldCenteredBig);
            EditorGUILayout.Separator();
            if (GUILayout.Button("Show Main Introduction Screen", GuiStyles.Button))
            {
                AuraEditorPrefs.DisplayMainIntroductionScreen = true;
                SceneView.RepaintAll();
            }
            EditorGUILayout.Separator();
            EditorGUILayout.Separator();
            if (GUILayout.Button("Show Camera Introduction Screen", GuiStyles.Button))
            {
                AuraEditorPrefs.DisplayCameraIntroductionScreen = true;
                SceneView.RepaintAll();
            }
            EditorGUILayout.Separator();
            EditorGUILayout.Separator();
            if (GUILayout.Button("Show Light Introduction Screen", GuiStyles.Button))
            {
                AuraEditorPrefs.DisplayLightIntroductionScreen = true;
                SceneView.RepaintAll();
            }
            EditorGUILayout.Separator();
            EditorGUILayout.Separator();
            if (GUILayout.Button("Show Volume Introduction Screen", GuiStyles.Button))
            {
                AuraEditorPrefs.DisplayVolumeIntroductionScreen = true;
                SceneView.RepaintAll();
            }
            EditorGUILayout.Separator();
            EditorGUILayout.EndVertical();
        }
        #endregion
    }
}
