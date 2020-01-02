// Unity built-in shader source. Copyright (c) 2016 Unity Technologies. MIT license (see license.txt)
// Modified by @GordGames on Unity's forums to include Aura

Shader "Hidden/TerrainEngine/BillboardTree"
{
	Properties
	{
		_MainTex ("Base (RGB) Alpha (A)", 2D) = "white" {}
	}
 
	SubShader
	{
		Tags { "Queue" = "Transparent-100" "IgnoreProjector"="True" "RenderType"="TreeBillboard" }
 
		Pass
		{
			ColorMask rgb
			Blend SrcAlpha OneMinusSrcAlpha
			ZWrite Off Cull Off
 
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_fog
			#pragma multi_compile _ AURA
			#pragma multi_compile _ AURA_USE_DITHERING
			#pragma multi_compile _ AURA_USE_CUBIC_FILTERING
			#pragma multi_compile _ AURA_DISPLAY_VOLUMETRIC_LIGHTING_ONLY

			#include "UnityCG.cginc"
			#include "TerrainEngine.cginc"
			//For Aura
			#include "../../Aura.cginc"
 
			struct v2f
			{
				float4 pos : SV_POSITION;
				fixed4 color : COLOR0;
				float2 uv : TEXCOORD0;
				UNITY_FOG_COORDS(1)
				UNITY_VERTEX_OUTPUT_STEREO

				//////////////////// Start : AURA
				#if defined(AURA)
				float3 frustrumSpacePosition : TEXCOORD2;
				#endif
				//////////////////// End : AURA
			};
 
			v2f vert (appdata_tree_billboard v)
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
				TerrainBillboardTree(v.vertex, v.texcoord1.xy, v.texcoord.y);
				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv.x = v.texcoord.x;
				o.uv.y = v.texcoord.y > 0;
				o.color = v.color;
				UNITY_TRANSFER_FOG(o,o.pos);

				//For Aura
				//////////////////// Start : AURA
				#if defined(AURA)
				o.frustrumSpacePosition = Aura2_GetFrustumSpaceCoordinates(v.vertex);
				#endif
				//////////////////// End : AURA

				return o;
			}
 
			sampler2D _MainTex;
			fixed4 frag(v2f input) : SV_Target
			{
				fixed4 col = tex2D( _MainTex, input.uv);
				col.rgb *= input.color.rgb;
				clip(col.a);
				UNITY_APPLY_FOG(input.fogCoord, col);

				//Aura
				//////////////////// Start : AURA
				#if defined(AURA)

				//// Debug fog only
				//////////////////// Start : AURA_DISPLAY_VOLUMETRIC_LIGHTING_ONLY
				#if defined(AURA_DISPLAY_VOLUMETRIC_LIGHTING_ONLY)
				col.xyz = float3(0.0f,0.0f,0.0f);
				#endif
				//////////////////// End : AURA_DISPLAY_VOLUMETRIC_LIGHTING_ONLY

				Aura2_ApplyFog(col, input.frustrumSpacePosition);
				#endif
				//////////////////// End : AURA

				return col;
			}
			ENDCG
		}
	}
 
	Fallback Off
}
