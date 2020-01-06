
/***************************************************************************
*                                                                          *
*  Copyright (c) Rapha�l Ernaelsten (@RaphErnaelsten)                      *
*  All Rights Reserved.                                                    *
*                                                                          *
*  NOTICE: Aura 2 is a commercial project.                                 * 
*  All information contained herein is, and remains the property of        *
*  Rapha�l Ernaelsten.                                                     *
*  The intellectual and technical concepts contained herein are            *
*  proprietary to Rapha�l Ernaelsten and are protected by copyright laws.  *
*  Dissemination of this information or reproduction of this material      *
*  is strictly forbidden.                                                  *
*                                                                          *
***************************************************************************/

uniform uint directionalLightCount;
uniform StructuredBuffer<DirectionalLightParameters> directionalLightDataBuffer;
uniform Texture2DArray<FP> directionalShadowMapsArray;
uniform Texture2DArray<FP4> directionalShadowDataArray;
uniform Texture2DArray<FP> directionalCookieMapsArray;

#if defined(DIRECTIONAL_LIGHTS_SHADOWS_FOUR_CASCADES)
	#define GET_SHADOW_CASCADES(worldPos, shadowData, depth) GetCascadeWeights_FourCascades(worldPos, shadowData)
	#define GET_SHADOW_COORDS(worldPos, shadowData, depth) GetCascadeShadowCoord_FourCascades(worldPos, GET_SHADOW_CASCADES(worldPos, shadowData, depth), shadowData)

	inline FP4 GetCascadeWeights_FourCascades(FP3 worldPos, DirectionalShadowData shadowData)
	{
		FP3 fromCenterA = worldPos - shadowData.shadowSplitSpheres[0].xyz;
		FP3 fromCenterB = worldPos - shadowData.shadowSplitSpheres[1].xyz;
		FP3 fromCenterC = worldPos - shadowData.shadowSplitSpheres[2].xyz;
		FP3 fromCenterD = worldPos - shadowData.shadowSplitSpheres[3].xyz;
		FP4 squareDistances = FP4(dot(fromCenterA, fromCenterA), dot(fromCenterB, fromCenterB), dot(fromCenterC, fromCenterC), dot(fromCenterD, fromCenterD));
		FP4 weights = FP4(squareDistances >= shadowData.shadowSplitSqRadii);

		return weights;
	}

	inline FP4 GetCascadeShadowCoord_FourCascades(FP3 worldPos, FP4 cascadeWeights, DirectionalShadowData shadowData)
	{
		return mul(shadowData.world2Shadow[(int)dot(cascadeWeights, FP4(1, 1, 1, 1))], FP4(worldPos, 1));
	}
#elif defined(DIRECTIONAL_LIGHTS_SHADOWS_TWO_CASCADES)
	#define GET_SHADOW_CASCADES(worldPos, shadowData, depth) GetCascadeWeights_TwoCascades(worldPos, shadowData, depth)
	#define GET_SHADOW_COORDS(worldPos, shadowData, depth) GetCascadeShadowCoord_TwoCascades(worldPos, GET_SHADOW_CASCADES(worldPos, shadowData, depth), shadowData)

	inline FP4 GetCascadeWeights_TwoCascades(FP3 worldPos, DirectionalShadowData shadowData, FP depth)
	{
		FP4 zNear = FP4(depth >= shadowData.lightSplitsNear);
		FP4 zFar = FP4(depth < shadowData.lightSplitsFar);
		FP4 weights = zNear * zFar;

		return weights;
	}

	inline FP4 GetCascadeShadowCoord_TwoCascades(FP3 worldPos, FP4 cascadeWeights, DirectionalShadowData shadowData)
	{
		FP3 sc0 = mul((shadowData.world2Shadow[0]), FP4(worldPos,1)).xyz;
		FP3 sc1 = mul((shadowData.world2Shadow[1]), FP4(worldPos,1)).xyz;
		FP3 sc2 = mul((shadowData.world2Shadow[2]), FP4(worldPos,1)).xyz;
		FP3 sc3 = mul((shadowData.world2Shadow[3]), FP4(worldPos,1)).xyz;
		FP4 shadowMapCoordinate = FP4(sc0 * cascadeWeights.x + sc1 * cascadeWeights.y + sc2 * cascadeWeights.z + sc3 * cascadeWeights.w, 1);

		FP  noCascadeWeights = 1 - dot(cascadeWeights, FP4(1, 1, 1, 1));
		shadowMapCoordinate.z += noCascadeWeights;

		return shadowMapCoordinate;
	}
#elif defined(DIRECTIONAL_LIGHTS_SHADOWS_ONE_CASCADE)
	#define GET_SHADOW_COORDS(worldPos, shadowData, depth) GetCascadeShadowCoord_OneCascade(worldPos, shadowData)

	inline FP4 GetCascadeShadowCoord_OneCascade(FP3 worldPos, DirectionalShadowData shadowData)
	{
		return mul((shadowData.world2Shadow[0]), FP4(worldPos, 1));
	}
#else
    #define GET_SHADOW_COORDS(worldPos, shadowData, depth) FP4(0,0,0,0);
#endif

FP SampleShadowMap(FP3 worldPosition, DirectionalShadowData shadowData, DirectionalLightParameters lightParameters, FP depth)
{
	FP4 samplePos = GET_SHADOW_COORDS(worldPosition, shadowData, depth);
	FP shadowMapValue = directionalShadowMapsArray.SampleLevel(_LinearRepeat, FP3(samplePos.xy, lightParameters.shadowmapIndex), 0).x;

	return step(samplePos.z, shadowMapValue);
}

void ComputeShadow(inout FP attenuation, DirectionalLightParameters lightParameters, FP3 worldPosition, FP depth)
{
        DirectionalShadowData shadowData;
		shadowData.shadowSplitSqRadii =		directionalShadowDataArray[int3(0, 0, lightParameters.shadowmapIndex)];
		shadowData.lightSplitsNear =		directionalShadowDataArray[int3(1, 0, lightParameters.shadowmapIndex)];
		shadowData.lightSplitsFar =			directionalShadowDataArray[int3(2, 0, lightParameters.shadowmapIndex)];
		shadowData.shadowSplitSpheres[0] =	directionalShadowDataArray[int3(3, 0, lightParameters.shadowmapIndex)];
		shadowData.shadowSplitSpheres[1] =	directionalShadowDataArray[int3(4, 0, lightParameters.shadowmapIndex)];
		shadowData.shadowSplitSpheres[2] =	directionalShadowDataArray[int3(5, 0, lightParameters.shadowmapIndex)];
		shadowData.shadowSplitSpheres[3] =	directionalShadowDataArray[int3(6, 0, lightParameters.shadowmapIndex)];
		shadowData.world2Shadow[0][0] =		directionalShadowDataArray[int3(7, 0, lightParameters.shadowmapIndex)];
		shadowData.world2Shadow[0][1] =		directionalShadowDataArray[int3(8, 0, lightParameters.shadowmapIndex)];
		shadowData.world2Shadow[0][2] =		directionalShadowDataArray[int3(9, 0, lightParameters.shadowmapIndex)];
		shadowData.world2Shadow[0][3] =		directionalShadowDataArray[int3(10, 0, lightParameters.shadowmapIndex)];
		shadowData.world2Shadow[1][0] =		directionalShadowDataArray[int3(11, 0, lightParameters.shadowmapIndex)];
		shadowData.world2Shadow[1][1] =		directionalShadowDataArray[int3(12, 0, lightParameters.shadowmapIndex)];
		shadowData.world2Shadow[1][2] =		directionalShadowDataArray[int3(13, 0, lightParameters.shadowmapIndex)];
		shadowData.world2Shadow[1][3] =		directionalShadowDataArray[int3(14, 0, lightParameters.shadowmapIndex)];
		shadowData.world2Shadow[2][0] =		directionalShadowDataArray[int3(15, 0, lightParameters.shadowmapIndex)];
		shadowData.world2Shadow[2][1] =		directionalShadowDataArray[int3(16, 0, lightParameters.shadowmapIndex)];
		shadowData.world2Shadow[2][2] =		directionalShadowDataArray[int3(17, 0, lightParameters.shadowmapIndex)];
		shadowData.world2Shadow[2][3] =		directionalShadowDataArray[int3(18, 0, lightParameters.shadowmapIndex)];
		shadowData.world2Shadow[3][0] =		directionalShadowDataArray[int3(19, 0, lightParameters.shadowmapIndex)];
		shadowData.world2Shadow[3][1] =		directionalShadowDataArray[int3(20, 0, lightParameters.shadowmapIndex)];
		shadowData.world2Shadow[3][2] =		directionalShadowDataArray[int3(21, 0, lightParameters.shadowmapIndex)];
		shadowData.world2Shadow[3][3] =		directionalShadowDataArray[int3(22, 0, lightParameters.shadowmapIndex)];
		shadowData.lightShadowData =		directionalShadowDataArray[int3(23, 0, lightParameters.shadowmapIndex)];
        
		FP shadowAttenuation = SampleShadowMap(worldPosition, shadowData, lightParameters, depth);
		shadowAttenuation = lerp(shadowData.lightShadowData.x, 1.0f, 1.0f - shadowAttenuation);

		attenuation *= shadowAttenuation;
}

FP SampleCookieMapArray(FP2 textureCoordinates, int index)
{
	return directionalCookieMapsArray.SampleLevel(_LinearRepeat, FP3(textureCoordinates, index), 0).x;
}

void ComputeDirectionalLightInjection(DirectionalLightParameters lightParameters, FP3 worldPosition, FP distanceToCam, FP3 viewVector, inout FP3 accumulationColor, bool useScattering, FP scattering)
{
    FP scatteringFactor = GetScatteringFactor(lightParameters.lightDirection, viewVector, useScattering, lightParameters.useDefaultScattering, scattering, lightParameters.scatteringOverride);
	FP attenuation = 1.0f;
	    
    FP3 lightPos = mul(ConvertMatrixFloatsToMatrix(lightParameters.worldToLightMatrix), FP4(worldPosition, 1)).xyz;
	
    BRANCH
	if (useDirectionalLightsShadows && lightParameters.shadowmapIndex > -1)
	{
		ComputeShadow(attenuation, lightParameters, worldPosition, distanceToCam);
	}
	
    BRANCH
    if (useLightsCookies && lightParameters.cookieMapIndex > -1)
	{
		lightPos.xy /= lightParameters.cookieParameters.x;
		lightPos.xy += 0.5;
        BRANCH
		if (lightParameters.cookieParameters.y > 0)
		{
			lightPos.xy = saturate(lightPos.xy);
		}
		lightPos.xy = frac(lightPos.xy);
    
		FP cookieMapValue = SampleCookieMapArray(lightPos.xy, lightParameters.cookieMapIndex).x;
    
		attenuation *= cookieMapValue; 
	}

    FP3 color = lerp(lightParameters.outOfPhaseColor * lightParameters.enableOutOfPhaseColor, lightParameters.color * scatteringFactor, saturate(scatteringFactor));

	accumulationColor += color * attenuation;
}

void ComputeDirectionalLightsInjection(FP3 worldPosition, FP distanceToCam, FP3 viewVector, inout FP3 accumulationColor, bool useScattering, FP scattering)
{
	ALLOW_UAV_CONDITION
	for (uint i = 0; i < directionalLightCount; ++i)
	{
        ComputeDirectionalLightInjection(directionalLightDataBuffer[i], worldPosition, distanceToCam, viewVector, accumulationColor, useScattering, scattering);
    }
}