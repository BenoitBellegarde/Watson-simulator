// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "ASESampleShaders/Aura 2"
{
	Properties
	{
		_Color("Color", Color) = (0.4980392,0.4980392,0.4980392,0.4980392)
		[Toggle(_APPLYFOG_ON)] _ApplyFog("Apply Fog", Float) = 0
		[Toggle(_APPLYILLUMINATION_ON)] _ApplyIllumination("Apply Illumination", Float) = 0
		_IlluminationFactor("Illumination Factor", Float) = 1
	}
	
	SubShader
	{
		
		
		Tags { "RenderType"="Transparent" "Queue"="AlphaTest" }
		LOD 100

		CGINCLUDE
		#pragma target 3.0
		ENDCG
		Blend SrcAlpha OneMinusSrcAlpha
		Cull Back
		ColorMask RGBA
		ZWrite On
		ZTest LEqual
		Offset 0 , 0
		
		
		
		Pass
		{
			Name "Unlit"
			
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_instancing
			#include "UnityCG.cginc"
			#pragma shader_feature _APPLYFOG_ON
			#pragma shader_feature _APPLYILLUMINATION_ON
			#include "Assets/Aura 2/System/Code/Shaders/Includes/AuraUsage.cginc"


			struct appdata
			{
				float4 vertex : POSITION;
				float4 color : COLOR;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				
			};
			
			struct v2f
			{
				float4 vertex : SV_POSITION;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
				float4 ase_texcoord : TEXCOORD0;
				float4 ase_texcoord1 : TEXCOORD1;
			};

			uniform float4 _Color;
			uniform float _IlluminationFactor;
			inline float4 ASE_ComputeGrabScreenPos( float4 pos )
			{
				#if UNITY_UV_STARTS_AT_TOP
				float scale = -1.0;
				#else
				float scale = 1.0;
				#endif
				float4 o = pos;
				o.y = pos.w * 0.5f;
				o.y = ( pos.y - o.y ) * _ProjectionParams.x * scale + o.y;
				return o;
			}
			
			float RescaleDepth( float Depth )
			{
				return Aura2_RescaleDepth(Depth);
			}
			
			
			v2f vert ( appdata v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
				UNITY_TRANSFER_INSTANCE_ID(v, o);

				float4 ase_clipPos = UnityObjectToClipPos(v.vertex);
				float4 screenPos = ComputeScreenPos(ase_clipPos);
				o.ase_texcoord = screenPos;
				float3 objectToViewPos = UnityObjectToViewPos(v.vertex.xyz);
				float eyeDepth = -objectToViewPos.z;
				o.ase_texcoord1.x = eyeDepth;
				
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord1.yzw = 0;
				float3 vertexValue =  float3(0,0,0) ;
				#if ASE_ABSOLUTE_VERTEX_POS
				v.vertex.xyz = vertexValue;
				#else
				v.vertex.xyz += vertexValue;
				#endif
				o.vertex = UnityObjectToClipPos(v.vertex);
				return o;
			}
			
			fixed4 frag (v2f i ) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID(i);
				fixed4 finalColor;
				float localApplyAuraLighting16_g193 = ( 0.0 );
				float4 temp_output_13_0_g193 = _Color;
				float3 Color16_g193 = (temp_output_13_0_g193).rgb;
				float4 screenPos = i.ase_texcoord;
				float4 ase_grabScreenPos = ASE_ComputeGrabScreenPos( screenPos );
				float4 ase_grabScreenPosNorm = ase_grabScreenPos / ase_grabScreenPos.w;
				float eyeDepth = i.ase_texcoord1.x;
				float Depth1_g194 = eyeDepth;
				float localRescaleDepth1_g194 = RescaleDepth( Depth1_g194 );
				float3 appendResult2_g194 = (float3(ase_grabScreenPosNorm.r , ase_grabScreenPosNorm.g , localRescaleDepth1_g194));
				float3 temp_output_25_0_g193 = appendResult2_g194;
				float3 BufferCoordinates16_g193 = temp_output_25_0_g193;
				float LightingFactor16_g193 = _IlluminationFactor;
				float3 LightingValue16_g193 = tex3D( Aura_VolumetricDataTexture, temp_output_25_0_g193 ).rgb;
				Aura2_ApplyLighting(Color16_g193, BufferCoordinates16_g193, LightingFactor16_g193, LightingValue16_g193);
				float4 appendResult23_g193 = (float4(Color16_g193 , (temp_output_13_0_g193).a));
				#ifdef _APPLYILLUMINATION_ON
				float4 staticSwitch117 = appendResult23_g193;
				#else
				float4 staticSwitch117 = _Color;
				#endif
				float localApplyAuraFog13_g195 = ( 0.0 );
				float4 Color13_g195 = staticSwitch117;
				float Depth1_g196 = eyeDepth;
				float localRescaleDepth1_g196 = RescaleDepth( Depth1_g196 );
				float3 appendResult2_g196 = (float3(ase_grabScreenPosNorm.r , ase_grabScreenPosNorm.g , localRescaleDepth1_g196));
				float3 temp_output_36_0_g195 = appendResult2_g196;
				float3 BufferCoordinates13_g195 = temp_output_36_0_g195;
				float4 fogValue13_g195 = tex3D( Aura_VolumetricLightingTexture, temp_output_36_0_g195 );
				Aura2_ApplyFog(Color13_g195, BufferCoordinates13_g195, fogValue13_g195);
				#ifdef _APPLYFOG_ON
				float4 staticSwitch118 = Color13_g195;
				#else
				float4 staticSwitch118 = staticSwitch117;
				#endif
				
				
				finalColor = staticSwitch118;
				return finalColor;
			}
			ENDCG
		}
	}
	CustomEditor "ASEMaterialInspector"
	
	Fallback "Unlit/Color"
}
/*ASEBEGIN
Version=16205
811;92;751;665;1525.402;440.3649;1;True;False
Node;AmplifyShaderEditor.ColorNode;3;-1552,-224;Float;False;Property;_Color;Color;0;0;Create;True;0;0;False;0;0.4980392,0.4980392,0.4980392,0.4980392;0.4980392,0.4980392,0.4980392,0.4980392;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;103;-1456,0;Float;False;Property;_IlluminationFactor;Illumination Factor;3;0;Create;True;0;0;False;0;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;119;-1184,-128;Float;False;Apply Aura Illumination;6;;193;689e7cb88e5f45447bdfd7327e649316;0;2;13;COLOR;0,0,0,0;False;22;FLOAT;1;False;1;FLOAT4;17
Node;AmplifyShaderEditor.StaticSwitch;117;-736,-224;Float;False;Property;_ApplyIllumination;Apply Illumination;2;0;Create;True;0;0;False;0;0;0;0;True;;Toggle;2;Key0;Key1;9;1;COLOR;0,0,0,0;False;0;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;4;COLOR;0,0,0,0;False;5;COLOR;0,0,0,0;False;6;COLOR;0,0,0,0;False;7;COLOR;0,0,0,0;False;8;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.FunctionNode;113;-368,-128;Float;False;Apply Aura Fog;4;;195;fb1436f7d9c811346a67189ce659e93e;0;1;3;COLOR;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.StaticSwitch;118;-34,-224;Float;False;Property;_ApplyFog;Apply Fog;1;0;Create;True;0;0;False;0;0;0;0;True;;Toggle;2;Key0;Key1;9;1;COLOR;0,0,0,0;False;0;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;4;COLOR;0,0,0,0;False;5;COLOR;0,0,0,0;False;6;COLOR;0,0,0,0;False;7;COLOR;0,0,0,0;False;8;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;0;256,-224;Float;False;True;2;Float;ASEMaterialInspector;0;1;ASESampleShaders/Aura 2;0770190933193b94aaa3065e307002fa;True;Unlit;0;0;Unlit;2;True;2;5;False;-1;10;False;-1;0;1;False;-1;0;False;-1;True;0;False;-1;0;False;-1;True;False;True;0;False;-1;True;True;True;True;True;0;False;-1;True;False;255;False;-1;255;False;-1;255;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;True;1;False;-1;True;3;False;-1;True;True;0;False;-1;0;False;-1;True;2;RenderType=Transparent=RenderType;Queue=AlphaTest=Queue=0;True;2;0;False;False;False;False;False;False;False;False;False;True;0;False;0;Unlit/Color;0;0;Standard;1;Vertex Position,InvertActionOnDeselection;1;0;1;True;False;2;0;FLOAT4;0,0,0,0;False;1;FLOAT3;0,0,0;False;0
WireConnection;119;13;3;0
WireConnection;119;22;103;0
WireConnection;117;1;3;0
WireConnection;117;0;119;17
WireConnection;113;3;117;0
WireConnection;118;1;117;0
WireConnection;118;0;113;0
WireConnection;0;0;118;0
ASEEND*/
//CHKSM=BD0A23EED3F7E48B3505F75A368FFD6D56BA90F9