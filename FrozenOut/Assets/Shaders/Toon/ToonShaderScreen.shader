// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "FrozenOut/ToonShaderScreen"
{
	Properties
	{
		_Tint("Tint", Color) = (1,1,1,0)
		_Albedo("Albedo", 2D) = "white" {}
		_TilingandOffset("Tiling and Offset", Vector) = (1,1,0,0)
		_ToonRamp("Toon Ramp", 2D) = "white" {}
		_NormalMap("Normal Map", 2D) = "bump" {}
		_ShadowOffset("Shadow Offset", Range( 0 , 1)) = 0.5
		_RimOffset("Rim Offset", Float) = 0.8
		_RimPower("Rim Power", Range( 0 , 1)) = 0
		_RimTint("Rim Tint", Color) = (1,1,1,0)
		_Gloss("Gloss", Range( 0 , 1)) = 0
		_SpecIntensity("Spec Intensity", Range( 0 , 1)) = 0.5
		_SpecMap("Spec Map", 2D) = "white" {}
		_SpecTransition("Spec Transition", Range( 0 , 1)) = 0
		_SpecularColor("Spec Color", Color) = (1,1,1,0)
		_Opacity("Opacity", Range( 0 , 1)) = 0
		[Toggle]_Selected("Selected", Float) = 0
		_SelectedColor("Selected Color", Color) = (0,0,0,0)
		_SelectedIntensity("Selected Intensity", Range( 0 , 1)) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		GrabPass{ }
		CGINCLUDE
		#include "UnityPBSLighting.cginc"
		#include "UnityCG.cginc"
		#include "UnityShaderVariables.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		#if defined(UNITY_STEREO_INSTANCING_ENABLED) || defined(UNITY_STEREO_MULTIVIEW_ENABLED)
		#define ASE_DECLARE_SCREENSPACE_TEXTURE(tex) UNITY_DECLARE_SCREENSPACE_TEXTURE(tex);
		#else
		#define ASE_DECLARE_SCREENSPACE_TEXTURE(tex) UNITY_DECLARE_SCREENSPACE_TEXTURE(tex)
		#endif
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
			float4 screenPos;
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

		uniform float _Selected;
		uniform float4 _SelectedColor;
		uniform float _SelectedIntensity;
		ASE_DECLARE_SCREENSPACE_TEXTURE( _GrabTexture )
		uniform float4 _Tint;
		uniform sampler2D _Albedo;
		uniform float4 _TilingandOffset;
		uniform float _Opacity;
		uniform sampler2D _ToonRamp;
		uniform sampler2D _NormalMap;
		uniform float4 _NormalMap_ST;
		uniform float _ShadowOffset;
		uniform float _RimOffset;
		uniform float _RimPower;
		uniform float4 _RimTint;
		uniform float _Gloss;
		uniform sampler2D _SpecMap;
		uniform float4 _SpecMap_ST;
		uniform float4 _SpecularColor;
		uniform float _SpecTransition;
		uniform float _SpecIntensity;


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
			float4 ase_screenPos = float4( i.screenPos.xyz , i.screenPos.w + 0.00000000001 );
			float4 ase_grabScreenPos = ASE_ComputeGrabScreenPos( ase_screenPos );
			float4 ase_grabScreenPosNorm = ase_grabScreenPos / ase_grabScreenPos.w;
			float4 screenColor149 = UNITY_SAMPLE_SCREENSPACE_TEXTURE(_GrabTexture,ase_grabScreenPosNorm.xy);
			float2 appendResult111 = (float2(_TilingandOffset.x , _TilingandOffset.y));
			float2 appendResult110 = (float2(_TilingandOffset.z , _TilingandOffset.w));
			float2 uv_TexCoord108 = i.uv_texcoord * appendResult111 + appendResult110;
			float4 blendOpSrc157 = screenColor149;
			float4 blendOpDest157 = ( _Tint * tex2D( _Albedo, uv_TexCoord108 ) );
			float4 lerpBlendMode157 = lerp(blendOpDest157,( 1.0 - ( 1.0 - blendOpSrc157 ) * ( 1.0 - blendOpDest157 ) ),_Opacity);
			float4 albedo34 = ( saturate( lerpBlendMode157 ));
			float2 uv_NormalMap = i.uv_texcoord * _NormalMap_ST.xy + _NormalMap_ST.zw;
			float3 normal23 = UnpackNormal( tex2D( _NormalMap, uv_NormalMap ) );
			float3 ase_worldPos = i.worldPos;
			#if defined(LIGHTMAP_ON) && UNITY_VERSION < 560 //aseld
			float3 ase_worldlightDir = 0;
			#else //aseld
			float3 ase_worldlightDir = Unity_SafeNormalize( UnityWorldSpaceLightDir( ase_worldPos ) );
			#endif //aseld
			float dotResult3 = dot( normalize( (WorldNormalVector( i , normal23 )) ) , ase_worldlightDir );
			float normal_lightdir4 = dotResult3;
			float temp_output_18_0 = (normal_lightdir4*_ShadowOffset + _ShadowOffset);
			float2 temp_cast_2 = (temp_output_18_0).xx;
			float4 shadow17 = ( albedo34 * tex2D( _ToonRamp, temp_cast_2 ) );
			#if defined(LIGHTMAP_ON) && ( UNITY_VERSION < 560 || ( defined(LIGHTMAP_SHADOW_MIXING) && !defined(SHADOWS_SHADOWMASK) && defined(SHADOWS_SCREEN) ) )//aselc
			float4 ase_lightColor = 0;
			#else //aselc
			float4 ase_lightColor = _LightColor0;
			#endif //aselc
			UnityGI gi42 = gi;
			float3 diffNorm42 = WorldNormalVector( i , normal23 );
			gi42 = UnityGI_Base( data, 1, diffNorm42 );
			float3 indirectDiffuse42 = gi42.indirect.diffuse + diffNorm42 * 0.0001;
			float4 lighting41 = ( shadow17 * ( ase_lightColor * float4( ( indirectDiffuse42 + ase_lightAtten ) , 0.0 ) ) );
			float3 ase_worldViewDir = Unity_SafeNormalize( UnityWorldSpaceViewDir( ase_worldPos ) );
			float dotResult7 = dot( normalize( (WorldNormalVector( i , normal23 )) ) , ase_worldViewDir );
			float normal_viewdir8 = dotResult7;
			float4 rim55 = ( saturate( ( pow( ( 1.0 - saturate( ( _RimOffset + normal_viewdir8 ) ) ) , _RimPower ) * ( normal_lightdir4 * ase_lightAtten ) ) ) * ( ase_lightColor * _RimTint ) );
			float dotResult75 = dot( ( ase_worldViewDir + _WorldSpaceLightPos0.xyz ) , (WorldNormalVector( i , normal23 )) );
			float smoothstepResult78 = smoothstep( 1.1 , 1.12 , pow( dotResult75 , _Gloss ));
			float2 uv_SpecMap = i.uv_texcoord * _SpecMap_ST.xy + _SpecMap_ST.zw;
			float4 lerpResult92 = lerp( _SpecularColor , ase_lightColor , _SpecTransition);
			float4 spec83 = ( ase_lightAtten * ( ( smoothstepResult78 * ( tex2D( _SpecMap, uv_SpecMap ) * lerpResult92 ) ) * _SpecIntensity ) );
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
			o.Emission = (( _Selected )?( ( _SelectedColor * _SelectedIntensity ) ):( float4( 0,0,0,0 ) )).rgb;
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf StandardCustomLighting keepalpha fullforwardshadows exclude_path:deferred 

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
				float4 screenPos : TEXCOORD2;
				float4 tSpace0 : TEXCOORD3;
				float4 tSpace1 : TEXCOORD4;
				float4 tSpace2 : TEXCOORD5;
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
				o.screenPos = ComputeScreenPos( o.pos );
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
				surfIN.screenPos = IN.screenPos;
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
0;9;1920;1010;4469.291;3773.045;2.158832;True;True
Node;AmplifyShaderEditor.CommentaryNode;24;-5647.75,-1149.726;Inherit;False;630.4309;280;Comment;2;22;23;Normal Map;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;120;-4700.568,-3775.559;Inherit;False;7599.329;2282.008;Comment;7;47;95;21;65;10;11;35;Toon Shader;1,1,1,1;0;0
Node;AmplifyShaderEditor.SamplerNode;22;-5597.75,-1099.726;Inherit;True;Property;_NormalMap;Normal Map;4;0;Create;True;0;0;False;0;-1;None;None;True;0;False;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.CommentaryNode;35;-3588.864,-3393.667;Inherit;False;2132.814;784.126;Comment;12;31;34;33;32;108;110;111;109;150;149;157;151;Albedo;1,1,1,1;0;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;23;-5241.319,-1098.856;Inherit;False;normal;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.CommentaryNode;11;-4632.5,-2554.073;Inherit;False;911.5872;420.6679;Comment;5;8;7;5;6;25;Normal.ViewDir;1,1,1,1;0;0
Node;AmplifyShaderEditor.Vector4Node;109;-3562.842,-3111.382;Inherit;False;Property;_TilingandOffset;Tiling and Offset;2;0;Create;True;0;0;False;0;1,1,0,0;1,1,0,0;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.CommentaryNode;10;-4651.163,-3122.068;Inherit;False;937.1509;422.1295;Comment;5;26;4;3;2;1;Normal.Light;1,1,1,1;0;0
Node;AmplifyShaderEditor.GetLocalVarNode;25;-4600.572,-2483.279;Inherit;False;23;normal;1;0;OBJECT;;False;1;FLOAT3;0
Node;AmplifyShaderEditor.DynamicAppendNode;111;-3340.842,-3111.382;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ViewDirInputsCoordNode;6;-4332.042,-2309.204;Inherit;False;World;True;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.DynamicAppendNode;110;-3340.842,-2985.382;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.WorldNormalVector;5;-4355.085,-2477.561;Inherit;False;True;1;0;FLOAT3;0,0,1;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.GetLocalVarNode;26;-4614.191,-3053.461;Inherit;False;23;normal;1;0;OBJECT;;False;1;FLOAT3;0
Node;AmplifyShaderEditor.DotProductOpNode;7;-4091.044,-2400.204;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WorldNormalVector;1;-4352.055,-3048.921;Inherit;False;True;1;0;FLOAT3;0,0,1;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.TextureCoordinatesNode;108;-3186.857,-3096.741;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.WorldSpaceLightDirHlpNode;2;-4385.123,-2878.326;Inherit;False;True;1;0;FLOAT;0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SamplerNode;31;-2946.47,-3126.579;Inherit;True;Property;_Albedo;Albedo;1;0;Create;True;0;0;False;0;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DotProductOpNode;3;-4088.452,-2971.172;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GrabScreenPosition;150;-2585.254,-2898.23;Inherit;False;0;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.CommentaryNode;65;-2032.573,-2461.12;Inherit;False;2038.134;634.727;Comment;17;48;49;50;51;52;53;54;57;59;60;56;55;61;62;63;64;67;Rim Light;1,1,1,1;0;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;8;-3928.582,-2412.592;Float;False;normal_viewdir;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;32;-2606.973,-3252.997;Inherit;False;Property;_Tint;Tint;0;0;Create;True;0;0;False;0;1,1,1,0;0.8207547,0.592007,0.4219918,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;49;-1950.633,-2411.12;Inherit;False;Property;_RimOffset;Rim Offset;6;0;Create;True;0;0;False;0;0.8;0.75;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;151;-2462.46,-2712.141;Inherit;False;Property;_Opacity;Opacity;14;0;Create;True;0;0;False;0;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.ScreenColorNode;149;-2361.254,-2898.23;Inherit;False;Global;_GrabScreen0;Grab Screen 0;17;0;Create;True;0;0;False;0;Object;-1;False;False;1;0;FLOAT2;0,0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;33;-2348.562,-3090.48;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;4;-3927.432,-2979.434;Float;False;normal_lightdir;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;21;-1368.173,-3386.102;Inherit;False;1389.021;693.9789;Comment;9;30;17;37;29;36;28;18;19;12;Shadow;1,1,1,1;0;0
Node;AmplifyShaderEditor.GetLocalVarNode;48;-1982.573,-2310.387;Inherit;False;8;normal_viewdir;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;95;260.3625,-3217.066;Inherit;False;2581.862;1342.243;Comment;21;86;83;82;79;81;80;78;76;94;77;75;92;85;90;73;72;93;89;71;70;74;Spec;1,1,1,1;0;0
Node;AmplifyShaderEditor.GetLocalVarNode;12;-1277.028,-3106.717;Inherit;False;4;normal_lightdir;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;19;-1279.091,-2966.935;Inherit;False;Property;_ShadowOffset;Shadow Offset;5;0;Create;True;0;0;False;0;0.5;0.434;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.BlendOpsNode;157;-1931.543,-3003.694;Inherit;False;Screen;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;1;False;1;COLOR;0
Node;AmplifyShaderEditor.WorldSpaceLightPos;71;310.3626,-2943.711;Inherit;False;0;3;FLOAT4;0;FLOAT3;1;FLOAT;2
Node;AmplifyShaderEditor.SimpleAddOpNode;50;-1719.686,-2369.353;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ViewDirInputsCoordNode;70;366.7977,-3152.066;Inherit;False;World;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.GetLocalVarNode;74;364.7616,-2809.197;Inherit;False;23;normal;1;0;OBJECT;;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleAddOpNode;72;629.7835,-3061.392;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.WorldNormalVector;73;583.9966,-2804.319;Inherit;False;False;1;0;FLOAT3;0,0,1;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.CommentaryNode;47;-3583.258,-2447.19;Inherit;False;1363.428;609.8745;Comment;9;39;41;40;42;45;44;43;46;38;Lighting;1,1,1,1;0;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;34;-1662.586,-3027.953;Inherit;False;albedo;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ScaleAndOffsetNode;18;-948.2922,-3006.979;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;1;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;51;-1534.19,-2370.581;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;52;-1311.841,-2369.353;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;93;753.1276,-2071.532;Inherit;False;Property;_SpecTransition;Spec Transition;12;0;Create;True;0;0;False;0;0;0.5;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.LightColorNode;90;873.9955,-2235.91;Inherit;False;0;3;COLOR;0;FLOAT3;1;FLOAT;2
Node;AmplifyShaderEditor.LightAttenuation;62;-1355.644,-2026.445;Inherit;False;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;77;842.0176,-2781.825;Inherit;False;Property;_Gloss;Gloss;9;0;Create;True;0;0;False;0;0;0.225;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;54;-1437.144,-2261.249;Inherit;False;Property;_RimPower;Rim Power;7;0;Create;True;0;0;False;0;0;0.25;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;61;-1359.644,-2141.445;Inherit;False;4;normal_lightdir;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;89;820.8136,-2435.134;Inherit;False;Property;_SpecularColor;Spec Color;13;0;Create;False;0;0;False;0;1,1,1,0;0.8196079,0.5921569,0.4235294,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;30;-671.5911,-2917.82;Inherit;True;Property;_ToonRamp;Toon Ramp;3;0;Create;True;0;0;False;0;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;36;-556.2292,-3224.942;Inherit;False;34;albedo;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.DotProductOpNode;75;839.3716,-2935.315;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;45;-3533.258,-2066.316;Inherit;False;23;normal;1;0;OBJECT;;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;37;-354.2294,-3220.942;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;85;1031.264,-2675.141;Inherit;True;Property;_SpecMap;Spec Map;11;0;Create;True;0;0;False;0;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PowerNode;76;1164.876,-2933.992;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;92;1181.805,-2351.943;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.PowerNode;53;-1074.751,-2360.753;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;63;-1121.644,-2095.445;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.IndirectDiffuseLighting;42;-3296.259,-2071.316;Inherit;False;Tangent;1;0;FLOAT3;0,0,1;False;1;FLOAT3;0
Node;AmplifyShaderEditor.LightAttenuation;43;-3275.259,-1948.316;Inherit;False;0;1;FLOAT;0
Node;AmplifyShaderEditor.LightColorNode;38;-3056.929,-2175.468;Inherit;False;0;3;COLOR;0;FLOAT3;1;FLOAT;2
Node;AmplifyShaderEditor.RegisterLocalVarNode;17;-189.6502,-3226.013;Inherit;False;shadow;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;94;1400.977,-2511.489;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;44;-3022.257,-2023.316;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.ColorNode;59;-882.6503,-2038.393;Inherit;False;Property;_RimTint;Rim Tint;8;0;Create;True;0;0;False;0;1,1,1,0;1,1,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LightColorNode;57;-834.6503,-2198.393;Inherit;False;0;3;COLOR;0;FLOAT3;1;FLOAT;2
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;64;-826.6443,-2359.445;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SmoothstepOpNode;78;1391.465,-2934.638;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;1.1;False;2;FLOAT;1.12;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;39;-2910.616,-2291.191;Inherit;False;17;shadow;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;86;1720.45,-2937.45;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;80;1801.234,-2755.362;Inherit;False;Property;_SpecIntensity;Spec Intensity;10;0;Create;True;0;0;False;0;0.5;0.172;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;60;-626.6494,-2118.393;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;46;-2859.56,-2109.143;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT3;0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SaturateNode;67;-614.2704,-2360.638;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;56;-395.6903,-2361.916;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.LightAttenuation;81;2120.122,-3096.744;Inherit;False;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;79;2175.696,-2935.315;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;40;-2648.154,-2254.516;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;82;2380.79,-3017.353;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;41;-2443.83,-2261.561;Inherit;False;lighting;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;55;-218.4387,-2366.531;Inherit;False;rim;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;20;3255.705,-1247.668;Inherit;False;41;lighting;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;153;3022.546,-1529.172;Inherit;False;Property;_SelectedColor;Selected Color;16;0;Create;True;0;0;False;0;0,0,0,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;83;2607.055,-3018.675;Inherit;False;spec;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;69;3255.576,-1157.817;Inherit;False;55;rim;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;152;2982.246,-1337.872;Inherit;False;Property;_SelectedIntensity;Selected Intensity;17;0;Create;True;0;0;False;0;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;68;3493.077,-1175.6;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;154;3291.047,-1356.272;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;84;3446.822,-1067.955;Inherit;False;83;spec;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.ToggleSwitchNode;155;3591.547,-1333.373;Inherit;False;Property;_Selected;Selected;15;0;Create;True;0;0;False;0;0;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GradientNode;28;-947.9552,-3128.616;Inherit;False;1;5;2;0.2264151,0.2264151,0.2264151,0.1647059;0.490566,0.490566,0.490566,0.3794156;0.7830189,0.7830189,0.7830189,0.6176547;1,1,1,0.8294194;1,1,1,1;1,0;1,1;0;1;OBJECT;0
Node;AmplifyShaderEditor.GradientSampleNode;29;-676.4132,-3129.86;Inherit;True;2;0;OBJECT;;False;1;FLOAT;0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;96;3635.292,-1135.985;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;3881.506,-1377.387;Float;False;True;-1;2;ASEMaterialInspector;0;0;CustomLighting;FrozenOut/ToonShaderScreen;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Translucent;5;True;True;0;False;Opaque;;Transparent;ForwardOnly;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;5;True;0;5;False;-1;10;False;-1;0;5;False;-1;10;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;1;False;-1;0;False;-1;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT3;0,0,0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;23;0;22;0
WireConnection;111;0;109;1
WireConnection;111;1;109;2
WireConnection;110;0;109;3
WireConnection;110;1;109;4
WireConnection;5;0;25;0
WireConnection;7;0;5;0
WireConnection;7;1;6;0
WireConnection;1;0;26;0
WireConnection;108;0;111;0
WireConnection;108;1;110;0
WireConnection;31;1;108;0
WireConnection;3;0;1;0
WireConnection;3;1;2;0
WireConnection;8;0;7;0
WireConnection;149;0;150;0
WireConnection;33;0;32;0
WireConnection;33;1;31;0
WireConnection;4;0;3;0
WireConnection;157;0;149;0
WireConnection;157;1;33;0
WireConnection;157;2;151;0
WireConnection;50;0;49;0
WireConnection;50;1;48;0
WireConnection;72;0;70;0
WireConnection;72;1;71;1
WireConnection;73;0;74;0
WireConnection;34;0;157;0
WireConnection;18;0;12;0
WireConnection;18;1;19;0
WireConnection;18;2;19;0
WireConnection;51;0;50;0
WireConnection;52;0;51;0
WireConnection;30;1;18;0
WireConnection;75;0;72;0
WireConnection;75;1;73;0
WireConnection;37;0;36;0
WireConnection;37;1;30;0
WireConnection;76;0;75;0
WireConnection;76;1;77;0
WireConnection;92;0;89;0
WireConnection;92;1;90;0
WireConnection;92;2;93;0
WireConnection;53;0;52;0
WireConnection;53;1;54;0
WireConnection;63;0;61;0
WireConnection;63;1;62;0
WireConnection;42;0;45;0
WireConnection;17;0;37;0
WireConnection;94;0;85;0
WireConnection;94;1;92;0
WireConnection;44;0;42;0
WireConnection;44;1;43;0
WireConnection;64;0;53;0
WireConnection;64;1;63;0
WireConnection;78;0;76;0
WireConnection;86;0;78;0
WireConnection;86;1;94;0
WireConnection;60;0;57;0
WireConnection;60;1;59;0
WireConnection;46;0;38;0
WireConnection;46;1;44;0
WireConnection;67;0;64;0
WireConnection;56;0;67;0
WireConnection;56;1;60;0
WireConnection;79;0;86;0
WireConnection;79;1;80;0
WireConnection;40;0;39;0
WireConnection;40;1;46;0
WireConnection;82;0;81;0
WireConnection;82;1;79;0
WireConnection;41;0;40;0
WireConnection;55;0;56;0
WireConnection;83;0;82;0
WireConnection;68;0;20;0
WireConnection;68;1;69;0
WireConnection;154;0;153;0
WireConnection;154;1;152;0
WireConnection;155;1;154;0
WireConnection;29;0;28;0
WireConnection;29;1;18;0
WireConnection;96;0;68;0
WireConnection;96;1;84;0
WireConnection;0;2;155;0
WireConnection;0;13;96;0
ASEEND*/
//CHKSM=08DFDB2307C03AF65C04598EB2CD1B3931EE70AE