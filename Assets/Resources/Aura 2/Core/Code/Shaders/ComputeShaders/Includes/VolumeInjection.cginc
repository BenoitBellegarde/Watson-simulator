
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

uniform StructuredBuffer<VolumeData> volumeDataBuffer;
uniform uint volumeCount;
uniform Texture2DArray<FP4> texture2DMaskAtlasTexture;
uniform Texture3D<FP4> texture3DMaskAtlasTexture;
uniform FP3 texture3DMaskAtlasTextureSize;

FP GetShapeGradient(VolumeData volumeData, inout FP3 position)
{
    FP gradient = 1;

    BRANCH
	if (volumeData.shape == 1)
	{
		position = TransformPoint(position, ConvertMatrixFloatsToMatrix(volumeData.transform));
		gradient = 1.0f - saturate(position.y);
	}
	else if (volumeData.shape == 2)
	{
		position = TransformPoint(position, ConvertMatrixFloatsToMatrix(volumeData.transform));
		FP x = ClampedInverseLerp(-0.5f, -0.5f + volumeData.xNegativeFade, position.x) - ClampedInverseLerp(0.5f - volumeData.xPositiveFade, 0.5f, position.x);
		FP y = ClampedInverseLerp(-0.5f, -0.5f + volumeData.yNegativeFade, position.y) - ClampedInverseLerp(0.5f - volumeData.yPositiveFade, 0.5f, position.y);
		FP z = ClampedInverseLerp(-0.5f, -0.5f + volumeData.zNegativeFade, position.z) - ClampedInverseLerp(0.5f - volumeData.zPositiveFade, 0.5f, position.z);
		gradient = x * y * z;
	}
	else if (volumeData.shape == 3)
	{
		position = TransformPoint(position, ConvertMatrixFloatsToMatrix(volumeData.transform));
		gradient = ClampedInverseLerp(0.5f, 0.5f - volumeData.xPositiveFade * 0.5f, length(position));
	}
	else if (volumeData.shape == 4)
	{
		position = TransformPoint(position, ConvertMatrixFloatsToMatrix(volumeData.transform));
		FP y = ClampedInverseLerp(-0.5f, -0.5f + volumeData.yNegativeFade, position.y) - ClampedInverseLerp(0.5f - volumeData.yPositiveFade, 0.5f, position.y);
		FP xz = ClampedInverseLerp(0.5f, 0.5f - volumeData.xPositiveFade * 0.5f, length(position.xz));
		gradient = xz * y;
	}
	else if (volumeData.shape == 5)
	{
		position = TransformPoint(position, ConvertMatrixFloatsToMatrix(volumeData.transform));
		FP z = ClampedInverseLerp(1, 1.0f - volumeData.zPositiveFade * 2, position.z);
		FP xy = ClampedInverseLerp(0.5f, 0.5f - volumeData.xPositiveFade * 0.5f, length(position.xy / saturate(position.z)));
		gradient = xy * z;
	}

	return gradient;
}

void ComputeVolumeContribution(VolumeData volumeData, FP3 jitteredWorldPosition, FP3 unjitteredWorldPosition, inout FP density, inout FP scattering, inout FP3 color, inout FP globalIlluminationMask, inout FP lightProbesMultiplier, inout FP ambientLightingMultiplier, inout FP3 tint, inout FP directIlluminationMultiplier)
{
    FP gradient = GetShapeGradient(volumeData, jitteredWorldPosition);

    BRANCH
	if (gradient > 0)
	{	
        FP densityMask = 1.0f;
        FP scatteringMask = 1.0f;
        FP3 colorMask = FP3(1.0f, 1.0f, 1.0f);
        FP ambientMask = 1.0f;
		FP4 tintMask = FP4(1.0f, 1.0f, 1.0f, 1.0f);
		FP boostMask = 1.0f;
				
        BRANCH
        if (useTexture2DMasks && volumeData.texture2DMaskData.index > -1)
        {
            FP3 samplingPosition = TransformPoint(jitteredWorldPosition, ConvertMatrixFloatsToMatrix(volumeData.texture2DMaskData.transform));
            FP4 textureMask = texture2DMaskAtlasTexture.SampleLevel(_LinearRepeat, FP3(samplingPosition.xz + FP2(0.5f, 0.5f), volumeData.texture2DMaskData.index), 0); // Texture is by default projected downwards
        
            densityMask *= LevelValue(volumeData.densityTexture2DMaskLevelsParameters, textureMask.w);
            scatteringMask *= LevelValue(volumeData.scatteringTexture2DMaskLevelsParameters, textureMask.w);
            colorMask *= LevelValue(volumeData.colorTexture2DMaskLevelsParameters, textureMask.xyz);
            ambientMask *= LevelValue(volumeData.ambientTexture2DMaskLevelsParameters, textureMask.w);
			tintMask.xyz *= LevelValue(volumeData.tintTexture2DMaskLevelsParameters, textureMask.xyz);
			boostMask *= LevelValue(volumeData.boostTexture2DMaskLevelsParameters, textureMask.w);
		}
		
        BRANCH
        if (useTexture3DMasks && volumeData.texture3DMaskData.index > -1)
        {
            FP3 samplingPosition = GetCombinedTexture3dCoordinates(jitteredWorldPosition, texture3DMaskAtlasTextureSize.x, texture3DMaskAtlasTextureSize.z, (FP) volumeData.texture3DMaskData.index, ConvertMatrixFloatsToMatrix(volumeData.texture3DMaskData.transform));
            FP4 textureMask = texture3DMaskAtlasTexture.SampleLevel(_LinearClamp, samplingPosition, 0);
        
            densityMask *= LevelValue(volumeData.densityTexture3DMaskLevelsParameters, textureMask.w);
            scatteringMask *= LevelValue(volumeData.scatteringTexture3DMaskLevelsParameters, textureMask.w);
            colorMask *= LevelValue(volumeData.colorTexture3DMaskLevelsParameters, textureMask.xyz);
            ambientMask *= LevelValue(volumeData.ambientTexture3DMaskLevelsParameters, textureMask.w);
			tintMask.xyz *= LevelValue(volumeData.tintTexture3DMaskLevelsParameters, textureMask.xyz);
			boostMask *= LevelValue(volumeData.boostTexture3DMaskLevelsParameters, textureMask.w);
		}
        
        BRANCH
        if (useVolumesNoise && volumeData.noiseData.enable)
        {
            FP3 noisePosition = TransformPoint(unjitteredWorldPosition, ConvertMatrixFloatsToMatrix(volumeData.noiseData.transform));
            FP noiseMask = snoise(FP4(noisePosition, time * volumeData.noiseData.speed)) * 0.5f + 0.5f;

			densityMask *= LevelValue(volumeData.densityNoiseLevelsParameters, noiseMask);
            scatteringMask *= LevelValue(volumeData.scatteringNoiseLevelsParameters, noiseMask);
            colorMask *= LevelValue(volumeData.colorNoiseLevelsParameters, noiseMask);
            ambientMask *= LevelValue(volumeData.ambientNoiseLevelsParameters, noiseMask);
			tintMask.w *= LevelValue(volumeData.tintNoiseLevelsParameters, noiseMask);
			boostMask *= LevelValue(volumeData.boostNoiseLevelsParameters, noiseMask);
		}
        
        gradient = pow(gradient, volumeData.falloffExponent);
    
        BRANCH
	    if (volumeData.injectDensity)
	    {
		    density += volumeData.densityValue * gradient * densityMask;
	    }
    
        BRANCH
        if (volumeData.injectScattering)
        {
            scattering += -volumeData.scatteringValue * gradient * scatteringMask;
        }
    
        BRANCH
	    if (volumeData.injectColor)
	    {
	        color += volumeData.colorValue * gradient * colorMask;
        }
    
        BRANCH
        if (volumeData.injectAmbient)
        {
            ambientLightingMultiplier += volumeData.ambientLightingValue * gradient * ambientMask;
        }

		BRANCH
		if (volumeData.useAsLightProbesProxyVolume)
		{
			globalIlluminationMask = max(globalIlluminationMask, gradient);
			lightProbesMultiplier += volumeData.lightProbesMultiplier * gradient;
		}

		BRANCH
		if (volumeData.injectTint)
		{
			tint = lerp(tint, tint * tintMask.xyz * volumeData.tintColor, gradient * tintMask.w);
		}

		BRANCH
		if (volumeData.injectBoost)
		{
			directIlluminationMultiplier *= lerp( 1.0f, volumeData.boostValue, gradient * boostMask);
		}
	}
}

void ComputeVolumesInjection(FP3 jitteredWorldPosition, FP3 unjitteredWorldPosition, inout FP3 color, inout FP density, inout FP scattering, inout FP globalIlluminationMask, inout FP lightProbesMultiplier, inout FP ambientLightingMultiplier, inout FP3 tint, inout FP directIlluminationMultiplier)
{
	ALLOW_UAV_CONDITION
	for (uint i = 0; i < volumeCount; ++i)
	{
        ComputeVolumeContribution(volumeDataBuffer[i], jitteredWorldPosition, unjitteredWorldPosition, density, scattering, color, globalIlluminationMask, lightProbesMultiplier, ambientLightingMultiplier, tint, directIlluminationMultiplier);
    }

	density = max(0, density);
    scattering = saturate(scattering);
}