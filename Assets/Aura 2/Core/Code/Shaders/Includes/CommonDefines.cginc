
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

// Global define helpers
//#define CONCATENATE_DEFINES(a, b) a##b // This seems to be a problem on some platforms :-/

// Floating point precision
#define PRECISION_HALF

#if defined(PRECISION_SIMPLE)
	#define FP		float
	#define FP2		float2
	#define FP3		float3
	#define FP4		float4
	#define FP2x2	float2x2
	#define FP3x3	float3x3
	#define FP4x4	float4x4
#elif defined(PRECISION_HALF)
	#define FP		half
	#define FP2		half2
	#define FP3		half3
	#define FP4		half4
	#define FP2x2	half2x2
	#define FP3x3	half3x3
	#define FP4x4	half4x4
#endif

//#define FP2		CONCATENATE_DEFINES(FP, 2)
//#define FP3		CONCATENATE_DEFINES(FP, 3)
//#define FP4		CONCATENATE_DEFINES(FP, 4)
//#define FP2x2		CONCATENATE_DEFINES(FP2, x2)
//#define FP3x3		CONCATENATE_DEFINES(FP3, x3)
//#define FP4x4		CONCATENATE_DEFINES(FP4, x4)

// Compute shaders dispatch threads dimensions
#define NUM_THREAD_X 8
#define NUM_THREAD_Y 8
#if defined(SHADER_API_METAL) || defined(SHADER_API_VULKAN)
	#define NUM_THREAD_Z 4
#else
	#define NUM_THREAD_Z 8
#endif

#define VISIBILITY_GROUPS_SIZE_X NUM_THREAD_X
#define VISIBILITY_GROUPS_SIZE_Y NUM_THREAD_Y
#define VISIBILITY_GROUPS_SIZE_Z NUM_THREAD_Z
#define VISIBILITY_GROUPS_SIZE uint3(VISIBILITY_GROUPS_SIZE_X, VISIBILITY_GROUPS_SIZE_Y, VISIBILITY_GROUPS_SIZE_Z)

// Compilation defines
#if defined(UNITY_COMPILER_HLSL) // HLSL only attributes
	#define BRANCH					[branch]
	#define FLATTEN					[flatten]
	#define UNROLL					[unroll]
	#define LOOP					[loop]
	#define FASTOPT					[fastopt]
	#define ALLOW_UAV_CONDITION		[allow_uav_condition]
#else
	#define BRANCH
	#define FLATTEN
	#define UNROLL
	#define LOOP
	#define FASTOPT
	#define ALLOW_UAV_CONDITION
#endif