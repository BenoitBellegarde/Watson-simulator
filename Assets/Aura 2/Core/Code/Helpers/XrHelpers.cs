
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

using UnityEngine;
using UnityEngine.XR;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Aura2API
{
    /// <summary>
    /// Collection of functions/variables related to Xr
    /// </summary>
    public static class XrHelpers
    {
        #region Properties
        /// <summary>
        /// Tells if the project is setup as Single Pass Stereo
        /// </summary>
        public static bool IsSinglePassStereo
        {
            get
            {
                return UnityEngine.XR.XRSettings.enabled &&
#if UNITY_EDITOR
                Application.isPlaying && PlayerSettings.virtualRealitySupported && PlayerSettings.stereoRenderingPath == StereoRenderingPath.SinglePass;
#else
                UnityEngine.XR.XRSettings.eyeTextureDesc.vrUsage == VRTextureUsage.TwoEyes;
#endif
            }
        }
#endregion
    }
}
