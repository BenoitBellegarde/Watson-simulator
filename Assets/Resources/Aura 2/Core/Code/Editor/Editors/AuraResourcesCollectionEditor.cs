
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

using System.IO;
using UnityEditor;
using UnityEngine;

namespace Aura2API
{
    /// <summary>
    /// Custom Inspector for AuraResourcesCollection class
    /// </summary>
    [CustomEditor(typeof(AuraResourcesCollection))]
    public class AuraResourcesCollectionEditor : Editor
    {
        #region Overriden base class functions (https://docs.unity3d.com/ScriptReference/Editor.html)

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.Separator();

            EditorGUILayout.BeginHorizontal(GuiStyles.BackgroundNoBorder);
            EditorGUILayout.LabelField(new GUIContent(" Aura <b>Ressources Collection</b>", Aura.ResourcesCollection.logoIconTexture), new GUIStyle(GuiStyles.LabelCenteredBigBackground) { fontSize = 24 });
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Separator();

            EditorGUILayout.BeginHorizontal(GuiStyles.Background);
            EditorGUILayout.Separator();
            GUILayout.Label(new GUIContent(" DO NOT DELETE THIS FILE!", Aura.ResourcesCollection.settingsIconTexture), GuiStyles.LabelBoldCenteredBig);
            EditorGUILayout.Separator();
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Separator();
            
            serializedObject.ApplyModifiedProperties();
        }
        #endregion
    }
}
