// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Custom/Snow"
{
	Properties
	{
		_MainMap("MainMap", 2D) = "white" {}
		_TrackMap("TrackMap", 2D) = "white" {}
		_Height("Height", Range( 0 , 5)) = 0.75
		_Tessellation("Tessellation", Range( 0 , 15)) = 4
		_TessellationDistance("Tessellation Distance", Range( 0 , 50)) = 30
		_SnowMap("SnowMap", 2D) = "white" {}
		_GroundMap("GroundMap", 2D) = "white" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" }
		Cull Back
		CGPROGRAM
		#include "Tessellation.cginc"
		#pragma target 4.6
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows exclude_path:deferred vertex:vertexDataFunc tessellate:tessFunction 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform sampler2D _MainMap;
		uniform float4 _MainMap_ST;
		uniform sampler2D _TrackMap;
		uniform float4 _TrackMap_ST;
		uniform float _Height;
		uniform sampler2D _SnowMap;
		uniform float4 _SnowMap_ST;
		uniform sampler2D _GroundMap;
		uniform float4 _GroundMap_ST;
		uniform float _TessellationDistance;
		uniform float _Tessellation;

		float4 tessFunction( appdata_full v0, appdata_full v1, appdata_full v2 )
		{
			return UnityDistanceBasedTess( v0.vertex, v1.vertex, v2.vertex, 0.0,_TessellationDistance,_Tessellation);
		}

		void vertexDataFunc( inout appdata_full v )
		{
			float3 ase_vertexNormal = v.normal.xyz;
			float2 uv_MainMap = v.texcoord * _MainMap_ST.xy + _MainMap_ST.zw;
			float2 uv_TrackMap = v.texcoord * _TrackMap_ST.xy + _TrackMap_ST.zw;
			float4 temp_output_12_0 = ( tex2Dlod( _MainMap, float4( uv_MainMap, 0, 0.0) ) - tex2Dlod( _TrackMap, float4( uv_TrackMap, 0, 0.0) ) );
			float3 appendResult32 = (float3(_Height , _Height , _Height));
			v.vertex.xyz += ( float4( ( ase_vertexNormal * float3( 0,1,0 ) ) , 0.0 ) * (float4( 0,0,0,0 ) + (temp_output_12_0 - float4( 0,0,0,0 )) * (float4( appendResult32 , 0.0 ) - float4( 0,0,0,0 )) / (float4( 1,1,1,0 ) - float4( 0,0,0,0 ))) ).rgb;
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_SnowMap = i.uv_texcoord * _SnowMap_ST.xy + _SnowMap_ST.zw;
			float2 uv_MainMap = i.uv_texcoord * _MainMap_ST.xy + _MainMap_ST.zw;
			float2 uv_TrackMap = i.uv_texcoord * _TrackMap_ST.xy + _TrackMap_ST.zw;
			float4 temp_output_12_0 = ( tex2D( _MainMap, uv_MainMap ) - tex2D( _TrackMap, uv_TrackMap ) );
			float2 uv_GroundMap = i.uv_texcoord * _GroundMap_ST.xy + _GroundMap_ST.zw;
			float4 blendOpSrc30 = ( tex2D( _SnowMap, uv_SnowMap ) - ( 1.0 - temp_output_12_0 ) );
			float4 blendOpDest30 = tex2D( _GroundMap, uv_GroundMap );
			o.Albedo = ( saturate( ( 1.0 - ( 1.0 - blendOpSrc30 ) * ( 1.0 - blendOpDest30 ) ) )).rgb;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=17700
0;0;1920;1019;3251.401;435.6266;2.389326;True;True
Node;AmplifyShaderEditor.TexturePropertyNode;6;-2628.817,397.9509;Inherit;True;Property;_TrackMap;TrackMap;1;0;Create;False;0;0;False;0;None;592326119cfea9742abc708dd1578e81;False;white;Auto;Texture2D;-1;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.TexturePropertyNode;4;-2628.73,-18.69707;Inherit;True;Property;_MainMap;MainMap;0;0;Create;False;0;0;False;0;None;584067f74f4ebb54fab613cc634a3cc7;False;white;Auto;Texture2D;-1;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.SamplerNode;5;-2195.415,-18.69716;Inherit;True;Property;_TextureSample0;Texture Sample 0;3;0;Create;True;0;0;False;0;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;7;-2188.481,397.9512;Inherit;True;Property;_TextureSample1;Texture Sample 1;4;0;Create;True;0;0;False;0;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TexturePropertyNode;10;-2513.593,-717.6364;Inherit;True;Property;_SnowMap;SnowMap;5;0;Create;True;0;0;False;0;None;1e2f276f8cc4f694881058b4bdf0a459;False;white;Auto;Texture2D;-1;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;12;-1797.601,221.1044;Inherit;True;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;24;-2104.732,681.0182;Inherit;False;Property;_Height;Height;2;0;Create;True;0;0;False;0;0.75;0.1;0;5;0;1;FLOAT;0
Node;AmplifyShaderEditor.NormalVertexDataNode;25;-1488.665,123.2267;Inherit;True;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TexturePropertyNode;11;-2498.021,-375.9096;Inherit;True;Property;_GroundMap;GroundMap;6;0;Create;True;0;0;False;0;None;b5c231fa3c5fe964f8d11551df320603;False;white;Auto;Texture2D;-1;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.DynamicAppendNode;32;-1733.216,660.5897;Inherit;False;FLOAT3;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SamplerNode;13;-2080.338,-718.5334;Inherit;True;Property;_TextureSample2;Texture Sample 2;8;0;Create;True;0;0;False;0;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.OneMinusNode;28;-1349.657,-588.8713;Inherit;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;14;-2070.115,-376.6286;Inherit;True;Property;_TextureSample3;Texture Sample 3;8;0;Create;True;0;0;False;0;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;2;-723.9008,430.3086;Inherit;False;Property;_Tessellation;Tessellation;3;0;Create;False;0;0;False;0;4;4;0;15;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;29;-1095.291,-712.9332;Inherit;True;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.TFHCRemapNode;22;-1496.571,410.7064;Inherit;True;5;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;1,1,1,0;False;3;COLOR;0,0,0,0;False;4;COLOR;1,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;3;-724.9008,532.3086;Inherit;False;Property;_TessellationDistance;Tessellation Distance;4;0;Create;False;0;0;False;0;30;30;0;50;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;33;-1172.67,157.6251;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,1,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.BlendOpsNode;30;-833.1141,-401.1149;Inherit;True;Screen;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;1;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;27;-919.8416,277.6958;Inherit;True;2;2;0;FLOAT3;0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.DistanceBasedTessNode;1;-311.9008,428.3086;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;0,0;Float;False;True;-1;6;ASEMaterialInspector;0;0;Standard;Custom/Snow;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;5;True;True;0;False;Opaque;;Geometry;ForwardOnly;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;True;2;15;10;25;False;5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;1;0.1646493,0.9359996,0.9433962,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;5;0;4;0
WireConnection;7;0;6;0
WireConnection;12;0;5;0
WireConnection;12;1;7;0
WireConnection;32;0;24;0
WireConnection;32;1;24;0
WireConnection;32;2;24;0
WireConnection;13;0;10;0
WireConnection;28;0;12;0
WireConnection;14;0;11;0
WireConnection;29;0;13;0
WireConnection;29;1;28;0
WireConnection;22;0;12;0
WireConnection;22;4;32;0
WireConnection;33;0;25;0
WireConnection;30;0;29;0
WireConnection;30;1;14;0
WireConnection;27;0;33;0
WireConnection;27;1;22;0
WireConnection;1;0;2;0
WireConnection;1;2;3;0
WireConnection;0;0;30;0
WireConnection;0;11;27;0
WireConnection;0;14;1;0
ASEEND*/
//CHKSM=5D5C76F97DE129481354B74CA1143B7C201810C6