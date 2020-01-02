
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

///
///			MatrixFloats
///
struct MatrixFloats
{
	FP4 a;
	FP4 b;
	FP4 c;
	FP4 d;
};

///
///			VolumeLevelsData
///
struct LevelsData
{
	FP levelLowThreshold;
	FP levelHiThreshold;
	FP outputLowValue;
	FP outputHiValue;
	FP contrast;
};

///
///			TextureMaskData
///
struct TextureMaskData
{
	MatrixFloats transform;
	int index;
};

///
///			VolumetricNoiseData
///
struct VolumetricNoiseData
{
	int enable;
	MatrixFloats transform;
	FP speed;
};

///
///			VolumeData
///
struct VolumeData
{
	MatrixFloats transform;
	int shape;
	/*
		Global      = 0
		Layer	    = 1
		Box         = 2
		Sphere      = 3
		Cylinder    = 4
		Cone        = 5
	*/
	FP falloffExponent;
	FP xPositiveFade;
	FP xNegativeFade;
	FP yPositiveFade;
	FP yNegativeFade;
	FP zPositiveFade;
	FP zNegativeFade;
    int useAsLightProbesProxyVolume;
    FP lightProbesMultiplier;
	TextureMaskData texture2DMaskData;
    TextureMaskData texture3DMaskData;
	VolumetricNoiseData noiseData;
	int injectDensity;
	FP densityValue;
    LevelsData densityTexture2DMaskLevelsParameters;
    LevelsData densityTexture3DMaskLevelsParameters;
    LevelsData densityNoiseLevelsParameters;
    int injectScattering;
    FP scatteringValue;
    LevelsData scatteringTexture2DMaskLevelsParameters;
    LevelsData scatteringTexture3DMaskLevelsParameters;
    LevelsData scatteringNoiseLevelsParameters;
    int injectColor;
    FP3 colorValue;
    LevelsData colorTexture2DMaskLevelsParameters;
    LevelsData colorTexture3DMaskLevelsParameters;
    LevelsData colorNoiseLevelsParameters;
	int injectTint;
	FP3 tintColor;
	LevelsData tintTexture2DMaskLevelsParameters;
	LevelsData tintTexture3DMaskLevelsParameters;
	LevelsData tintNoiseLevelsParameters;
    int injectAmbient;
    FP ambientLightingValue;
    LevelsData ambientTexture2DMaskLevelsParameters;
    LevelsData ambientTexture3DMaskLevelsParameters;
    LevelsData ambientNoiseLevelsParameters;
	int injectBoost;
	FP boostValue;
	LevelsData boostTexture2DMaskLevelsParameters;
	LevelsData boostTexture3DMaskLevelsParameters;
	LevelsData boostNoiseLevelsParameters;
};

///
///			DirectionalShadowData
///
struct DirectionalShadowData
{
	FP4 shadowSplitSqRadii;
	FP4 lightSplitsNear;
	FP4 lightSplitsFar;
	FP4 shadowSplitSpheres[4];
	half4x4 world2Shadow[4];
	FP4 lightShadowData;
};

///
///			DirectionalLightParameters
///
struct DirectionalLightParameters
{
    FP3 color;
	int useDefaultScattering;
    FP scatteringOverride;
	FP3 lightPosition;
	FP3 lightDirection;
	MatrixFloats worldToLightMatrix;
	MatrixFloats lightToWorldMatrix;
	int shadowmapIndex;
	int cookieMapIndex;
	FP2 cookieParameters;
	int enableOutOfPhaseColor;
    FP3 outOfPhaseColor;
};

///
///			SpotLightParameters
///
struct SpotLightParameters
{
    FP3 color;
	int useDefaultScattering;
    FP scatteringOverride;
	FP3 lightPosition;
	FP3 lightDirection;
	FP lightRange;
	FP lightCosHalfAngle;
	FP2 angularFalloffParameters;
	FP2 distanceFalloffParameters;
	MatrixFloats worldToShadowMatrix;
	int shadowMapIndex;
	FP shadowStrength;
	int cookieMapIndex;
	FP3 cookieParameters;
};

///
///			PointLightParameters
///
struct PointLightParameters
{
    FP3 color;
	int useDefaultScattering;
	FP scatteringOverride;
    FP3 lightPosition;
    FP lightRange;
    FP2 distanceFalloffParameters;
    MatrixFloats worldToShadowMatrix;
	#if UNITY_VERSION >= 201730
    FP2 lightProjectionParameters;
	#endif
    int shadowMapIndex;
    FP shadowStrength;
	int cookieMapIndex;
	FP3 cookieParameters;
};

/// 
///			SphericalHarmonicsFirstBandCoefficients
///
struct SphericalHarmonicsFirstBandCoefficients
{
	FP4 redColorCoefficients;
	FP4 greenColorCoefficients;
	FP4 blueColorCoefficients;
};

/// 
///			CellData
///
struct CellData
{
	uint3 id;
	FP4 localPosition; //(x, y, z-biased, z-unbiased)
	FP3 jitteredLocalPosition;
};