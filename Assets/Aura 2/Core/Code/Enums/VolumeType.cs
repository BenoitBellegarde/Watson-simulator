
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

namespace Aura2API
{
    /// <summary>
    /// Defines the type of injection volume used in an AuraVolume
    /// </summary>
    public enum VolumeType
    {
        Global = 0,
        Layer = 1,
        Box = 2,
        Sphere = 3,
        Cylinder = 4,
        Cone = 5
    }
}
