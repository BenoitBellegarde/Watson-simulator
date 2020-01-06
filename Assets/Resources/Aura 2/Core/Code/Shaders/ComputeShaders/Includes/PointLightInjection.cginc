
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

uniform uint pointLightCount;
uniform StructuredBuffer<PointLightParameters> pointLightDataBuffer;
#if UNITY_VERSION >= 201730
uniform Texture2DArray<FP3> pointShadowMapsArray;
#else
uniform Texture2DArray<FP2> pointShadowMapsArray;
#endif
uniform Texture2DArray<FP> pointCookieMapsArray;

FP SamplePointShadowMap(PointLightParameters lightParameters, FP3 samplingDirection, FP2 polarCoordinates)
{
	#if UNITY_VERSION >= 201730
	FP3 shadowMapValue = pointShadowMapsArray.SampleLevel(_LinearClamp, FP3(polarCoordinates, lightParameters.shadowMapIndex), 0).xyz;
	FP4 lightProjectionParams = FP4( lightParameters.lightProjectionParameters, shadowMapValue.yz); // From UnityShaderVariables.cginc:114        
	float3 absVec = abs(samplingDirection);
	// From UnityShadowLibrary.cginc:119
    float dominantAxis = max(max(absVec.x, absVec.y), absVec.z);
		dominantAxis = max(0.00001, dominantAxis - lightProjectionParams.z);
		dominantAxis *= lightProjectionParams.w;
    FP biasedReferenceDistance = -lightProjectionParams.x + lightProjectionParams.y/dominantAxis;
		biasedReferenceDistance = 1.0f - biasedReferenceDistance;
	return step(shadowMapValue.x, biasedReferenceDistance);
	#else
	FP2 shadowMapValue = pointShadowMapsArray.SampleLevel(_LinearClamp, FP3(polarCoordinates, lightParameters.shadowMapIndex), 0).xy;
    FP biasedReferenceDistance = length(samplingDirection) * shadowMapValue.y;
        biasedReferenceDistance *= 0.97f; // bias
	return step(biasedReferenceDistance, shadowMapValue.x);
	#endif
}

void ComputePointLightInjection(PointLightParameters lightParameters, FP3 worldPosition, FP3 viewVector, inout FP3 accumulationColor, bool useScattering, FP scattering)
{
	FP3 lightVector = worldPosition - lightParameters.lightPosition;
    FP3 normalizedLightVector = normalize(lightVector);
	FP dist = distance(lightParameters.lightPosition, worldPosition);

	BRANCH
	if (dist > lightParameters.lightRange)
	{
		return; 
	}
	else
	{
		FP normalizedDistance = saturate(dist / lightParameters.lightRange);
		FP attenuation = GetScatteringFactor(normalizedLightVector, viewVector, useScattering, lightParameters.useDefaultScattering, scattering, lightParameters.scatteringOverride);
	
		attenuation *= GetLightDistanceAttenuation(lightParameters.distanceFalloffParameters, normalizedDistance);
		
        FP2 polarCoordinates = GetNormalizedYawPitchFromNormalizedVector(normalizedLightVector);
		
		BRANCH
        if (usePointLightsShadows && lightParameters.shadowMapIndex > -1)
		{
			FP shadowAttenuation = SamplePointShadowMap(lightParameters, lightVector, polarCoordinates);
			shadowAttenuation = lerp(lightParameters.shadowStrength, 1.0f, shadowAttenuation);
		
			attenuation *= shadowAttenuation;
		}

		BRANCH
        if (useLightsCookies && lightParameters.cookieMapIndex > -1)
		{        
			FP cookieMapValue = pointCookieMapsArray.SampleLevel(_LinearClamp, FP3(polarCoordinates, lightParameters.cookieMapIndex), 0).x;
			cookieMapValue = lerp(1, cookieMapValue, pow(smoothstep(lightParameters.cookieParameters.x, lightParameters.cookieParameters.y, normalizedDistance), lightParameters.cookieParameters.z));
        
			attenuation *= cookieMapValue;
		}

		accumulationColor += lightParameters.color * attenuation;
	}
}

void ComputePointLightsInjection(FP3 worldPosition, FP3 viewVector, inout FP3 accumulationColor, bool useScattering, FP scattering)
{
	ALLOW_UAV_CONDITION
	for (uint i = 0; i < pointLightCount; ++i)
	{
        ComputePointLightInjection(pointLightDataBuffer[i], worldPosition, viewVector, accumulationColor, useScattering, scattering);
    }
}