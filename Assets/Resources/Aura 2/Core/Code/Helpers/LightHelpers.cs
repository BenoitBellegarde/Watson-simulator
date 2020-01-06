
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
using UnityEngine.Rendering;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Aura2API
{
    /// <summary>
    /// Collection of functions/variables related to Light objects
    /// </summary>
    public static class LightHelpers
    {
        #region Properties
        /// <summary>
        /// Tells if the color temperature mode is available for lights
        /// </summary>
        public static bool IsColorTemperatureAvailable
        {
            get
            {
                return GraphicsSettings.lightsUseLinearIntensity && GraphicsSettings.lightsUseColorTemperature;
            }
        }
        #endregion
    }
}
