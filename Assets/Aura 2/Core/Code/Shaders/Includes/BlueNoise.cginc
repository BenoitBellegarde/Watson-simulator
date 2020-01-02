
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

#if defined(SHADER_STAGE_FRAGMENT)
UNITY_DECLARE_TEX2DARRAY(_blueNoiseTexturesArray);
#endif

FP4 GetBlueNoise(FP2 screenPos, int idOffset)
{
#if defined(SHADER_STAGE_FRAGMENT)
	const FP blueNoiseTexturesSize = 64;
	FP3 blueNoiseSamplingPosition = FP3(fmod(screenPos * _ScreenParams.xy * rcp(blueNoiseTexturesSize), blueNoiseTexturesSize), (_frameID + idOffset) % blueNoiseTexturesSize);
	FP4 blueNoise = UNITY_SAMPLE_TEX2DARRAY(_blueNoiseTexturesArray, blueNoiseSamplingPosition);
	blueNoise = mad(blueNoise, 2.0f, -1.0f);
	blueNoise = sign(blueNoise)*(1.0f - sqrt(1.0f - abs(blueNoise)));
	blueNoise /= 255.0f;

    return blueNoise;
#else
	return FP4(0, 0, 0, 0);
#endif
}