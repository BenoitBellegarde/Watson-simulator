
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

Shader "Aura 2/Particles/Alpha Test"
{
	Properties
	{
		[Header(Aura usage properties)][Space][KeywordEnum(Pixel, Vertex)] _UsageStage("Stage", Float) = 0
		[KeywordEnum(Light, Fog, Both)] _UsageType("Type", Float) = 0
		_LightingFactor("Lighting Factor", Float) = 1

		[Header(Properties)][Space]_TintColor ("Tint Color", Color) = (1,1,1,1)
		_MainTex ("Particle Texture", 2D) = "white" {}
		_ClipValue("Clip Value", Float) = 0.5
		_SoftParticleDistanceFade("Soft Particles Distance Fade", Float) = 0.1
	}

	Category
	{
		SubShader
		{
 
			Pass
			{
				Tags {  "LightMode" = "ShadowCaster" }				
				CGPROGRAM
					#pragma vertex vert
					#pragma fragment frag
					#include "UnityCG.cginc"

					sampler2D _MainTex;
					float4 _MainTex_ST;
					float _ClipValue;

					struct appdata_t
					{
						float4 vertex : POSITION;
						float2 texcoord : TEXCOORD0;
					};

					struct v2f
					{
						float4 vertex : SV_POSITION;
						float2 texcoord : TEXCOORD0;
					};

					v2f vert(appdata_t v)
					{
						v2f o;
						o.vertex = UnityObjectToClipPos(v.vertex);
						o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
						return o;
					}

					fixed4 frag(v2f i) : SV_Target
					{
						clip(tex2D(_MainTex, i.texcoord).w - _ClipValue);
						return float4(1,1,1,1);
					}
				ENDCG
			}

			Pass
			{
				Tags{ "IgnoreProjector"="True" "PreviewType"="Plane" }
				Tags { "RenderType"="Transparent" "Queue"="Transparent" }
				ColorMask RGB
				Cull Back
				Lighting Off
				ZWrite On
				ZTest LEqual

				CGPROGRAM
					#pragma vertex vert
					#pragma fragment frag
					#pragma target 4.0
					#pragma shader_feature ALPHA_TEST
					#define ALPHA_TEST
					#pragma shader_feature _USAGESTAGE_VERTEX _USAGESTAGE_PIXEL
					#pragma shader_feature _USAGETYPE_LIGHT _USAGETYPE_FOG _USAGETYPE_BOTH
					#pragma multi_compile _ AURA
					#pragma multi_compile _ AURA_USE_DITHERING
					#pragma multi_compile _ AURA_USE_CUBIC_FILTERING
					#pragma multi_compile _ AURA_DISPLAY_VOLUMETRIC_LIGHTING_ONLY

					#define PREMULTIPLY_ALPHA(color)

					float _LightingFactor;
					sampler2D _MainTex;
					float4 _MainTex_ST;
					fixed4 _TintColor;
					float _ClipValue;
					float _SoftParticleDistanceFade;
					sampler2D_float _CameraDepthTexture;
						
					#include "UnityCG.cginc"
					#include "../../Includes/AuraParticlesUsage.cginc"
				ENDCG 
			}
		}	
	}
}
