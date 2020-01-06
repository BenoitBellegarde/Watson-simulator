// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Aura 2/Standard/Unlit Textured"
{
	Properties
	{
		_MainTex("Texture", 2D) = "gray" {}
		_Tint("Tint", Color) = (1,1,1,1)
		[Toggle(_USEILLUMINATION_ON)] _UseIllumination("Use Illumination", Float) = 1
		_IlluminationStrength("Illumination Strength", Float) = 1
		[Toggle(_USEFOG_ON)] _UseFog("Use Fog", Float) = 1
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
	}
	
	SubShader
	{
		
		
		Tags { "RenderType"="Transparent" "Queue"="Transparent" }
		LOD 100

		CGINCLUDE
		#pragma target 3.0
		ENDCG
		Blend SrcAlpha OneMinusSrcAlpha , SrcAlpha OneMinusSrcAlpha
		Cull Off
		ColorMask RGBA
		ZWrite Off
		ZTest LEqual
		
		
		
		Pass
		{
			Name "Unlit"
			
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_instancing
			#include "UnityCG.cginc"
			#pragma shader_feature _USEFOG_ON
			#pragma shader_feature _USEILLUMINATION_ON
			#include "Assets/Aura 2/System/Code/Shaders/Includes/AuraUsage.cginc"


			struct appdata
			{
				float4 vertex : POSITION;
				float4 color : COLOR;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				float4 ase_texcoord : TEXCOORD0;
			};
			
			struct v2f
			{
				float4 vertex : SV_POSITION;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
				float4 ase_texcoord : TEXCOORD0;
				float4 ase_texcoord1 : TEXCOORD1;
			};

			uniform float4 _Tint;
			uniform sampler2D _MainTex;
			uniform float4 _MainTex_ST;
			uniform float _IlluminationStrength;
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
				o.ase_texcoord1 = screenPos;
				float3 objectToViewPos = UnityObjectToViewPos(v.vertex.xyz);
				float eyeDepth = -objectToViewPos.z;
				o.ase_texcoord.z = eyeDepth;
				
				o.ase_texcoord.xy = v.ase_texcoord.xy;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord.w = 0;
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
				float2 uv_MainTex = i.ase_texcoord.xy * _MainTex_ST.xy + _MainTex_ST.zw;
				float4 Color7 = ( _Tint * tex2D( _MainTex, uv_MainTex ) );
				float localApplyAuraLighting16_g1 = ( 0.0 );
				float4 temp_output_13_0_g1 = Color7;
				float3 Color16_g1 = (temp_output_13_0_g1).rgb;
				float4 screenPos = i.ase_texcoord1;
				float4 ase_grabScreenPos = ASE_ComputeGrabScreenPos( screenPos );
				float4 ase_grabScreenPosNorm = ase_grabScreenPos / ase_grabScreenPos.w;
				float eyeDepth = i.ase_texcoord.z;
				float Depth1_g7 = eyeDepth;
				float localRescaleDepth1_g7 = RescaleDepth( Depth1_g7 );
				float3 appendResult2_g7 = (float3(ase_grabScreenPosNorm.r , ase_grabScreenPosNorm.g , localRescaleDepth1_g7));
				float3 temp_output_25_0_g1 = appendResult2_g7;
				float3 BufferCoordinates16_g1 = temp_output_25_0_g1;
				float LightingFactor16_g1 = _IlluminationStrength;
				float3 LightingValue16_g1 = tex3D( Aura_VolumetricDataTexture, temp_output_25_0_g1 ).rgb;
				Aura2_ApplyLighting(Color16_g1, BufferCoordinates16_g1, LightingFactor16_g1, LightingValue16_g1);
				float4 appendResult23_g1 = (float4(Color16_g1 , (temp_output_13_0_g1).a));
				#ifdef _USEILLUMINATION_ON
				float4 staticSwitch4 = appendResult23_g1;
				#else
				float4 staticSwitch4 = Color7;
				#endif
				float4 ColorAfterIllumination14 = staticSwitch4;
				float localApplyAuraFog13_g10 = ( 0.0 );
				float4 Color13_g10 = ColorAfterIllumination14;
				float Depth1_g11 = eyeDepth;
				float localRescaleDepth1_g11 = RescaleDepth( Depth1_g11 );
				float3 appendResult2_g11 = (float3(ase_grabScreenPosNorm.r , ase_grabScreenPosNorm.g , localRescaleDepth1_g11));
				float3 temp_output_36_0_g10 = appendResult2_g11;
				float3 BufferCoordinates13_g10 = temp_output_36_0_g10;
				float4 fogValue13_g10 = tex3D( Aura_VolumetricLightingTexture, temp_output_36_0_g10 );
				Aura2_ApplyFog(Color13_g10, BufferCoordinates13_g10, fogValue13_g10);
				#ifdef _USEFOG_ON
				float4 staticSwitch19 = Color13_g10;
				#else
				float4 staticSwitch19 = ColorAfterIllumination14;
				#endif
				float4 ColorAfterFog20 = staticSwitch19;
				
				
				finalColor = ColorAfterFog20;
				return finalColor;
			}
			ENDCG
		}
	}
	CustomEditor "ASEMaterialInspector"
	
	Fallback "Unlit/Texture"
}
/*ASEBEGIN
Version=16205
1408;92;1247;1296;1367.798;638.446;1.145791;False;False
Node;AmplifyShaderEditor.CommentaryNode;25;-1200,-544;Float;False;1093;472;Initiate Base Color;5;7;13;2;1;12;;1,0.4344828,0,1;0;0
Node;AmplifyShaderEditor.TexturePropertyNode;1;-1152,-304;Float;True;Property;_MainTex;Texture;0;0;Create;False;0;0;False;0;None;80786530c07364f4bbadc3e454f5a1df;False;gray;LockedToTexture2D;Texture2D;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.ColorNode;12;-800,-496;Float;False;Property;_Tint;Tint;1;0;Create;True;0;0;False;0;1,1,1,1;1,1,1,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;2;-880,-304;Float;True;Property;_TextureSample0;Texture Sample 0;1;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;13;-544,-400;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.CommentaryNode;24;-1344,-48;Float;False;1372;358;Then apply Aura Illumination (with a material switch to leave the choice);6;5;14;4;3;9;8;;0.0147059,0.6738335,1,1;0;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;7;-352,-400;Float;False;Color;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;8;-1232,64;Float;False;7;Color;1;0;OBJECT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;5;-1296,192;Float;False;Property;_IlluminationStrength;Illumination Strength;3;0;Create;True;0;0;False;0;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;9;-816,0;Float;False;7;Color;1;0;OBJECT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.FunctionNode;3;-1008,112;Float;False;Apply Aura Illumination;5;;1;689e7cb88e5f45447bdfd7327e649316;0;2;13;COLOR;0,0,0,0;False;22;FLOAT;1;False;1;FLOAT4;17
Node;AmplifyShaderEditor.StaticSwitch;4;-592,48;Float;False;Property;_UseIllumination;Use Illumination;2;0;Create;True;0;0;False;0;0;1;0;True;;Toggle;2;Key0;Key1;9;1;COLOR;0,0,0,0;False;0;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;4;COLOR;0,0,0,0;False;5;COLOR;0,0,0,0;False;6;COLOR;0,0,0,0;False;7;COLOR;0,0,0,0;False;8;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;14;-272,48;Float;False;ColorAfterIllumination;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.CommentaryNode;22;-1248,336;Float;False;1157;261;And finally apply Aura Fog  (with a material switch to leave the choice);5;18;19;20;21;15;;0.9779412,0.9779412,0.9779412,1;0;0
Node;AmplifyShaderEditor.GetLocalVarNode;15;-1200,496;Float;False;14;ColorAfterIllumination;1;0;OBJECT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.FunctionNode;21;-880,496;Float;False;Apply Aura Fog;7;;10;fb1436f7d9c811346a67189ce659e93e;0;1;3;COLOR;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.GetLocalVarNode;18;-896,400;Float;False;14;ColorAfterIllumination;1;0;OBJECT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.StaticSwitch;19;-592,432;Float;False;Property;_UseFog;Use Fog;4;0;Create;True;0;0;False;0;0;1;0;True;;Toggle;2;Key0;Key1;9;1;COLOR;0,0,0,0;False;0;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;4;COLOR;0,0,0,0;False;5;COLOR;0,0,0,0;False;6;COLOR;0,0,0,0;False;7;COLOR;0,0,0,0;False;8;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;20;-336,432;Float;False;ColorAfterFog;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.CommentaryNode;27;-928,624;Float;False;522.1458;164.501;Output;2;0;26;;0.03448272,1,0,1;0;0
Node;AmplifyShaderEditor.GetLocalVarNode;26;-880,672;Float;False;20;ColorAfterFog;1;0;OBJECT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;0;-640,672;Float;False;True;2;Float;ASEMaterialInspector;0;1;Aura 2/Standard/Unlit Textured;0770190933193b94aaa3065e307002fa;True;Unlit;0;0;Unlit;2;True;2;5;False;-1;10;False;-1;2;5;False;-1;10;False;-1;True;0;False;-1;0;False;-1;True;False;True;2;False;-1;True;True;True;True;True;0;False;-1;True;False;255;False;-1;255;False;-1;255;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;True;2;False;-1;True;3;False;-1;True;False;0;False;-1;0;False;-1;True;2;RenderType=Transparent=RenderType;Queue=Transparent=Queue=0;True;2;0;False;False;False;False;False;False;False;False;False;True;0;False;0;Unlit/Texture;0;0;Standard;1;Vertex Position,InvertActionOnDeselection;1;0;1;True;False;2;0;FLOAT4;0,0,0,0;False;1;FLOAT3;0,0,0;False;0
WireConnection;2;0;1;0
WireConnection;13;0;12;0
WireConnection;13;1;2;0
WireConnection;7;0;13;0
WireConnection;3;13;8;0
WireConnection;3;22;5;0
WireConnection;4;1;9;0
WireConnection;4;0;3;17
WireConnection;14;0;4;0
WireConnection;21;3;15;0
WireConnection;19;1;18;0
WireConnection;19;0;21;0
WireConnection;20;0;19;0
WireConnection;0;0;26;0
ASEEND*/
//CHKSM=5FA84E67EFD6F76B7F4009A91EBA0DFAE6ADE170