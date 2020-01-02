
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

Shader "Hidden/Aura2/StorePointLightShadowMap"
{
	SubShader
	{
		Pass
		{
			ZTest Off
			Cull Front
			ZWrite Off

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0

			#define SHADOWS_CUBE
			#define POINT

			#ifdef SHADOWS_DEPTH
				#define SHADOWS_NATIVE
			#endif

			#if UNITY_VERSION >= 201730
				#define SHADOWS_CUBE_IN_DEPTH_TEX
			#endif
		
			#include "UnityCG.cginc"
			#include "UnityShadowLibrary.cginc"
			#include "../../Aura.cginc"
		
			float4x4 _WorldViewProj;

			struct appdata
			{
				float4 vertex : POSITION;
			};

			struct v2f
			{
				float4 pos : SV_POSITION;
				float4 uv : TEXCOORD0;
			};

			v2f vert(appdata v)
			{
				v2f o;
				o.pos = mul(_WorldViewProj, v.vertex);
				o.uv = ComputeScreenPos(o.pos);
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				float2 uv = i.uv.xy / i.uv.w;
				float3 ray = GetNormalizedVectorFromNormalizedYawPitch(uv);
				
				#if UNITY_VERSION >= 201730
					float depth = _ShadowMapTexture.SampleLevel(_PointClamp, ray, 0).x;
					return float4(depth, _LightProjectionParams.z, _LightProjectionParams.w, 0);
				#else
					float depth = SampleCubeDistance(ray);
					return float4(depth, _LightPositionRange.w, 0, 0);
				#endif
			}
			ENDCG
		}
	}
}
