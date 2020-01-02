
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

#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

namespace Aura2API
{
    /// <summary>
    /// Collection of accessors/functions related to Aura editor preferences
    /// </summary>
    public static class AuraEditorPrefs
    {
        #region Private Members
        private const string _displayMainIntroductionScreenString = "AURA2_DisplayMainIntroductionScreen";
        private const string _displayCameraIntroductionScreenString = "AURA2_DisplayCameraIntroductionScreen";
        private const string _displayLightIntroductionScreenString = "AURA2_DisplayLightIntroductionScreen";
        private const string _displayVolumeIntroductionScreenString = "AURA2_DisplayVolumeIntroductionScreen";
        private const string _displayToolboxString = "AURA2_DisplayToolbox";
        private const string _expandToolboxString = "AURA2_ExpandToolbox";
        private const string _showToolboxNotificationsString = "AURA2_ShowToolboxNotifications";
        private const string _enableToolboxAnimationsString = "AURA2_EnableToolboxAnimations";
        private const string _toolboxPositionString = "AURA2_ToolboxPosition";
        private const string _toolboxPresetsPreviewsPerRowString = "AURA2_ToolboxPresetsPreviewsPerRow";
        private const string _displayCameraSlicesInEditionString = "AURA2_DisplayCameraSlicesInEdition";
        private const string _displayDebugPanelInToolboxString = "AURA2_DisplayDebugPanelInToolbox";
        private const string _enableAuraInSceneViewString = "AURA2_EnableAuraInSceneView";
        private const string _displayAuraGuiInParentComponentsString = "AURA2_DisplayAuraGuiInParentComponents";
        private const string _displayGizmosWhenSelectedString = "AURA2_DisplayGizmosWhenSelected";
        private const string _displayGizmosWhenUnselectedString = "AURA2_DisplayGizmosWhenUnselected";
        private const string _displayGizmosOnCamerasString = "AURA2_DisplayGizmosOnCameras";
        private const string _displayGizmosOnLightsString = "AURA2_DisplayGizmosOnLights";
        private const string _displayGizmosOnVolumesString = "AURA2_DisplayGizmosOnVolumes";
        #endregion

        #region Properties
        public static bool DisplayMainIntroductionScreen
        {
            get
            {
                return EditorPrefs.GetBool(_displayMainIntroductionScreenString, true);
            }
            set
            {
                EditorPrefs.SetBool(_displayMainIntroductionScreenString, value);
            }
        }

        public static bool DisplayCameraIntroductionScreen
        {
            get
            {
                return EditorPrefs.GetBool(_displayCameraIntroductionScreenString, true);
            }
            set
            {
                EditorPrefs.SetBool(_displayCameraIntroductionScreenString, value);
            }
        }

        public static bool DisplayLightIntroductionScreen
        {
            get
            {
                return EditorPrefs.GetBool(_displayLightIntroductionScreenString, true);
            }
            set
            {
                EditorPrefs.SetBool(_displayLightIntroductionScreenString, value);
            }
        }

        public static bool DisplayVolumeIntroductionScreen
        {
            get
            {
                return EditorPrefs.GetBool(_displayVolumeIntroductionScreenString, true);
            }
            set
            {
                EditorPrefs.SetBool(_displayVolumeIntroductionScreenString, value);
            }
        }

        public static bool DisplayToolbox
        {
            get
            {
                return EditorPrefs.GetBool(_displayToolboxString, true);
            }
            set
            {
                EditorPrefs.SetBool(_displayToolboxString, value);
            }
        }

        public static bool ExpandToolbox
        {
            get
            {
                return EditorPrefs.GetBool(_expandToolboxString, true);
            }
            set
            {
                EditorPrefs.SetBool(_expandToolboxString, value);
            }
        }

        public static bool ShowToolboxNotifications
        {
            get
            {
                return EditorPrefs.GetBool(_showToolboxNotificationsString, true);
            }
            set
            {
                EditorPrefs.SetBool(_showToolboxNotificationsString, value);
            }
        }

        public static bool EnableToolboxAnimations
        {
            get
            {
                return EditorPrefs.GetBool(_enableToolboxAnimationsString, true);
            }
            set
            {
                EditorPrefs.SetBool(_enableToolboxAnimationsString, value);
            }
        }

        public static int ToolboxPosition
        {
            get
            {
                return EditorPrefs.GetInt(_toolboxPositionString, 0);
            }
            set
            {
                EditorPrefs.SetInt(_toolboxPositionString, value);
            }
        }

        public static int ToolboxPresetsPreviewsPerRow
        {
            get
            {
                return EditorPrefs.GetInt(_toolboxPresetsPreviewsPerRowString, 2);
            }
            set
            {
                EditorPrefs.SetInt(_toolboxPresetsPreviewsPerRowString, value);
            }
        }

        public static bool DisplayCameraSlicesInEdition
        {
            get
            {
                return EditorPrefs.GetBool(_displayCameraSlicesInEditionString, false);
            }
            set
            {
                EditorPrefs.SetBool(_displayCameraSlicesInEditionString, value);
            }
        }

        public static bool DisplayDebugPanelInToolbox
        {
            get
            {
                return EditorPrefs.GetBool(_displayDebugPanelInToolboxString, false);
            }
            set
            {
                EditorPrefs.SetBool(_displayDebugPanelInToolboxString, value);
            }
        }

        public static bool EnableAuraInSceneView
        {
            get
            {
                return EditorPrefs.GetBool(_enableAuraInSceneViewString, true);
            }
            set
            {
                EditorPrefs.SetBool(_enableAuraInSceneViewString, value);
            }
        }

        public static bool DisplayAuraGuiInParentComponents
        {
            get
            {
                return EditorPrefs.GetBool(_displayAuraGuiInParentComponentsString, true);
            }
            set
            {
                EditorPrefs.SetBool(_displayAuraGuiInParentComponentsString, value);
            }
        }

        public static bool DisplayGizmosWhenSelected
        {
            get
            {
                return EditorPrefs.GetBool(_displayGizmosWhenSelectedString, true);
            }
            set
            {
                EditorPrefs.SetBool(_displayGizmosWhenSelectedString, value);
            }
        }

        public static bool DisplayGizmosWhenUnselected
        {
            get
            {
                return EditorPrefs.GetBool(_displayGizmosWhenUnselectedString, false);
            }
            set
            {
                EditorPrefs.SetBool(_displayGizmosWhenUnselectedString, value);
            }
        }

        public static bool DisplayGizmosOnCameras
        {
            get
            {
                return EditorPrefs.GetBool(_displayGizmosOnCamerasString, true);
            }
            set
            {
                EditorPrefs.SetBool(_displayGizmosOnCamerasString, value);
            }
        }

        public static bool DisplayGizmosOnLights
        {
            get
            {
                return EditorPrefs.GetBool(_displayGizmosOnLightsString, true);
            }
            set
            {
                EditorPrefs.SetBool(_displayGizmosOnLightsString, value);
            }
        }

        public static bool DisplayGizmosOnVolumes
        {
            get
            {
                return EditorPrefs.GetBool(_displayGizmosOnVolumesString, true);
            }
            set
            {
                EditorPrefs.SetBool(_displayGizmosOnVolumesString, value);
            }
        }
        #endregion
    }
}
#endif
