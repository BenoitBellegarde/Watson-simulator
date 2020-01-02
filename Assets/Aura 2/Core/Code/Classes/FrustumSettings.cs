
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
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Aura2API
{
    /// <summary>
    /// Collection of settings for the data computation
    /// </summary>
    [Serializable]
    public class FrustumSettings
    {
        #region Public members
        /// <summary>
        /// The ambient injection settings
        /// </summary>
        public AuraBaseSettings baseSettings;
        /// <summary>
        /// The quality of the data computation
        /// </summary>
        public AuraQualitySettings qualitySettings;
        #endregion

        #region Properties
        /// <summary>
        /// The ambient injection settings
        /// </summary>
        public AuraBaseSettings BaseSettings
        {
            get
            {
                if(baseSettings == null)
                {
                    baseSettings = AuraBaseSettings.CreateInstance<AuraBaseSettings>();
                    baseSettings.name = "New Aura Base Settings";
                }

                return baseSettings;
            }
            set
            {
                baseSettings = value;
            }
        }

        /// <summary>
        /// The quality of the data computation
        /// </summary>
        public AuraQualitySettings QualitySettings
        {
            get
            {
                if (qualitySettings == null)
                {
                    qualitySettings = AuraQualitySettings.CreateInstance<AuraQualitySettings>();
                    qualitySettings.name = "Default Aura Quality Settings";
                }

#if UNITY_EDITOR
                if (CameraExtensions.IsCurrentSceneViewCamera)
                {
                    return Aura.ResourcesCollection.editionCameraQualitySettings;
                }
                else
#endif
                {
                    return qualitySettings;
                }
            }
            set
            {
                qualitySettings = value;
                RaiseOnQualityChanged();
            }
        }
        #endregion

        #region Events
        /// <summary>
        /// Event raised when the frustum quality preset has been changed
        /// </summary>
        public event Action OnFrustumQualityChanged;
        #endregion

        #region Functions
        /// <summary>
        /// Replaces the current frustum base settings
        /// </summary>
        /// <param name="baseSettings">the replacing frustum base settings</param>
        public void LoadBaseSettings(AuraBaseSettings baseSettings)
        {
            this.BaseSettings = baseSettings;
        }

        #if UNITY_EDITOR
        /// <summary>
        /// Shows the file selection window then replaces the current frustum base settings with the selected one
        /// </summary>
        public void LoadBaseSettings()
        {
            string dataFolderPath = Application.dataPath + "/";
            string path = EditorUtility.OpenFilePanelWithFilters("Load Base Settings", Application.dataPath, new string[] { "Aura Base Settings", "asset" });
            if (path.StartsWith(dataFolderPath, StringComparison.CurrentCultureIgnoreCase))
            {
                path = "Assets/" + path.Substring(dataFolderPath.Length);
                AuraBaseSettings loadedSettings = AssetDatabase.LoadAssetAtPath<AuraBaseSettings>(path);
                LoadBaseSettings(loadedSettings);
            }
        }

        /// <summary>
        /// Shows the file saving window then saves the frustum quality settings as an asset
        /// </summary>
        public void SaveBaseSettings()
        {
            string fullPathName = EditorUtility.SaveFilePanelInProject("Save Base Settings", BaseSettings.name, "asset", "Please select a location and a filename.");
            if(fullPathName.Length > 0)
            {
                AuraBaseSettings baseSettingsInstance = AuraBaseSettings.Instantiate<AuraBaseSettings>(BaseSettings);
                AssetDatabase.CreateAsset(baseSettingsInstance, fullPathName);
                BaseSettings = baseSettingsInstance;
            }
        }
        #endif

        /// <summary>
        /// Replaces the current quality settings
        /// </summary>
        /// <param name="qualitySettings">the replacing quality settings</param>
        public void LoadQualitySettings(AuraQualitySettings qualitySettings)
        {
            this.QualitySettings = qualitySettings;
        }

        #if UNITY_EDITOR
        /// <summary>
        /// Shows the file selection window then replaces the current frustum quality settings with the selected one
        /// </summary>
        public void LoadQualitySettings()
        {
            string dataFolderPath = Application.dataPath + "/";
            string path = EditorUtility.OpenFilePanelWithFilters("Load Quality Settings", Application.dataPath, new string[] { "Aura Quality Settings", "asset" });
            if(path.StartsWith(dataFolderPath, StringComparison.CurrentCultureIgnoreCase))
            {
                path = "Assets/" + path.Substring(dataFolderPath.Length);
                AuraQualitySettings loadedSettings = AssetDatabase.LoadAssetAtPath<AuraQualitySettings>(path);
                LoadQualitySettings(loadedSettings);
            }
        }

        /// <summary>
        /// Shows the file saving window then saves the frustum quality settings as an asset
        /// </summary>
        public void SaveQualitySettings()
        {
            string fullPathName = EditorUtility.SaveFilePanelInProject("Save Quality Settings", QualitySettings.name, "asset", "Please select a location and a filename.");
            if (fullPathName.Length > 0)
            {
                AuraQualitySettings qualitySettingsInstance = AuraQualitySettings.Instantiate<AuraQualitySettings>(QualitySettings);
                AssetDatabase.CreateAsset(qualitySettingsInstance, fullPathName);
                QualitySettings = qualitySettingsInstance;
            }
        }
        #endif

        /// <summary>
        /// Raises the OnFrustumQualityChanged event
        /// </summary>
        public void RaiseOnQualityChanged()
        {
            if (OnFrustumQualityChanged != null)
            {
                OnFrustumQualityChanged();
            }
        }
        #endregion
    }
}
