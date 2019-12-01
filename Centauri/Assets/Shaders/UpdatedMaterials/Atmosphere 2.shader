// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/PlanetUpdated2"
{
    Properties
    {
       	_Tint ("Tint", Color) = (1, 1, 1, 1)
		_MainTex ("Albedo", 2D) = "white" {}
		[NoScaleOffset] _NormalMap ("Normals", 2D) = "bump" {}
		_BumpScale ("Bump Scale", Float) = 1
		[Gamma] _Metallic ("Metallic", Range(0, 1)) = 0
		_Smoothness ("Smoothness", Range(0, 1)) = 0.1
		_DetailTex ("Detail Texture", 2D) = "gray" {}
		[NoScaleOffset] _DetailNormalMap ("Detail Normals", 2D) = "bump" {}
		_DetailBumpScale ("Detail Bump Scale", Float) = 1
		   _Color("Color", Color) = (0, 0, 0, 1)
        _AtmoColor("Atmosphere Color", Color) = (0.5, 0.5, 1.0, 1)
        _Size("Size", Float) = 0.1
        _Falloff("Falloff", Float) = 5
        _FalloffPlanet("Falloff Planet", Float) = 5
        _Transparency("Transparency", Float) = 15
        _TransparencyPlanet("Transparency Planet", Float) = 1
        _CloudTexture("Cloud Texture", 2D) = "Black" {}
        _CloudSize("Cloud Size", Float) = 0.1
        _CloudColor("Cloud Color", Color) = (0,0,0,0)
    }
 
	SubShader
    {
		Tags {"LightMode" = "ForwardBase"}
        Pass
        {
            Name "PlanetBase"
            Cull Back
 
            CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag

                #include "UnityCG.cginc"
                #include "AutoLight.cginc"
                #include "UnityPBSLighting.cginc"
    			#pragma target 3.0
                
 
                uniform sampler2D _MainTex, _DetailTex;
                uniform float4 _MainTex_ST, _DetailTex_ST;
                uniform float4 _Color;
                uniform float4 _AtmoColor;
                uniform float _FalloffPlanet;
                uniform float _TransparencyPlanet;

                uniform sampler2D _NormalMap, _DetailNormalMap;
                uniform float _BumpScale, _DetailBumpScale;

                uniform float _Metallic;
                uniform float _Smoothness;


                struct VertexData {
                    float4 position : POSITION;
                    float3 normal : NORMAL;
                    float2 uv : TEXCOORD0;
                    float2 texcoord : TEXCOORD1;
                };


                struct v2f
                {
                    float4 pos : SV_POSITION;
                    float3 normal : TEXCOORD0;
                    float3 worldPos : TEXCOORD1;
                    float2 texcoord : TEXCOORD2;
                    float4 uv : TEXCOORD3;

                    #if defined(VERTEXLIGHT_ON)
		            float3 vertexLightColor : TEXCOORD3;
	                #endif
                };

                    void ComputeVertexLightColor (inout v2f i) {
        #if defined(VERTEXLIGHT_ON)
            i.vertexLightColor = Shade4PointLights(
                unity_4LightPosX0, unity_4LightPosY0, unity_4LightPosZ0,
                unity_LightColor[0].rgb, unity_LightColor[1].rgb,
                unity_LightColor[2].rgb, unity_LightColor[3].rgb,
                unity_4LightAtten0, i.worldPos, i.normal
            );
        #endif
                }





 
                v2f vert(VertexData v)
                {
                    v2f o;
 
                    o.pos = UnityObjectToClipPos (v.position);
                    o.normal = UnityObjectToWorldNormal(v.normal);
                    // o.normal = v.normal;
                    o.worldPos = mul(unity_ObjectToWorld, v.position).xyz;
                    o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);

                    o.uv.xy = TRANSFORM_TEX(v.uv, _MainTex);
                    o.uv.zw = TRANSFORM_TEX(v.uv, _DetailTex);
                    ComputeVertexLightColor(o);
 
                    return o;
                }


                void InitializeFragmentNormal(inout v2f i) {
	                float3 mainNormal =
		            UnpackScaleNormal(tex2D(_NormalMap, i.uv.xy), _BumpScale);
	                float3 detailNormal =
		            UnpackScaleNormal(tex2D(_DetailNormalMap, i.uv.zw), _DetailBumpScale);
	                i.normal = BlendNormals(mainNormal, detailNormal);
	                i.normal = i.normal.xzy;
                }

                UnityLight CreateLight (v2f i) {
                    UnityLight light;

                    #if defined(POINT) || defined(POINT_COOKIE) || defined(SPOT)
		            light.dir = normalize(_WorldSpaceLightPos0.xyz - i.worldPos);
	                #else
		                light.dir = _WorldSpaceLightPos0.xyz;
	                #endif

                    light.color = _LightColor0.rgb;
                    light.ndotl = DotClamped(i.normal, light.dir);
                    return light;
                }

                UnityIndirect CreateIndirectLight (v2f i) 
                {
                    UnityIndirect indirectLight;
                    indirectLight.diffuse = 0;
                    indirectLight.specular = 0;
                    #if defined(VERTEXLIGHT_ON)
		                indirectLight.diffuse = i.vertexLightColor;
	                #endif

                	#if defined(FORWARD_BASE_PASS)
		                indirectLight.diffuse += max(0, ShadeSH9(float4(i.normal, 1)));
	                #endif

                    return indirectLight;
                }
 
                float4 frag(v2f i) : COLOR
                {
                    i.normal = normalize(i.normal);
                    float3 viewdir = normalize(_WorldSpaceCameraPos-i.worldPos);
 
                    float4 atmo = _AtmoColor;
                    atmo.a = pow(1.0-saturate(dot(viewdir, i.normal)), _FalloffPlanet);
                    atmo.a *= _TransparencyPlanet*_Color;
 
                    float4 color = tex2D(_MainTex, i.texcoord)*_Color;
                    color.rgb = lerp(color.rgb, atmo.rgb, atmo.a);
                    
 
                    // float4 atmo = _AtmoColor;
                    // atmo.a = pow(1.0-saturate(dot(viewDir, i.normal)), _FalloffPlanet);
                    // atmo.a *= _TransparencyPlanet*_Color;
 
                    // float4 color = tex2D(_MainTex, i.texcoord)*_Color;
                    // color.rgb = lerp(color.rgb, atmo.rgb, atmo.a);
                    // albedo *= color;

                    // albedo.rgb = lerp(albedo.rgb, atmo.rgb, atmo.a);



 
                    return color*dot(_WorldSpaceLightPos0, i.normal);

                }
            ENDCG
        }
 
		// Tags {"LightMode" = "ForwardBase" "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
        // Pass
        // {
        //     Name "FORWARD"
        //     Cull Front
        //     Blend SrcAlpha One
		// 	ZWrite Off
 
        //     CGPROGRAM
        //         #pragma vertex vert
        //         #pragma fragment frag
 
        //         #include "UnityCG.cginc"
    	// 		#pragma target 3.0

 
        //         uniform float4 _Color;
        //         uniform float4 _AtmoColor;
        //         uniform float _Size;
        //         uniform float _Falloff;
        //         uniform float _Transparency;
        //         uniform sampler2D _CloudTexture;
        //         uniform float4 _CloudTexture_ST;
        //         uniform float4 _CloudColor;

        //          struct appdata            
        //          {                
        //              float4 vertex : POSITION;                
        //              float2 uv : TEXCOORD0;            
        //              float3 normal : NORMAL;
        //         };
 
        //         struct v2f
        //         {
        //             float4 pos : SV_POSITION;
        //             float3 normal : TEXCOORD0;
        //             float3 worldvertpos : TEXCOORD1;
        //             float2 uv : TEXCOORD2;
        //         };

        //         v2f vert(appdata v)
        //         {
        //             v2f o;
 
        //             v.vertex.xyz += v.normal*_Size;
        //             o.pos = UnityObjectToClipPos (v.vertex);
        //             o.normal = mul((float3x3)unity_ObjectToWorld, v.normal);
        //             // o.normal = v.normal;
        //             o.worldvertpos = mul(unity_ObjectToWorld, v.vertex);
        //             o.uv = TRANSFORM_TEX(v.uv, _CloudTexture);
 
        //             return o;
        //         }
 
        //         float4 frag(v2f i) : COLOR
        //         {
        //             i.normal = normalize(i.normal);
        //             float3 viewdir = normalize(i.worldvertpos-_WorldSpaceCameraPos);

        //             float4 cloudTex = tex2D(_CloudTexture, i.uv) * _CloudColor;
        //             cloudTex.a = _CloudColor.a;

        //             float4 color = _AtmoColor;
		// 			color.a = dot(viewdir, i.normal);
		// 			color.a *=dot(i.normal, _WorldSpaceLightPos0);
		// 			color.a = saturate(color.a);
		// 			color.a = pow(color.a, _Falloff);
		// 			color.a *= _Transparency;

        //             return color;
        //         }
        //     ENDCG
        // }

        // Tags {"LightMode" = "ForwardBase" "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
        // Pass
        // {
        //     Name "CLOUD"
        //     Blend SrcAlpha One
		// 	ZWrite Off
 
        //     CGPROGRAM
        //         #pragma vertex vert
        //         #pragma fragment frag
 
 				
        //         #include "UnityCG.cginc"
    	// 		#pragma target 3.0

 
        //         uniform float4 _Color;
        //         uniform float4 _AtmoColor;
        //         uniform float _CloudSize;
        //         uniform float _Falloff;
        //         uniform float _Transparency;
        //         uniform sampler2D _CloudTexture;
        //         uniform float4 _CloudTexture_ST;
        //         uniform float4 _CloudColor;

        //          struct appdata            
        //          {                
        //              float4 vertex : POSITION;                
        //              float2 uv : TEXCOORD0;            
        //              float3 normal : NORMAL;
        //         };
 
        //         struct v2f
        //         {
        //             float4 pos : SV_POSITION;
        //             float3 normal : TEXCOORD0;
        //             float2 uv : TEXCOORD2;
        //             float3 worldvertpos : TEXCOORD1;
        //         };

        //         v2f vert(appdata v)
        //         {
        //             v2f o;
 
        //             v.vertex.xyz += v.normal*_CloudSize;
        //             o.pos = UnityObjectToClipPos (v.vertex);
        //              o.normal = v.normal;
		// 			 o.worldvertpos = mul(unity_ObjectToWorld, v.vertex);
        //             o.uv = TRANSFORM_TEX(v.uv, _CloudTexture);
 
        //             return o;
        //         }
 
        //         float4 frag(v2f i) : COLOR
        //         {
        //             float3 viewdir = normalize(i.worldvertpos + _WorldSpaceCameraPos);

        //             float4 cloudTex = tex2D(_CloudTexture, i.uv) * _CloudColor;
        //             cloudTex.a = _CloudColor.a;
        //             cloudTex.a *= dot(viewdir, i.normal);
        //             cloudTex.a *= dot(i.normal, _WorldSpaceLightPos0);

        //             return cloudTex;
        //         }
        //     ENDCG
        // }
    }

 
}