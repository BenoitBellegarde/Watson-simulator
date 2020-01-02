
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

///-----------------------------------------------------------------------------------------
///			Depth Accumulation functions
///-----------------------------------------------------------------------------------------
#include "DepthAccumulation.cginc"

///-----------------------------------------------------------------------------------------
///			Spherical Harmonics functions
///-----------------------------------------------------------------------------------------
#include "SphericalHarmonics.cginc"

///-----------------------------------------------------------------------------------------
///			Texture Sampling functions
///-----------------------------------------------------------------------------------------
#include "TextureSampling.cginc"

///-----------------------------------------------------------------------------------------
///			Noise functions
///-----------------------------------------------------------------------------------------
#include "Noise.cginc"

///-----------------------------------------------------------------------------------------
///			GetBiasedDepth
///			Bias the depth towards the camera
///-----------------------------------------------------------------------------------------
FP GetBiasedNormalizedDepth(FP normalizedDepth, FP biasCoefficient)
{
	// https://www.desmos.com/calculator/kwnyuioj2z
	FP inverseDepth = max(0.000001f, 1.0f - normalizedDepth);
	return 1.0f - pow(inverseDepth, biasCoefficient);
}

///-----------------------------------------------------------------------------------------
///			BiasNormalizedDepth
///			Bias the depth towards the camera
///-----------------------------------------------------------------------------------------
FP BiasNormalizedDepth(FP normalizedDepth)
{
	return GetBiasedNormalizedDepth(normalizedDepth, Aura_DepthBiasCoefficient);
}

///-----------------------------------------------------------------------------------------
///			ApplyDepthBiasToNormalizedPosition
///			Apply depth bias to normalized position
///-----------------------------------------------------------------------------------------
void ApplyDepthBiasToNormalizedPosition(inout FP4 normalizedPosition)
{
	normalizedPosition.w = normalizedPosition.z;
	normalizedPosition.z = BiasNormalizedDepth(normalizedPosition.w);
}

///-----------------------------------------------------------------------------------------
///			GetNormalizedLocalPosition
///			Gets the normalized coordinates from the thread id and the Aura_BufferTexelSize
///-----------------------------------------------------------------------------------------
FP3 GetNormalizedLocalPosition(uint3 id)
{
    return ((FP3)id + 0.5f) * Aura_BufferTexelSize.xyz;
}

///-----------------------------------------------------------------------------------------
///			GetIdFromLocalPosition
///			Gets the id from the normalized coordinates and the Aura_BufferTexelSize
///-----------------------------------------------------------------------------------------
uint3 GetIdFromLocalPosition(FP3 localPosition)
{
	return (uint3)floor(localPosition * Aura_BufferResolution.xyz);
}

///-----------------------------------------------------------------------------------------
///			GetNormalizedLocalLayerPosition
///			Gets the normalized position along width and height from the thread id and the Aura_BufferTexelSize
///-----------------------------------------------------------------------------------------
FP2 GetNormalizedLocalLayerPosition(uint2 idXY)
{
	return ((FP2)idXY + 0.5f) * Aura_BufferTexelSize.xy;
}

///-----------------------------------------------------------------------------------------
///			GetNormalizedLocalDepth
///			Gets the normalized depth from the thread id and the Aura_BufferTexelSize
///-----------------------------------------------------------------------------------------
FP GetNormalizedLocalDepth(uint idZ)
{
	return ((FP)idZ + 0.5f) * Aura_BufferTexelSize.z;
}

///-----------------------------------------------------------------------------------------
///			GetNormalizedLocalPositionWithDepthBias
///			Gets the volume normalized coordinates with a depth biased towards the camera
///-----------------------------------------------------------------------------------------
FP4 GetNormalizedLocalPositionWithDepthBias(uint3 id)
{
    FP3 normalizedLocalPos = GetNormalizedLocalPosition(id);
    FP biasedDepth = BiasNormalizedDepth(normalizedLocalPos.z);
    return FP4(normalizedLocalPos.xy, biasedDepth, normalizedLocalPos.z); 
}

///-----------------------------------------------------------------------------------------
///			GetNormalizedLocalPositionWithDepthBias
///			Gets the volume normalized coordinates with a depth biased towards the camera
///-----------------------------------------------------------------------------------------
FP GetNormalizedLocalDepthWithDepthBias(uint idZ)
{
    FP normalizedLocalDepth = GetNormalizedLocalDepth(idZ);
    FP biasedDepth = BiasNormalizedDepth(normalizedLocalDepth);
    return biasedDepth; 
}

///-----------------------------------------------------------------------------------------
///			GetCameraSpaceDepth
///			Gets the camera space depth from the normalized volume depth
///-----------------------------------------------------------------------------------------
FP GetCameraSpaceDepth(half normalizedDepth)
{
    return lerp(cameraRanges.x, cameraRanges.y, normalizedDepth);
}

///-----------------------------------------------------------------------------------------
///			GetWorldPosition
///			Gets the world position from normalized coordinates and the corners' position of the frustum
///-----------------------------------------------------------------------------------------
FP3 GetWorldPosition(FP3 normalizedLocalPos, FP4 cornersPosition[8])
{
	FP3 AtoB = lerp(cornersPosition[0].xyz, cornersPosition[1].xyz, normalizedLocalPos.x);
	FP3 DtoC = lerp(cornersPosition[3].xyz, cornersPosition[2].xyz, normalizedLocalPos.x);
	FP3 nearBottomToTop = lerp(DtoC, AtoB, normalizedLocalPos.y);

	FP3 EtoF = lerp(cornersPosition[4].xyz, cornersPosition[5].xyz, normalizedLocalPos.x);
	FP3 HtoG = lerp(cornersPosition[7].xyz, cornersPosition[6].xyz, normalizedLocalPos.x);
	FP3 farBottomToTop = lerp(HtoG, EtoF, normalizedLocalPos.y);

	FP3 worldPosition = lerp(nearBottomToTop, farBottomToTop, normalizedLocalPos.z);

	return worldPosition;
}

///-----------------------------------------------------------------------------------------
///			TransformPositions
///			Gets the 3d texture coordinates to be used with combined Texture 3D
///-----------------------------------------------------------------------------------------
FP3 TransformPoint(FP3 p, FP4x4 transform)
{
	return mul(transform, FP4(p, 1)).xyz;
}

///-----------------------------------------------------------------------------------------
///			GetNormalizedYawPitchFromNormalizedVector
///			Compute normalized Yaw Pitch angles from a normalized direction vector
///-----------------------------------------------------------------------------------------
FP2 GetNormalizedYawPitchFromNormalizedVector(FP3 NormalizedVector)
{
	FP Yaw = (atan2(NormalizedVector.z, NormalizedVector.x) * invPi + 1.0f) * 0.5f;
	FP Pitch = (asin(NormalizedVector.y) * twoInvPi + 1.0f) * 0.5f;

	return FP2(Yaw, Pitch);
}
///-----------------------------------------------------------------------------------------
///			GetNormalizedVectorFromNormalizedYawPitch
///			Compute a normalized direction vector from normalized Yaw Pitch angles
///-----------------------------------------------------------------------------------------
FP3 GetNormalizedVectorFromNormalizedYawPitch(FP Yaw, FP Pitch)
{
	Yaw = (Yaw * 2.0f - 1.0f) * pi;
	Pitch = (Pitch * 2.0f - 1.0f) * halfPi;
	return FP3(cos(Yaw) * cos(Pitch), sin(Pitch), cos(Pitch) * sin(Yaw));
}
FP3 GetNormalizedVectorFromNormalizedYawPitch(FP2 YawPitch)
{
	return GetNormalizedVectorFromNormalizedYawPitch(YawPitch.x, YawPitch.y);
}

///-----------------------------------------------------------------------------------------
///			GetCombinedTexture3dCoordinates
///			Gets the 3d texture coordinates to be used with combined Texture 3D
///-----------------------------------------------------------------------------------------
FP3 GetCombinedTexture3dCoordinates(FP3 positions, FP textureWidth, FP textureDepth, FP index, FP4x4 transform)
{
	FP textureCount = textureDepth / textureWidth;
	FP borderClamp = 0.5f / textureWidth;
	FP offset = index / textureCount;

    FP3 textureCoordinates = frac(TransformPoint(positions, transform) + +FP3(0.5f, 0.5f, 0.5f));
	textureCoordinates.z /= textureCount;
	textureCoordinates.z += offset;
	textureCoordinates.z = clamp(offset + borderClamp, offset + 1.0f - borderClamp, textureCoordinates.z);

	return textureCoordinates;
}

///-----------------------------------------------------------------------------------------
///			GetExponentialValue
///			Gets "exponentialized" value based on 0->1 gradient
///-----------------------------------------------------------------------------------------
FP GetExponentialValue(FP value)
{
	return pow(abs(value), e);
}
///-----------------------------------------------------------------------------------------
///			GetLogarithmicValue
///			Gets "logarithmized" value based on 0->1 gradient
///-----------------------------------------------------------------------------------------
FP GetLogarithmicValue(FP value)
{
	return pow(abs(value), n);
}

///-----------------------------------------------------------------------------------------
///			(Clamped)InverseLerp
///			Gets the linear gradient, returning where the value locates between the low and hi thresholds
///-----------------------------------------------------------------------------------------
FP InverseLerp(FP lowThreshold, FP hiThreshold, FP value)
{
	return (value - lowThreshold) / (hiThreshold - lowThreshold);
}
FP ClampedInverseLerp(FP lowThreshold, FP hiThreshold, FP value)
{
	return saturate(InverseLerp(lowThreshold, hiThreshold, value));
}
FP3 InverseLerp(FP lowThreshold, FP hiThreshold, FP3 value)
{
	return (value - lowThreshold) / (hiThreshold - lowThreshold);
}
FP3 ClampedInverseLerp(FP lowThreshold, FP hiThreshold, FP3 value)
{
	return saturate(InverseLerp(lowThreshold, hiThreshold, value));
}

///-----------------------------------------------------------------------------------------
///			LevelValue
///			Filters value between "levelLowThreshold" and "levelHiThreshold", contrast by "contrast" factor, then rescale the result between "outputLowValue" and "outputHiValue". Similar to the Levels adjustment tool in Photoshop.
///-----------------------------------------------------------------------------------------
FP LevelValue(LevelsData levelsParameters, FP value)
{
	FP tmp = ClampedInverseLerp(levelsParameters.levelLowThreshold, levelsParameters.levelHiThreshold, value);
	tmp = saturate(lerp(0.5f, tmp, levelsParameters.contrast));
	tmp = lerp(levelsParameters.outputLowValue, levelsParameters.outputHiValue, tmp);

	return tmp;
}
FP3 LevelValue(LevelsData levelsParameters, FP3 value)
{
	FP3 tmp;
	tmp.x = LevelValue(levelsParameters, value.x);
	tmp.y = LevelValue(levelsParameters, value.y);
	tmp.z = LevelValue(levelsParameters, value.z);

	return tmp;
}

//-----------------------------------------------------------------------------------------
//			GetScatteringFactor
//			Lights phase function. Returns the anisotropic scattering factor.
//			http://renderwonk.com/publications/s2003-course/premoze1/notes-premoze.pdf
//-----------------------------------------------------------------------------------------
FP HenyeyGreensteinPhaseFunction(FP cosAngle, FP coefficient, FP squareCoefficient)
{	
	
	FP topPart = 1.0f - squareCoefficient;
	FP bottomPart = sqrt(1.0f + squareCoefficient - 2.0f * coefficient * cosAngle);
	bottomPart *= bottomPart * bottomPart;
	//FP bottomPart = 1.0f + squareCoefficient - 2.0f * coefficient * cosAngle; // More controllable
	//FP bottomPart = pow(1.0f + squareCoefficient - 2.0f * coefficient * cosAngle, 0.75f); // More controllable
    bottomPart = rcp(bottomPart);
    return topPart * bottomPart;
}
FP CornetteShanksPhaseFunction(FP cosAngle, FP coefficient, FP squareCoefficient)
{
	return (3.0f / 2.0f) * ((1.0f + cosAngle * cosAngle) / (2.0f + squareCoefficient)) * HenyeyGreensteinPhaseFunction(cosAngle, coefficient, squareCoefficient);
}
FP GetScatteringFactor(FP3 lightVector, FP3 viewVector, bool useScattering, bool useDefaultScattering, FP coefficient, FP scatteringOverride)
{
	bool shouldComputeScattering = useDefaultScattering ? useScattering : (scatteringOverride > -2 ? true : false);
	BRANCH
	if (shouldComputeScattering)
	{
		FP cosAngle = saturate(dot(-lightVector, viewVector));
		BRANCH
		if (scatteringOverride > -1)
		{
			coefficient = scatteringOverride;
		}

		FP squareCoefficient = coefficient * coefficient;
		return quarterPi * CornetteShanksPhaseFunction(cosAngle, coefficient, squareCoefficient);
	}
	else
	{
		return 1.0f;
	}
}

//-----------------------------------------------------------------------------------------
//			GetLinearDepth
//			Linearize depth/shadow maps
//			
//			Params : Values used to linearize the Z buffer (http://www.humus.name/temp/Linearize%20depth.txt)
//          x = 1-far/near
//          y = far/near
//          z = x/far
//          w = y/far
//          or in case of a reversed depth buffer (UNITY_REVERSED_Z is 1) -> Our case
//          x = -1+far/near
//          y = 1
//          z = x/far
//          w = 1/far
//-----------------------------------------------------------------------------------------
FP GetLinearDepthOrtho(FP depth, FP2 params)
{
	return lerp(params.x, params.y, 1.0f - depth);
}
FP GetLinearDepth(FP depth, FP4 params)
{
	return 1.0f / (params.z * depth + params.w);
}
FP GetLinearDepth(FP depth, FP4 params, bool isOrthographic)
{
	if (isOrthographic)
	{
		return GetLinearDepthOrtho(depth, params.xy);
	}
	else
	{
#if defined(SHADER_STAGE_COMPUTE)
		return GetLinearDepth(depth, params); // Custom replica of the original function as it is inaccessible in compute
#else
		return LinearEyeDepth(depth); // Call original function if not in a compute
#endif
	}
}
FP GetLinearDepth01(FP depth, FP4 params)
{
	return 1.0f / (params.z * depth + params.y);
}

//-----------------------------------------------------------------------------------------
//			GetLightDistanceAttenuation
//			Computes the distance attenuation factor for Point and Spot lights
//-----------------------------------------------------------------------------------------
half GetLightDistanceAttenuation(FP2 distanceFalloffParameters, half normalizedDistance)
{
    FP distanceAttenuation = ClampedInverseLerp(1.0f, distanceFalloffParameters.x, normalizedDistance);
    distanceAttenuation = pow(distanceAttenuation, distanceFalloffParameters.y);

    return distanceAttenuation;
}

//-----------------------------------------------------------------------------------------
//			ConvertMatrixFloatsToMatrix
//			Converts a MatrixFloats struct into a FP4x4 matrix
//-----------------------------------------------------------------------------------------
half4x4 ConvertMatrixFloatsToMatrix(MatrixFloats data)
{
    return half4x4(FP4(data.a.x, data.b.x, data.c.x, data.d.x), FP4(data.a.y, data.b.y, data.c.y, data.d.y), FP4(data.a.z, data.b.z, data.c.z, data.d.z), FP4(data.a.w, data.b.w, data.c.w, data.d.w));
}

//-----------------------------------------------------------------------------------------
//			GetFlattenedIndex
//			Computes the thread id according to its position in the total dispatch and the amount of dispatched threads
//-----------------------------------------------------------------------------------------
uint GetFlattenedIndex(uint3 id, uint3 size)
{
	return id.z * size.x * size.y + id.y * size.x + id.x;
}
uint GetFlattenedIndex(uint2 id, uint2 size)
{
	return id.y * size.x + id.x;
}