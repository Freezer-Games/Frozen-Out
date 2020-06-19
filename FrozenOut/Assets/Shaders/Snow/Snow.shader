// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Custom/Snow"
{
	Properties
	{
		_MainMap("MainMap", 2D) = "white" {}
		_TrackMap("TrackMap", 2D) = "white" {}
		_SnowColor("Snow Color", Color) = (1,1,1,0)
		_SnowSmooth("Snow Smooth", Range( 0 , 1)) = 0.6
		_SandColor("Sand Color", Color) = (0.254539,0.6686114,0.8301887,0)
		_SandSmooth("Sand Smooth", Range( 0 , 1)) = 0.1529412
		_SandIntensity("Sand Intensity", Float) = 0.15
		_TilingandOffset("Tiling and Offset", Vector) = (10,10,0,0)
		_Height("Height", Range( 0 , 5)) = 0.75
		_Tessellation("Tessellation", Range( 0 , 15)) = 4
		_TessellationDistance("Tessellation Distance", Range( 0 , 50)) = 30
		_ToonRamp("Toon Ramp", 2D) = "white" {}
		_ShadowOffset("Shadow Offset", Range( 0 , 1)) = 0.524439
		_RimOffset("Rim Offset", Float) = 0.8
		_RimPower("Rim Power", Range( 0 , 1)) = 0
		_RimTint("Rim Tint", Color) = (1,1,1,0)
		_Gloss("Gloss", Range( 0 , 1)) = 0
		_SnowSpecIntensity("Snow Spec Intensity", Range( 0 , 1)) = 0
		_SandSpecIntensity("Sand Spec Intensity", Range( 0 , 1)) = 0.5
		_SpecTransition("Spec Transition", Range( 0 , 1)) = 0
		_SpecularColor1("Spec Color", Color) = (1,1,1,0)
		_GroundTexture("GroundTexture", 2D) = "white" {}
		_GroundMap("Ground Map", 2D) = "white" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" }
		Cull Back
		CGINCLUDE
		#include "UnityPBSLighting.cginc"
		#include "UnityCG.cginc"
		#include "UnityShaderVariables.cginc"
		#include "Tessellation.cginc"
		#include "Lighting.cginc"
		#pragma target 4.6
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

		uniform sampler2D _MainMap;
		uniform float4 _MainMap_ST;
		uniform sampler2D _TrackMap;
		uniform float4 _TrackMap_ST;
		uniform float _Height;
		uniform float4 _SnowColor;
		uniform float4 _TilingandOffset;
		uniform float _SnowSmooth;
		uniform sampler2D _GroundMap;
		uniform float4 _GroundMap_ST;
		uniform sampler2D _GroundTexture;
		uniform float4 _SandColor;
		uniform float _SandSmooth;
		uniform float _SandIntensity;
		uniform sampler2D _ToonRamp;
		uniform float _ShadowOffset;
		uniform float _Gloss;
		uniform float4 _SpecularColor1;
		uniform float _SpecTransition;
		uniform float _SnowSpecIntensity;
		uniform float _SandSpecIntensity;
		uniform float _RimOffset;
		uniform float _RimPower;
		uniform float4 _RimTint;
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


		float2 voronoihash129( float2 p )
		{
			p = p - 10 * floor( p / 10 );
			p = float2( dot( p, float2( 127.1, 311.7 ) ), dot( p, float2( 269.5, 183.3 ) ) );
			return frac( sin( p ) *43758.5453);
		}


		float voronoi129( float2 v, float time, inout float2 id, float smoothness )
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
			 		float2 o = voronoihash129( n + g );
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


		//https://www.shadertoy.com/view/XdXGW8
		float2 GradientNoiseDir( float2 x )
		{
			const float2 k = float2( 0.3183099, 0.3678794 );
			x = x * k + k.yx;
			return -1.0 + 2.0 * frac( 16.0 * k * frac( x.x * x.y * ( x.x + x.y ) ) );
		}
		
		float GradientNoise( float2 UV, float Scale )
		{
			float2 p = UV * Scale;
			float2 i = floor( p );
			float2 f = frac( p );
			float2 u = f * f * ( 3.0 - 2.0 * f );
			return lerp( lerp( dot( GradientNoiseDir( i + float2( 0.0, 0.0 ) ), f - float2( 0.0, 0.0 ) ),
					dot( GradientNoiseDir( i + float2( 1.0, 0.0 ) ), f - float2( 1.0, 0.0 ) ), u.x ),
					lerp( dot( GradientNoiseDir( i + float2( 0.0, 1.0 ) ), f - float2( 0.0, 1.0 ) ),
					dot( GradientNoiseDir( i + float2( 1.0, 1.0 ) ), f - float2( 1.0, 1.0 ) ), u.x ), u.y );
		}


		float2 voronoihash274( float2 p )
		{
			p = p - 10 * floor( p / 10 );
			p = float2( dot( p, float2( 127.1, 311.7 ) ), dot( p, float2( 269.5, 183.3 ) ) );
			return frac( sin( p ) *43758.5453);
		}


		float voronoi274( float2 v, float time, inout float2 id, float smoothness )
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
			 		float2 o = voronoihash274( n + g );
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


		float4 tessFunction( appdata_full v0, appdata_full v1, appdata_full v2 )
		{
			float4 tessellation215 = UnityDistanceBasedTess( v0.vertex, v1.vertex, v2.vertex, 0.0,_TessellationDistance,_Tessellation);
			return tessellation215;
		}

		void vertexDataFunc( inout appdata_full v )
		{
			float3 ase_vertexNormal = v.normal.xyz;
			float2 uv_MainMap = v.texcoord * _MainMap_ST.xy + _MainMap_ST.zw;
			float2 uv_TrackMap = v.texcoord * _TrackMap_ST.xy + _TrackMap_ST.zw;
			float4 splatMap210 = (float4( 0,0,0,0 ) + (( tex2Dlod( _MainMap, float4( uv_MainMap, 0, 0.0) ) - tex2Dlod( _TrackMap, float4( uv_TrackMap, 0, 0.0) ) ) - float4( 0,0,0,0 )) * (float4( 1,1,1,0 ) - float4( 0,0,0,0 )) / (float4( 1,1,1,0 ) - float4( 0,0,0,0 )));
			float3 appendResult32 = (float3(_Height , _Height , _Height));
			float4 vertexOffset213 = ( float4( ( ase_vertexNormal * float3( 0,1,0 ) ) , 0.0 ) * (float4( 0,0,0,0 ) + (splatMap210 - float4( 0,0,0,0 )) * (float4( appendResult32 , 0.0 ) - float4( 0,0,0,0 )) / (float4( 1,1,1,0 ) - float4( 0,0,0,0 ))) );
			v.vertex.xyz += vertexOffset213.rgb;
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
			float2 uv_MainMap = i.uv_texcoord * _MainMap_ST.xy + _MainMap_ST.zw;
			float2 uv_TrackMap = i.uv_texcoord * _TrackMap_ST.xy + _TrackMap_ST.zw;
			float4 splatMap210 = (float4( 0,0,0,0 ) + (( tex2D( _MainMap, uv_MainMap ) - tex2D( _TrackMap, uv_TrackMap ) ) - float4( 0,0,0,0 )) * (float4( 1,1,1,0 ) - float4( 0,0,0,0 )) / (float4( 1,1,1,0 ) - float4( 0,0,0,0 )));
			float temp_output_51_0 = (( 1.0 - splatMap210 )).r;
			float2 appendResult401 = (float2(( 1.0 - temp_output_51_0 ) , temp_output_51_0));
			float2 appendResult104 = (float2(_TilingandOffset.x , _TilingandOffset.y));
			float2 appendResult105 = (float2(_TilingandOffset.z , _TilingandOffset.w));
			float2 uv_TexCoord81 = i.uv_texcoord * appendResult104 + appendResult105;
			float2 tex_coordinates218 = uv_TexCoord81;
			float gradientNoise82 = UnityGradientNoise(tex_coordinates218,0.1);
			float gradientNoise83 = UnityGradientNoise(tex_coordinates218,1.0);
			float gradientNoise84 = UnityGradientNoise(tex_coordinates218,0.5);
			float4 perlinNoise221 = ( _SnowColor * (_SnowSmooth + (( ( gradientNoise82 + gradientNoise83 + gradientNoise84 ) / 3.0 ) - 0.0) * (1.0 - _SnowSmooth) / (1.0 - 0.0)) );
			float2 uv_GroundMap = i.uv_texcoord * _GroundMap_ST.xy + _GroundMap_ST.zw;
			float4 tex2DNode320 = tex2D( _GroundMap, uv_GroundMap );
			float2 appendResult397 = (float2(tex2DNode320.r , ( 1.0 - tex2DNode320.r )));
			float time129 = 0.87;
			float2 coords129 = tex_coordinates218 * 1.0;
			float2 id129 = 0;
			float voroi129 = voronoi129( coords129, time129,id129, 0 );
			float gradientNoise227 = GradientNoise(tex_coordinates218,2.0);
			gradientNoise227 = gradientNoise227*0.5 + 0.5;
			float temp_output_228_0 = ( voroi129 + gradientNoise227 );
			float4 sandNoise234 = (float4( 0,0,0,0 ) + (( _SandColor + ( (_SandSmooth + (temp_output_228_0 - 0.0) * (1.0 - _SandSmooth) / (1.0 - 0.0)) * _SandIntensity ) ) - float4( 0,0,0,0 )) * (float4( 1,1,1,0 ) - float4( 0,0,0,0 )) / (float4( 1,1,1,0 ) - float4( 0,0,0,0 )));
			float2 weightedBlendVar393 = appendResult397;
			float4 weightedBlend393 = ( weightedBlendVar393.x*tex2D( _GroundTexture, tex_coordinates218 ) + weightedBlendVar393.y*sandNoise234 );
			float4 groundTex319 = weightedBlend393;
			float2 weightedBlendVar399 = appendResult401;
			float4 weightedBlend399 = ( weightedBlendVar399.x*perlinNoise221 + weightedBlendVar399.y*groundTex319 );
			float4 albedo167 = weightedBlend399;
			float3 ase_worldNormal = WorldNormalVector( i, float3( 0, 0, 1 ) );
			float3 ase_normWorldNormal = normalize( ase_worldNormal );
			float3 ase_worldPos = i.worldPos;
			#if defined(LIGHTMAP_ON) && UNITY_VERSION < 560 //aseld
			float3 ase_worldlightDir = 0;
			#else //aseld
			float3 ase_worldlightDir = Unity_SafeNormalize( UnityWorldSpaceLightDir( ase_worldPos ) );
			#endif //aseld
			float dotResult139 = dot( ase_normWorldNormal , ase_worldlightDir );
			float normal_lightdir149 = dotResult139;
			float temp_output_161_0 = (normal_lightdir149*_ShadowOffset + _ShadowOffset);
			float2 temp_cast_0 = (temp_output_161_0).xx;
			float4 shadow188 = ( albedo167 * tex2D( _ToonRamp, temp_cast_0 ) );
			#if defined(LIGHTMAP_ON) && ( UNITY_VERSION < 560 || ( defined(LIGHTMAP_SHADOW_MIXING) && !defined(SHADOWS_SHADOWMASK) && defined(SHADOWS_SCREEN) ) )//aselc
			float4 ase_lightColor = 0;
			#else //aselc
			float4 ase_lightColor = _LightColor0;
			#endif //aselc
			UnityGI gi246 = gi;
			float3 diffNorm246 = ase_worldNormal;
			gi246 = UnityGI_Base( data, 1, diffNorm246 );
			float3 indirectDiffuse246 = gi246.indirect.diffuse + diffNorm246 * 0.0001;
			float4 lighting204 = ( shadow188 * ( ase_lightColor * float4( ( indirectDiffuse246 + ase_lightAtten ) , 0.0 ) ) );
			float3 ase_worldViewDir = Unity_SafeNormalize( UnityWorldSpaceViewDir( ase_worldPos ) );
			float dotResult175 = dot( ( ase_worldViewDir + _WorldSpaceLightPos0.xyz ) , ase_worldNormal );
			float smoothstepResult192 = smoothstep( 1.1 , 1.12 , pow( dotResult175 , _Gloss ));
			float4 lerpResult179 = lerp( _SpecularColor1 , ase_lightColor , _SpecTransition);
			float4 groundSplatMap335 = tex2DNode320;
			float temp_output_404_0 = (( splatMap210 + groundSplatMap335 )).r;
			float2 appendResult406 = (float2(temp_output_404_0 , ( 1.0 - temp_output_404_0 )));
			float time274 = 0.0;
			float2 coords274 = tex_coordinates218 * 2.0;
			float2 id274 = 0;
			float fade274 = 0.5;
			float voroi274 = 0;
			float rest274 = 0;
			for( int it = 0; it <2; it++ ){
			voroi274 += fade274 * voronoi274( coords274, time274, id274,0 );
			rest274 += fade274;
			coords274 *= 2;
			fade274 *= 0.5;
			}
			voroi274 /= rest274;
			float2 weightedBlendVar402 = appendResult406;
			float weightedBlend402 = ( weightedBlendVar402.x*_SnowSpecIntensity + weightedBlendVar402.y*( voroi274 * _SandSpecIntensity ) );
			float4 spec207 = ( ase_lightAtten * ( smoothstepResult192 * ( lerpResult179 * weightedBlend402 ) ) );
			float dotResult137 = dot( ase_normWorldNormal , ase_worldViewDir );
			float normal_viewdir140 = dotResult137;
			float4 rim205 = ( saturate( ( pow( ( 1.0 - saturate( ( _RimOffset + normal_viewdir140 ) ) ) , _RimPower ) * ( normal_lightdir149 * ase_lightAtten ) ) ) * ( ase_lightColor * _RimTint ) );
			c.rgb = ( ( lighting204 + spec207 ) + rim205 ).rgb;
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
		#pragma surface surf StandardCustomLighting keepalpha fullforwardshadows exclude_path:deferred vertex:vertexDataFunc tessellate:tessFunction 

		ENDCG
		Pass
		{
			Name "ShadowCaster"
			Tags{ "LightMode" = "ShadowCaster" }
			ZWrite On
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 4.6
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
				vertexDataFunc( v );
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
0;3;1920;1016;1069.381;4837.035;1.361221;True;True
Node;AmplifyShaderEditor.CommentaryNode;219;-5596.512,-593.5671;Inherit;False;926.1399;331;Comment;5;81;218;104;105;102;Texture Coordinates;1,1,1,1;0;0
Node;AmplifyShaderEditor.Vector4Node;102;-5546.512,-525.8671;Inherit;False;Property;_TilingandOffset;Tiling and Offset;7;0;Create;True;0;0;False;0;10,10,0,0;1,1,0,0;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;105;-5296.51,-397.5672;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;104;-5290.51,-543.5671;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;81;-5118.789,-508.3345;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.CommentaryNode;235;-4481.167,-655.0692;Inherit;False;2388.752;775.6974;Comment;13;234;324;251;248;141;233;133;135;125;228;227;129;226;Voronoi Noise (Sand);1,1,1,1;0;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;218;-4900.372,-512.895;Inherit;False;tex_coordinates;-1;True;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.GetLocalVarNode;226;-4431.167,-400.5639;Inherit;False;218;tex_coordinates;1;0;OBJECT;;False;1;FLOAT2;0
Node;AmplifyShaderEditor.CommentaryNode;211;-5917.7,-173.9449;Inherit;False;1281.886;509.2598;Comment;7;210;310;12;5;7;6;4;SplatMap;1,1,1,1;0;0
Node;AmplifyShaderEditor.VoronoiNode;129;-4158.697,-395.3651;Inherit;True;0;0;1;0;1;True;10;False;False;4;0;FLOAT2;0,0;False;1;FLOAT;0.87;False;2;FLOAT;1;False;3;FLOAT;0.12;False;2;FLOAT;0;FLOAT;1
Node;AmplifyShaderEditor.NoiseGeneratorNode;227;-4156.938,-145.7494;Inherit;True;Gradient;True;False;2;0;FLOAT2;0,0;False;1;FLOAT;2;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;228;-3736.364,-395.2076;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TexturePropertyNode;6;-5863.887,82.10316;Inherit;True;Property;_TrackMap;TrackMap;1;0;Create;False;0;0;False;0;592326119cfea9742abc708dd1578e81;592326119cfea9742abc708dd1578e81;False;white;Auto;Texture2D;-1;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.RangedFloatNode;125;-3650.794,-175.0927;Inherit;False;Property;_SandSmooth;Sand Smooth;5;0;Create;True;0;0;False;0;0.1529412;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.TexturePropertyNode;4;-5867.7,-123.9449;Inherit;True;Property;_MainMap;MainMap;0;0;Create;False;0;0;False;0;f88b84b71b88e364896269e8120e6154;584067f74f4ebb54fab613cc634a3cc7;False;white;Auto;Texture2D;-1;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.RangedFloatNode;135;-3334.325,-172.6751;Inherit;False;Property;_SandIntensity;Sand Intensity;6;0;Create;True;0;0;False;0;0.15;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;133;-3305.134,-394.7215;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;5;-5635.885,-123.9449;Inherit;True;Property;_TextureSample0;Texture Sample 0;3;0;Create;True;0;0;False;0;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;7;-5636.751,83.40346;Inherit;True;Property;_TextureSample1;Texture Sample 1;4;0;Create;True;0;0;False;0;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.CommentaryNode;222;-4477.932,-1304.457;Inherit;False;1668.62;489.6171;;11;221;94;95;220;84;83;82;85;96;97;87;Perlin Noise (Snow);1,1,1,1;0;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;12;-5274.472,-34.94346;Inherit;True;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;220;-4427.932,-1111.565;Inherit;False;218;tex_coordinates;1;0;OBJECT;;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ColorNode;233;-2985.264,-605.0692;Inherit;False;Property;_SandColor;Sand Color;4;0;Create;True;0;0;False;0;0.254539,0.6686114,0.8301887,0;1,1,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;141;-2985.684,-394.9451;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;329;-4477.333,271.8323;Inherit;False;2120.895;754.9613;Comment;11;335;319;393;314;397;322;316;318;398;328;320;Ground Texture;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;116;-3761.701,-4612.024;Inherit;False;7599.329;2282.008;Comment;7;111;163;147;145;144;124;119;Toon Shader;1,1,1,1;0;0
Node;AmplifyShaderEditor.TFHCRemapNode;310;-5034.556,-34.64689;Inherit;False;5;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;1,1,1,0;False;3;COLOR;0,0,0,0;False;4;COLOR;1,1,1,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;320;-4225.171,687.1765;Inherit;True;Property;_GroundMap;Ground Map;22;0;Create;True;0;0;False;0;-1;7a0bb364ac109f548b17dd84089934b9;7a0bb364ac109f548b17dd84089934b9;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.NoiseGeneratorNode;84;-4168.417,-974.2368;Inherit;False;Gradient;False;True;2;0;FLOAT2;0,0;False;1;FLOAT;0.5;False;1;FLOAT;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;83;-4165.945,-1111.102;Inherit;False;Gradient;False;True;2;0;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;82;-4165.402,-1254.457;Inherit;False;Gradient;False;True;2;0;FLOAT2;0,0;False;1;FLOAT;0.1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;248;-2727.5,-492.848;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;85;-3926.274,-1131.009;Inherit;False;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;111;-2678.521,-4324.146;Inherit;False;2132.814;784.126;Comment;9;167;28;223;51;224;236;400;399;401;Albedo;1,1,1,1;0;0
Node;AmplifyShaderEditor.BreakToComponentsNode;328;-3672.891,691.8249;Inherit;False;COLOR;1;0;COLOR;0,0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.RegisterLocalVarNode;210;-4855.347,-40.02757;Inherit;False;splatMap;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.TFHCRemapNode;324;-2568.952,-492.2196;Inherit;False;5;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;1,1,1,0;False;3;COLOR;0,0,0,0;False;4;COLOR;1,1,1,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;223;-2627.513,-4206.221;Inherit;False;210;splatMap;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;87;-3787.841,-1131.316;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;3;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;97;-3850.614,-910.004;Inherit;False;Property;_SnowSmooth;Snow Smooth;3;0;Create;True;0;0;False;0;0.6;0.4705882;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;234;-2323.941,-495.6401;Inherit;False;sandNoise;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.TexturePropertyNode;316;-4427.333,321.8323;Inherit;True;Property;_GroundTexture;GroundTexture;21;0;Create;True;0;0;False;0;b5c231fa3c5fe964f8d11551df320603;b5c231fa3c5fe964f8d11551df320603;False;white;Auto;Texture2D;-1;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.OneMinusNode;398;-3365.205,690.5227;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;318;-4414.471,583.373;Inherit;False;218;tex_coordinates;1;0;OBJECT;;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;397;-3144.205,423.5227;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;314;-4104.825,440.1955;Inherit;True;Property;_TextureSample3;Texture Sample 3;22;0;Create;True;0;0;False;0;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TFHCRemapNode;96;-3528.817,-1053.56;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;322;-3465.998,825.5582;Inherit;True;234;sandNoise;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;95;-3552.185,-1246.497;Inherit;False;Property;_SnowColor;Snow Color;2;0;Create;True;0;0;False;0;1,1,1,0;0.7390531,0.9013809,0.9056604,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.CommentaryNode;119;-3693.633,-3390.539;Inherit;False;911.5872;420.6679;Comment;5;140;137;130;126;122;Normal.ViewDir;1,1,1,1;0;0
Node;AmplifyShaderEditor.OneMinusNode;28;-2423.677,-4200.634;Inherit;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.CommentaryNode;124;-3712.296,-3958.534;Inherit;False;937.1509;422.1295;Comment;5;149;139;134;132;127;Normal.Light;1,1,1,1;0;0
Node;AmplifyShaderEditor.ViewDirInputsCoordNode;130;-3393.175,-3145.67;Inherit;False;World;True;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;94;-3256.287,-1158.337;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;335;-3892.123,792.8984;Inherit;False;groundSplatMap;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SummedBlendNode;393;-2913.791,424.355;Inherit;True;5;0;FLOAT2;0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ComponentMaskNode;51;-2184.681,-4205.491;Inherit;False;True;False;False;False;1;0;COLOR;0,0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WorldNormalVector;126;-3416.218,-3314.027;Inherit;False;True;1;0;FLOAT3;0,0,1;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.WorldNormalVector;132;-3413.188,-3885.387;Inherit;False;True;1;0;FLOAT3;0,0,1;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.WorldSpaceLightDirHlpNode;134;-3446.256,-3714.792;Inherit;False;True;1;0;FLOAT;0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.CommentaryNode;147;1199.229,-4053.532;Inherit;False;2581.862;1342.243;Comment;30;207;206;198;197;192;185;182;179;175;169;168;166;162;160;159;157;155;154;274;275;276;282;283;284;336;337;404;405;406;402;Spec;1,1,1,1;0;0
Node;AmplifyShaderEditor.DotProductOpNode;139;-3149.585,-3807.638;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;221;-3020.241,-1163.907;Inherit;False;perlinNoise;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;336;1609.231,-3129.625;Inherit;False;335;groundSplatMap;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.DotProductOpNode;137;-3152.177,-3236.67;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;400;-1962.432,-4255.669;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;319;-2558.19,420.1077;Inherit;False;groundTex;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;276;1609.248,-3216.485;Inherit;False;210;splatMap;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.CommentaryNode;145;-429.3059,-4222.568;Inherit;False;1389.021;693.9789;Comment;9;208;188;183;178;176;164;161;153;152;Shadow;1,1,1,1;0;0
Node;AmplifyShaderEditor.GetLocalVarNode;236;-1845.02,-4041.066;Inherit;False;319;groundTex;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;149;-2988.565,-3810.9;Float;False;normal_lightdir;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;140;-2989.715,-3249.058;Float;False;normal_viewdir;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;401;-1804.432,-4225.668;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.GetLocalVarNode;224;-1846.654,-4126.176;Inherit;False;221;perlinNoise;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;337;1844.231,-3209.625;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.CommentaryNode;144;-1093.706,-3297.586;Inherit;False;2038.134;634.727;Comment;17;205;201;199;196;193;190;189;186;181;180;171;170;165;158;151;146;309;Rim Light;1,1,1,1;0;0
Node;AmplifyShaderEditor.ViewDirInputsCoordNode;157;1305.665,-3988.532;Inherit;False;World;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.GetLocalVarNode;151;-1043.706,-3146.853;Inherit;False;140;normal_viewdir;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.WorldSpaceLightPos;155;1249.229,-3780.177;Inherit;False;0;3;FLOAT4;0;FLOAT3;1;FLOAT;2
Node;AmplifyShaderEditor.GetLocalVarNode;152;-338.1609,-3943.183;Inherit;False;149;normal_lightdir;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;275;1371.496,-3053.87;Inherit;False;218;tex_coordinates;1;0;OBJECT;;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;153;-361.1325,-3801.077;Inherit;False;Property;_ShadowOffset;Shadow Offset;12;0;Create;True;0;0;False;0;0.524439;0.434;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.ComponentMaskNode;404;1971.455,-3214.411;Inherit;False;True;False;False;False;1;0;COLOR;0,0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SummedBlendNode;399;-1576.922,-4225.895;Inherit;True;5;0;FLOAT2;0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;146;-1011.766,-3247.586;Inherit;False;Property;_RimOffset;Rim Offset;13;0;Create;True;0;0;False;0;0.8;0.55;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;158;-780.819,-3205.819;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;405;2169.456,-3152.411;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ScaleAndOffsetNode;161;-9.425211,-3843.445;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;1;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;167;-1272.647,-4232.147;Inherit;False;albedo;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;160;1568.65,-3897.858;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.VoronoiNode;274;1626.496,-3048.87;Inherit;True;0;0;1;1;2;True;10;False;False;4;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT;2;False;3;FLOAT;0;False;2;FLOAT;0;FLOAT;1
Node;AmplifyShaderEditor.RangedFloatNode;198;1854.97,-2974.345;Inherit;False;Property;_SandSpecIntensity;Sand Spec Intensity;18;0;Create;True;0;0;False;0;0.5;0.188;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.WorldNormalVector;162;1522.864,-3640.785;Inherit;False;False;1;0;FLOAT3;0,0,1;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.ColorNode;159;1302.883,-3527.401;Inherit;False;Property;_SpecularColor1;Spec Color;20;0;Create;False;0;0;False;0;1,1,1,0;0.7921569,0.5019608,0.2862745,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;178;382.6377,-4061.408;Inherit;False;167;albedo;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.LightColorNode;168;1356.065,-3328.178;Inherit;False;0;3;COLOR;0;FLOAT3;1;FLOAT;2
Node;AmplifyShaderEditor.RangedFloatNode;169;1780.885,-3618.291;Inherit;False;Property;_Gloss;Gloss;16;0;Create;True;0;0;False;0;0;0.26;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.DotProductOpNode;175;1778.239,-3771.781;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;163;-2644.391,-3283.656;Inherit;False;1363.428;609.8745;Comment;9;204;202;200;195;191;187;177;245;246;Lighting;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;282;2174.471,-3296.284;Inherit;False;Property;_SnowSpecIntensity;Snow Spec Intensity;17;0;Create;True;0;0;False;0;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;283;2129.013,-3048.834;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;166;1235.198,-3163.8;Inherit;False;Property;_SpecTransition;Spec Transition;19;0;Create;True;0;0;False;0;0;0.5;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;165;-595.3229,-3207.047;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;208;267.2759,-3754.286;Inherit;True;Property;_ToonRamp;Toon Ramp;11;0;Create;True;0;0;False;0;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;406;2313.456,-3208.411;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.LightAttenuation;309;-419.6433,-2848.463;Inherit;False;0;1;FLOAT;0
Node;AmplifyShaderEditor.IndirectDiffuseLighting;246;-2376.754,-2899.505;Inherit;False;Tangent;1;0;FLOAT3;0,0,1;False;1;FLOAT3;0
Node;AmplifyShaderEditor.OneMinusNode;171;-372.9739,-3205.819;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;170;-420.777,-2977.911;Inherit;False;149;normal_lightdir;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;217;-2456.507,1638.159;Inherit;False;2474.98;892.3594;Comment;2;214;216;Geometry Shader;1,1,1,1;0;0
Node;AmplifyShaderEditor.LightAttenuation;245;-2355.754,-2776.506;Inherit;False;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;180;-498.277,-3097.715;Inherit;False;Property;_RimPower;Rim Power;14;0;Create;True;0;0;False;0;0;0.25;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;179;1663.875,-3444.211;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.PowerNode;185;2103.743,-3770.458;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SummedBlendNode;402;2512.387,-3207.764;Inherit;True;5;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;183;584.6377,-4057.408;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SmoothstepOpNode;192;2330.332,-3771.104;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;1.1;False;2;FLOAT;1.12;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;186;-135.884,-3197.219;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;181;-182.777,-2931.911;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LightColorNode;187;-2118.062,-3011.934;Inherit;False;0;3;COLOR;0;FLOAT3;1;FLOAT;2
Node;AmplifyShaderEditor.CommentaryNode;214;-2386.643,1943.721;Inherit;False;1356.089;516.8297;Comment;8;22;32;212;24;27;33;25;213;Vertex Offset;1,1,1,1;0;0
Node;AmplifyShaderEditor.SimpleAddOpNode;191;-2083.39,-2859.782;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;182;2819.125,-3439.521;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;188;757.2168,-4063.479;Inherit;False;shadow;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;200;-1920.693,-2945.609;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT3;0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;195;-1934.749,-3233.657;Inherit;False;188;shadow;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;24;-2336.643,2319.379;Inherit;False;Property;_Height;Height;8;0;Create;True;0;0;False;0;0.75;0.1;0;5;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;190;56.21664,-2874.859;Inherit;False;Property;_RimTint;Rim Tint;15;0;Create;True;0;0;False;0;1,1,1,0;1,1,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LightAttenuation;284;2993.472,-3854.1;Inherit;False;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;197;3048.183,-3768.832;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;189;112.2227,-3195.911;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LightColorNode;193;104.2166,-3034.859;Inherit;False;0;3;COLOR;0;FLOAT3;1;FLOAT;2
Node;AmplifyShaderEditor.SaturateNode;199;324.5967,-3197.104;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;202;-1709.287,-3090.982;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.CommentaryNode;216;-925.8459,1956.542;Inherit;False;882.7192;268;Comment;4;3;2;1;215;Tessellation;1,1,1,1;0;0
Node;AmplifyShaderEditor.DynamicAppendNode;32;-2026.228,2301.551;Inherit;False;FLOAT3;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;196;312.2176,-2954.859;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;206;3319.656,-3853.819;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.NormalVertexDataNode;25;-1992.3,1993.721;Inherit;True;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;212;-2055.046,2199.534;Inherit;False;210;splatMap;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.TFHCRemapNode;22;-1854.583,2205.067;Inherit;True;5;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;1,1,1,0;False;3;COLOR;0,0,0,0;False;4;COLOR;1,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;3;-875.8459,2108.542;Inherit;False;Property;_TessellationDistance;Tessellation Distance;10;0;Create;False;0;0;False;0;30;30;0;50;0;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;204;-1504.963,-3098.027;Inherit;False;lighting;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;207;3545.922,-3859.141;Inherit;False;spec;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;201;543.1768,-3198.382;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;33;-1725.512,1994.325;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,1,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;2;-874.8459,2006.542;Inherit;False;Property;_Tessellation;Tessellation;9;0;Create;False;0;0;False;0;4;4;0;15;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;27;-1506.477,2093.19;Inherit;True;2;2;0;FLOAT3;0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;237;-590.1691,84.39368;Inherit;False;204;lighting;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.DistanceBasedTessNode;1;-521.2678,2036.999;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.GetLocalVarNode;238;-592.1691,172.3937;Inherit;False;207;spec;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;205;720.4282,-3202.997;Inherit;False;rim;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.CommentaryNode;115;-4482.569,-1790.495;Inherit;False;606.5291;272.7734;Comment;2;121;409;Normal Map;1,1,1,1;0;0
Node;AmplifyShaderEditor.GetLocalVarNode;239;-441.1691,258.3937;Inherit;False;205;rim;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;215;-267.1268,2031.612;Inherit;False;tessellation;-1;True;1;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleAddOpNode;240;-394.2339,153.7908;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;213;-1254.554,2087.65;Inherit;False;vertexOffset;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GradientNode;164;-9.088175,-3966.082;Inherit;False;1;4;2;0.2264151,0.2264151,0.2264151,0.1647059;0.490566,0.490566,0.490566,0.3794156;0.7830189,0.7830189,0.7830189,0.6176547;1,1,1,1;1,0;1,1;0;1;OBJECT;0
Node;AmplifyShaderEditor.GetLocalVarNode;127;-3675.324,-3889.927;Inherit;False;121;normal;1;0;OBJECT;;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;177;-2594.391,-2902.782;Inherit;False;121;normal;1;0;OBJECT;;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;251;-3476.583,-508.5632;Inherit;False;sandRawNoise;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;241;-245.2339,238.7908;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;154;1303.628,-3645.663;Inherit;False;121;normal;1;0;OBJECT;;False;1;FLOAT3;0
Node;AmplifyShaderEditor.WorldNormalVector;409;-4415.664,-1720.229;Inherit;False;True;1;0;FLOAT3;0,0,1;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.GetLocalVarNode;122;-3661.705,-3319.745;Inherit;False;121;normal;1;0;OBJECT;;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;244;-306.3131,342.9434;Inherit;False;213;vertexOffset;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;243;-305.3131,430.9434;Inherit;False;215;tessellation;1;0;OBJECT;;False;1;FLOAT4;0
Node;AmplifyShaderEditor.GradientSampleNode;176;262.4539,-3966.326;Inherit;True;2;0;OBJECT;;False;1;FLOAT;0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;121;-4109.195,-1706.469;Inherit;False;normal;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;0,0;Float;False;True;-1;6;ASEMaterialInspector;0;0;CustomLighting;Custom/Snow;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;5;True;True;0;False;Opaque;;Geometry;ForwardOnly;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;True;2;15;10;25;False;5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;1;0.1646493,0.9359996,0.9433962,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;1;False;-1;0;False;-1;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT3;0,0,0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;105;0;102;3
WireConnection;105;1;102;4
WireConnection;104;0;102;1
WireConnection;104;1;102;2
WireConnection;81;0;104;0
WireConnection;81;1;105;0
WireConnection;218;0;81;0
WireConnection;129;0;226;0
WireConnection;227;0;226;0
WireConnection;228;0;129;0
WireConnection;228;1;227;0
WireConnection;133;0;228;0
WireConnection;133;3;125;0
WireConnection;5;0;4;0
WireConnection;7;0;6;0
WireConnection;12;0;5;0
WireConnection;12;1;7;0
WireConnection;141;0;133;0
WireConnection;141;1;135;0
WireConnection;310;0;12;0
WireConnection;84;0;220;0
WireConnection;83;0;220;0
WireConnection;82;0;220;0
WireConnection;248;0;233;0
WireConnection;248;1;141;0
WireConnection;85;0;82;0
WireConnection;85;1;83;0
WireConnection;85;2;84;0
WireConnection;328;0;320;0
WireConnection;210;0;310;0
WireConnection;324;0;248;0
WireConnection;87;0;85;0
WireConnection;234;0;324;0
WireConnection;398;0;328;0
WireConnection;397;0;328;0
WireConnection;397;1;398;0
WireConnection;314;0;316;0
WireConnection;314;1;318;0
WireConnection;96;0;87;0
WireConnection;96;3;97;0
WireConnection;28;0;223;0
WireConnection;94;0;95;0
WireConnection;94;1;96;0
WireConnection;335;0;320;0
WireConnection;393;0;397;0
WireConnection;393;1;314;0
WireConnection;393;2;322;0
WireConnection;51;0;28;0
WireConnection;139;0;132;0
WireConnection;139;1;134;0
WireConnection;221;0;94;0
WireConnection;137;0;126;0
WireConnection;137;1;130;0
WireConnection;400;0;51;0
WireConnection;319;0;393;0
WireConnection;149;0;139;0
WireConnection;140;0;137;0
WireConnection;401;0;400;0
WireConnection;401;1;51;0
WireConnection;337;0;276;0
WireConnection;337;1;336;0
WireConnection;404;0;337;0
WireConnection;399;0;401;0
WireConnection;399;1;224;0
WireConnection;399;2;236;0
WireConnection;158;0;146;0
WireConnection;158;1;151;0
WireConnection;405;0;404;0
WireConnection;161;0;152;0
WireConnection;161;1;153;0
WireConnection;161;2;153;0
WireConnection;167;0;399;0
WireConnection;160;0;157;0
WireConnection;160;1;155;1
WireConnection;274;0;275;0
WireConnection;175;0;160;0
WireConnection;175;1;162;0
WireConnection;283;0;274;0
WireConnection;283;1;198;0
WireConnection;165;0;158;0
WireConnection;208;1;161;0
WireConnection;406;0;404;0
WireConnection;406;1;405;0
WireConnection;171;0;165;0
WireConnection;179;0;159;0
WireConnection;179;1;168;0
WireConnection;179;2;166;0
WireConnection;185;0;175;0
WireConnection;185;1;169;0
WireConnection;402;0;406;0
WireConnection;402;1;282;0
WireConnection;402;2;283;0
WireConnection;183;0;178;0
WireConnection;183;1;208;0
WireConnection;192;0;185;0
WireConnection;186;0;171;0
WireConnection;186;1;180;0
WireConnection;181;0;170;0
WireConnection;181;1;309;0
WireConnection;191;0;246;0
WireConnection;191;1;245;0
WireConnection;182;0;179;0
WireConnection;182;1;402;0
WireConnection;188;0;183;0
WireConnection;200;0;187;0
WireConnection;200;1;191;0
WireConnection;197;0;192;0
WireConnection;197;1;182;0
WireConnection;189;0;186;0
WireConnection;189;1;181;0
WireConnection;199;0;189;0
WireConnection;202;0;195;0
WireConnection;202;1;200;0
WireConnection;32;0;24;0
WireConnection;32;1;24;0
WireConnection;32;2;24;0
WireConnection;196;0;193;0
WireConnection;196;1;190;0
WireConnection;206;0;284;0
WireConnection;206;1;197;0
WireConnection;22;0;212;0
WireConnection;22;4;32;0
WireConnection;204;0;202;0
WireConnection;207;0;206;0
WireConnection;201;0;199;0
WireConnection;201;1;196;0
WireConnection;33;0;25;0
WireConnection;27;0;33;0
WireConnection;27;1;22;0
WireConnection;1;0;2;0
WireConnection;1;2;3;0
WireConnection;205;0;201;0
WireConnection;215;0;1;0
WireConnection;240;0;237;0
WireConnection;240;1;238;0
WireConnection;213;0;27;0
WireConnection;251;0;228;0
WireConnection;241;0;240;0
WireConnection;241;1;239;0
WireConnection;176;0;164;0
WireConnection;176;1;161;0
WireConnection;0;13;241;0
WireConnection;0;11;244;0
WireConnection;0;14;243;0
ASEEND*/
//CHKSM=8D13714380EDADF3243407E44CD865871967A9F8