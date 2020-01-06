// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Aura 2/Sprites/Sprite"
{
	Properties
	{
		_MainTex("Sprite", 2D) = "white" {}
		[Toggle(_TWOSIDEDILLUMINATION_ON)] _TWOSIDEDILLUMINATION("TWO SIDED ILLUMINATION", Float) = 1
		[Toggle(_FLIPFACE_ON)] _FLIPFACE("FLIP FACE", Float) = 0
		_Cutoff( "Mask Clip Value", Float ) = 0.5
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "TransparentCutout"  "Queue" = "AlphaTest+0" }
		Cull Off
		CGPROGRAM
		#pragma target 3.0
		#pragma shader_feature _TWOSIDEDILLUMINATION_ON
		#pragma shader_feature _FLIPFACE_ON
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows 
		struct Input
		{
			half ASEVFace : VFACE;
			float2 uv_texcoord;
		};

		uniform sampler2D _MainTex;
		uniform float4 _MainTex_ST;
		uniform float _Cutoff = 0.5;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float3 _Vector1 = float3(0,0,1);
			#ifdef _FLIPFACE_ON
				float3 staticSwitch43 = -_Vector1;
			#else
				float3 staticSwitch43 = _Vector1;
			#endif
			float3 switchResult42 = (((i.ASEVFace>0)?(_Vector1):(-_Vector1)));
			#ifdef _TWOSIDEDILLUMINATION_ON
				float3 staticSwitch40 = switchResult42;
			#else
				float3 staticSwitch40 = staticSwitch43;
			#endif
			float3 Normal12 = staticSwitch40;
			o.Normal = Normal12;
			float2 uv_MainTex = i.uv_texcoord * _MainTex_ST.xy + _MainTex_ST.zw;
			float4 tex2DNode2 = tex2D( _MainTex, uv_MainTex );
			float4 SpriteColor3 = tex2DNode2;
			o.Albedo = SpriteColor3.rgb;
			o.Alpha = 1;
			float SpriteAlpha4 = tex2DNode2.a;
			clip( SpriteAlpha4 - _Cutoff );
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=16205
1592;92;895;775;2194.142;984.1063;2.709747;True;False
Node;AmplifyShaderEditor.Vector3Node;36;-1558.518,262.9799;Float;False;Constant;_Vector1;Vector 1;2;0;Create;True;0;0;False;0;0,0,1;0,0,0;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.NegateNode;41;-1354.726,442.1036;Float;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.TexturePropertyNode;1;-1194.982,-28.23465;Float;True;Property;_MainTex;Sprite;0;0;Create;False;0;0;False;0;None;None;False;white;LockedToTexture2D;Texture2D;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.StaticSwitch;43;-1178.415,224.6812;Float;False;Property;_FLIPFACE;FLIP FACE;2;0;Create;True;0;0;False;0;0;0;0;True;;Toggle;2;Key0;Key1;9;1;FLOAT3;0,0,0;False;0;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT3;0,0,0;False;4;FLOAT3;0,0,0;False;5;FLOAT3;0,0,0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SwitchByFaceNode;42;-1171.938,370.1404;Float;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.StaticSwitch;40;-911.8129,216.8816;Float;False;Property;_TWOSIDEDILLUMINATION;TWO SIDED ILLUMINATION;1;0;Create;True;0;0;False;0;0;1;0;True;;Toggle;2;Key0;Key1;9;1;FLOAT3;0,0,0;False;0;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT3;0,0,0;False;4;FLOAT3;0,0,0;False;5;FLOAT3;0,0,0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SamplerNode;2;-945.5924,-34.11237;Float;True;Property;_TextureSample0;Texture Sample 0;2;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;4;-577.9457,65.71032;Float;False;SpriteAlpha;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;12;-596.2879,217.7882;Float;False;Normal;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;3;-574.8117,-30.32445;Float;False;SpriteColor;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;6;-256.3841,221.6563;Float;False;4;SpriteAlpha;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;13;-265.5566,52.42226;Float;False;12;Normal;1;0;OBJECT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;5;-273.2785,-27.53478;Float;False;3;SpriteColor;1;0;OBJECT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;2,1.3;Float;False;True;2;Float;ASEMaterialInspector;0;0;Standard;Aura 2/Sprites/Sprite;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Off;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Masked;0.5;True;True;0;False;TransparentCutout;;AlphaTest;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;3;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;41;0;36;0
WireConnection;43;1;36;0
WireConnection;43;0;41;0
WireConnection;42;0;36;0
WireConnection;42;1;41;0
WireConnection;40;1;43;0
WireConnection;40;0;42;0
WireConnection;2;0;1;0
WireConnection;4;0;2;4
WireConnection;12;0;40;0
WireConnection;3;0;2;0
WireConnection;0;0;5;0
WireConnection;0;1;13;0
WireConnection;0;10;6;0
ASEEND*/
//CHKSM=E85B8F580D5DDE335E04D2F598AB8ACF72DA9E42