// Volumetric Atmospheric Scattering
// by Alan Zucconi
// http://www.alanzucconi.com/?p=7374
Shader "Custom/Atmospheric"
{
	Properties
	{
		// ----------------------------------------------
		// --- Standard Shader ---
		_Color("Main Color", Color) = (1,1,1,1)
		_MainTex("Main Texture", 2D) = "white" {}
		_NormalMap("Normal map", 2D) = "white" {}
		[KeywordEnum(Off,On)] _UseNormal("Use Normal Map?", Float) = 0
		_Diffuse("Diffuse %", Range(0,1)) = 1
		[KeywordEnum(Off, Vert, Frag)] _Lighting("Lighting Mode", Float) = 0
		_SpecularMap("Specular Map", 2D) = "black" {}
		_SpecularFactor("Specular %",Range(0,1)) = 1
		_SpecularPower("Specular Power", Float) = 100
		[Toggle] _AmbientMode("Ambient Light?", Float) = 0
		_AmbientFactor("Ambient %", Range(0,1)) = 1
		_BumpScale("BumpScale", Range(-5,5)) = 1
        _MainTex("Texture (RGB)", 2D) = "black" {}
        _AtmoColor("Atmosphere Color", Color) = (0.5, 0.5, 1.0, 1)
        _Size("Size", Float) = 0.1
        _Falloff("Falloff", Float) = 5
        _FalloffPlanet("Falloff Planet", Float) = 5
        _Transparency("Transparency", Float) = 15
        _TransparencyPlanet("Transparency Planet", Float) = 1
		[KeywordEnum(Off, Enable)] _EnableAtmosphere("EnableAtmos", Float) = 0

		[Space]
		_CircleRadius("Circle Radius", Float) = 1
		_CircleColor("Circle Color", Color) = (1,1,1,1)

		[Space]
		_CloudTexture("Cloud Texture", 2D) = "white"{}
		_CloudColor("Cloud color", Color) = (1,1,1,1)
		_CloudSize("Cloud size", Float) = 1



		// ---------------------------------------------
		
		// ----------------------------------------------
		// --- Scattering ---
		[Space]
		_AtmosphereModifier ("Atmosphere Modifier", Float) = 1
		_ScatteringModifier ("Scattering Modifier", Float) = 1
		_AtmosphereColor ("Atmosphere Color", Color) = (1,1,1,1)

		[Space]
		_SphereRadius("Sphere Radius (units)", Range(0.1,25)) = 6.371
		
		// Total radius + _PlanetRadius + _AtmosphereHeight
		_PlanetRadius ("Planet Radius (metres)", Float) = 6371000 // 6731Km
		_AtmosphereHeight ("Atmosphere Height (metres)", Float) = 60000 // 60Km
		// ----------------------------------------------

		// ----------------------------------------------
		// --- Rayleigh Scattering ---
		[Space]
		_RayScatteringCoefficient("βᵣ, Rayleight Scattering Coefficient (RGB, metres^-1)", Vector) = (0.000005804542996261093, 0.000013562911419845635, 0.00003026590629238531, 0)
		_RayScaleHeight("H₀, Rayleigh Scale Height (metres)", Float) = 8000 // 8Km
		// ----------------------------------------------

		// ----------------------------------------------
		// --- Mie Scattering ---
		[Space]
		_MieScatteringCoefficient("βₘ, Mie Scattering Coefficient (metres^-1)", Float) = 0.0021
		_MieAnisotropy ("g, Mie preferred scattering direction", Range(-1,1)) = 0.758
		_MieScaleHeight("H₀, Mie Scale Height (metres)", Float) = 1200 // 1.2Km
		
	
		[Space]
		//_SunCentre ("Sun Centre", Vector) = (0,0,0,0)
		_SunIntensity("Sun intensity", Range(0,100)) = 22


		[Space]
		_ViewSamples("View ray samples (out-scattering)", Range(0,256)) = 16
		_LightSamples("Light ray samples (in-scattering)", Range(0,256)) = 8
	}
	SubShader 
	{

		Pass
		{
			//http://docs.unity3d.com/462/Documentation/Manual/SL-SubshaderTags.html
			// Background : 1000     -        0 - 1499 = Background
			// Geometry   : 2000     -     1500 - 2399 = Geometry
			// AlphaTest  : 2450     -     2400 - 2699 = AlphaTest
			// Transparent: 3000     -     2700 - 3599 = Transparent
			// Overlay    : 4000     -     3600 - 5000 = Overlay

				Tags {"LightMode" = "ForwardBase" "Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent"}
				Blend SrcAlpha OneMinusSrcAlpha

				CGPROGRAM
			//http://docs.unity3d.com/Manual/SL-ShaderPrograms.html
			#pragma vertex vert
			#pragma fragment frag
			#pragma shader_feature _USENORMAL_OFF _USENORMAL_ON
			#pragma shader_feature _LIGHTING_OFF _LIGHTING_VERT _LIGHTING_FRAG
			#pragma shader_feature _AMBIENTMODE_OFF _AMBIENTMODE_ON
			#pragma shader_feature _ENABLEATMOSPHERE_OFF _ENABLEATMOSPHERE_ENABLE

			#include "CVGLighting.cginc" 
   			 #include "UnityStandardUtils.cginc"


			//http://docs.unity3d.com/ru/current/Manual/SL-ShaderPerformance.html
			//http://docs.unity3d.com/Manual/SL-ShaderPerformance.html
			uniform half4 _Color;
			uniform sampler2D _MainTex;
			uniform float4 _MainTex_ST;

			uniform sampler2D _NormalMap;
			uniform float4 _NormalMap_ST;

			uniform float _Diffuse;
			uniform float4 _LightColor0;
			uniform float4 _AtmoColor;
			uniform float _FalloffPlanet;
			uniform float _TransparencyPlanet;



			uniform sampler2D _SpecularMap;
			uniform float _SpecularFactor;
			uniform float _SpecularPower;
			uniform float _BumpScale;


			#if _AMBIENTMODE_ON
				uniform float _AmbientFactor;
			#endif

				//https://msdn.microsoft.com/en-us/library/windows/desktop/bb509647%28v=vs.85%29.aspx#VS
				struct vertexInput
				{
					float4 vertex : POSITION;
					float4 normal : NORMAL;
					float4 texcoord : TEXCOORD0;
					#if _USENORMAL_ON
						float4 tangent : TANGENT;
					#endif
				};

				struct vertexOutput
				{
					float4 pos : SV_POSITION;
					float4 texcoord : TEXCOORD0;
					float4 normalWorld : TEXCOORD1;
					float4 posWorld : TEXCOORD2;
					#if _USENORMAL_ON
						float4 tangentWorld : TEXCOORD3;
						float3 binormalWorld : TEXCOORD4;
						float4 normalTexCoord : TEXCOORD5;
					#endif
					#if _LIGHTING_VERT
						float4 surfaceColor : COLOR0;
					#endif
				};


				vertexOutput vert(vertexInput v)
				{
					vertexOutput o; UNITY_INITIALIZE_OUTPUT(vertexOutput, o); // d3d11 requires initialization
					o.pos = UnityObjectToClipPos(v.vertex);
					o.texcoord.xy = (v.texcoord.xy * _MainTex_ST.xy + _MainTex_ST.zw);
					o.normalWorld = float4(normalize(mul(normalize(v.normal.xyz), (float3x3)unity_WorldToObject)),v.normal.w);

					#if _USENORMAL_ON
					// World space T, B, N values
					o.normalTexCoord.xy = (v.texcoord.xy * _NormalMap_ST.xy + _NormalMap_ST.zw);
					//o.tangentWorld = normalize(mul(v.tangent,_Object2World));
					o.tangentWorld = (normalize(mul((float3x3)unity_ObjectToWorld, v.tangent.xyz)),v.tangent.w);
					o.binormalWorld = normalize(cross(o.normalWorld, o.tangentWorld) * v.tangent.w);
				#endif
				#if _LIGHTING_VERT
					float3 lightDir = normalize(_WorldSpaceLightPos0.xyz);
					float3 lightColor = _LightColor0.xyz;
					float attenuation = 1;
					float3 diffuseCol = DiffuseLambert(o.normalWorld, lightDir, lightColor, _Diffuse, attenuation);

					float4 specularMap = tex2Dlod(_SpecularMap, float4(o.texcoord.xy, 0, 0));//float4 specularMap = tex2D(_SpecularMap, o.texcoord.xy);//float4 specularMap = tex2D(_SpecularMap, o.texcoord.xy);
					o.posWorld = mul(unity_ObjectToWorld, v.vertex);
					float3 worldSpaceViewDir = normalize(_WorldSpaceCameraPos - o.posWorld);
					float3 specularCol = SpecularBlinnPhong(o.normalWorld, lightDir, worldSpaceViewDir, specularMap.rgb , _SpecularFactor, attenuation, _SpecularPower);
					o.surfaceColor = float4(diffuseCol + specularCol,1);

					#if _AMBIENTMODE_ON
						float3 ambientColor = _AmbientFactor * UNITY_LIGHTMODEL_AMBIENT;
						o.surfaceColor = float4(o.surfaceColor.rgb + ambientColor,1);
					#endif
				#endif
				return o;
			}

			half4 frag(vertexOutput i) : COLOR
			{

					float3 worldSpaceViewDir = normalize(_WorldSpaceCameraPos - i.posWorld);

				#if _USENORMAL_ON
					 float3 worldNormalAtPixel = WorldNormalFromNormalMapWithScale(_NormalMap, i.normalTexCoord.xy,_BumpScale ,i.tangentWorld.xyz, i.binormalWorld.xyz, i.normalWorld.xyz);
					// worldNormalAtPixel += textureColor;
					//return tex2D(_MainTex, i.texcoord) * _Color;
				#else
					float3 worldNormalAtPixel = i.normalWorld.xyz;
				#endif

				#if _LIGHTING_FRAG

					float3 lightDir = normalize(_WorldSpaceLightPos0.xyz);
					float3 lightColor = _LightColor0.xyz;
					float attenuation = 1;
					float3 diffuseCol = DiffuseLambert(worldNormalAtPixel, lightDir, lightColor, _Diffuse, attenuation);
					
					float4 textureColor = tex2D(_MainTex, i.texcoord);
					textureColor.a *= dot(worldSpaceViewDir, i.normalWorld);
					textureColor.a *= dot(i.normalWorld, _WorldSpaceLightPos0);
					
					diffuseCol *= textureColor;
					diffuseCol *= _Color;

					float4 atmo = _AtmoColor;
                    atmo.a = pow(1.0-saturate(dot(worldSpaceViewDir, i.normalWorld)), _FalloffPlanet);
                    atmo.a *= _TransparencyPlanet*_Color;
					diffuseCol.rgb *= lerp(diffuseCol.rgb, atmo.rgb, atmo.a);

					float4 specularMap = tex2D(_SpecularMap, i.texcoord.xy);

					float3 specularCol = SpecularBlinnPhong(worldNormalAtPixel, lightDir, worldSpaceViewDir, specularMap.rgb , _SpecularFactor, attenuation, _SpecularPower);


					#if _AMBIENTMODE_ON
						float3 ambientColor = _AmbientFactor * UNITY_LIGHTMODEL_AMBIENT;
						return float4(diffuseCol + specularCol + ambientColor,1);
					#else
						return float4(diffuseCol + specularCol,1);
					#endif

				#elif _LIGHTING_VERT
					return i.surfaceColor;
				#else
					return float4(worldNormalAtPixel,1);
				#endif
			}
			ENDCG
		}

		Tags {"LightMode" = "ForwardBase" "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
        Pass
        {
            Name "FORWARD"
            Cull Front
            Blend SrcAlpha One
			ZWrite Off
 
            CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
 
                #include "UnityCG.cginc"
    			#pragma target 3.0

 
                float4 _Color;
                float4 _AtmoColor;
                float _Size;
                float _Falloff;
                float _Transparency;
                sampler2D _CloudTexture;
                float4 _CloudTexture_ST;
                float4 _CloudColor;

                 struct appdata            
                 {                
                     float4 vertex : POSITION;                
                     float2 uv : TEXCOORD0;            
                     float3 normal : NORMAL;
                };
 
                struct v2f
                {
                    float4 pos : SV_POSITION;
                    float3 normal : TEXCOORD0;
                    float3 worldvertpos : TEXCOORD1;
                    float2 uv : TEXCOORD2;
                };

                v2f vert(appdata v)
                {
                    v2f o;
 
                    v.vertex.xyz += v.normal*_Size;
                    o.pos = UnityObjectToClipPos (v.vertex);
                    o.normal = mul((float3x3)unity_ObjectToWorld, v.normal);
                    // o.normal = v.normal;
                    o.worldvertpos = mul(unity_ObjectToWorld, v.vertex);
                    o.uv = TRANSFORM_TEX(v.uv, _CloudTexture);
 
                    return o;
                }
 
                float4 frag(v2f i) : COLOR
                {
                    i.normal = normalize(i.normal);
                    float3 viewdir = normalize(i.worldvertpos-_WorldSpaceCameraPos);

                    float4 color = _AtmoColor;
					color.a = dot(viewdir, i.normal);
					color.a *=dot(i.normal, _WorldSpaceLightPos0);
					color.a = saturate(color.a);
					color.a = pow(color.a, _Falloff);
					color.a *= _Transparency;

                    return color;
                }
            ENDCG
        }

	}
	FallBack "Diffuse"

}
