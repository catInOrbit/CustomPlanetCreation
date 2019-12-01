// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/PlanetUpdated"
{
    Properties
    {
        _MainTex("Texture (RGB)", 2D) = "black" {}
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
    			#pragma target 3.0
                
 
                sampler2D _MainTex;
                float4 _MainTex_ST;
                float4 _Color;
                float4 _AtmoColor;
                float _FalloffPlanet;
                float _TransparencyPlanet;

                struct v2f
                {
                    float4 pos : SV_POSITION;
                    float3 normal : TEXCOORD0;
                    float3 worldvertpos : TEXCOORD1;
                    float2 texcoord : TEXCOORD2;
                };


 
                v2f vert(appdata_base v)
                {
                    v2f o;
 
                    o.pos = UnityObjectToClipPos (v.vertex);
                    o.normal = mul((float3x3)unity_ObjectToWorld, v.normal);
                    // o.normal = v.normal;
                    o.worldvertpos = mul(unity_ObjectToWorld, v.vertex).xyz;
                    o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
 
                    return o;
                }
 
                float4 frag(v2f i) : COLOR
                {
                    i.normal = normalize(i.normal);
                    float3 viewdir = normalize(_WorldSpaceCameraPos-i.worldvertpos);
 
                    float4 atmo = _AtmoColor;
                    atmo.a = pow(1.0-saturate(dot(viewdir, i.normal)), _FalloffPlanet);
                    atmo.a *= _TransparencyPlanet*_Color;
 
                    float4 color = tex2D(_MainTex, i.texcoord)*_Color;
                    color.rgb = lerp(color.rgb, atmo.rgb, atmo.a);
 
                    return color*dot(_WorldSpaceLightPos0, i.normal);
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

                    float4 cloudTex = tex2D(_CloudTexture, i.uv) * _CloudColor;
                    cloudTex.a = _CloudColor.a;

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

        Tags {"LightMode" = "ForwardBase" "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
        Pass
        {
            Name "CLOUD"
            Blend SrcAlpha One
			ZWrite Off
 
            CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
 
 				
                #include "UnityCG.cginc"
    			#pragma target 3.0

 
                float4 _Color;
                float4 _AtmoColor;
                float _CloudSize;
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
                    float2 uv : TEXCOORD2;
                    float3 worldvertpos : TEXCOORD1;
                };

                v2f vert(appdata v)
                {
                    v2f o;
 
                    v.vertex.xyz += v.normal*_CloudSize;
                    o.pos = UnityObjectToClipPos (v.vertex);
                     o.normal = v.normal;
					 o.worldvertpos = mul(unity_ObjectToWorld, v.vertex);
                    o.uv = TRANSFORM_TEX(v.uv, _CloudTexture);
 
                    return o;
                }
 
                float4 frag(v2f i) : COLOR
                {
                    float3 viewdir = normalize(i.worldvertpos + _WorldSpaceCameraPos);

                    float4 cloudTex = tex2D(_CloudTexture, i.uv) * _CloudColor;
                    cloudTex.a = _CloudColor.a;
                    cloudTex.a *= dot(viewdir, i.normal);
                    cloudTex.a *= dot(i.normal, _WorldSpaceLightPos0);

                    return cloudTex;
                }
            ENDCG
        }
    }

 
}