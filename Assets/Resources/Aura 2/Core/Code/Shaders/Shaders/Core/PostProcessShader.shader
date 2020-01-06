
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

Shader "Hidden/Aura2/PostProcessShader"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
	}
	SubShader
	{
		// No culling or depth
		Cull Off ZWrite Off ZTest Always

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
			#pragma multi_compile _ AURA
			#pragma multi_compile _ AURA_USE_DITHERING
			#pragma multi_compile _ AURA_USE_CUBIC_FILTERING
			#pragma multi_compile _ AURA_DISPLAY_VOLUMETRIC_LIGHTING_ONLY
			
			#include "UnityCG.cginc"
			#include "../../Aura.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}

			sampler2D _MainTex;
			float4 _MainTex_ST;
			sampler2D _CameraDepthTexture;

			fixed4 frag (v2f psIn) : SV_Target
			{
				float2 uv = psIn.uv;
				float2 stereoUv = UnityStereoScreenSpaceUVAdjust(uv, _MainTex_ST).xy;

				float4 backColor = tex2D(_MainTex, stereoUv);
				
				//////////////////// Start : AURA
				#if defined(AURA)
				float depth = SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, stereoUv);
				depth = GetLinearEyeDepth(depth);

				//// Debug fog only
				//////////////////// Start : AURA_DISPLAY_VOLUMETRIC_LIGHTING_ONLY
				#if defined(AURA_DISPLAY_VOLUMETRIC_LIGHTING_ONLY)
				backColor.xyz = float3(0.0f,0.0f,0.0f);
				#endif
				//////////////////// End : AURA_DISPLAY_VOLUMETRIC_LIGHTING_ONLY

				Aura2_ApplyFog(backColor.xyz, float3(stereoUv, depth));

				//// Debug fogless backbuffer
				//const float thumbnailFactor = 0.25f;
				//float4 thumbnail = tex2D(_MainTex, uv / thumbnailFactor);
				//float thumbnailMask = step(uv.x, thumbnailFactor) * step(uv.y, thumbnailFactor);
				//backColor = lerp(backColor, thumbnail, thumbnailMask);

				#endif
				//////////////////// End : AURA

				return backColor;
			}
			ENDCG
		}
	}
}
