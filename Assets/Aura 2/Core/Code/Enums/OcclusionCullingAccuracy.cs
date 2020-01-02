
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

namespace Aura2API
{
    /// <summary>
    /// Defines the accuracy of maxima detection for the occlusion culling
    /// </summary>
    public enum OcclusionCullingAccuracy
    {
        Lowest = 0,
        Low = 1,
        Medium = 2,
        High = 3,
        [Obsolete("This setting will temporarilly fallback on \"High\" for compatibility reasons.")]
        Highest = 3/*4 // Not available on all platforms*/
    }
}
