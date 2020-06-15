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
		_SnowColor("Snow Color", Color) = (0.7390531,0.9013809,0.9056604,0)
		_SnowTilingOffset("Snow Tiling Offset", Vector) = (10,10,0,0)
		_SnowSmooth("Snow Smooth", Range( 0 , 1)) = 0.6
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
		uniform float4 _SnowColor;
		uniform float4 _SnowTilingOffset;
		uniform float _SnowSmooth;
		uniform sampler2D _GroundMap;
		uniform float4 _GroundMap_ST;
		uniform float _TessellationDistance;
		uniform float _Tessellation;


		float2 UnityGradientNoiseDir( float2 p )
		{
			p = fmod(p , 289);
			float x = fmod((34 * p.x + 1) * p.x , 289) + p.y;
			x = fmod( (34 * x + 1) * x , 289);
			x = frac( x / 41 ) * 2 - 1;
			return normalize( float2(x - floor(x + 0.5 ), abs( x ) - 0.5 ) );
		}
		
		float UnityGradientNoise( float2 UV, float Scale )
		{
			float2 p = UV * Scale;
			float2 ip = floor( p );
			float2 fp = frac( p );
			float d00 = dot( UnityGradientNoiseDir( ip ), fp );
			float d01 = dot( UnityGradientNoiseDir( ip + float2( 0, 1 ) ), fp - float2( 0, 1 ) );
			float d10 = dot( UnityGradientNoiseDir( ip + float2( 1, 0 ) ), fp - float2( 1, 0 ) );
			float d11 = dot( UnityGradientNoiseDir( ip + float2( 1, 1 ) ), fp - float2( 1, 1 ) );
			fp = fp * fp * fp * ( fp * ( fp * 6 - 15 ) + 10 );
			return lerp( lerp( d00, d01, fp.y ), lerp( d10, d11, fp.y ), fp.x ) + 0.5;
		}


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
			float2 uv_MainMap = i.uv_texcoord * _MainMap_ST.xy + _MainMap_ST.zw;
			float2 uv_TrackMap = i.uv_texcoord * _TrackMap_ST.xy + _TrackMap_ST.zw;
			float4 temp_output_12_0 = ( tex2D( _MainMap, uv_MainMap ) - tex2D( _TrackMap, uv_TrackMap ) );
			float4 temp_output_28_0 = ( 1.0 - temp_output_12_0 );
			float temp_output_51_0 = (temp_output_28_0).r;
			float2 appendResult104 = (float2(_SnowTilingOffset.x , _SnowTilingOffset.y));
			float2 appendResult105 = (float2(_SnowTilingOffset.z , _SnowTilingOffset.w));
			float2 uv_TexCoord81 = i.uv_texcoord * appendResult104 + appendResult105;
			float gradientNoise82 = UnityGradientNoise(uv_TexCoord81,0.1);
			float gradientNoise83 = UnityGradientNoise(uv_TexCoord81,1.0);
			float gradientNoise84 = UnityGradientNoise(uv_TexCoord81,0.5);
			float temp_output_87_0 = ( ( gradientNoise82 + gradientNoise83 + gradientNoise84 ) / 3.0 );
			float4 temp_output_29_0 = ( ( _SnowColor * (_SnowSmooth + (temp_output_87_0 - 0.0) * (1.0 - _SnowSmooth) / (1.0 - 0.0)) ) - temp_output_28_0 );
			float weightedBlendVar140 = ( 1.0 - temp_output_51_0 );
			float4 weightedBlend140 = ( weightedBlendVar140*temp_output_29_0 );
			float2 uv_GroundMap = i.uv_texcoord * _GroundMap_ST.xy + _GroundMap_ST.zw;
			float4 tex2DNode14 = tex2D( _GroundMap, uv_GroundMap );
			float weightedBlendVar139 = temp_output_51_0;
			float4 weightedBlend139 = ( weightedBlendVar139*tex2DNode14 );
			o.Albedo = ( weightedBlend140 + weightedBlend139 ).rgb;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=17700
0;0;1920;1019;4467.948;1359.441;3.208643;True;True
Node;AmplifyShaderEditor.Vector4Node;102;-4195.013,-686.6466;Inherit;False;Property;_SnowTilingOffset;Snow Tiling Offset;6;0;Create;True;0;0;False;0;10,10,0,0;1,1,0,0;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;104;-3936.01,-694.3466;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;105;-3942.01,-548.3466;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;81;-3728.29,-658.114;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.NoiseGeneratorNode;82;-3421.775,-803.951;Inherit;False;Gradient;False;True;2;0;FLOAT2;0,0;False;1;FLOAT;0.1;False;1;FLOAT;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;83;-3422.318,-660.5967;Inherit;False;Gradient;False;True;2;0;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;84;-3424.79,-523.7308;Inherit;False;Gradient;False;True;2;0;FLOAT2;0,0;False;1;FLOAT;0.5;False;1;FLOAT;0
Node;AmplifyShaderEditor.TexturePropertyNode;6;-2628.817,397.9509;Inherit;True;Property;_TrackMap;TrackMap;1;0;Create;False;0;0;False;0;592326119cfea9742abc708dd1578e81;592326119cfea9742abc708dd1578e81;False;white;Auto;Texture2D;-1;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.TexturePropertyNode;4;-2628.73,-18.69707;Inherit;True;Property;_MainMap;MainMap;0;0;Create;False;0;0;False;0;f88b84b71b88e364896269e8120e6154;584067f74f4ebb54fab613cc634a3cc7;False;white;Auto;Texture2D;-1;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.SamplerNode;7;-2188.481,397.9512;Inherit;True;Property;_TextureSample1;Texture Sample 1;4;0;Create;True;0;0;False;0;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;85;-3147.685,-680.5031;Inherit;False;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;5;-2195.415,-18.69716;Inherit;True;Property;_TextureSample0;Texture Sample 0;3;0;Create;True;0;0;False;0;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleDivideOpNode;87;-2974.289,-680.8101;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;3;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;97;-3078.749,-361.3337;Inherit;False;Property;_SnowSmooth;Snow Smooth;7;0;Create;True;0;0;False;0;0.6;0.4705882;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;12;-1797.601,221.1044;Inherit;True;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.TFHCRemapNode;96;-2693.749,-557.3336;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;95;-2971.835,-868.7496;Inherit;False;Property;_SnowColor;Snow Color;5;0;Create;True;0;0;False;0;0.7390531,0.9013809,0.9056604,0;0.7390531,0.9013809,0.9056604,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.OneMinusNode;28;-1548.527,-624.0056;Inherit;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ComponentMaskNode;51;-1239.188,-907.0519;Inherit;False;True;False;False;False;1;0;COLOR;0,0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TexturePropertyNode;11;-2498.021,-375.9096;Inherit;True;Property;_GroundMap;GroundMap;8;0;Create;True;0;0;False;0;b5c231fa3c5fe964f8d11551df320603;b5c231fa3c5fe964f8d11551df320603;False;white;Auto;Texture2D;-1;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.RangedFloatNode;24;-2104.732,681.0182;Inherit;False;Property;_Height;Height;2;0;Create;True;0;0;False;0;0.75;0.1;0;5;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;94;-2462.836,-698.9496;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.OneMinusNode;133;-1010.167,-902.0269;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;32;-1733.216,660.5897;Inherit;False;FLOAT3;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.NormalVertexDataNode;25;-1488.665,123.2267;Inherit;True;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;14;-2070.115,-376.6286;Inherit;True;Property;_TextureSample3;Texture Sample 3;8;0;Create;True;0;0;False;0;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleSubtractOpNode;29;-1225.773,-707.0737;Inherit;True;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.TFHCRemapNode;22;-1496.571,410.7064;Inherit;True;5;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;1,1,1,0;False;3;COLOR;0,0,0,0;False;4;COLOR;1,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;3;-724.9008,532.3086;Inherit;False;Property;_TessellationDistance;Tessellation Distance;4;0;Create;False;0;0;False;0;30;30;0;50;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;33;-1170.876,155.8307;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,1,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;2;-723.9008,430.3086;Inherit;False;Property;_Tessellation;Tessellation;3;0;Create;False;0;0;False;0;4;4;0;15;0;1;FLOAT;0
Node;AmplifyShaderEditor.SummedBlendNode;139;-973.267,-237.0271;Inherit;True;5;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SummedBlendNode;140;-758.8668,-729.827;Inherit;True;5;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;108;-3136.948,-210.6549;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;141;-367.6603,-429.8284;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;110;-3343.456,-358.5518;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;2;False;1;FLOAT;0
Node;AmplifyShaderEditor.LayeredBlendNode;56;-695.2632,-422.2881;Inherit;True;6;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.DistanceBasedTessNode;1;-311.9008,428.3086;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;27;-919.8416,277.6958;Inherit;True;2;2;0;FLOAT3;0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.VoronoiNode;106;-3454.255,-242.3861;Inherit;True;0;0;1;0;2;False;1;False;False;4;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT;0.5;False;3;FLOAT;0;False;2;FLOAT;0;FLOAT;1
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;0,0;Float;False;True;-1;6;ASEMaterialInspector;0;0;Standard;Custom/Snow;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;5;True;True;0;False;Opaque;;Geometry;ForwardOnly;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;True;2;15;10;25;False;5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;1;0.1646493,0.9359996,0.9433962,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;104;0;102;1
WireConnection;104;1;102;2
WireConnection;105;0;102;3
WireConnection;105;1;102;4
WireConnection;81;0;104;0
WireConnection;81;1;105;0
WireConnection;82;0;81;0
WireConnection;83;0;81;0
WireConnection;84;0;81;0
WireConnection;7;0;6;0
WireConnection;85;0;82;0
WireConnection;85;1;83;0
WireConnection;85;2;84;0
WireConnection;5;0;4;0
WireConnection;87;0;85;0
WireConnection;12;0;5;0
WireConnection;12;1;7;0
WireConnection;96;0;87;0
WireConnection;96;3;97;0
WireConnection;28;0;12;0
WireConnection;51;0;28;0
WireConnection;94;0;95;0
WireConnection;94;1;96;0
WireConnection;133;0;51;0
WireConnection;32;0;24;0
WireConnection;32;1;24;0
WireConnection;32;2;24;0
WireConnection;14;0;11;0
WireConnection;29;0;94;0
WireConnection;29;1;28;0
WireConnection;22;0;12;0
WireConnection;22;4;32;0
WireConnection;33;0;25;0
WireConnection;139;0;51;0
WireConnection;139;1;14;0
WireConnection;140;0;133;0
WireConnection;140;1;29;0
WireConnection;108;0;110;0
WireConnection;108;1;106;0
WireConnection;141;0;140;0
WireConnection;141;1;139;0
WireConnection;110;0;87;0
WireConnection;56;0;51;0
WireConnection;56;1;29;0
WireConnection;56;2;14;0
WireConnection;1;0;2;0
WireConnection;1;2;3;0
WireConnection;27;0;33;0
WireConnection;27;1;22;0
WireConnection;106;0;81;0
WireConnection;0;0;141;0
WireConnection;0;11;27;0
WireConnection;0;14;1;0
ASEEND*/
//CHKSM=1CEF658A85D04A8B2179A939D7B5041074307185