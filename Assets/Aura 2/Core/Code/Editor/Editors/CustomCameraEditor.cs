


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

using System;
using UnityEditor;
using UnityEngine;

namespace Aura2API
{
    /// <summary>
    /// Custom Inspector for Light class
    /// </summary>
    //[CustomEditor(typeof(Camera))]
    [CanEditMultipleObjects]
    public class CustomCameraEditor : Editor
    {
        #region Private Members
        Editor _defaultEditor;
        Camera _targetObject;
        #endregion

        #region Overriden base class functions (https://docs.unity3d.com/ScriptReference/Editor.html)
        public void OnEnable()
        {
            _defaultEditor = CreateEditor(targets, Type.GetType("UnityEditor.CameraEditor, UnityEditor"));
            _targetObject = (Camera)target;
        }
        
        public void OnDisable()
        {
            _defaultEditor.Destroy(); 
        }

        public override void OnInspectorGUI()
        {
            try
            {
                serializedObject.Update();
                _defaultEditor.OnInspectorGUI();
                serializedObject.ApplyModifiedProperties();
            
                if (AuraEditorPrefs.DisplayAuraGuiInParentComponents)
                {
                    EditorGUILayout.Separator();    
                    EditorGUILayout.BeginVertical(GuiStyles.Background);

                    AuraCamera component = _targetObject.gameObject.GetComponent<AuraCamera>();
                    if(component == null)
                    {
                        if(GUILayout.Button(new GUIContent(" Add <b>Aura Camera</b>", Aura.ResourcesCollection.cameraUiIconTexture), new GUIStyle(GuiStyles.ButtonNoBorder) { fontSize = 18 }, GUILayout.Height(32)))
                        {
                            Undo.AddComponent<AuraCamera>(_targetObject.gameObject);
                        }
                    }
                    else
                    {
                        EditorGUILayout.BeginHorizontal();
                        if(GUILayout.Button(new GUIContent((component.enabled ? " Disable" : " Enable") +  " <b>Aura Camera</b>", Aura.ResourcesCollection.cameraUiIconTexture), new GUIStyle(GuiStyles.ButtonNoBorder) { fontSize = 18 }, GUILayout.Height(32)))
                        {
                            component.enabled = !component.enabled;
                        }
                        if(GUILayout.Button(new GUIContent(Aura.ResourcesCollection.removeIconTexture, "Remove Aura Camera"), GuiStyles.ButtonImageOnlyNoBorder, GUILayout.Width(32), GUILayout.Height(32)))
                        {
                            component.Destroy();
                        }
                        EditorGUILayout.EndHorizontal();
                    }

                    EditorGUILayout.EndVertical();
                    EditorGUILayout.Separator();
                }
            }
            catch{}
        }
        #endregion
    }
}
