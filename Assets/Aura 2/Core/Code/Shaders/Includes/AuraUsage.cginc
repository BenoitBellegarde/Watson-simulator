
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

#include "Common.cginc"
#include "BlueNoise.cginc"

FP4 Aura_FrustumRanges;
sampler3D Aura_VolumetricDataTexture;
sampler3D Aura_VolumetricLightingTexture;

//////////// Helper functions
FP GetLinearEyeDepth(FP depth)
{
	return GetLinearDepth(depth, _ProjectionParams.yzzz, unity_OrthoParams.w);
}

FP Aura2_RescaleDepth(FP depth)
{
	FP rescaledDepth = InverseLerp(Aura_FrustumRanges.x, Aura_FrustumRanges.y, depth);
    return GetBiasedNormalizedDepth(rescaledDepth, Aura_DepthBiasReciproqualCoefficient);
}

FP3 Aura2_GetFrustumSpaceCoordinates(FP4 inVertex)
{
	FP4 clipPos = UnityObjectToClipPos(inVertex);

	FP4 frustumPosition = ComputeScreenPos(clipPos);
	frustumPosition.xy /= frustumPosition.w;

	// Perspective depth
	FP perspectiveFrustumZ = -UnityObjectToViewPos(inVertex).z;
	// Orthographic depth
	FP orthographicFrustumZ = lerp(_ProjectionParams.y, _ProjectionParams.z, (1.0f - frustumPosition.z));
	
	frustumPosition.z = lerp(perspectiveFrustumZ, orthographicFrustumZ, unity_OrthoParams.w);

    return frustumPosition;
}

//////////// Lighting
FP4 Aura2_SampleDataTexture(FP3 position)
{
    return SampleTexture3D(Aura_VolumetricDataTexture, position, Aura_BufferTexelSize);
}
FP4 Aura2_GetData(FP3 screenSpacePosition)
{
    return Aura2_SampleDataTexture(FP3(screenSpacePosition.xy, Aura2_RescaleDepth(screenSpacePosition.z)));
}

void Aura2_ApplyLighting(inout FP3 colorToApply, FP3 screenSpacePosition, FP lightingFactor)
{
	//////////////////// Start : AURA_USE_DITHERING
	#if defined(AURA_USE_DITHERING)
    screenSpacePosition.xy += GetBlueNoise(screenSpacePosition.xy, 0).xy;
	#endif
	//////////////////// End : AURA_USE_DITHERING

	FP3 lightingValue = Aura2_GetData(screenSpacePosition).xyz;
	colorToApply *= lightingValue * lightingFactor;
}

//////////// Fog
FP4 Aura2_SampleFogTexture(FP3 position)
{
    return SampleTexture3D(Aura_VolumetricLightingTexture, position, Aura_BufferTexelSize);
}

FP4 Aura2_GetFogValue(FP3 screenSpacePosition)
{
    return Aura2_SampleFogTexture(FP3(screenSpacePosition.xy, Aura2_RescaleDepth(screenSpacePosition.z)));
}

void Aura2_ApplyFog(inout FP3 colorToApply, FP3 screenSpacePosition, FP4 fogValue)
{
	colorToApply = (colorToApply * fogValue.w) + fogValue.xyz;
}
void Aura2_ApplyFog(inout FP3 colorToApply, FP3 screenSpacePosition)
{
	//////////////////// Start : AURA_USE_DITHERING
	#if defined(AURA_USE_DITHERING)
    screenSpacePosition.xyz += GetBlueNoise(screenSpacePosition.xy, 1).xyz;
	#endif
	//////////////////// End : AURA_USE_DITHERING

    FP4 fogValue = Aura2_GetFogValue(screenSpacePosition);
    Aura2_ApplyFog(colorToApply, screenSpacePosition, fogValue);
}

// From https://github.com/Unity-Technologies/VolumetricLighting/blob/master/Assets/Scenes/Materials/StandardAlphaBlended-VolumetricFog.shader
void Aura2_ApplyFog(inout FP4 colorToApply, FP3 screenSpacePosition, FP4 fogValue)
{
	// Always apply fog attenuation - also in the forward add pass.
    colorToApply.xyz *= fogValue.w;

	// Alpha premultiply mode (used with alpha and Standard lighting function, or explicitly alpha:premul)
	#if _ALPHAPREMULTIPLY_ON
	fogValue.xyz *= colorToApply.w;
	#endif

	// Add inscattering only once, so in forward base, but not forward add.
	#ifndef UNITY_PASS_FORWARDADD
    colorToApply.xyz += fogValue.xyz;
	#endif
}
void Aura2_ApplyFog(inout FP4 colorToApply, FP3 screenSpacePosition)
{    
	//////////////////// Start : AURA_USE_DITHERING
	#if defined(AURA_USE_DITHERING)
    screenSpacePosition.xy += GetBlueNoise(screenSpacePosition.xy, 2).xy;
	#endif
	//////////////////// End : AURA_USE_DITHERING

    FP4 fogValue = Aura2_GetFogValue(screenSpacePosition);
    Aura2_ApplyFog(colorToApply, screenSpacePosition, fogValue);
} 