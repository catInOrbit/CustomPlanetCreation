Shader "Unlit/LightingPBR"
{

    Properties
    {
        _Color("Color", Color) = (1,0,0,1)
        _DiffuseTex("Texture", 2D) = "white"{}
        _Ambient("Ambient", Range(0,1)) = 0.25
    }

    SubShader
    {
        Tags{ "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma target 3.0

            #include "UnityCG.cginc"
            #include "UnityLightingCommon.cginc"

            sampler2D _DiffuseTex;
            float4 _Color;
            float4 _DiffuseTex_ST;
            float _Ambient;

            struct appdata
            {
                float4 vertex: POSITION;
                float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float3 worldNormal : TEXCOORD0;
                float2 uv : TEXCOORD1;
            };

            v2f vert(appdata v)
            {
                v2f o;

                o.vertex = UnityObjectToClipPos(v.vertex);

                float3 worldNormal = UnityObjectToWorldNormal(v.normal);
                o.worldNormal = worldNormal;
                o.uv = TRANSFORM_TEX(v.uv, _DiffuseTex);
                    return o;
            }

            float4 frag (v2f i) : SV_TARGET
            {
                float3 normalDir = normalize(i.worldNormal);
                float4 tex = tex2D(_DiffuseTex, i.uv);
                float dotNormalLight = max(_Ambient, dot(normalDir, _WorldSpaceLightPos0.xyz));
                float4 diffuseTerm = dotNormalLight * _Color * tex *_LightColor0;

                return diffuseTerm;
            }

            ENDCG
        }
    }
}