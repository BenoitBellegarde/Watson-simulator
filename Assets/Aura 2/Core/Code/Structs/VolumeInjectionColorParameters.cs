
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

namespace Aura2API
{
    /// <summary>
    ///Volume injection parameters for color
    /// </summary>
    [Serializable]
    public struct VolumeInjectionColorParameters
    {
        /// <summary>
        /// Injection parameters
        /// </summary>
        public VolumeInjectionCommonParameters injectionParameters;
        /// <summary>
        /// Color of the injection
        /// </summary>
        [SerializeField]
        [ColorCircularPicker]
        public Color color;
    }
}
