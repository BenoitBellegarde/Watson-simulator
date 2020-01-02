
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

Shader "Aura 2/Surface/Standard Alpha Blend"
{
	Properties
	{
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
	}
	SubShader
	{
		Tags { "RenderType"="Transparent" "Queue"="Transparent" }
		LOD 200
   
		CGPROGRAM

		#pragma surface surf Standard fullforwardshadows alpha finalcolor:Aura2_Fog
		#pragma target 4.5
		#pragma multi_compile _ AURA
		#pragma multi_compile _ AURA_USE_CUBIC_FILTERING
		#pragma multi_compile _ AURA_DISPLAY_VOLUMETRIC_LIGHTING_ONLY

		sampler2D _MainTex;
		sampler2D _CameraDepthTexture;

		// TODO : MAKE THIS PROPER WITH INCLUDES (SURFACE SHADERS ARE A NIGHTMARE)
		float4 Aura_FrustumRanges;
		sampler3D Aura_VolumetricLightingTexture;
		float InverseLerp(float lowThreshold, float hiThreshold, float value)
		{
			return (value - lowThreshold) / (hiThreshold - lowThreshold);
		}
		float4 Aura2_GetFogValue(float3 screenSpacePosition)
		{
			return tex3Dlod(Aura_VolumetricLightingTexture, float4(screenSpacePosition, 0));
		}
		void Aura2_ApplyFog(inout fixed4 colorToApply, float3 screenSpacePosition)
		{    
			float4 fogValue = Aura2_GetFogValue(screenSpacePosition);
			// Always apply fog attenuation - also in the forward add pass.
			colorToApply.xyz *= fogValue.w;
			// Alpha premultiply mode (used with alpha and Standard lighting function, or explicitly alpha:premul)
			#if _ALPHAPREMULTIPLY_ON
			fogValue.xyz *= colorToApply.w;
			#endif
			// Add inscattering only once, so in forward base, but not forward add.
			#ifndef UNITY_PASS_FORWARDADD
			colorToApply.xyz += fogValue.xyz;
			#endif
		}
		
		struct Input
		{
			float2 uv_MainTex;
			float4 screenPos;
		};
		
		// From https://github.com/Unity-Technologies/VolumetricLighting/blob/master/Assets/Scenes/Materials/StandardAlphaBlended-VolumetricFog.shader
		void Aura2_Fog(Input IN, SurfaceOutputStandard o, inout fixed4 color)
		{
				//////////////////// Start : AURA
				#if defined(AURA)
				half3 screenSpacePosition = IN.screenPos.xyz/IN.screenPos.w;
				screenSpacePosition.z = InverseLerp(Aura_FrustumRanges.x, Aura_FrustumRanges.y, LinearEyeDepth(screenSpacePosition.z));

				//// Debug fog only
				//////////////////// Start : AURA_DISPLAY_VOLUMETRIC_LIGHTING_ONLY
				#if defined(AURA_DISPLAY_VOLUMETRIC_LIGHTING_ONLY)
				color.xyz = float3(0.0f,0.0f,0.0f);
				#endif
				//////////////////// End : AURA_DISPLAY_VOLUMETRIC_LIGHTING_ONLY

				Aura2_ApplyFog(color, screenSpacePosition);
				#endif
				//////////////////// End : AURA
		}

		half _Glossiness;
		half _Metallic;
		fixed4 _Color;
 
		void surf (Input IN, inout SurfaceOutputStandard o)
		{
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb;
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Alpha = c.a;
		}
		ENDCG
	}
	FallBack "Standard"
}