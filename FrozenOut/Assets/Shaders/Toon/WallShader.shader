// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "FrozenOut/WallShader"
{
	Properties
	{
		_Tint("Tint", Color) = (1,1,1,0)
		_TilingandOffset("Tiling and Offset", Vector) = (1,1,0,0)
		_VoronoiSmooth("Voronoi Smooth", Range( 0 , 1)) = 0
		_VoronoiIntensity("Voronoi Intensity", Range( 0 , 1)) = 0
		[Toggle]_Icy("Icy", Float) = 0
		_IcySmooth("Icy Smooth", Range( 0 , 1)) = 0
		_ToonRamp1("Toon Ramp", 2D) = "white" {}
		_ShadowOffset1("Shadow Offset", Range( 0 , 1)) = 0.5408505
		_RimOffset("Rim Offset", Float) = 0.8
		_RimPower("Rim Power", Range( 0 , 1)) = 0
		_RimTint("Rim Tint", Color) = (1,1,1,0)
		_Gloss("Gloss", Range( 0 , 1)) = 0
		_SpecIntensity("Spec Intensity", Range( 0 , 1)) = 0.5
		_SpecMap("Spec Map", 2D) = "white" {}
		_SpecTransition("Spec Transition", Range( 0 , 1)) = 0
		_SpecularColor("Spec Color", Color) = (1,1,1,0)
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" }
		Cull Back
		CGINCLUDE
		#include "UnityPBSLighting.cginc"
		#include "UnityCG.cginc"
		#include "UnityShaderVariables.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		#ifdef UNITY_PASS_SHADOWCASTER
			#undef INTERNAL_DATA
			#undef WorldReflectionVector
			#undef WorldNormalVector
			#define INTERNAL_DATA half3 internalSurfaceTtoW0; half3 internalSurfaceTtoW1; half3 internalSurfaceTtoW2;
			#define WorldReflectionVector(data,normal) reflect (data.worldRefl, half3(dot(data.internalSurfaceTtoW0,normal), dot(data.internalSurfaceTtoW1,normal), dot(data.internalSurfaceTtoW2,normal)))
			#define WorldNormalVector(data,normal) half3(dot(data.internalSurfaceTtoW0,normal), dot(data.internalSurfaceTtoW1,normal), dot(data.internalSurfaceTtoW2,normal))
		#endif
		struct Input
		{
			float2 uv_texcoord;
			float3 worldNormal;
			INTERNAL_DATA
			float3 worldPos;
		};

		struct SurfaceOutputCustomLightingCustom
		{
			half3 Albedo;
			half3 Normal;
			half3 Emission;
			half Metallic;
			half Smoothness;
			half Occlusion;
			half Alpha;
			Input SurfInput;
			UnityGIInput GIData;
		};

		uniform float4 _Tint;
		uniform float4 _TilingandOffset;
		uniform float _VoronoiSmooth;
		uniform float _VoronoiIntensity;
		uniform float _Icy;
		uniform float _IcySmooth;
		uniform sampler2D _ToonRamp1;
		uniform float _ShadowOffset1;
		uniform float _RimOffset;
		uniform float _RimPower;
		uniform float4 _RimTint;
		uniform float _Gloss;
		uniform sampler2D _SpecMap;
		uniform float4 _SpecMap_ST;
		uniform float4 _SpecularColor;
		uniform float _SpecTransition;
		uniform float _SpecIntensity;


		float2 voronoihash133( float2 p )
		{
			p = p - 10 * floor( p / 10 );
			p = float2( dot( p, float2( 127.1, 311.7 ) ), dot( p, float2( 269.5, 183.3 ) ) );
			return frac( sin( p ) *43758.5453);
		}


		float voronoi133( float2 v, float time, inout float2 id, float smoothness )
		{
			float2 n = floor( v );
			float2 f = frac( v );
			float F1 = 8.0;
			float F2 = 8.0; float2 mr = 0; float2 mg = 0;
			for ( int j = -1; j <= 1; j++ )
			{
				for ( int i = -1; i <= 1; i++ )
			 	{
			 		float2 g = float2( i, j );
			 		float2 o = voronoihash133( n + g );
					o = ( sin( time + o * 6.2831 ) * 0.5 + 0.5 ); float2 r = g - f + o;
					float d = 0.5 * dot( r, r );
			 		if( d<F1 ) {
			 			F2 = F1;
			 			F1 = d; mg = g; mr = r; id = o;
			 		} else if( d<F2 ) {
			 			F2 = d;
			 		}
			 	}
			}
			return F1;
		}


		float2 voronoihash107( float2 p )
		{
			p = p - 10 * floor( p / 10 );
			p = float2( dot( p, float2( 127.1, 311.7 ) ), dot( p, float2( 269.5, 183.3 ) ) );
			return frac( sin( p ) *43758.5453);
		}


		float voronoi107( float2 v, float time, inout float2 id, float smoothness )
		{
			float2 n = floor( v );
			float2 f = frac( v );
			float F1 = 8.0;
			float F2 = 8.0; float2 mr = 0; float2 mg = 0;
			for ( int j = -1; j <= 1; j++ )
			{
				for ( int i = -1; i <= 1; i++ )
			 	{
			 		float2 g = float2( i, j );
			 		float2 o = voronoihash107( n + g );
					o = ( sin( time + o * 6.2831 ) * 0.5 + 0.5 ); float2 r = g - f + o;
					float d = 0.5 * dot( r, r );
			 		if( d<F1 ) {
			 			F2 = F1;
			 			F1 = d; mg = g; mr = r; id = o;
			 		} else if( d<F2 ) {
			 			F2 = d;
			 		}
			 	}
			}
			return F2;
		}


		inline half4 LightingStandardCustomLighting( inout SurfaceOutputCustomLightingCustom s, half3 viewDir, UnityGI gi )
		{
			UnityGIInput data = s.GIData;
			Input i = s.SurfInput;
			half4 c = 0;
			#ifdef UNITY_PASS_FORWARDBASE
			float ase_lightAtten = data.atten;
			if( _LightColor0.a == 0)
			ase_lightAtten = 0;
			#else
			float3 ase_lightAttenRGB = gi.light.color / ( ( _LightColor0.rgb ) + 0.000001 );
			float ase_lightAtten = max( max( ase_lightAttenRGB.r, ase_lightAttenRGB.g ), ase_lightAttenRGB.b );
			#endif
			#if defined(HANDLE_SHADOWS_BLENDING_IN_GI)
			half bakedAtten = UnitySampleBakedOcclusion(data.lightmapUV.xy, data.worldPos);
			float zDist = dot(_WorldSpaceCameraPos - data.worldPos, UNITY_MATRIX_V[2].xyz);
			float fadeDist = UnityComputeShadowFadeDistance(data.worldPos, zDist);
			ase_lightAtten = UnityMixRealtimeAndBakedShadows(data.atten, bakedAtten, UnityComputeShadowFade(fadeDist));
			#endif
			float time133 = 0.87;
			float2 appendResult111 = (float2(_TilingandOffset.x , _TilingandOffset.y));
			float2 appendResult110 = (float2(_TilingandOffset.z , _TilingandOffset.w));
			float2 uv_TexCoord108 = i.uv_texcoord * appendResult111 + appendResult110;
			float2 coords133 = uv_TexCoord108 * 5.0;
			float2 id133 = 0;
			float voroi133 = voronoi133( coords133, time133,id133, 0 );
			float time107 = 0.0;
			float2 coords107 = uv_TexCoord108 * 10.0;
			float2 id107 = 0;
			float fade107 = 0.5;
			float voroi107 = 0;
			float rest107 = 0;
			for( int it = 0; it <2; it++ ){
			voroi107 += fade107 * voronoi107( coords107, time107, id107,0 );
			rest107 += fade107;
			coords107 *= 2;
			fade107 *= 0.5;
			}
			voroi107 /= rest107;
			float ifLocalVar132 = 0;
			if( (( _Icy )?( voroi107 ):( 0.0 )) > 0.0 )
				ifLocalVar132 = voroi107;
			else if( (( _Icy )?( voroi107 ):( 0.0 )) == 0.0 )
				ifLocalVar132 = (( _Icy )?( voroi107 ):( 0.0 ));
			else if( (( _Icy )?( voroi107 ):( 0.0 )) < 0.0 )
				ifLocalVar132 = voroi107;
			float ice_voronoi121 = ifLocalVar132;
			float4 albedo34 = ( ( _Tint + ( (_VoronoiSmooth + (voroi133 - 0.0) * (1.0 - _VoronoiSmooth) / (1.0 - 0.0)) * _VoronoiIntensity ) ) + (0.0 + (ice_voronoi121 - 0.0) * (_IcySmooth - 0.0) / (1.0 - 0.0)) );
			float3 ase_worldNormal = WorldNormalVector( i, float3( 0, 0, 1 ) );
			float3 ase_normWorldNormal = normalize( ase_worldNormal );
			float3 ase_worldPos = i.worldPos;
			#if defined(LIGHTMAP_ON) && UNITY_VERSION < 560 //aseld
			float3 ase_worldlightDir = 0;
			#else //aseld
			float3 ase_worldlightDir = Unity_SafeNormalize( UnityWorldSpaceLightDir( ase_worldPos ) );
			#endif //aseld
			float dotResult3 = dot( ase_normWorldNormal , ase_worldlightDir );
			float normal_lightdir4 = dotResult3;
			float temp_output_242_0 = (normal_lightdir4*_ShadowOffset1 + _ShadowOffset1);
			float2 temp_cast_0 = (temp_output_242_0).xx;
			float4 shadow246 = ( albedo34 * tex2D( _ToonRamp1, temp_cast_0 ) );
			#if defined(LIGHTMAP_ON) && ( UNITY_VERSION < 560 || ( defined(LIGHTMAP_SHADOW_MIXING) && !defined(SHADOWS_SHADOWMASK) && defined(SHADOWS_SCREEN) ) )//aselc
			float4 ase_lightColor = 0;
			#else //aselc
			float4 ase_lightColor = _LightColor0;
			#endif //aselc
			UnityGI gi42 = gi;
			float3 diffNorm42 = ase_worldNormal;
			gi42 = UnityGI_Base( data, 1, diffNorm42 );
			float3 indirectDiffuse42 = gi42.indirect.diffuse + diffNorm42 * 0.0001;
			float4 lighting41 = ( shadow246 * ( ase_lightColor * float4( ( indirectDiffuse42 + ase_lightAtten ) , 0.0 ) ) );
			float3 ase_worldViewDir = Unity_SafeNormalize( UnityWorldSpaceViewDir( ase_worldPos ) );
			float dotResult7 = dot( ase_normWorldNormal , ase_worldViewDir );
			float normal_viewdir8 = dotResult7;
			float4 rim55 = ( saturate( ( pow( ( 1.0 - saturate( ( _RimOffset + normal_viewdir8 ) ) ) , _RimPower ) * ( normal_lightdir4 * ase_lightAtten ) ) ) * ( ase_lightColor * _RimTint ) );
			float dotResult75 = dot( ( ase_worldViewDir + _WorldSpaceLightPos0.xyz ) , ase_worldNormal );
			float smoothstepResult78 = smoothstep( 1.1 , 1.12 , pow( dotResult75 , _Gloss ));
			float2 uv_SpecMap = i.uv_texcoord * _SpecMap_ST.xy + _SpecMap_ST.zw;
			float4 lerpResult92 = lerp( _SpecularColor , ase_lightColor , _SpecTransition);
			float ifLocalVar130 = 0;
			if( ice_voronoi121 == 0.0 )
				ifLocalVar130 = 1.0;
			else
				ifLocalVar130 = ice_voronoi121;
			float4 spec83 = ( ase_lightAtten * ( ( smoothstepResult78 * ( ( tex2D( _SpecMap, uv_SpecMap ) * lerpResult92 ) * ifLocalVar130 ) ) * _SpecIntensity ) );
			c.rgb = ( ( lighting41 + rim55 ) + spec83 ).rgb;
			c.a = 1;
			return c;
		}

		inline void LightingStandardCustomLighting_GI( inout SurfaceOutputCustomLightingCustom s, UnityGIInput data, inout UnityGI gi )
		{
			s.GIData = data;
		}

		void surf( Input i , inout SurfaceOutputCustomLightingCustom o )
		{
			o.SurfInput = i;
			o.Normal = float3(0,0,1);
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf StandardCustomLighting keepalpha fullforwardshadows 

		ENDCG
		Pass
		{
			Name "ShadowCaster"
			Tags{ "LightMode" = "ShadowCaster" }
			ZWrite On
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
			#pragma multi_compile_shadowcaster
			#pragma multi_compile UNITY_PASS_SHADOWCASTER
			#pragma skip_variants FOG_LINEAR FOG_EXP FOG_EXP2
			#include "HLSLSupport.cginc"
			#if ( SHADER_API_D3D11 || SHADER_API_GLCORE || SHADER_API_GLES || SHADER_API_GLES3 || SHADER_API_METAL || SHADER_API_VULKAN )
				#define CAN_SKIP_VPOS
			#endif
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "UnityPBSLighting.cginc"
			struct v2f
			{
				V2F_SHADOW_CASTER;
				float2 customPack1 : TEXCOORD1;
				float4 tSpace0 : TEXCOORD2;
				float4 tSpace1 : TEXCOORD3;
				float4 tSpace2 : TEXCOORD4;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};
			v2f vert( appdata_full v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_INITIALIZE_OUTPUT( v2f, o );
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO( o );
				UNITY_TRANSFER_INSTANCE_ID( v, o );
				Input customInputData;
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				half3 worldNormal = UnityObjectToWorldNormal( v.normal );
				half3 worldTangent = UnityObjectToWorldDir( v.tangent.xyz );
				half tangentSign = v.tangent.w * unity_WorldTransformParams.w;
				half3 worldBinormal = cross( worldNormal, worldTangent ) * tangentSign;
				o.tSpace0 = float4( worldTangent.x, worldBinormal.x, worldNormal.x, worldPos.x );
				o.tSpace1 = float4( worldTangent.y, worldBinormal.y, worldNormal.y, worldPos.y );
				o.tSpace2 = float4( worldTangent.z, worldBinormal.z, worldNormal.z, worldPos.z );
				o.customPack1.xy = customInputData.uv_texcoord;
				o.customPack1.xy = v.texcoord;
				TRANSFER_SHADOW_CASTER_NORMALOFFSET( o )
				return o;
			}
			half4 frag( v2f IN
			#if !defined( CAN_SKIP_VPOS )
			, UNITY_VPOS_TYPE vpos : VPOS
			#endif
			) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				Input surfIN;
				UNITY_INITIALIZE_OUTPUT( Input, surfIN );
				surfIN.uv_texcoord = IN.customPack1.xy;
				float3 worldPos = float3( IN.tSpace0.w, IN.tSpace1.w, IN.tSpace2.w );
				half3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				surfIN.worldPos = worldPos;
				surfIN.worldNormal = float3( IN.tSpace0.z, IN.tSpace1.z, IN.tSpace2.z );
				surfIN.internalSurfaceTtoW0 = IN.tSpace0.xyz;
				surfIN.internalSurfaceTtoW1 = IN.tSpace1.xyz;
				surfIN.internalSurfaceTtoW2 = IN.tSpace2.xyz;
				SurfaceOutputCustomLightingCustom o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutputCustomLightingCustom, o )
				surf( surfIN, o );
				#if defined( CAN_SKIP_VPOS )
				float2 vpos = IN.pos;
				#endif
				SHADOW_CASTER_FRAGMENT( IN )
			}
			ENDCG
		}
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=17700
0;9;1920;1010;-2544.791;1938.129;1;True;True
Node;AmplifyShaderEditor.CommentaryNode;120;-4700.568,-3775.559;Inherit;False;7599.329;2282.008;Comment;12;47;95;21;65;10;11;145;133;147;148;109;246;Toon Shader;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;35;-3617.388,-3487.68;Inherit;False;2132.814;784.126;Comment;14;34;106;116;113;107;32;108;110;111;121;114;132;134;135;Albedo;1,1,1,1;0;0
Node;AmplifyShaderEditor.Vector4Node;109;-3591.366,-3205.396;Inherit;False;Property;_TilingandOffset;Tiling and Offset;1;0;Create;True;0;0;False;0;1,1,0,0;0,0,0,0;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;111;-3369.366,-3205.396;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;110;-3369.366,-3079.396;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;108;-3215.381,-3190.754;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.VoronoiNode;107;-2893.288,-3036.881;Inherit;True;0;0;1;1;2;True;10;False;False;4;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT;10;False;3;FLOAT;0;False;2;FLOAT;0;FLOAT;1
Node;AmplifyShaderEditor.CommentaryNode;11;-4632.5,-2554.073;Inherit;False;911.5872;420.6679;Comment;5;8;7;5;6;25;Normal.ViewDir;1,1,1,1;0;0
Node;AmplifyShaderEditor.ToggleSwitchNode;106;-2622.857,-3087.917;Inherit;False;Property;_Icy;Icy;4;0;Create;True;0;0;False;0;0;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;146;-2693.101,-2979.888;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.VoronoiNode;133;-2888.066,-3291.073;Inherit;True;0;0;1;0;1;True;10;False;False;4;0;FLOAT2;0,0;False;1;FLOAT;0.87;False;2;FLOAT;5;False;3;FLOAT;0.12;False;2;FLOAT;0;FLOAT;1
Node;AmplifyShaderEditor.WorldNormalVector;5;-4355.085,-2477.561;Inherit;False;True;1;0;FLOAT3;0,0,1;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;135;-2920.45,-3386.372;Inherit;False;Property;_VoronoiSmooth;Voronoi Smooth;2;0;Create;True;0;0;False;0;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.ViewDirInputsCoordNode;6;-4332.042,-2309.204;Inherit;False;World;True;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.CommentaryNode;10;-4651.163,-3122.068;Inherit;False;937.1509;422.1295;Comment;5;26;4;3;2;1;Normal.Light;1,1,1,1;0;0
Node;AmplifyShaderEditor.WorldSpaceLightDirHlpNode;2;-4385.123,-2878.326;Inherit;False;True;1;0;FLOAT;0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.WorldNormalVector;1;-4352.055,-3048.921;Inherit;False;True;1;0;FLOAT3;0,0,1;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;147;-2630.694,-3190.383;Inherit;False;Property;_VoronoiIntensity;Voronoi Intensity;3;0;Create;True;0;0;False;0;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;134;-2546.456,-3374.32;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.DotProductOpNode;7;-4091.044,-2400.204;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ConditionalIfNode;132;-2403.492,-3058.702;Inherit;False;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;148;-2322.34,-3265.224;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;121;-2241.36,-3063.792;Inherit;False;ice_voronoi;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DotProductOpNode;3;-4088.452,-2971.172;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;8;-3928.582,-2412.592;Float;False;normal_viewdir;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;114;-2400.776,-2863.641;Inherit;False;Property;_IcySmooth;Icy Smooth;5;0;Create;True;0;0;False;0;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;65;-2032.573,-2461.12;Inherit;False;2038.134;634.727;Comment;17;48;49;50;51;52;53;54;57;59;60;56;55;61;62;63;64;67;Rim Light;1,1,1,1;0;0
Node;AmplifyShaderEditor.ColorNode;32;-2319.723,-3439.887;Inherit;False;Property;_Tint;Tint;0;0;Create;True;0;0;False;0;1,1,1,0;0.7924528,0.5032943,0.2878248,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.CommentaryNode;95;260.3625,-3217.066;Inherit;False;2581.862;1342.243;Comment;25;123;86;130;83;82;79;81;80;78;76;94;77;75;122;92;85;131;90;73;72;93;89;71;70;74;Spec;1,1,1,1;0;0
Node;AmplifyShaderEditor.TFHCRemapNode;113;-2049.791,-3060.302;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;48;-1982.573,-2310.387;Inherit;False;8;normal_viewdir;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;49;-1950.633,-2411.12;Inherit;False;Property;_RimOffset;Rim Offset;8;0;Create;True;0;0;False;0;0.8;0.55;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;4;-3927.432,-2979.434;Float;False;normal_lightdir;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;145;-2008.475,-3288.769;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;50;-1719.686,-2369.353;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WorldSpaceLightPos;71;310.3626,-2943.711;Inherit;False;0;3;FLOAT4;0;FLOAT3;1;FLOAT;2
Node;AmplifyShaderEditor.RangedFloatNode;240;-1302.42,-2983.539;Inherit;False;Property;_ShadowOffset1;Shadow Offset;7;0;Create;True;0;0;False;0;0.5408505;0.434;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;21;-1368.173,-3386.102;Inherit;False;1389.021;693.9789;Comment;4;243;241;245;242;Shadow;1,1,1,1;0;0
Node;AmplifyShaderEditor.ViewDirInputsCoordNode;70;366.7977,-3152.066;Inherit;False;World;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.GetLocalVarNode;239;-1300.357,-3123.321;Inherit;False;4;normal_lightdir;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;116;-1803.82,-3116.823;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;89;623.4786,-2430.32;Inherit;False;Property;_SpecularColor;Spec Color;15;0;Create;False;0;0;False;0;1,1,1,0;0.7921569,0.5019608,0.2862745,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;34;-1689.252,-3120.865;Inherit;False;albedo;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.LightColorNode;90;676.6605,-2231.097;Inherit;False;0;3;COLOR;0;FLOAT3;1;FLOAT;2
Node;AmplifyShaderEditor.SimpleAddOpNode;72;629.7835,-3061.392;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;93;555.7926,-2066.719;Inherit;False;Property;_SpecTransition;Spec Transition;14;0;Create;True;0;0;False;0;0;0.5;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.WorldNormalVector;73;583.9966,-2804.319;Inherit;False;False;1;0;FLOAT3;0,0,1;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.ScaleAndOffsetNode;242;-971.621,-3023.583;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;1;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;51;-1534.19,-2370.581;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;122;973.925,-2205.402;Inherit;False;121;ice_voronoi;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;247;-695.9199,-2949.424;Inherit;True;Property;_ToonRamp1;Toon Ramp;6;0;Create;True;0;0;False;0;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;244;-580.558,-3256.546;Inherit;False;34;albedo;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.CommentaryNode;47;-3583.258,-2447.19;Inherit;False;1363.428;609.8745;Comment;9;39;41;40;42;45;44;43;46;38;Lighting;1,1,1,1;0;0
Node;AmplifyShaderEditor.DotProductOpNode;75;839.3716,-2935.315;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;61;-1359.644,-2141.445;Inherit;False;4;normal_lightdir;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;77;842.0176,-2781.825;Inherit;False;Property;_Gloss;Gloss;11;0;Create;True;0;0;False;0;0;0.26;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;92;984.47,-2347.13;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.OneMinusNode;52;-1311.841,-2369.353;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;54;-1437.144,-2261.249;Inherit;False;Property;_RimPower;Rim Power;9;0;Create;True;0;0;False;0;0;0.25;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;85;833.929,-2670.328;Inherit;True;Property;_SpecMap;Spec Map;13;0;Create;True;0;0;False;0;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;131;993.8047,-2134.003;Inherit;False;Constant;_Float0;Float 0;17;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.LightAttenuation;62;-1355.644,-2026.445;Inherit;False;0;1;FLOAT;0
Node;AmplifyShaderEditor.LightAttenuation;43;-3275.259,-1948.316;Inherit;False;0;1;FLOAT;0
Node;AmplifyShaderEditor.ConditionalIfNode;130;1252.564,-2199.708;Inherit;False;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;76;1164.876,-2933.992;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;245;-378.5582,-3252.546;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;63;-1121.644,-2095.445;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.IndirectDiffuseLighting;42;-3296.259,-2071.316;Inherit;False;Tangent;1;0;FLOAT3;0,0,1;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;94;1203.642,-2506.675;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.PowerNode;53;-1074.751,-2360.753;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.LightColorNode;38;-3056.929,-2175.468;Inherit;False;0;3;COLOR;0;FLOAT3;1;FLOAT;2
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;123;1456.271,-2508.507;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;246;-190.979,-3257.617;Inherit;False;shadow;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.LightColorNode;57;-834.6503,-2198.393;Inherit;False;0;3;COLOR;0;FLOAT3;1;FLOAT;2
Node;AmplifyShaderEditor.ColorNode;59;-882.6503,-2038.393;Inherit;False;Property;_RimTint;Rim Tint;10;0;Create;True;0;0;False;0;1,1,1,0;1,1,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SmoothstepOpNode;78;1391.465,-2934.638;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;1.1;False;2;FLOAT;1.12;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;64;-826.6443,-2359.445;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;44;-3022.257,-2023.316;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;80;1801.234,-2755.362;Inherit;False;Property;_SpecIntensity;Spec Intensity;12;0;Create;True;0;0;False;0;0.5;0.188;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;60;-626.6494,-2118.393;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;86;1720.45,-2937.45;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SaturateNode;67;-614.2704,-2360.638;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;39;-2873.616,-2397.191;Inherit;False;246;shadow;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;46;-2859.56,-2109.143;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT3;0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;79;2175.696,-2935.315;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;40;-2648.154,-2254.516;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;56;-395.6903,-2361.916;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.LightAttenuation;81;2120.122,-3096.744;Inherit;False;0;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;55;-218.4387,-2366.531;Inherit;False;rim;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;82;2380.79,-3017.353;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;41;-2443.83,-2261.561;Inherit;False;lighting;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;69;3280.298,-1157.589;Inherit;False;55;rim;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;83;2607.055,-3018.675;Inherit;False;spec;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;20;3280.427,-1247.44;Inherit;False;41;lighting;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.CommentaryNode;24;-5647.75,-1149.726;Inherit;False;630.4309;280;Comment;2;23;236;Normal Map;1,1,1,1;0;0
Node;AmplifyShaderEditor.SimpleAddOpNode;68;3517.799,-1175.372;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;84;3471.544,-1067.727;Inherit;False;83;spec;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;74;364.7616,-2809.197;Inherit;False;23;normal;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;25;-4600.572,-2483.279;Inherit;False;23;normal;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;236;-5615.234,-1098.217;Inherit;True;Property;_TextureSample0;Texture Sample 0;16;0;Create;True;0;0;False;0;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;26;-4614.191,-3053.461;Inherit;False;23;normal;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;96;3660.014,-1135.757;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GradientSampleNode;243;-699.742,-3161.464;Inherit;True;2;0;OBJECT;;False;1;FLOAT;0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GradientNode;241;-974.284,-3162.22;Inherit;False;1;5;2;0.2264151,0.2264151,0.2264151,0.1647059;0.490566,0.490566,0.490566,0.3794156;0.7830189,0.7830189,0.7830189,0.6176547;1,1,1,0.8294194;1,1,1,1;1,0;1,1;0;1;OBJECT;0
Node;AmplifyShaderEditor.GetLocalVarNode;45;-3533.258,-2066.316;Inherit;False;23;normal;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;23;-5241.319,-1098.856;Inherit;False;normal;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;3881.506,-1377.387;Float;False;True;-1;2;ASEMaterialInspector;0;0;CustomLighting;FrozenOut/WallShader;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Translucent;5;True;True;0;False;Opaque;;Transparent;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;5;True;0;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;1;False;-1;0;False;-1;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT3;0,0,0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;111;0;109;1
WireConnection;111;1;109;2
WireConnection;110;0;109;3
WireConnection;110;1;109;4
WireConnection;108;0;111;0
WireConnection;108;1;110;0
WireConnection;107;0;108;0
WireConnection;106;1;107;0
WireConnection;146;0;107;0
WireConnection;133;0;108;0
WireConnection;134;0;133;0
WireConnection;134;3;135;0
WireConnection;7;0;5;0
WireConnection;7;1;6;0
WireConnection;132;0;106;0
WireConnection;132;2;107;0
WireConnection;132;3;106;0
WireConnection;132;4;146;0
WireConnection;148;0;134;0
WireConnection;148;1;147;0
WireConnection;121;0;132;0
WireConnection;3;0;1;0
WireConnection;3;1;2;0
WireConnection;8;0;7;0
WireConnection;113;0;121;0
WireConnection;113;4;114;0
WireConnection;4;0;3;0
WireConnection;145;0;32;0
WireConnection;145;1;148;0
WireConnection;50;0;49;0
WireConnection;50;1;48;0
WireConnection;116;0;145;0
WireConnection;116;1;113;0
WireConnection;34;0;116;0
WireConnection;72;0;70;0
WireConnection;72;1;71;1
WireConnection;242;0;239;0
WireConnection;242;1;240;0
WireConnection;242;2;240;0
WireConnection;51;0;50;0
WireConnection;247;1;242;0
WireConnection;75;0;72;0
WireConnection;75;1;73;0
WireConnection;92;0;89;0
WireConnection;92;1;90;0
WireConnection;92;2;93;0
WireConnection;52;0;51;0
WireConnection;130;0;122;0
WireConnection;130;2;122;0
WireConnection;130;3;131;0
WireConnection;130;4;122;0
WireConnection;76;0;75;0
WireConnection;76;1;77;0
WireConnection;245;0;244;0
WireConnection;245;1;247;0
WireConnection;63;0;61;0
WireConnection;63;1;62;0
WireConnection;94;0;85;0
WireConnection;94;1;92;0
WireConnection;53;0;52;0
WireConnection;53;1;54;0
WireConnection;123;0;94;0
WireConnection;123;1;130;0
WireConnection;246;0;245;0
WireConnection;78;0;76;0
WireConnection;64;0;53;0
WireConnection;64;1;63;0
WireConnection;44;0;42;0
WireConnection;44;1;43;0
WireConnection;60;0;57;0
WireConnection;60;1;59;0
WireConnection;86;0;78;0
WireConnection;86;1;123;0
WireConnection;67;0;64;0
WireConnection;46;0;38;0
WireConnection;46;1;44;0
WireConnection;79;0;86;0
WireConnection;79;1;80;0
WireConnection;40;0;39;0
WireConnection;40;1;46;0
WireConnection;56;0;67;0
WireConnection;56;1;60;0
WireConnection;55;0;56;0
WireConnection;82;0;81;0
WireConnection;82;1;79;0
WireConnection;41;0;40;0
WireConnection;83;0;82;0
WireConnection;68;0;20;0
WireConnection;68;1;69;0
WireConnection;96;0;68;0
WireConnection;96;1;84;0
WireConnection;243;0;241;0
WireConnection;243;1;242;0
WireConnection;0;13;96;0
ASEEND*/
//CHKSM=C8902EB4D647530FAFAF73FDDC0D901C3148A4AC