
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

uniform Texture3D<FP4> lightProbesCoefficientsTexture;
uniform FP4 lightProbesCoefficientsTextureHalfTexelSize;

// viewDirection should be normalized, w=1.0
FP3 ComputeLightProbesInjection(FP4 viewDirection, FP3 normalizedPos, FP scattering)
{
    normalizedPos.x = lerp(lightProbesCoefficientsTextureHalfTexelSize.x, 1.0f / 3.0f - lightProbesCoefficientsTextureHalfTexelSize.x, normalizedPos.x);
    normalizedPos.y = lerp(lightProbesCoefficientsTextureHalfTexelSize.y, 1.0f - lightProbesCoefficientsTextureHalfTexelSize.y, normalizedPos.y);
    normalizedPos.z = lerp(0, 1.0f - lightProbesCoefficientsTextureHalfTexelSize.z, GetBiasedNormalizedDepth(normalizedPos.z, Aura_DepthBiasReciproqualCoefficient));

    FP4 redColorCoefficients = lightProbesCoefficientsTexture.SampleLevel(_LinearClamp, saturate(normalizedPos), 0);
    normalizedPos.x += 1.0f / 3.0f;
    FP4 greenColorCoefficients = lightProbesCoefficientsTexture.SampleLevel(_LinearClamp, saturate(normalizedPos), 0);
    normalizedPos.x += 1.0f / 3.0f;
    FP4 blueColorCoefficients = lightProbesCoefficientsTexture.SampleLevel(_LinearClamp, saturate(normalizedPos), 0);
    
    viewDirection.xyz *= scattering; // Henyey-Greenstein phase function (https://bartwronski.files.wordpress.com/2014/08/bwronski_volumetric_fog_siggraph2014.pdf#page=55)
    return SHEvalLinearL0L1(viewDirection, redColorCoefficients, greenColorCoefficients, blueColorCoefficients);
}

