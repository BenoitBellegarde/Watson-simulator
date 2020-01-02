
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

uniform uint spotLightCount;
uniform StructuredBuffer<SpotLightParameters> spotLightDataBuffer;
uniform Texture2DArray<FP> spotShadowMapsArray;
uniform Texture2DArray<FP> spotCookieMapsArray;

FP SampleSpotShadowMap(SpotLightParameters lightParameters, FP4 shadowPosition, FP2 offset)
{
	// TODO : CHECK FOR OFFSET (BECAUSE OF SHADOW BIAS)
	FP shadowMapValue = 1.0f - spotShadowMapsArray.SampleLevel(_LinearClamp, FP3((shadowPosition.xy + offset) / shadowPosition.w, lightParameters.shadowMapIndex), 0);
	return step(shadowPosition.z / shadowPosition.w, shadowMapValue);
}

void ComputeSpotLightInjection(SpotLightParameters lightParameters, FP3 worldPosition, FP3 viewVector, inout FP3 accumulationColor, bool useScattering, FP scattering)
{
	FP3 lightVector = normalize(worldPosition - lightParameters.lightPosition);
	FP cosAngle = dot(lightParameters.lightDirection.xyz, lightVector);
	FP dist = distance(lightParameters.lightPosition.xyz, worldPosition);

    BRANCH
	if (dist > lightParameters.lightRange || cosAngle < lightParameters.lightCosHalfAngle)
	{
		return;
	}
	else
	{
        FP attenuation = GetScatteringFactor(lightVector, viewVector, useScattering, lightParameters.useDefaultScattering, scattering, lightParameters.scatteringOverride);

		FP4 lightPos = mul(ConvertMatrixFloatsToMatrix(lightParameters.worldToShadowMatrix), FP4(worldPosition, 1));
		FP normalizedDistance = saturate(lightPos.z / lightParameters.lightRange);
        
		attenuation *= GetLightDistanceAttenuation(lightParameters.distanceFalloffParameters, normalizedDistance);
        
		FP angleAttenuation = 1;
		angleAttenuation = smoothstep(lightParameters.lightCosHalfAngle, lerp(1, lightParameters.lightCosHalfAngle, lightParameters.angularFalloffParameters.x), cosAngle);
		angleAttenuation = pow(angleAttenuation, lightParameters.angularFalloffParameters.y);
		attenuation *= angleAttenuation;
        
        BRANCH
        if (useSpotLightsShadows && lightParameters.shadowMapIndex > -1)
		{
			FP shadowAttenuation = SampleSpotShadowMap(lightParameters, lightPos, 0);
			shadowAttenuation = lerp(lightParameters.shadowStrength, 1.0f, shadowAttenuation);
			
			attenuation *= shadowAttenuation;
		}
        
		BRANCH
        if (useLightsCookies &&  lightParameters.cookieMapIndex > -1)
		{        
			FP cookieMapValue = spotCookieMapsArray.SampleLevel(_LinearRepeat, FP3(lightPos.xy / lightPos.w, lightParameters.cookieMapIndex), 0).x;        
            cookieMapValue = lerp(1, cookieMapValue, pow(smoothstep(lightParameters.cookieParameters.x, lightParameters.cookieParameters.y, normalizedDistance), lightParameters.cookieParameters.z));
        
			attenuation *= cookieMapValue;
		}
        
		accumulationColor += lightParameters.color * attenuation;
	}
}

void ComputeSpotLightsInjection(FP3 worldPosition, FP3 viewVector, inout FP3 accumulationColor, bool useScattering, FP scattering)
{
	ALLOW_UAV_CONDITION
	for (uint i = 0; i < spotLightCount; ++i)
	{
        ComputeSpotLightInjection(spotLightDataBuffer[i], worldPosition, viewVector, accumulationColor, useScattering, scattering);
    }
}