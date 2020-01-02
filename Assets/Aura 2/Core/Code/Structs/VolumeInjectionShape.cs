
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
    [Serializable]
    public struct VolumeInjectionShape
    {
        /// <summary>
        /// Shape of the volume
        /// </summary>
        [SerializeField]
        public VolumeType shape;
        /// <summary>
        /// Fading parameters for the borders of the shape
        /// </summary>
        [SerializeField]
        public VolumeGradient fading;
    }
}
