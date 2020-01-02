
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

Shader "Aura 2/Particles/Additive"
{
	Properties
	{
		[Header(Aura usage properties)][Space][KeywordEnum(Pixel, Vertex)] _UsageStage("Stage", Float) = 0
		[KeywordEnum(Light, Fog, Both)] _UsageType("Type", Float) = 0
		_LightingFactor("Lighting Factor", Float) = 1
		
		[Header(Properties)][Space]_TintColor ("Tint Color", Color) = (1,1,1,1)
		_MainTex ("Particle Texture", 2D) = "white" {}
		_SoftParticleDistanceFade("Soft Particles Distance Fade", Float) = 0.1
	}

	Category
	{
		SubShader
		{
			Pass
			{
				Tags{ "IgnoreProjector"="True" "PreviewType"="Plane" }
				Tags { "RenderType"="Transparent" "Queue"="Transparent" }
				ColorMask RGB
				Cull Back
				Lighting Off
				ZWrite Off
				ZTest LEqual

				Blend One One
					
				CGPROGRAM
					#pragma vertex vert
					#pragma fragment frag
					#pragma target 4.0
					#pragma shader_feature _USAGESTAGE_VERTEX _USAGESTAGE_PIXEL
					#pragma shader_feature _USAGETYPE_LIGHT _USAGETYPE_FOG _USAGETYPE_BOTH
					#pragma multi_compile _ AURA
					#pragma multi_compile _ AURA_USE_DITHERING
					#pragma multi_compile _ AURA_USE_CUBIC_FILTERING
					#pragma multi_compile _ AURA_DISPLAY_VOLUMETRIC_LIGHTING_ONLY

					#define PREMULTIPLY_ALPHA(color) color.xyz *= color.w;

					float _LightingFactor;
					sampler2D _MainTex;
					float4 _MainTex_ST;
					fixed4 _TintColor;
					sampler2D_float _CameraDepthTexture;
					float _SoftParticleDistanceFade;
						
					#include "UnityCG.cginc"
					#include "../../Includes/AuraParticlesUsage.cginc"
				ENDCG 
			}
		}	
	}
}
