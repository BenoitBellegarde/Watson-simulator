
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
using UnityEngine.Rendering;

namespace Aura2API
{
    /// <summary>
    /// Custom Inspector for AuraLight class
    /// </summary>
    [CustomEditor(typeof(AuraSprite))]
    [CanEditMultipleObjects]
    public class AuraSpriteEditor : Editor
    {
        #region Private Members
        /// <summary>
        /// The inspected aura sprite
        /// </summary>
        private AuraSprite _component;
        /// <summary>
        /// The content of the title of the common settings area
        /// </summary>
        private GUIContent _settingsTitleContent;
        /// <summary>
        /// The property for casting shadows
        /// </summary>
        private SerializedProperty _shadowCastingModeProperty;
        /// <summary>
        /// The property for receiving shadows
        /// </summary>
        private SerializedProperty _receiveShadowsProperty;
        #endregion

        #region Overriden base class functions (https://docs.unity3d.com/ScriptReference/Editor.html)
        private void OnEnable()
        {
            _component = (AuraSprite)target;
            _settingsTitleContent = new GUIContent("Parameters", Aura.ResourcesCollection.settingsIconTexture);
            _shadowCastingModeProperty = serializedObject.FindProperty("shadowCastingMode");
            _receiveShadowsProperty = serializedObject.FindProperty("receiveShadows");

        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.Separator();
            EditorGUILayout.Separator();

            EditorGUILayout.BeginVertical(GuiStyles.ButtonNoHover);

            EditorGUILayout.BeginHorizontal(GuiStyles.BackgroundNoBorder);
            EditorGUILayout.LabelField(new GUIContent(" Aura <b>Sprite</b>", Aura.ResourcesCollection.spriteUiIconTexture), new GUIStyle(GuiStyles.LabelCenteredBig) { fontSize = 24 });
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Separator();

            EditorGUILayout.BeginVertical(GuiStyles.Background);

            DisplaySettingsArea();

            EditorGUILayout.EndVertical();

            EditorGUILayout.Separator();

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
        private void DisplaySettingsArea()
        {
            EditorGUILayout.BeginVertical(GuiStyles.ButtonNoHover);

            EditorGUILayout.Separator();
            GUILayout.Label(_settingsTitleContent, new GUIStyle(GuiStyles.LabelBoldCenteredBig) { fontSize = 15 });
            EditorGUILayout.Separator();
            
            ShadowCastingMode shadowCastingMode = (ShadowCastingMode)_shadowCastingModeProperty.enumValueIndex;
            bool castShadow = shadowCastingMode != ShadowCastingMode.Off;
            
            EditorGUI.BeginChangeCheck();

            castShadow = GuiHelpers.DrawToggleChecker(castShadow, "Cast Shadows");
            if(EditorGUI.EndChangeCheck())
            {
                if(castShadow)
                {
                    _shadowCastingModeProperty.enumValueIndex = (int)ShadowCastingMode.TwoSided;
                }
                else
                {
                    _shadowCastingModeProperty.enumValueIndex = (int)ShadowCastingMode.Off;
                }

            }

            EditorGUILayout.Separator();
            EditorGUILayout.Separator();

            GuiHelpers.DrawToggleChecker(ref _receiveShadowsProperty, "Receive Shadows");

            EditorGUILayout.Separator();
            EditorGUILayout.Separator();
            
            if(GUILayout.Button("Set Lit Shader", GuiStyles.Button))
            {
                _component.SetLitShader();
            }

            EditorGUILayout.Separator();

            if(GUILayout.Button("Set Unlit Shader", GuiStyles.Button))
            {
                _component.SetUnlitShader();
            }

            EditorGUILayout.Separator();

            EditorGUILayout.EndVertical();
        }
        #endregion
    }
}
