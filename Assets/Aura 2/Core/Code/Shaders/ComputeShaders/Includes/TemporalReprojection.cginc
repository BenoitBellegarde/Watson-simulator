
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

uniform FP temporalReprojectionFactor;
uniform FP4x4 previousFrameWorldToClipMatrix;
uniform FP4x4 previousFrameSecondaryWorldToClipMatrix;
uniform Texture3D<FP4> previousFrameLightingVolumeTexture;
uniform Texture2D<int> previousMaximumSliceAmountTexture;

// https://github.com/bartwronski/PoissonSamplingGenerator
static const uint SAMPLE_NUM = 64;
static const FP2 POISSON_SAMPLES[SAMPLE_NUM] =
{
    FP2(-0.47666945771717517f, 0.1278339303865048f),
	FP2(0.4573181093106745f, -0.48039337197508913f),
	FP2(0.4958090683929387f, 0.07353428460207634f),
	FP2(-0.24215716351290184f, -0.4896019266648153f),
	FP2(-0.06922327838674125f, 0.340485210512975f),
	FP2(0.10021111323473408f, -0.019853328597419728f),
	FP2(-0.45222874389351875f, -0.2446643415218489f),
	FP2(0.050522885501381065f, -0.4308515486341682f),
	FP2(0.3421952866379253f, 0.40281643750969365f),
	FP2(-0.1390507894318307f, -0.20432859504235967f),
	FP2(-0.40732209011160747f, 0.4323492756538758f),
	FP2(0.3112696173739976f, -0.1587914304719349f),
	FP2(-0.2681834621268484f, -0.024183610689426982f),
	FP2(-0.2905288495499303f, -0.2154391530155022f),
	FP2(0.3339054652872995f, 0.2266969002396153f),
	FP2(0.1417632692042997f, 0.3793788475566262f),
	FP2(-0.260375807118736f, 0.24727133452329642f),
	FP2(0.2507183540322714f, -0.37639830046486367f),
	FP2(0.2794987518796219f, 0.04200516859639114f),
	FP2(-0.09434221931243869f, 0.043646708158577074f),
	FP2(-0.19502627020636176f, 0.4854923589442314f),
	FP2(0.18365148169406142f, 0.1562418239929989f),
	FP2(0.13757346903163525f, -0.17015863477371718f),
	FP2(0.4693315966936751f, -0.2413643436433025f),
	FP2(0.03348976642084889f, -0.2926494918997441f),
	FP2(-0.30416422669875975f, -0.357839720534722f),
	FP2(-0.4772781454197298f, -0.03611603088605042f),
	FP2(0.22610186885469719f, 0.491821795661998f),
	FP2(-0.02273893179024078f, 0.4712596237063277f),
	FP2(0.02900893354145062f, 0.14692298500968803f),
	FP2(-0.08047708126853481f, -0.44515676234303225f),
	FP2(0.4697686982261233f, 0.4274987902927708f),
	FP2(0.4880447241065048f, -0.06363598002464299f),
	FP2(-0.39564233165983853f, 0.2486818259161021f),
	FP2(-0.15175053458560184f, 0.1515745012180042f),
	FP2(-0.2718135278716576f, 0.09668737471744637f),
	FP2(-0.46079943277320645f, -0.4912851819368942f),
	FP2(-0.05488314003562167f, -0.10643481987265657f),
	FP2(0.447458124138567f, 0.18384779322567035f),
	FP2(0.35684597494537285f, -0.26954551060654974f),
	FP2(-0.23881348564937643f, 0.37876454334632437f),
	FP2(0.449764133898259f, 0.31167467471421806f),
	FP2(0.1781279672658872f, -0.46207852924744186f),
	FP2(-0.3676795179492348f, -0.11476042179465318f),
	FP2(0.02952873150166535f, 0.3809426036120256f),
	FP2(-0.2967439309965578f, 0.49113078857044945f),
	FP2(-0.17026603029106413f, -0.387272594015833f),
	FP2(0.23148274270834823f, 0.31904201390749276f),
	FP2(0.14979928852139435f, -0.3646631800068241f),
	FP2(-0.47570816925253523f, -0.15457782119022523f),
	FP2(-0.49327304413535367f, -0.36423960901809016f),
	FP2(0.09863625912964236f, 0.23547843841930238f),
	FP2(0.22194023782999928f, -0.24094414498226113f),
	FP2(0.2000741580458637f, -0.06138023499952727f),
	FP2(-0.3704066703585073f, -0.286420108574738f),
	FP2(-0.3777194791365821f, -0.006602143654658277f),
	FP2(-0.481682562442486f, 0.4969793961927147f),
	FP2(0.39680828854150973f, 0.07323060162578832f),
	FP2(0.39453264928220433f, -0.3495820998664234f),
	FP2(-0.08886136079620721f, 0.23388088273718255f),
	FP2(-0.42112183425107497f, -0.36750106277817773f),
	FP2(-0.06556392996533233f, -0.3318918802654117f),
	FP2(-0.4103528454219004f, 0.35306475209146837f),
	FP2(-0.009185149205523602f, -0.19909938373616587f),
};

int GetJitterOffsetArrayIndex(uint3 id)
{
    return (_frameID + id.x + id.y * 2 + id.z) % SAMPLE_NUM;
}

FP GetNoise(FP x) // Finger-crossing it tends to uniform distribution
{
    return frac(sin(x) * 43758.5453123);
}

FP3 GetJitterOffset(uint3 id)
{
    FP noise = GetNoise(id.x * 1.23f + id.y * 0.97f + (id.z + _frameID) * 1.01f + 236526.0f);
    FP3 jitter = FP3(0,0,0);
    jitter.xy = POISSON_SAMPLES[GetJitterOffsetArrayIndex(id + uint3(0, 0, uint(noise * SAMPLE_NUM)))];
    jitter.z = (noise - 0.5f);
	
    return jitter;// * 2.0f;
}


void JitterPosition(inout FP3 position, uint3 id)
{
    FP3 offset = GetJitterOffset(id);
    position.xyz += offset.xyz * Aura_BufferTexelSize.xyz;
}

FP4 ComputeReprojectedNormalizedPosition(FP3 worldPosition, bool isSecondaryFrustum)
{
    FP4x4 worldToClipMatrix = previousFrameWorldToClipMatrix;
	#if defined(SINGLE_PASS_STEREO)
	if (isSecondaryFrustum)
	{
		worldToClipMatrix = previousFrameSecondaryWorldToClipMatrix;
	}
	#endif

    FP4 previousNormalizedPosition = mul(worldToClipMatrix, FP4(worldPosition, 1));
    previousNormalizedPosition.xy /= previousNormalizedPosition.w;
	previousNormalizedPosition.xy = previousNormalizedPosition.xy * 0.5f + 0.5f;
	previousNormalizedPosition.z = InverseLerp(cameraRanges.x, cameraRanges.y, previousNormalizedPosition.z);
	
    return previousNormalizedPosition;
}

void ReprojectPreviousFrame(inout FP3 accumulationColor, FP3 unjitteredUnbiasedWorldPosition, bool isSecondaryFrustum)
{	
	FP4 reprojectedNormalizedPosition = ComputeReprojectedNormalizedPosition(unjitteredUnbiasedWorldPosition, isSecondaryFrustum);

	#if defined(SINGLE_PASS_STEREO)
	reprojectedNormalizedPosition.x *= 0.5f;
    if (isSecondaryFrustum)
    {
		reprojectedNormalizedPosition.x += 0.5f;
    }
	#endif
	
	uint3 reprojectedId = GetIdFromLocalPosition(reprojectedNormalizedPosition.xyz);
	uint reprojectedPreviousMaximumDepthSlice = (uint)previousMaximumSliceAmountTexture[reprojectedId.xy] + 1;
	bool reprojectedPositionIsVisible = (reprojectedNormalizedPosition.w > 0.0f) && (dot(reprojectedNormalizedPosition.xyz - saturate(reprojectedNormalizedPosition.xyz), 1) == 0.0f);
	#if defined(OCCLUSION)
	reprojectedPositionIsVisible = reprojectedPositionIsVisible && (reprojectedPreviousMaximumDepthSlice >= reprojectedId.z);
	#endif

	BRANCH
    if (reprojectedPositionIsVisible)
    {
		FP4 previousData = previousFrameLightingVolumeTexture.SampleLevel(_LinearClamp, reprojectedNormalizedPosition.xyz, 0);
		accumulationColor = lerp(accumulationColor.xyz, previousData.xyz, temporalReprojectionFactor);
    }
}