
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
    /// Bitmask representing the possible parameters for the volumetric data computation
    /// </summary>
    [Flags]
    public enum FrustumParameters
    {
        EnableNothing                           = 0,
        EnableOcclusionCulling                  = 1 << 0,
        EnableTemporalReprojection              = 1 << 1,
        EnableVolumes                           = 1 << 2,
        EnableVolumesNoiseMask                  = 1 << 3,
        EnableVolumesTexture2DMask              = 1 << 4,
        EnableVolumesTexture3DMask              = 1 << 5,
        EnableAmbientLighting                   = 1 << 6,
        EnableLightProbes                       = 1 << 7,
        EnableDirectionalLights                 = 1 << 8,
        EnableDirectionalLightsShadows          = 1 << 9,
        DirectionalLightsShadowsOneCascade      = 1 << 10,
        DirectionalLightsShadowsTwoCascades     = 1 << 11,
        DirectionalLightsShadowsFourCascades    = 1 << 12,
        EnableSpotLights                        = 1 << 13,
        EnableSpotLightsShadows                 = 1 << 14,
        EnablePointLights                       = 1 << 15,
        EnablePointLightsShadows                = 1 << 16,
        EnableLightsCookies                     = 1 << 17,
        EnableDenoisingFilter                   = 1 << 18,
        EnableBlurFilter                        = 1 << 19
    }
}
