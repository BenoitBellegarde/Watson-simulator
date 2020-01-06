
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

// Time (to be set from code)
uniform float time;
// Frame ID (to be set from code)
uniform int _frameID;

// SamplerStates
uniform SamplerState _LinearClamp;
uniform SamplerState _LinearRepeat;
uniform SamplerState _PointClamp;
uniform SamplerState _PointRepeat;

// Const variables
static const float pi = 3.141592653589793f;
static const float twoPi = pi * 2;
static const float halfPi = pi * 0.5;
static const float quarterPi = pi * 0.25;
static const float invPi = rcp(pi);
static const float twoInvPi = 2 * invPi;
static const float e = 2.71828182845904523536f;
static const float n = rcp(e);

// Common variables
uniform FP4 Aura_BufferResolution;
uniform FP4 Aura_BufferTexelSize;
uniform FP4 cameraPosition;
uniform FP4 cameraDirection;
uniform FP4 cameraRanges;
uniform FP Aura_DepthBiasCoefficient;
uniform FP Aura_DepthBiasReciproqualCoefficient;